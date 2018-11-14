using System;
using DbUp;
using System.Reflection;

namespace Nexxo.CXN.BP.NYCHA.Database
{
    class Program
    {
        static int Main(string[] args)
        {
            var connectionString = 
                System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
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
