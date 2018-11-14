using TCF.Channel.Zeo.Web.Common;
using System.Web;
using System.Web.Mvc;

namespace TCF.Channel.Zeo.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
			filters.Add(new DisableCache());
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());
			filters.Add(new NoDirectAccessAttribute());
        }
        protected void Application_Start()
        {

        }
    }
}