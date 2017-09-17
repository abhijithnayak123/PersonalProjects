using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.Models;

namespace MGI.Channel.DMS.Web.Controllers
{
    public class CheckCancelController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkCancel"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomHandleErrorAttribute(ViewName = "CheckCancel", MasterName = "_Common")]
        public ActionResult CheckCancel(CheckDetails checkCancel)
        {
            ProductInfo productInfo = new ProductInfo();
            productInfo.CheckLimit = checkCancel.CheckLimit;
            return View("ProductInformation", productInfo);
        }

        public ActionResult FeeConformationAfterChangingCheckType(decimal baseFee, decimal netFee, string checkId)
        {
            CheckAmount amount = new CheckAmount();
            amount.BaseFee = baseFee;
            amount.NetFee = netFee;
            amount.CheckId = checkId;
            return PartialView("_CertegyReClassifiedPopup", amount);
        }
    }
}
