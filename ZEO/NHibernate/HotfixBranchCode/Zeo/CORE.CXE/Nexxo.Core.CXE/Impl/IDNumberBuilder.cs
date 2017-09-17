using MGI.Common.DataAccess.Contract;

namespace MGI.Core.CXE.Impl
{
	public class IDNumberBuilder : IIDNumberBuilder
	{
		private IIDNumberGenerator _idGenerator;
		public IIDNumberGenerator IDGenerator { set { _idGenerator = value; } }

		public IDNumberBuilder() { }

		public long NextPAN()
		{
			return GenerateNewId( "PAN", 100000000000000 );
		}

		public long GenerateNewId( string sequenceName, long first15 )
		{
			first15 += _idGenerator.NextIDNumber( sequenceName );
            long newId = (10 * first15); //+ ISOCard.CheckDigit( first15 );
			return newId;
		}
	}
}