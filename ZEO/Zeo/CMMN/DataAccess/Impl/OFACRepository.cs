using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using MGI.Common.Util;


using MGI.Common.DataAccess.Contract;

using Spring.Data.Generic;
using Spring.Data.Common;

namespace MGI.Common.DataAccess.Impl
{
	public class OFACRepository : IOFACRepository
	{
		private DbProvider _bridgerDBProvider;
		public DbProvider BridgerDBProvider { set { _bridgerDBProvider = value; } }   
        private DbProvider _nexxo3DBProvider;
		public DbProvider Nexxo3DBProvider { set { _nexxo3DBProvider = value; } }
        public NLoggerCommon NLogger = new NLoggerCommon();        		
	}

	public class DictionaryRowMapper<T> : IRowMapper<T> where T : Dictionary<string, object>, new()
	{
		public T MapRow( IDataReader reader, int rowNum )
		{
			T dict = new T();
			for ( var i = 0; i < reader.FieldCount; i++ )
			{
				dict.Add( reader.GetName( i ), reader.GetValue( i ) );
			}
			return dict;
		}
	}
}
