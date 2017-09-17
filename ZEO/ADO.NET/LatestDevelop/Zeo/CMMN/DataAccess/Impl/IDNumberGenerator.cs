using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;

using MGI.Common.DataAccess.Contract;

using Spring.Data.Generic;
using Spring.Data.Common;

namespace MGI.Common.DataAccess.Impl
{
	public class IDNumberGenerator : IIDNumberGenerator
	{
		private DbProvider _dbProvider;
		public DbProvider DBProvider { set { _dbProvider = value; } }

		public long NextIDNumber( string IDType )
		{
			AdoTemplate adoTemplate = new AdoTemplate( _dbProvider );

			string sql = "GetNextSequenceNumber";

			IDbParameters dbParams = adoTemplate.CreateDbParameters();
			dbParams.AddWithValue( "sequenceName", IDType );
			dbParams.AddOut( "sequenceNumber", SqlDbType.BigInt );

			int numRows = adoTemplate.ExecuteNonQuery( CommandType.StoredProcedure, sql, dbParams );

			if ( numRows == 1 )
				return (long)dbParams["@sequenceNumber"].Value;
			else
				throw new Exception( "db failure!" );
		}
	}
}
