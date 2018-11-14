using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Cxn.MoneyTransfer.WU.Data;
using MGI.Common.DataAccess.Contract;
using Spring.Context;
using Spring.Context.Support;
using Spring.Transaction.Interceptor;

namespace MGI.Providers.WU.DASMonitor
{
    public class DASMonitor
    {
        static void Main(string[] args)
        {
            using (IApplicationContext ctx = ContextRegistry.GetContext())
            {								
					IDASServices westernUnionIO = (IDASServices)ctx.GetObject("westernUnionIO");
					if (args.Length != 0 && args[0].ToLower().ToString()   ==  "hb")
					{
						try
						{
							//This is used to check the heart beat of WU. 
							westernUnionIO.CheckHeartBeat();
						}
						catch (Exception ex)
						{
							Logger.WriteLogHeartBeat("DASMonitor Exception -  HeartBeat testing:" + ex.Message);
						}
					}
					else
					{
						try
						{
							Logger.WriteLog("==============" + "DASMonitor Starting" + "===============================================");
							//This is used to get a countries name in Spanish
							westernUnionIO.PopulateCountryTransalations((Convert.ToInt64(DateTime.Now.ToString("yyyyMMddhhmmssff"))), "es");
							//This is used to get Deliveryservices in spanish
							westernUnionIO.PopulateDeliveryServiceTransalations((Convert.ToInt64(DateTime.Now.ToString("yyyyMMddhhmmssff"))), "es");
							//This Used to get a Countries ISO code.				
							westernUnionIO.PopulateWUCountries();
							////This is used to get the US & Mexico State codes.
							westernUnionIO.PopulateWUCountryStates();
							//This is used to get the Biller Name and WU Biller ID.
							westernUnionIO.PopulateWUQQCompanyNames();
							//This request returns the list of destination countries and currencies supported for those countries
							westernUnionIO.PopulateWUCountriesCurrencies();
							//////This is used to get the Error Code description from the actual WU error code.
							westernUnionIO.PopulateWUGetErrorMessagesInfo();
							//////This is used to get the mexico cities. 
							westernUnionIO.PopulateWUMexxicoCity();
							Logger.WriteLog("==============" + "DASMonitor Ending" + "===============================================");
						}
						catch (Exception ex)
						{
							Logger.WriteLog("DASMonitor Exception: " + ex.Message + " Inner Exception :" + ex.ToString());
						}
					}			
				}	
        }
    }
}

