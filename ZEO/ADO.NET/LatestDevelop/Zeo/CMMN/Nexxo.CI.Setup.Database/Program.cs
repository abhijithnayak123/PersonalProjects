using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DbUp;
using System.Reflection;
using DbUp.Engine.Output;
using DbUp.Helpers;

namespace Nexxo.CI.Setup.Database
{
    class Program
    {
        static int Main(string[] args)
        {
            var connectionString =
                System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;
            var workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var remotePath = System.Configuration.ConfigurationManager.AppSettings["DBBackupPath"].ToString();

            if (!string.IsNullOrWhiteSpace(remotePath) && remotePath != "0")
                { workingDirectory = remotePath; }

            AdHocSqlRunner adHocSqlRunner=new AdHocSqlRunner(() => new SqlConnection(connectionString), "dbo");

            Assembly _assembly;
            _assembly = Assembly.GetExecutingAssembly();
            List<string> filenames = new List<string>();
            filenames = _assembly.GetManifestResourceNames().ToList<string>();
            List<string> sqlFiles = new List<string>();
            for (int i = 0; i < filenames.Count(); i++)
            {
                string[] items = filenames.ToArray();
                if (items[i].ToString().EndsWith(".sql"))
                {
                    sqlFiles.Add(items[i].ToString());
                }
            }
            sqlFiles.Sort();
            string sql = string.Empty;
            Console.WriteLine("Beginning Database Upgrade..");

            foreach (var sqlFile in sqlFiles)
            {
                sql = EmbeddedResource.GetString(sqlFile);
                foreach (var key in System.Configuration.ConfigurationManager.AppSettings)
                {
                    sql = sql.Contains("$" + key + "$") ? sql.Replace("$" + key + "$", System.Configuration.ConfigurationManager.AppSettings[""+key+""]) : sql;   
                }

                sql = sql.Contains("$WorkingDirectory$") ? sql.Replace("$WorkingDirectory$", workingDirectory) : sql;
                
                try
                {
                    Console.WriteLine("Executing " + sqlFile);
                    var result = adHocSqlRunner.ExecuteNonQuery(sql);
                    Console.WriteLine(Convert.ToString((result < 0 ? 0 : result)) + " records modified");
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Trace: " + ex.StackTrace);
                    Console.ResetColor();
                    return -1;
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Upgrade successful!");
            Console.ResetColor();
            return 0;
        }
    }
}
