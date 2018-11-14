using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;

namespace TCF.Zeo.Core.Data.Exceptions
{
    public class PromotionException : ZeoException
    {
        public static string PromotionCode = ((int)Helper.ProductCode.Alloy).ToString();
        public static string ZeoCode = ((int)Helper.ProviderId.Alloy).ToString();

        public PromotionException(string alloyErrorCode, Exception innerException)
            : base(PromotionCode, ZeoCode, alloyErrorCode, innerException)
        {
        }

        public static string GET_PROMOTIONS                 = "3661";
        public static string GET_PROMOTION_BY_ID            = "3662";
        public static string INSERT_OR_UPDATE_PROMOTION     = "3664";
        public static string VALIDATE_PROMO_NAME            = "3665";
        public static string UPDATE_STATUS                  = "3663";
        public static string SAVE_PROMO_DETAILS_FAILED      = "3664";
        public static string SAVE_PROMO_PROVISION_FAILED    = "3665";
        public static string SAVE_PROMO_QUALIFIER_FAILED    = "3666";
        public static string DELETE_PROMO_QUALIFIER_FAILED  = "3667";
        public static string DELETE_PROMO_PROVISION_FAILED  = "3668";
        public static string ADD_UPDATE_QUALIFIERS_FAILED   = "3669";
        public static string ADD_UPDATE_PROVISIONS_FAILED   = "3670";
    }
}
