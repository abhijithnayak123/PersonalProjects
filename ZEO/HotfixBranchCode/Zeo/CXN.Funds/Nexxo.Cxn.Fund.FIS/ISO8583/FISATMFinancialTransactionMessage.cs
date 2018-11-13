using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MGI.Cxn.Fund.FIS.ISO8583
{
	internal class FISATMFinancialTransactionMessage : BaseFISISOMessage
	{
		protected string _addDataPrefix = "u";

		//public FISATMFinancialTransactionMessage( decimal amount, DateTime kioskDT, string transactionId, KioskProfile kiosk, long cardNumber, string processingCode) :
		//    this( amount, kioskDT, transactionId, kiosk, cardNumber, "x", processingCode )
		//{
		//}

		public FISATMFinancialTransactionMessage( Dictionary<string, string> fisVars, decimal amount, DateTime kioskDT, string transactionId, Terminal terminal, string cardNumber )
			: base( fisVars, amount, kioskDT, transactionId, terminal, cardNumber)
		{
		}

		public override void Create()
		{
			base.Create();

			AddDE( 48, string.Format( "{0}4{1}000840           ", _addDataPrefix.PadRight( 24 ), ( (int)Enum.Parse( typeof( ANSIStateCodes ), _terminal.State ) ).ToString().PadLeft( 2, '0' ) ) );
		}
	}
}
