using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class ChannelPartnerConfig
    {
        public ChannelPartner ChannelPartner { get; set; }
        public Guid ChannelPartnerID { get; set; }
        public bool DisableWithdrawCNP { get; set; }
        public bool CashOverCounter { get; set; }

        //US1421 Changes
        public string FrankData { get; set; }
        public bool IsCheckFrank { get; set; }
        public bool IsNotesEnable { get; set; }
        //US1800 Referral promotions – Free check cashing to referrer and referee 
        public bool IsReferralSectionEnable { get; set; }
        //AL-291 
        public bool IsMGIAlloyLogoEnable { get; set; }
        public string MasterSSN { get; set; }
        //Developed by: Sunil Shetty Date: 09/06/2015
        //AL-533 : A new boolean value is introduced to configure Mailing address option in Customer registration page
        public  bool IsMailingAddressEnable { get; set; }
        //AL-586
        public  bool CanEnableProfileStatus { get; set; }
        //AL-1626
        public  int CustomerMinimumAge { get; set; }
    }
}
