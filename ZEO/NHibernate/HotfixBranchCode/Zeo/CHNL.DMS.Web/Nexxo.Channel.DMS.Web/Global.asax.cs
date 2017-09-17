using MGI.Channel.DMS.Web.Common;
using System;
using System.Globalization;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace MGI.Channel.DMS.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

			//removing the default MVC viewengine collection and adding the customized view engine. 
			ViewEngines.Engines.Clear();
			ViewEngines.Engines.Add(new AlloyViewEngine());
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Request.UserLanguages != null)
            {
                if (Request.UserLanguages.Length == 0)
                    return;
                string language = Request.UserLanguages[0];
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(language);
            }
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            var user = HttpContext.Current.User;
            if (user.Identity.IsAuthenticated && user.Identity is FormsIdentity)
            {
                // user is authenticated; get roles for user
                var identity = (FormsIdentity)user.Identity;
                var authTicket = identity.Ticket;

                // only one role per user for now
                var roles = new string[] { authTicket.UserData };
                var principal = new GenericPrincipal(identity, roles);

                // sets the principal with associated roles for the current context
                HttpContext.Current.User = Thread.CurrentPrincipal = principal;
            }
        }

        // TODO: need to define the correct way of determining channel partner name in 
        // UAT and production. Possibly change validation logic if extracted from URL.
        //protected void Session_Start()
        //{
        //    string hostName = HttpContext.Current.Request.Url.Host;
        //    string ValidHostnameRegex = @"^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])$";

        //    if (System.Text.RegularExpressions.Regex.IsMatch(hostName, ValidHostnameRegex))
        //    {
        //        string[] host = hostName.Split('.');
        //        if (host.Length == 3)
        //        {
        //            HttpContext.Current.Session["ChannelPartnerName"] = host[0].ToString();
        //        }
        //    }
        //    else
        //    {

        //    }
        //}
    }
}