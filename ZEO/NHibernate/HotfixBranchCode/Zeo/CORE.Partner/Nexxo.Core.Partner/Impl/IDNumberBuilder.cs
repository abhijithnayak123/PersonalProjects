using MGI.Common.DataAccess.Contract;
using MGI.Common.Util;
using System;

namespace MGI.Core.Partner.Impl
{
	public class IDNumberBuilder : IIDNumberBuilder
	{
		private IIDNumberGenerator _idGenerator;
		public IIDNumberGenerator IDGenerator { set { _idGenerator = value; } }

		public bool IsRealPAN { get; set; }

		public IDNumberBuilder() { }

		public long NextPAN()
		{
			if (IsRealPAN)
			{
				return GenerateNewId("PAN", 100000000000000);
			}
			return GenerateNewId();
		}

		public long GenerateNewId( string sequenceName, long first15 )
		{
			first15 += _idGenerator.NextIDNumber(sequenceName);
			long newId = (10 * first15); //+ ISOCard.CheckDigit( first15 );
			return newId;
		}

		public long GenerateNewId()
		{ 		
			string r = "2";
			RandomCryptoServiceProvider randomCryptoServiceProvider = new RandomCryptoServiceProvider();
			for (int i = 0; i < 15; i++)
			{
				r += randomCryptoServiceProvider.Next(0, 9).ToString();
			}
			long newId = Convert.ToInt64(r);
			return newId;
		}
	}
}