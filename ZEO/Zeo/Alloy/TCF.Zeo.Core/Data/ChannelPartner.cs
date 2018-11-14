using TCF.Zeo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class ChannelPartner : ZeoModel
    {
        public string PhoneNumber;
        public string AuthenticationType;
        public string Name { get; set; }
        public bool FeesFollowCustomer { get; set; }
        public string CashFeeDescriptionEN { get; set; }
        public string CashFeeDescriptionES { get; set; }
        public string DebitFeeDescriptionEN { get; set; }
        public string DebitFeeDescriptionES { get; set; }
        public decimal ConvenienceFeeCash { get; set; }
        public decimal ConvenienceFeeDebit { get; set; }
        public string ConvenienceFeeDescriptionEN { get; set; }
        public string ConvenienceFeeDescriptionES { get; set; }
        public bool CanCashCheckWOGovtId { get; set; }
        public string LogoFileName { get; set; }
        public bool IsEFSPartner { get; set; }
        public int EFSClientId { get; set; }
        public bool UsePINForNonGPR { get; set; }
        public bool IsCUPartner { get; set; }
        public bool HasNonGPRCard { get; set; }
        public bool ManagesCash { get; set; }
        public bool AllowPhoneNumberAuthentication { get; set; }
        public short TIM { get; set; }
        public bool DisableWithdrawCNP { get; set; }
        public bool CashOverCounter { get; set; }
        public string FrankData { get; set; }
        public string MasterSSN { get; set; }
        public bool IsCheckFrank { get; set; }
        public bool IsNotesEnable { get; set; }
        public bool IsMGIAlloyLogoEnable { get; set; }
        public bool IsReferralSectionEnable { get; set; }
        public bool IsMailingAddressEnable { get; set; }
        public bool CanEnableProfileStatus { get; set; }
        public int CustomerMinimumAge { get; set; }
        public string ComplianceProgramName { get; set; }
        public int CardPresenceVerificationConfig { get; set; }
        public ChannelPartnerConfig ChannelPartnerConfig { get; set; }
        public IList<ChannelPartnerProductProvider> Providers { get; set; }

        //  public ICollection<Prospect> Prospects { get; set; }

        //  public ICollection<Customer> Customers { get; set; }
    }

}
