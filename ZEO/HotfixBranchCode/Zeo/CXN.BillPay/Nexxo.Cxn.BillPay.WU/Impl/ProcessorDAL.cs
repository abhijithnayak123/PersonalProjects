using MGI.Common.DataAccess.Contract;
using MGI.Cxn.BillPay.WU.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGI.Cxn.BillPay.WU.Impl
{
	public class ProcessorDAL
	{
		#region Dependencies
		public IRepository<WesternUnionAccount> WesternUnionAccountRepo { get; set; }
		public IRepository<WesternUnionTrx> WesternUnionTrxRepo { get; set; }
		public IRepository<ImportedBiller> WesternUnionImportBillersRepo { get; set; }
		#endregion

		public long AddAccount(WesternUnionAccount account)
		{
			object id = WesternUnionAccountRepo.AddWithFlush(account);
			return account.Id;
		}

		public WesternUnionAccount GetAccountById(long accountID)
		{
			WesternUnionAccount account = WesternUnionAccountRepo.FindBy(a => a.Id == accountID);
			return account;
		}

		public long AddTrx(WesternUnionTrx trx)
		{
			object id = WesternUnionTrxRepo.AddWithFlush(trx);
			return trx.Id;
		}

		public WesternUnionTrx GetTrxById(long trxId)
		{
			WesternUnionTrx trx = WesternUnionTrxRepo.FindBy(a => a.Id == trxId);
			return trx;
		}

		public void AddImportedBiller(ImportedBiller biller)
		{
			WesternUnionImportBillersRepo.AddWithFlush(biller);
		}

		public bool UpdateImportedBiller(ImportedBiller biller)
		{
			WesternUnionImportBillersRepo.UpdateWithFlush(biller);
			return true;
		}

		public ImportedBiller GetExistingBiller(ImportedBiller biller)
		{
			ImportedBiller pastBiller = WesternUnionImportBillersRepo.FindBy(c => c.CardNumber == biller.CardNumber && c.BillerName == biller.BillerName && c.WUIndex == biller.WUIndex);
			return pastBiller;
		}
		public long UpdateAccount(WesternUnionAccount account)
		{
			object id = WesternUnionAccountRepo.Update(account);
			return account.Id;
		}
	}
}
