using MGI.Common.Util;
using MGI.Cxn.Check.Certegy.Certegy;
using MGI.Cxn.Check.Certegy.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.Check.Certegy.Impl
{
	public class SimulatorIO : IIO
	{
		public TLoggerCommon MongoDBLogger { get; set; }

		public Certegy.authorizeResponse AuthorizeCheck(Data.Transaction transaction, Data.Credential credential, MGIContext mgiContext)
		{
			authorizeResponse response = new authorizeResponse();

			response.TransID = Convert.ToString(transaction.Id);

			response.ResponseCode = GetResponseCode(transaction.CheckAmount);

			if (response.ResponseCode == "00")
				response.ApprovalNumber = "123456";

			response.CertegyUID = "12345678915";

			response.CheckABA = transaction.RoutingNumber;

			response.CheckAcct = transaction.AccountNumber;

			response.CheckNumber = transaction.CheckNumber;

			response.CheckType = transaction.CertegySubmitType;

			return response;
		}

		public Certegy.reverseResponse ReverseCheck(Data.Transaction transaction, Data.Credential credential, MGIContext mgiContext)
		{
			Certegy.reverseResponse response = new reverseResponse() { 
				TransID = Convert.ToString(transaction.Id),
				ResponseCode = "13",
			};

			return response;
		}

		private string GetResponseCode(decimal amount)
		{
			List<string> responceCode = null;

			Int32 lastDigit = (int)(amount * 100) % 10;

			if (lastDigit == 0 || lastDigit == 1 || lastDigit == 2 || lastDigit == 3)
			{
				return "00";
			}
			else
			{
				responceCode = new List<string>() { "01", "02", "03", "04", "05", "06", "07", "08", 
					"09", "10", "11", "14", "15", "16", "22", "23", "24" };

				RandomCryptoServiceProvider randomCryptoServiceProvider = new RandomCryptoServiceProvider();
				int ran = randomCryptoServiceProvider.Next(17);
				return responceCode[ran];
			}
		}

		public echoResponse Diagnostics(Data.Credential credential, MGIContext mgiContext)
		{
			throw new NotImplementedException();
		}
	}
}
