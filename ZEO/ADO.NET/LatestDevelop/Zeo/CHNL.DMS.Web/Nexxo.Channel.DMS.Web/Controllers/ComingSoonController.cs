using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Models;

namespace TCF.Channel.Zeo.Web.Controllers
{
    public class ComingSoonController : BaseController
    {
        //
        // GET: /ComingSoon/

        public ActionResult DisplayComingSoon()
        {
            BaseModel model = new BaseModel();

            return View("ComingSoon", model);
        }

    }
}
