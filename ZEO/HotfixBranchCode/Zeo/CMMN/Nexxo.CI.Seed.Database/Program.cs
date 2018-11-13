using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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
            var DBUserName = System.Configuration.ConfigurationManager.AppSettings["DBUserName"].ToString();
            var DBUserPassword = System.Configuration.ConfigurationManager.AppSettings["DBUserPassword"].ToString();
            var DBPrefix = System.Configuration.ConfigurationManager.AppSettings["DBPrifix"].ToString();

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
            Console.WriteLine("Beginning Database Upgrade");

            foreach (var sqlFile in sqlFiles)
            {
                sql = EmbeddedResource.GetString(sqlFile);

                sql = sql.Contains("$DBPrifix$") ? sql.Replace("$DBPrifix$", DBPrefix) : sql;
                sql = sql.Contains("$DBUserName$") ? sql.Replace("$DBUserName$", DBUserName) : sql;
                sql = sql.Contains("$DBUserPassword$") ? sql.Replace("$DBUserPassword$", DBUserPassword) : sql;
                
                try
                {
                    Console.WriteLine("Executing " + sqlFile);
                    adHocSqlRunner.ExecuteNonQuery(sql);
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
