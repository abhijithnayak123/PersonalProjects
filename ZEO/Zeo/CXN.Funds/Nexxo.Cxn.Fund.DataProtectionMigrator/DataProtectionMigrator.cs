using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;
using System.Diagnostics;

using Spring.Transaction.Interceptor;

using MGI.Cxn.Fund.FirstView.Data;
using MGI.Cxn.Fund.TSys.Data;

using MGI.Common.DataAccess.Data;
using MGI.Common.DataAccess.Contract;

using MGI.Common.DataProtection.Contract;
using MGI.Common.DataProtection.Impl;

using MGI.Common.Util;

namespace MGI.Cxn.Fund.DataProtectionMigrator
{
	public class DataMigrator : IDataMigrator
	{
		private DataProtectionService encryptionSvc = new DataProtectionService();
		private DataProtectionSimulator simulatorSvc = new DataProtectionSimulator();

		private IRepository<FirstViewCard> _fvCardSvc;
		public IRepository<FirstViewCard> FvCardSvc { set { _fvCardSvc = value; } }

		private IRepository<FirstViewCard_Aud> _fvCardAudSvc;
		public IRepository<FirstViewCard_Aud> FvCardAudSvc { set { _fvCardAudSvc = value; } }

		private IRepository<TSysAccount> _tSysAccountSvc;
		public IRepository<TSysAccount> TSysAccountSvc { set { _tSysAccountSvc = value; } }

		[Transaction()]
		public void Run(bool testRun, int oldSlot, int newSlot)
		{
			UpdateTSys(oldSlot, newSlot);
			UpdateFirstView(oldSlot, newSlot);

			if (testRun)
				throw new Exception("Don't commit transaction - test run");
		}

		private void UpdateFirstView(int oldSlot, int newSlot)
		{
			// 1. get list of FV account
			IQueryable<FirstViewCard> qFirstViewAccts = _fvCardSvc.All();

			Trace.WriteLine(string.Format("{0} FirstView cards found", qFirstViewAccts.Count()));

			if (qFirstViewAccts.Count() == 0)
				return;

			List<FirstViewCard> firstViewAccts = qFirstViewAccts.ToList();

			// 2. for each account, decrypt with simulator and re-encrypt with the real encryption service
			foreach (FirstViewCard firstViewAcct in firstViewAccts)
			{
				Trace.WriteLine(string.Format("Processing acct ID {0}...", firstViewAcct.Id));
				Trace.Indent();

				if (firstViewAcct.AccountNumber == null)
				{
					Trace.WriteLine("No account value - skipping");
					Trace.Unindent();
					continue;
				}

				firstViewAcct.AccountNumber = getNewEncryptedValue(firstViewAcct.AccountNumber, oldSlot, newSlot);
				firstViewAcct.BSAccountNumber = getNewEncryptedValue(firstViewAcct.BSAccountNumber, oldSlot, newSlot);

				Trace.WriteLine(string.Format("new value is {0} chars", firstViewAcct.AccountNumber.Length));

				checkEncryption(firstViewAcct.AccountNumber, newSlot);
				_fvCardSvc.UpdateWithFlush(firstViewAcct);

				Trace.Unindent();
				Trace.WriteLine("done.");
			}
		}

		private void UpdateTSys(int oldSlot, int newSlot)
		{
			// 1. get list of FV account
			IQueryable<TSysAccount> qTSysAccounts = _tSysAccountSvc.All();

			Trace.WriteLine(string.Format("{0} TSys cards found", qTSysAccounts.Count()));

			if (qTSysAccounts.Count() == 0)
				return;

			List<TSysAccount> tSysAccounts = qTSysAccounts.ToList();

			// 2. for each account, decrypt with simulator and re-encrypt with the real encryption service
			foreach (TSysAccount tSysAccount in tSysAccounts)
			{
				Trace.WriteLine(string.Format("Processing acct ID {0}...", tSysAccount.Id));
				Trace.Indent();

				tSysAccount.CardNumber = getNewEncryptedValue(tSysAccount.CardNumber, oldSlot, newSlot);

				Trace.WriteLine(string.Format("new value is {0} chars", tSysAccount.CardNumber.Length));

				checkEncryption(tSysAccount.CardNumber, newSlot);
				_tSysAccountSvc.UpdateWithFlush(tSysAccount);

				Trace.Unindent();
				Trace.WriteLine("done.");
			}
		}

		private string getNewEncryptedValue(string encryptedValue, int oldSlot, int newSlot)
		{
			IDataProtectionService decryptSvc;
			
			// if same, assume simulator-encrypted
			if (oldSlot == newSlot)
			{
				if (encryptedValue.Length == 44) // assume already encrypted - do not re-encrypt
					return encryptedValue;

				decryptSvc = simulatorSvc;
			}
			else
				decryptSvc = encryptionSvc;

			string dataValue = decryptSvc.Decrypt(encryptedValue, oldSlot);
			return encryptionSvc.Encrypt(dataValue, newSlot);
		}

		private void checkEncryption(string encryptedValue, int slot)
		{
			string dataValue = encryptionSvc.Decrypt(encryptedValue, slot);
			Trace.WriteLine(string.Format("Decrypted value is {0} chars", dataValue.Length));
			string reEncryptedValue = encryptionSvc.Encrypt(dataValue, slot);
			Trace.WriteLine(string.Format("Re-encrypted value is {0} chars", reEncryptedValue.Length));
			Trace.WriteLineIf(encryptedValue == reEncryptedValue, "encryption working properly");
		}
	}
}
