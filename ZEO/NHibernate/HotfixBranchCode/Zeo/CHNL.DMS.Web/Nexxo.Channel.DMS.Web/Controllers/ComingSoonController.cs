using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.Models;

namespace MGI.Channel.DMS.Web.Controllers
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
