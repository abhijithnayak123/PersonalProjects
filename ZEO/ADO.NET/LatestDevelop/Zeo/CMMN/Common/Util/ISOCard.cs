using System;

namespace MGI.Common.Util
{
	public class ISOCard
	{
		protected ISOCard() { }

		// ISO 7813: track 1 & 2: %B4873901891836934^SHIELDS/MITCHELL DE^0609101000006544681 00094000000?;4873901891836934=06091016544681094?

		public static string EncodeCardNumber( long CardNumber )
		{
			if ( CardNumber == 0 )
				return new string( '0', 16 );
			string _sCardNumber = CardNumber.ToString();
			if (_sCardNumber.IsCreditCardNumber())
				return "*****" + _sCardNumber.Substring( _sCardNumber.Length - 4 ); // show last 4 digits
			return _sCardNumber;
		}

		public static string BankNameEncodeCardNumber( long CardNumber ) 
		{
			string bankName = IsVisaCard( CardNumber ) ? "VISA" : ( IsMasterCard( CardNumber ) ? "MC" : "" );
			return string.Format( "{0}*{1}", BankName( CardNumber ), CardNumber.ToString().Substring( CardNumber.ToString().Length - 4 ) ); 
		}

		public static long GetCardNumber( string CardData, bool RequireFlaggedCard )
		{
			string CardNumber = string.Empty;

			if ( CardData.Equals( ";;" ) || CardData.Equals( "E;E;" ) )
				throw new System.Exception( "Bad card read." );

			int nStart = CardData.IndexOf( "%B" );
			if ( nStart >= 0 ) // CardData is Track 1
			{
				nStart += 2; // skip '%B'
				CardNumber = CardData.Substring( nStart, CardData.IndexOf( '^' ) - 2 );
				if ( RequireFlaggedCard && !( CardNumber.Length == 15 || CardNumber.Length == 16 ) )
					throw new System.Exception( "Not an ISO 7813 format card." );
			}
			else if ( CardData.StartsWith( "B" ) )
			{
				nStart = 1; // skip 'B'
				CardNumber = CardData.Substring( nStart, CardData.IndexOf( '^' ) - 1 );
				if ( RequireFlaggedCard && !(CardNumber.Length == 15 || CardNumber.Length == 16) )
					throw new System.Exception( "Not an ISO 7813 format card." );
			}
			else // CardData may be Track 2
			{
				int nEnd;
				nStart = CardData.IndexOf( ";" );
				if ( nStart < 0 )
				{
					if ( RequireFlaggedCard && !(CardNumber.Length == 15 || CardNumber.Length == 16) )
						throw new System.Exception( "Not an ISO 7813 format card." );
					try
					{
						long.Parse( CardData );
						CardNumber = CardData;
					}
					catch
					{
						throw new System.Exception( "Not an ISO 7813 format card." );
					}
				}
				else
				{
					nStart += 1; // skip ';'
					nEnd = CardData.IndexOf( '=', nStart );
					if ( nEnd < 0 )
					{
						if ( RequireFlaggedCard )
							throw new System.Exception( "Not an ISO 7813 format card." );
						nEnd = CardData.IndexOf( '?', nStart );
						if ( nEnd < 0 )
						{
							if ( RequireFlaggedCard )
								throw new System.Exception( "Not an ISO 7813 format card." );
							// Uniteller card: "%506590^028?;;"
							if ( CardData[ 0 ] == '%' )
							{
								nStart = 1;
								CardData = CardData.Replace( "^", string.Empty ).Replace( " ", string.Empty );
								nEnd = CardData.IndexOf( '?', nStart );
								if ( nEnd < 0 )
									throw new System.Exception( "Not known card format." );
							}
							else if ( char.IsDigit( CardData[ 0 ] ) )
							{
								nStart = 0;
								CardData = CardData.Replace( "^", string.Empty ).Replace( " ", string.Empty );
								nEnd = CardData.IndexOf( '?', nStart );
								if ( nEnd < 0 )
									nEnd = CardData.IndexOf( ';', nStart );
								if ( nEnd < 0 )
									throw new System.Exception( "Not known card format." );
							}
						}
					}
					CardNumber = CardData.Substring( nStart, nEnd-nStart );
					if ( RequireFlaggedCard && !(CardNumber.Length == 15 || CardNumber.Length == 16) )
						throw new System.Exception( "Not an ISO 7813 format card." );
				}
			}

			long nCardNumber = long.Parse( CardNumber );

			if ( RequireFlaggedCard && !IsValidCardNumber( nCardNumber ) ) throw new System.Exception( "Not an ISO 7813 format card." );

			return nCardNumber;
		}

		public static long GetCardNumber( string CardData )
		{
			return GetCardNumber( CardData, true );
		}

