using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.Models;
using System.Collections;

namespace MGI.Channel.DMS.Web.Controllers
{
    public class LandingPageController : BaseController
    {
        #region Variables
        Hashtable SessionCollection = new Hashtable();
        #endregion

        /// <summary>
        /// Something about what the <c>MySomeFunction</c> does
        /// with some of the sample like
        /// <code>
        /// Some more code statement to comment it better
        /// </code>
        /// For more information seee <see cref="http://www.me.com"/>
        /// </summary>
        /// <param name="someObj">What the input to the function is</param>
        /// <returns>What it returns</returns>
        public ActionResult LandingPage()
        {
            SessionCollection = (Hashtable)Session["HTSessions"];
            int noofdaysvalid = Convert.ToInt32(SessionCollection["ExpDays"]);
            int noofdayspwdchange = System.Configuration.ConfigurationManager.AppSettings.Get("MIN_PWDCHANGE_DAYS") != null ? Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings.Get("MIN_PWDCHANGE_DAYS")) : 10;

            if (noofdaysvalid <= noofdayspwdchange)
            { ViewBag.ExceptionMessage = "Your password expires in " + noofdaysvalid + " days. Please change your password."; }

            return View("LandingPage", new LandingPage());
        }
    }
}
