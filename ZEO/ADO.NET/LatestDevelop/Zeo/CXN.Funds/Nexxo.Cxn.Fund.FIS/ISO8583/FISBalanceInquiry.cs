using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MGI.Cxn.Fund.FIS.ISO8583
{
	internal class FISBalanceInquiry : FISATMFinancialTransactionMessage
	{
		private string _pinData;

		public FISBalanceInquiry( Dictionary<string, string> fisVars, DateTime txnDT, string transactionId, Terminal terminal, string track2, string cardNumber ) :
			this( fisVars, txnDT, transactionId, terminal, track2, "", cardNumber ) { }

		public FISBalanceInquiry( Dictionary<string, string> fisVars, DateTime txnDT, string transactionId, Terminal terminal, string track2, string PINData, string cardNumber ) :
			base( fisVars, 0, txnDT, transactionId, terminal, cardNumber )
		{
			_processingCode = "312000";

			_pinData = PINData;
			_track2 = track2;

			_de120 = "KIOSK BALANCE INQUIRY";
		}

		public override void Create()
		{
			base.Create();

			if (_pinData.Length>0)
				AddDE( 52, _pinData );
		}
	}
}
