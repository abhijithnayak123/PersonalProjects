using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.WU.DASMonitor
{
    public interface IDASService
    {
        void PopulateCountryTransalations();
        void PopulateDeliveryServiceTransalations();
        void PopulateWUCountries();
        void PopulateWUCountryStates();
        void PopulateWUQQCompanyNames();
        void PopulateWUCountriesCurrencies();
        void PopulateWUErrorMessagesInfo();
        void PopulateWUMexxicoCities();
        void CheckHeartBeat();
    }
}
