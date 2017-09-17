using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nexxo;

namespace MGI.Cxn.Fund.FIS.ISO8583
{
	public enum FISTransactionType
	{
		ATM = 1,
		POS = 2
	}

	internal class FISISOResponseMessage : BaseFISISOMessage
	{
		public FISISOResponseMessage( string msg )
		{
			_msg = new List<DataElement>();
			string[] dataElements = msg.Split( ';' );
			foreach ( string de in dataElements )
				_msg.Add( new DataElement( de ) );
		}

		public bool Success
		{
			get { return DEValue(39) == "00"; }
		}

		public string ResponseCode
		{
			get { return DEValue( 39 ); }
		}

		public string AuthIDResponse
		{
			get { return DEValue( 38 ); }
		}

		public string CaptureDate
		{
			get { return DEValue( 17 ); }
		}

		public bool HasBalance
		{
			get { return DEValue( 44 ).Length > 0; }
		}

		public FISTransactionType TransactionType
		{
			get { return DEValue( 22 ).Length > 0 ? FISTransactionType.POS : FISTransactionType.ATM; }
		}

		public decimal Balance
		{
			get 
			{
				string de44 = DEValue( 44 );
				if ( de44.Length > 0 )
				{
					int index = 1;
					if ( de44.StartsWith( "2" ) )
						index = 13;
					return decimal.Parse( de44.Substring( index,12 ) ) / 100;
				}
				return decimal.Zero;
			}
		}

		public string DEValue( int id )
		{
			DataElement de = _msg.Find( d => d.id == id );
			if ( de != null )
				return de.value;
			else
				return "";
		}

		public bool LimitVerified
		{
			get { return DEValue( 126 ).ToUpper().Contains( "000VALID" ); }
		}
	}
}
