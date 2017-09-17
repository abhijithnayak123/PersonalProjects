using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.DMS.Web.Controllers
{
    public class ThanksScreenController : BaseController
    {
        public ActionResult GPRCards()
        {
            CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
            AccountBalance accountBalance = new AccountBalance();
            //accountBalance.AccountBalanceAmount = customerSession.Customer.Purse.Balance;

            return View("GPRCards",accountBalance);
        }

        public ActionResult NonGPRCards()
        {
            CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
            AccountBalance accountBalance = new AccountBalance();
            //accountBalance.AccountBalanceAmount = customerSession.Customer.Purse.Balance;

            return View("NonGPRCards",accountBalance);
        }
    }
}
