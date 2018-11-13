using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbUp;
using System.Reflection;

namespace Nexxo.Cxn.Database
{
    class Program
    {
        static int Main(string[] args)
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;
			var ptnrDatabaseName = System.Configuration.ConfigurationManager.AppSettings["PTNRDATABASE"] ?? String.Empty;

			Dictionary<string, string> dbname = new Dictionary<string, string>();
			dbname.Add("PTNRDATABASE", ptnrDatabaseName);

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
					.WithVariables(dbname)
					.LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }
    }
}
