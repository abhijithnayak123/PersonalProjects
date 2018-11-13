using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MGI.Channel.DMS.Web.Controllers
{
    public class WesternUnionReceiveMoneyController : ReceiveMoneyController
    {
        public ActionResult ReceiveMoney()
        {
			Session["isCashierAgree"] = "true";
            Session["activeButton"] = "receivemoney";
            Models.ReceiveMoney receive = new Models.ReceiveMoney();
            return View("ReceiveMoney", "_Common", receive);
        }
    }
}
