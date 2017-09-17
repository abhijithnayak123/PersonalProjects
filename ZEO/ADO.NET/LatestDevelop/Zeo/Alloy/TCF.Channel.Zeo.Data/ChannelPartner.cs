using TCF.Zeo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class ChannelPartner
    {
        [DataMember]
        public string PhoneNumber;
        [DataMember]
        public string AuthenticationType;
        [DataMember]
        public Guid rowguid { get; set; }
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public bool FeesFollowCustomer { get; set; }
        [DataMember]
        public string CashFeeDescriptionEN { get; set; }
        [DataMember]
        public string CashFeeDescriptionES { get; set; }
        [DataMember]
        public string DebitFeeDescriptionEN { get; set; }
        [DataMember]
        public string DebitFeeDescriptionES { get; set; }
        [DataMember]
        public decimal ConvenienceFeeCash { get; set; }
        [DataMember]
        public decimal ConvenienceFeeDebit { get; set; }
        [DataMember]
        public string ConvenienceFeeDescriptionEN { get; set; }
        [DataMember]
        public string ConvenienceFeeDescriptionES { get; set; }
        [DataMember]
        public bool CanCashCheckWOGovtId { get; set; }
        [DataMember]
        public string LogoFileName { get; set; }
        [DataMember]
        public bool IsEFSPartner { get; set; }
        [DataMember]
        public int EFSClientId { get; set; }
        [DataMember]
        public bool UsePINForNonGPR { get; set; }
        [DataMember]
        public bool IsCUPartner { get; set; }
        [DataMember]
        public bool HasNonGPRCard { get; set; }
        [DataMember]
        public bool ManagesCash { get; set; }
        [DataMember]
        public bool AllowPhoneNumberAuthentication { get; set; }
        [DataMember]
        public short TIM { get; set; }
        [DataMember]
        public bool DisableWithdrawCNP { get; set; }
        [DataMember]
        public bool CashOverCounter { get; set; }
        //US1421 Changes
        [DataMember]
        public string FrankData { get; set; }
        [DataMember]
        public bool IsCheckFrank { get; set; }
        [DataMember]
        public List<ChannelPartnerProductProvider> Providers { get; set; }
        [DataMember]
        public bool IsNotesEnable { get; set; }
        [DataMember]
        public bool IsMGIAlloyLogoEnable { get; set; }

        //Developed by: Sunil Shetty || Date: 09\06\2015
        //AL-533 : Mailing Address will not be visible in Customer registration on the base of IsMailingAddress value
        [DataMember]
        public bool IsMailingAddressEnable { get; set; }
        //AL-586
        [DataMember]
        public virtual bool CanEnableProfileStatus { get; set; }

        [DataMember]
        public int CustomerMinimumAge { get; set; }

        [DataMember]
        public string MasterSSN { get; set; }

        [DataMember]
        public bool IsReferralSectionEnable { get; set; }

        [DataMember]
        public string ComplianceProgramName { get; set; }

        [DataMember]
        public int CardPresenceVerificationConfig { get; set; }

        override public string ToString()
        {
            string str = string.Empty;
            str = string.Concat(str, "rowguid = ", rowguid, "\r\n");
            str = string.Concat(str, "Id = ", Id, "\r\n");
            str = string.Concat(str, "Name = ", Name, "\r\n");
            str = string.Concat(str, "FeesFollowCustomer = ", FeesFollowCustomer, "\r\n");
            str = string.Concat(str, "CashFeeDescriptionEN = ", CashFeeDescriptionEN, "\r\n");
            str = string.Concat(str, "CashFeeDescriptionES = ", CashFeeDescriptionES, "\r\n");
            str = string.Concat(str, "DebitFeeDescriptionEN = ", DebitFeeDescriptionEN, "\r\n");
            str = string.Concat(str, "DebitFeeDescriptionES = ", DebitFeeDescriptionES, "\r\n");
            str = string.Concat(str, "ConvenienceFeeCash = ", ConvenienceFeeCash, "\r\n");
            str = string.Concat(str, "ConvenienceFeeDebit = ", ConvenienceFeeDebit, "\r\n");
            str = string.Concat(str, "ConvenienceFeeDescriptionEN = ", ConvenienceFeeDescriptionEN, "\r\n");
            str = string.Concat(str, "ConvenienceFeeDescriptionES = ", ConvenienceFeeDescriptionES, "\r\n");
            str = string.Concat(str, "CanCashCheckWOGovtId = ", CanCashCheckWOGovtId, "\r\n");
            str = string.Concat(str, "LogoFileName = ", LogoFileName, "\r\n");
            str = string.Concat(str, "IsEFSPartner = ", IsEFSPartner, "\r\n");
            str = string.Concat(str, "EFSClientId = ", EFSClientId, "\r\n");
            str = string.Concat(str, "UsePINForNonGPR = ", UsePINForNonGPR, "\r\n");
            str = string.Concat(str, "IsCUPartner = ", IsCUPartner, "\r\n");
            str = string.Concat(str, "HasNonGPRCard = ", HasNonGPRCard, "\r\n");
            str = string.Concat(str, "ManagesCash = ", ManagesCash, "\r\n");
            str = string.Concat(str, "AllowPhoneNumberAuthentication = ", AllowPhoneNumberAuthentication, "\r\n");
            str = string.Concat(str, "TIM = ", TIM, "\r\n");
            str = string.Concat(str, "DisableWithdrawCNP = ", DisableWithdrawCNP, "\r\n");
            str = string.Concat(str, "CashOverCounter = ", CashOverCounter, "\r\n");
            str = string.Concat(str, "IsNotesEnable = ", IsNotesEnable, "\r\n");
            str = string.Concat(str, "IsMGIAlloyLogoEnable = ", IsMGIAlloyLogoEnable, "\r\n");
            //Developed by: Sunil Shetty || Date: 09\06\2015
            //AL-533 : Mailing Address will not be visible in Customer registration on the base of IsMailingAddressEnable value
            str = string.Concat(str, "IsMailingAddressEnable = ", IsMailingAddressEnable, "\r\n");
            str = string.Concat(str, "CanEnableProfileStatus = ", CanEnableProfileStatus, "\r\n");
            str = string.Concat(str, "CustomerMinimumAge = ", CustomerMinimumAge, "\r\n");
            str = string.Concat(str, "MasterSSN = ", MasterSSN, "\r\n");
            str = string.Concat(str, "IsReferralSectionEnable = ", IsReferralSectionEnable, "\r\n");
            str = string.Concat(str, "ComplianceProgramName = ", ComplianceProgramName, "\r\n");
            str = string.Concat(str, "CardPresenceVerificationConfig = ", CardPresenceVerificationConfig, "\r\n");
            return str;
        }

    }

}
