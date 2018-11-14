using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Biz.Common.Data.Exceptions
{
    public class PromotionException : ZeoException
    {
        public static string PromotionCode = ((int)Helper.ProductCode.Alloy).ToString();
        public static string ZeoCode = ((int)ProviderId.Alloy).ToString();

        public PromotionException(string alloyErrorCode, Exception innerException)
            : base(PromotionCode, ZeoCode, alloyErrorCode, innerException)
        {
        }

        public static string CREATE_OR_UPDATE_PROMOTION     = "4921";
        public static string GET_PROMOTIONS                 = "4922";
        public static string GET_PROMOTION_BY_ID            = "4923";
        public static string VALIDATE_PROMO_NAME            = "4925";
        public static string UPDATE_PROMOTION_STATUS        = "4924";
        public static string SAVE_PROMO_DETAILS_FAILED      = "4925";
        public static string SAVE_PROMO_PROVISION_FAILED    = "4926";
        public static string SAVE_PROMO_QUALIFIER_FAILED    = "4927";
        public static string DELETE_PROMO_QUALIFIER_FAILED  = "4928";
        public static string DELETE_PROMO_PROVISION_FAILED  = "4929";
        public static string ADD_UPDATE_QUALIFIERS_FAILED   = "4930";
        public static string ADD_UPDATE_PROVISIONS_FAILED   = "4931";
    }
}