		public static void GetCardInformation( string CardData, out long CardNumber, out string LastName, out string FirstName, out DateTime ExpDate, bool RequireFlaggedCard )
		// ISO 7813: track 1 & 2: %B4873901891836934^SHIELDS/MITCHELL DE^0609101000006544681 00094000000?;4873901891836934=06091016544681094?
		{
			CardNumber = 0;
			LastName = string.Empty;
			FirstName = string.Empty;
			ExpDate = DateTime.MinValue;

			// Parse Card Number
			CardNumber = GetCardNumber( CardData, RequireFlaggedCard );

			// Parse Last Name
			int nStart = CardData.IndexOf( '^' );
			int nEnd = CardData.IndexOf( '/' );
			if ( nStart < 0 || nEnd < nStart )
			{
				if ( RequireFlaggedCard )
					throw new System.Exception( "Not an ISO 7813 format card." );
			}
			else
				LastName = CardData.Substring( nStart + 1, nEnd - nStart - 1 ).Trim();

			// Parse First Name
			nStart = nEnd;
			nEnd = CardData.IndexOf( '^', nEnd + 1 );
			if ( nStart < 0 || nEnd < nStart )
			{
				if ( RequireFlaggedCard )
					throw new System.Exception( "Not an ISO 7813 format card." );
			}
			else
				FirstName = CardData.Substring( nStart + 1, nEnd - nStart - 1 ).Trim();

			try
			{
				// Parse Exp Year and Month
				nStart = nEnd;
				short ExpYear = short.Parse( CardData.Substring( nStart + 1, 2 ) );
				short ExpMonth = short.Parse( CardData.Substring( nStart + 3, 2 ) );

				// Convert 2 digit year to 4 digits
				ExpYear += 2000;

				ExpDate = new DateTime( ExpYear, ExpMonth, 1 );
			}
			catch
			{
				if ( RequireFlaggedCard )
					throw new System.Exception( "Not an ISO 7813 format card." );
			}
		}

		public static void GetCardInformation( string CardData, out long CardNumber, out string LastName, out string FirstName, out DateTime ExpDate )
		{
			GetCardInformation( CardData, out CardNumber, out LastName, out FirstName, out ExpDate, true );
		}

		public static bool IsVisaCard( long CardNumber )
		{
			// first digit is 4 for VISA
			string sCardNumber = CardNumber.ToString();
			if ( sCardNumber[ 0 ] == '4' )
				return true;
			return false;
		}

		public static bool IsMasterCard( long CardNumber )
		{
			// first 2 digits are between 51-55
			string sCardNumber = CardNumber.ToString();
			if ( sCardNumber[ 0 ] != '5' )
				return false;
			if ( !( sCardNumber[ 1 ] == '1' || sCardNumber[ 1 ] == '2' || sCardNumber[ 1 ] == '3' || sCardNumber[ 1 ] == '4' || sCardNumber[ 1 ] == '5' ) )
				return false;
			return true;
		}

		public static bool IsDebitCard( long CardNumber )
		{
			return (IsMasterCard( CardNumber ) || IsVisaCard( CardNumber ) || IsAmericanExpressCard( CardNumber ));
		}

		public static bool IsAmericanExpressCard( long CardNumber )
		{
			// first 2 digits are 34 or 37
			string sCardNumber = CardNumber.ToString();
			if ( sCardNumber.Length != 15 )
				return false;
			if ( sCardNumber[ 0 ] != '3' )
				return false;
			if ( !(sCardNumber[ 1 ] == '4' || sCardNumber[ 1 ] == '7' ) )
				return false;
			return true;
		}

		public static bool IsCentrisCard(long cardNumber)
		{
			const int CARD_LENGTH = 8;
			string sCardNumber = cardNumber.ToString();
			if (sCardNumber.Length != CARD_LENGTH)
				return false;

			if (int.Parse(sCardNumber.Substring(sCardNumber.Length - 1)) != CheckDigit(long.Parse(sCardNumber.Substring(0, sCardNumber.Length - 1))))
				return false;

			return true;
		}

		public static string BankName( long CardNumber )
		{
			if ( IsVisaCard( CardNumber ) )
				return "VISA®";

			if ( IsMasterCard( CardNumber ) )
				return "MasterCard®";

			if ( IsAmericanExpressCard( CardNumber ) )
				return "American Express®";

			return string.Empty;
		}

		public static int CheckDigit( long CardNumber )
		{
			int[ , ] table = new int[ 2, 10 ] { { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, { 0, 2, 4, 6, 8, 1, 3, 5, 7, 9 } };

			string sCardNumber = CardNumber.ToString();

			int size = sCardNumber.Length;
			int sum = 0;
			int odd = 0;

			for ( int i = size - 1; i >= 0; i-- )
				if ( Char.IsDigit( sCardNumber[ i ] ) )
					sum += table[ odd = 1 - odd, sCardNumber[ i ] - '0' ];

			sum %= 10;

			return ( sum != 0 ? 10 - sum : 0 );
		}

		public static bool IsValidCardNumber( long CardNumber )
		{
			string sCardNumber = CardNumber.ToString();

			if ( sCardNumber.Length == 15 )
				return true; // American Express Card

			if ( sCardNumber.Length != 16 )
				return false;

			if ( int.Parse( sCardNumber.Substring( 15, 1 ) ) != CheckDigit( long.Parse( sCardNumber.Substring( 0, 15 ) ) ) )
				return false;

			return true;
		}

	}
}
