using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.MoneyTransfer.WU.Data
{
    public enum DASServices
    {
        GetDeliveryService,
		GetDeliveryTranslations,
        GetDestinationCountries,
        GetDestinationCurrencies,
        GetErrorMessagesInfo,
        GetISOCountries,
        GetISOCurrencies,
        GetMexicoCityState,
        GetQQCCompanyName,
        GetUSStateList,
        GetUniversalComplianceTemplate,
        GetCountriesCurrencies
    }

    public enum TransferType : int
    {
        sendMoney = 1,
        receiveMoney
    }

    public enum MTReleaseStatus : int
    {
         Hold = 1,
         Release = 2,
         Cancel = 3
    }

    public enum SendMoneyTransactionSubType : int
    {
        Cancel = 1,
        Modify =2,
        Refund=3
    }

}
