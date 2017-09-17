using System;

namespace TCF.Zeo.WU.DASMonitor
{
    class DASMonitor
    {
        static void Main(string[] args)
        {
            IDASService westernUnionIO = new DASServiceImpl();

            if (args.Length != 0 && args[0].ToLower().ToString() == "hb")
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

                    //This Used to get a Countries ISO code.				
                    westernUnionIO.PopulateWUCountries();
                    //This is used to get the US &Mexico State codes.
                    westernUnionIO.PopulateWUCountryStates();
                    //This is used to get the mexico cities.
                    westernUnionIO.PopulateWUMexxicoCities();
                    //This request returns the list of destination countries and currencies supported for those countries
                    westernUnionIO.PopulateWUCountriesCurrencies();
                    //This is used to get the Error Code description from the actual WU error code.
                    westernUnionIO.PopulateWUErrorMessagesInfo();
                    //This is used to get a countries name in Spanish
                    westernUnionIO.PopulateCountryTransalations();
                    //This is used to get Deliveryservices in spanish
                    westernUnionIO.PopulateDeliveryServiceTransalations();
                    //This is used to get the Biller Name and WU Biller ID.
                    westernUnionIO.PopulateWUQQCompanyNames();

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
