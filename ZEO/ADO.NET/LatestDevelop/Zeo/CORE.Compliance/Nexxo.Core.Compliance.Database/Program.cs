using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbUp;
using System.Reflection;

namespace Nexxo.Core.Partner.Nexxo.Core.Compliance.Database.DeployApp
{
	class Program
	{
		static int Main( string[] args )
		{
			var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;

			var upgrader =
				DeployChanges.To
					.SqlDatabase( connectionString )
					.WithScriptsEmbeddedInAssembly( Assembly.GetExecutingAssembly() )
					.LogToConsole()
					.Build();

			var result = upgrader.PerformUpgrade();

			if ( !result.Successful )
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine( result.Error );
				Console.ResetColor();
				Console.ReadLine();
				return -1;
			}

			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine( "Success!" );
			Console.ResetColor();
			return 0;
		}
	}
}