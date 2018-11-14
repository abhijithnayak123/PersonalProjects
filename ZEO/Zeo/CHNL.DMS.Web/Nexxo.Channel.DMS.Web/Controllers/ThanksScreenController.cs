//using System.Web.Mvc;
//using TCF.Channel.Zeo.Web.Models;
//using MGI.Channel.Shared.Server.Data;

//namespace TCF.Channel.Zeo.Web.Controllers
//{
//    public class ThanksScreenController : BaseController
//    {
//        public ActionResult GPRCards()
//        {
//            CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
//            AccountBalance accountBalance = new AccountBalance();
//            //accountBalance.AccountBalanceAmount = customerSession.Customer.Purse.Balance;

//            return View("GPRCards",accountBalance);
//        }

//        public ActionResult NonGPRCards()
//        {
//            CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
//            AccountBalance accountBalance = new AccountBalance();
//            //accountBalance.AccountBalanceAmount = customerSession.Customer.Purse.Balance;

//            return View("NonGPRCards",accountBalance);
//        }
//    }
//}
