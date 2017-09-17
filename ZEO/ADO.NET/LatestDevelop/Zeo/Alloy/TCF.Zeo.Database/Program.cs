using DbUp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Database
{
    class Program
    {
        static int Main(string[] args)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ZeoConnectionString"].ConnectionString;
            //var ptnrDatabaseName = ConfigurationManager.AppSettings["PTNRDATABASE"].ToString();
            //var commandTimeout = ConfigurationManager.AppSettings["CommandTimeOut"].ToString();

            //Dictionary<string, string> dbname = new Dictionary<string, string>();
            //dbname.Add("PTNRDATABASE", ptnrDatabaseName);

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    //.WithVariables(dbname)
                    .LogToConsole()
                    //.WithExecutionTimeout(TimeSpan.FromSeconds(Convert.ToDouble(commandTimeout)))
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
                Console.ReadLine();
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();

            return 0;
        }
    }
}
