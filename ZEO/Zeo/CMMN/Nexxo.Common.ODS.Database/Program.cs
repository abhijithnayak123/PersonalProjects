using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using DbUp;

namespace Nexxo.Cxn.Check.Database
{
	class Program
	{
		static int Main(string[] args)
		{
			var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ZeoConnectionString"].ConnectionString;

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
