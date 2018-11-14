using TCF.Zeo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.MoneyTransfer.WU.Data
{

    public class WUCountry : ZeoModel
    {
        public virtual string CountryCode { get; set; }
        public virtual string Name { get; set; }

    }

    public class WUState : ZeoModel
    {
        public virtual string StateCode { get; set; }
        public virtual string Name { get; set; }
        public virtual string ISOCountryCode { get; set; }


        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var wuStateobj = obj as WUState;

            if (wuStateobj == null)
                return false;

            if (StateCode.ToLower() == wuStateobj.StateCode.ToLower() && ISOCountryCode.ToLower() == wuStateobj.ISOCountryCode.ToLower())
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return (StateCode + "|" + ISOCountryCode).GetHashCode();
        }
    }

    public class WUCity : ZeoModel
    {
        public virtual string Name { get; set; }
        public virtual string StateCode { get; set; }
    }

    public class WUCountryCurrencyDeliveryMethod : ZeoModel
    {
        public virtual string CountryCode { get; set; }
        public virtual string CurrencyCode { get; set; }
        public virtual string SvcCode { get; set; }
        public virtual string SvcName { get; set; }
        public virtual string Route { get; set; }
        public virtual string Banner { get; set; }
        public virtual string Description { get; set; }
        public virtual string Templt { get; set; }
        public virtual string CountryViewFilter { get; set; }
        public virtual string ExclFlags { get; set; }
        public virtual System.Nullable<decimal> SourceMinCurrency { get; set; }
        public virtual System.Nullable<decimal> SourceMaxCurrency { get; set; }
        public virtual System.Nullable<decimal> SourceCurrencyIncr { get; set; }
        public virtual System.Nullable<decimal> DestinationMinCurrency { get; set; }
        public virtual System.Nullable<decimal> DestinationMaxCurrency { get; set; }
        public virtual System.Nullable<decimal> DestinationCurrencyIncr { get; set; }
    }

    public class WUDeliveryOption : ZeoModel
    {
        public virtual string Product { get; set; }
        public virtual string Category { get; set; }
        public virtual string T_Index { get; set; }
        public virtual string Description { get; set; }
    }

    public class WUPaymentMethod : ZeoModel
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }

    public class WUPickupMethod : ZeoModel
    {
        public virtual string Name { get; set; }
        public virtual string DescriptionEN { get; set; }
        public virtual string DescriptionES { get; set; }
        public virtual string CurrencyConversionDescriptionEN { get; set; }
        public virtual string CurrencyConversionDescriptionES { get; set; }
    }

    public class WUPickupDetail : ZeoModel   // Need clarification - R we using this Class?
    {
        //public virtual WUReceiver WUnionRecipient { get; set; }
        public virtual string PickupPoPName { get; set; }
        public virtual string PickupPoPRegion { get; set; }
        public virtual string PickupPoPBranch { get; set; }
        public virtual string PickupPoPBranchOperator { get; set; }
        public virtual string PickupPoPReferenceNumber { get; set; }
        public virtual string BeneficiaryGovernmentIDType { get; set; }
        public virtual string BeneficiaryGovernmentIDNumber { get; set; }
        public virtual string BeneficiaryGovernmentIDIssuer { get; set; }
        public virtual string BeneficiaryGovernmentIDIssuerState { get; set; }
        public virtual string BeneficiaryGovernmentIDIssuerCountry { get; set; }
        public virtual System.Nullable<System.DateTime> BeneficiaryGovernmentIDExpirationDate { get; set; }
        public virtual string BeneficiaryName { get; set; }
    }

    public class WUQQCcompanyNames : ZeoModel
    {
        //public virtual string ClientId { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string Country { get; set; }
        public virtual string CurrencyCode { get; set; }
        public virtual string ISOCountryCode { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual long ChannelPartnerId { get; set; }
    }

    public class WUErrorMessages : ZeoModel
    {
        public virtual string ErrorCode { get; set; }
        public virtual string ErrorDesc { get; set; }
    }


    public class CountryTransalation : ZeoModel
    {
        public virtual string CountryCode { get; set; }
        public virtual string Language { get; set; }
        public virtual string Name { get; set; }
    }
    public class DeliveryServiceTransalation : ZeoModel
    {
        public virtual string EnglishName { get; set; }
        public virtual string Name { get; set; }
        public virtual string Language { get; set; }
    }
}
