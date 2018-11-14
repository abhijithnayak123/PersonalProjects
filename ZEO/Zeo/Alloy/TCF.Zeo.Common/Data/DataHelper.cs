using P3Net.Data.Common;
using System.Configuration;
using P3Net.Data.Sql;

namespace TCF.Zeo.Common.Data
{
    public static class DataHelper
    {
        public static ConnectionManager GetConnectionManager(string connectionStringKey = null)
        {
            string connectionString = string.Empty;

            if (string.IsNullOrWhiteSpace(connectionStringKey))
                { connectionString = getConnectionString("ZeoConnectionString"); }
            else
                { connectionString = getConnectionString(connectionStringKey); }

            return new SqlConnectionManager(connectionString);
        }

        private static string getConnectionString(string appSettingsKey)
        {
            return ConfigurationManager.ConnectionStrings[appSettingsKey].ConnectionString;
            
        }
    }
}
