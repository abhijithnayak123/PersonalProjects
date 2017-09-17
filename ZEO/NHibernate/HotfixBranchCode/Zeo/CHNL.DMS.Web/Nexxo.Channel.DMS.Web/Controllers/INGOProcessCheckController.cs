using MGI.Channel.DMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MGI.Channel.DMS.Web.Controllers
{
    public class INGOProcessCheckController : CashaCheckController
    {
        public ActionResult ProcessCheck()
        {
            Session["activeButton"] = "processcheck";
            Session["MicrErrorCount"] = null;
            ProductInfo prdinfo = new ProductInfo();
            Models.CashACheck cashCheck = new CashACheck();

            cashCheck.CheckLimit = prdinfo.CheckLimit;
            ViewBag.Navigation = Resources.NexxoSiteMap.ProcessCheck;

            return View("CashCheck", cashCheck);
        }

        public ActionResult ReScanCheck()
        {
            Session["activeButton"] = "processcheck";
            ProductInfo prdinfo = new ProductInfo();
            Models.CashACheck cashCheck = new CashACheck();
            cashCheck.CheckLimit = prdinfo.CheckLimit;
            ViewBag.Navigation = Resources.NexxoSiteMap.ProcessCheck;

            return View("CashCheck", cashCheck);
        }

    }
}
