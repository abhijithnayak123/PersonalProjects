using Spring.Context;
using Spring.Context.Support;

namespace MGI.Providers.MG.Monitor
{
	public class Program
	{
		static void Main(string[] args)
		{
			using (IApplicationContext ctx = ContextRegistry.GetContext())
			{
				Logger.WriteLog("============= MoneyGramMonitor Started ==============================================");
				MoneyGramMonitor MGMonitor = (MoneyGramMonitor)ctx.GetObject("MoneyGramMonitor");
				MGMonitor.PopulateMetaData();
				MGMonitor.PopulateBillers();
				Logger.WriteLog("============= MoneyGramMonitor Ended ================================================");
			}
		}
	}
}
