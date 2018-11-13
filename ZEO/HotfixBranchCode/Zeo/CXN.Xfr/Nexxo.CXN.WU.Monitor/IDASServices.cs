using System;

namespace MGI.Providers.WU.DASMonitor
{

    public interface IDASServices
    {
		//string AccountIdentifier { get; set; }
		//string CounterId { get; set; }
		void PopulateCountryTransalations(long transactionId,string language);
		void PopulateDeliveryServiceTransalations(long transactionId,string language);
        void PopulateWUCountries();
        void PopulateWUCountryStates();
		void PopulateWUQQCompanyNames();
		void PopulateWUCountriesCurrencies();
		void PopulateWUGetErrorMessagesInfo();
		//void PopulateWUDestinationCountries();
		void PopulateWUMexxicoCity();
		void CheckHeartBeat();
	}

}
