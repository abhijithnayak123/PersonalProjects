using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MGI.Cxn.Fund.FIS.ISO8583
{
	internal class  BaseFISISOMessage
	{
		public int MessageType { get; protected set; }
        protected List<DataElement> _msg;
		protected string _cardIssAuth = "0L10MET10100P"; // prod value? "0GT2PRO1";
		protected string _instID = "1840109200"; //prod value "00840109200";
		protected string _processingCode = "";
		protected decimal _amount;
		protected DateTime _txnDT;
		protected string _transactionId;
		protected Terminal _terminal;
		protected string _cardNumber;
		protected string _track2 = "";
		protected string _de41 = "";
		protected string _de60 = "NEXONEXO 000";
		protected string _de120 = "";
		protected int _de120_length = 33;
		protected string _captureDate = "0000";
		protected string _acqInstID = "1840109200";
		protected string _cardAcceptorName = "";
		private Dictionary<string,string> _fisVars;

		public BaseFISISOMessage() { }

		public BaseFISISOMessage( Dictionary<string,string> fisVars, decimal amount, DateTime TxnDT, string transactionId, Terminal terminal, string cardNumber )
		{
			// use 0x200 as default
			MessageType = MsgType._0200_TRAN_REQ;

			_msg = new List<DataElement>();
			_amount = amount;
			_txnDT = TxnDT;
			_transactionId = transactionId;
            _terminal = new Terminal() { Id = "123456", City = "Burlingame", State = "CA" };
			_cardNumber = cardNumber;
			_fisVars = fisVars;

			TryToSetStaticVar( "cardIssAuth", ref _cardIssAuth );
			TryToSetStaticVar( "instID", ref _instID );
			TryToSetStaticVar( "acqInstID", ref _acqInstID );
			TryToSetStaticVar( "de60", ref _de60 );
		}

		public virtual void Create()
		{
			if ( _cardNumber.Length > 0 )
				AddDE( 2, _cardNumber );
			AddDE( 3, _processingCode );
			AddDE( 4, ( (int)(_amount * 100) ).ToString().PadLeft( 12, '0' ) );
			AddDE( 7, DateTime.Now.ToString( "MMddHHmmss" ) );
			AddDE( 12, _txnDT.ToString( "HHmmss" ) );
			AddDE( 13, _txnDT.ToString( "MMdd" ) );
			AddDE( 17, _captureDate );
			AddDE( 18, "6012" );
			AddDE( 32, _acqInstID );
			if (_track2.Length>0)
				AddDE( 35, GetFISISOTrack2( _track2 ) );
			AddDE( 37, _transactionId );
			AddDE( 41, (string.IsNullOrEmpty(_de41) ? "NEXO" + _terminal.Id.Substring(_terminal.Id.Length-4) : _de41).PadRight( 16 ) );
			string DE43 = string.Format( "{0}{1}{2}US", "Nexxo Kiosk".PadRight( 22 ), TruncateOrPadRight( 13, _terminal.City ), _terminal.State.PadRight( 3 ) );
			AddDE( 43, string.IsNullOrEmpty(_cardAcceptorName)?DE43:_cardAcceptorName );
			AddDE( 49, "840" );
			AddDE( 60, _de60 );
			AddDE( 61, _cardIssAuth );
			AddDE( 120, TruncateOrPadRight( _de120_length, _de120 ) );
		}

		protected string GetFISISOTrack2( string t2 )
		{
			Regex matcher = new Regex( @".*;(\d{1,19}[=D]([=D]|\d{4})[=D]?\d*).*" );
			Match m = matcher.Match( t2 );
			return m.Groups[1].Value;
		}

		protected string TruncateOrPadRight( int len, string s )
		{
			return s.Length > len ? s.Substring( 0, len ) : s.PadRight( len );
		}

		protected void AddDE( int id, string v )
		{
			_msg.Add( new DataElement( id, v ) );
		}

		private void TryToSetStaticVar( string varName, ref string var )
		{
			try
			{
				var = _fisVars[varName];
			}
			catch { }
		}

		public DataElement[] ToArray()
		{
			return _msg.ToArray();
		}

		public override string ToString()
		{
			return string.Join<DataElement>(";",_msg.ToArray());
		}
	}

	internal enum ANSIStateCodes
	{
		AL = 1,
		AK = 2,
		AZ = 4,
		AR = 5,
		CA = 6,
		CO = 8,
		CT = 9,
		DE = 10,
		DC = 11,
		FL = 12,
		GA = 13,
		HI = 15,
		ID = 16,
		IL = 17,
		IN = 18,
		IA = 19,
		KS = 20,
		KY = 21,
		LA = 22,
		ME = 23,
		MD = 24,
		MA = 25,
		MI = 26,
		MN = 27,
		MS = 28,
		MO = 29,
		MT = 30,
		NE = 31,
		NV = 32,
		NH = 33,
		NJ = 34,
		NM = 35,
		NY = 36,
		NC = 37,
		ND = 38,
		OH = 39,
		OK = 40,
		OR = 41,
		PA = 42,
		RI = 44,
		SC = 45,
		SD = 46,
		TN = 47,
		TX = 48,
		UT = 49,
		VT = 50,
		VA = 51,
		WA = 53,
		WV = 54,
		WI = 55,
		WY = 56
	}

	internal class MsgType
	{
		/// <summary>
		///   Auth Request
		/// </summary>
		public const int _0100_AUTH_REQ = 0x100;

		/// <summary>
		///   Auth Response
		/// </summary>
		public const int _0110_AUTH_REQ_RSP = 0x110;

		/// <summary>
		///   Auth Advice
		/// </summary>
		public const int _0120_AUTH_ADV = 0x120;

		/// <summary>
		///   Auth Advice Response
		/// </summary>
		public const int _0130_AUTH_ADV_RSP = 0x130;

		/// <summary>
		///   Transaction Request
		/// </summary>
		public const int _0200_TRAN_REQ = 0x200;

		/// <summary>
		///   Transaction Request Repeat
		/// </summary>
		public const int _0201_TRAN_REQ_REP = 0x201;

		/// <summary>
		///   Transaction Completion
		/// </summary>
		public const int _0202_TRAN_CMP = 0x202;

		/// <summary>
		///   Transaction Completion Repeat
		/// </summary>
		public const int _0203_TRAN_CMP_REP = 0x203;

		/// <summary>
		///   Transaction Response
		/// </summary>
		public const int _0210_TRAN_REQ_RSP = 0x210;

		/// <summary>
		///   Transaction Completion Response
		/// </summary>
		public const int _0212_TRAN_CMP_RSP = 0x212;

		/// <summary>
		///   Transaction Advice
		/// </summary>
		public const int _0220_TRAN_ADV = 0x220;

		/// <summary>
		///   Transaction Advice Repeat
		/// </summary>
		public const int _0221_TRAN_ADV_REP = 0x221;

		/// <summary>
		///   Transaction Advice Response
		/// </summary>
		public const int _0230_TRAN_ADV_RSP = 0x230;

		/// <summary>
		///   Acquirer file update request
		/// </summary>
		public const int _0300_ACQUIRER_FILE_UPDATE_REQ = 0x300;

		/// <summary>
		///   Acquirer file update response
		/// </summary>
		public const int _0310_ACQUIRER_FILE_UPDATE_RSP = 0x310;

		/// <summary>
		///   Acquirer File Update Advice
		/// </summary>
		public const int _0320_ACQUIRER_FILE_UPDATE_ADV = 0x320;

		/// <summary>
		///   Issuer File Update Advice
		/// </summary>
		public const int _0322_ISSUER_FILE_UPDATE_ADV = 0x322;

		/// <summary>
		///   Acquirer File Update Advice Response
		/// </summary>
		public const int _0330_ACQUIRER_FILE_UPDATE_ADV_RSP = 0x330;

		/// <summary>
		///   Issuer File Update Advice Response
		/// </summary>
		public const int _0332_ISSUER_FILE_UPDATE_ADV_RSP = 0x332;

		/// <summary>
		///   Acquirer Reversal Request
		/// </summary>
		public const int _0400_ACQUIRER_REV_REQ = 0x400;

		/// <summary>
		///   Acquirer Reversal Request Response
		/// </summary>
		public const int _0410_ACQUIRER_REV_REQ_RSP = 0x410;

		/// <summary>
		///   Acquirer Reversal Advice
		/// </summary>
		public const int _0420_ACQUIRER_REV_ADV = 0x420;

		/// <summary>
		///   Acquirer Reversal Advice Repeat
		/// </summary>
		public const int _0421_ACQUIRER_REV_ADV_REP = 0x421;

		/// <summary>
		///   Acquirer Reversal Advice Response
		/// </summary>
		public const int _0430_ACQUIRER_REV_ADV_RSP = 0x430;

		/// <summary>
		///   Acquirer Recon Request
		/// </summary>
		public const int _0500_ACQUIRER_RECONCILE_REQ = 0x500;

		/// <summary>
		///   Acquirer Recon Request Response
		/// </summary>
		public const int _0510_ACQUIRER_RECONCILE_REQ_RSP = 0x510;

		/// <summary>
		///   Acquirer Recon Advice
		/// </summary>
		public const int _0520_ACQUIRER_RECONCILE_ADV = 0x520;

		/// <summary>
		///   Acquirer Recon Advice Repeat
		/// </summary>
		public const int _0521_ACQUIRER_RECONCILE_ADV_REP = 0x521;

		/// <summary>
		///   Acquirer Recon Advice Response
		/// </summary>
		public const int _0530_ACQUIRER_RECONCILE_ADV_RSP = 0x530;

		/// <summary>
		///   Administrative Request
		/// </summary>
		public const int _0600_ADMIN_REQ = 0x600;

		/// <summary>
		///   Administrative Request
		/// </summary>
		public const int _0601_ADMIN_REQ_REP = 0x601;

		/// <summary>
		///   Administrative Request Response
		/// </summary>
		public const int _0610_ADMIN_REQ_RSP = 0x610;

		/// <summary>
		///   Network Management Request
		/// </summary>
		public const int _0800_NWRK_MNG_REQ = 0x800;

		/// <summary>
		///   Network Management Request Repeat
		/// </summary>
		public const int _0801_NWRK_MNG_REQ_REP = 0x801;

		/// <summary>
		///   Network Management Response
		/// </summary>
		public const int _0810_NWRK_MNG_REQ_RSP = 0x810;


		/// <summary>
		///   Gets the response message type for the given message type. E.g. 0220 -> 0230, 0421 -> 0430
		/// </summary>
		/// <param name = "msgType">Request Message Type</param>
		/// <returns>Response Message Type</returns>
		public static int GetResponse( int msgType )
		{
			return msgType - ( msgType % 2 ) + 0x10;
		}
	}

}
