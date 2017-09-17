using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Common;
using TCF.Channel.Zeo.Web.Models;

namespace TCF.Channel.Zeo.Web.Controllers
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
			try
			{
				ProductInfo productInfo = new ProductInfo();
				productInfo.CheckLimit = checkCancel.CheckLimit;
				return View("ProductInformation", productInfo);
			}
			catch(Exception ex)
			{
				VerifyException(ex); return null;
			}
        }

   ////     public ActionResult FeeConformationAfterChangingCheckType(decimal baseFee, decimal netFee, string checkId)
   ////     {
			////try
			////{
			////	CheckAmount amount = new CheckAmount();
			////	amount.BaseFee = baseFee;
			////	amount.NetFee = netFee;
			////	amount.CheckId = checkId;
			////	return PartialView("_CertegyReClassifiedPopup", amount);
			////}
			////catch(Exception ex)
			////{
			////	VerifyException(ex); return null;
			////}
   ////     }
    }
}
