using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;

using MGI.Common.DataAccess.Contract;

using Spring.Data.Generic;
using Spring.Data.Common;

namespace MGI.Common.DataAccess.Impl
{
	public class Nexxo3AlertRepository : IAlertRepository
	{
		private DbProvider _nexxo3DBProvider;
		public DbProvider Nexxo3DBProvider { set { _nexxo3DBProvider = value; } }

		public object Add( string subject, string message, string recipientAddress )
		{
			AdoTemplate ado = new AdoTemplate( _nexxo3DBProvider );

			string sql = string.Format( @"INSERT tDataDogAlerts([Subject],[Message],[AlertRecipient])
										 VALUES('{0}','{1}','{2}')", subject, message, recipientAddress );

			return ado.ExecuteNonQuery( CommandType.Text, sql );
		}
	}
}
