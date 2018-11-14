using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Common;
using TCF.Channel.Zeo.Web.Models;
#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
#endregion
namespace TCF.Channel.Zeo.Web.Controllers
{
    public class WesternUnionBannerController : BaseController
    {
        public ActionResult AgentBannerMessage(bool IsException = false, string ExceptionMessage = "")
        {
            ViewBag.IsException = IsException;
            ViewBag.ExceptionMessage = ExceptionMessage;
            string errorMessage = "Unable to establish communication with Western Union. Please setup ZEO Terminal Location.";
            WesternUnionBanner westernUnionBanner = new WesternUnionBanner();
            string bannerMessage = string.Empty;
            try
            {
                ZeoClient.ZeoContext context = GetZeoContext();
                System.Collections.Hashtable htSessions = (System.Collections.Hashtable)Session["HTSessions"];
                ZeoClient.AgentSession agentSession = ((ZeoClient.AgentSession)(htSessions["TempSessionAgent"]));
                if (agentSession.PeripheralServiceUrl == null || string.IsNullOrEmpty(agentSession.PeripheralServiceUrl))
                {
                    bannerMessage = errorMessage;
                }
                else
                {
                    if (agentSession.PeripheralServiceUrl != null && agentSession.LocationName != null)
                    {
                        ZeoClient.ZeoServiceClient serviceClient = new ZeoClient.ZeoServiceClient();
                        ZeoClient.Response response = new ZeoClient.Response();
                        response = serviceClient.WUGetAgentBannerMessage(Convert.ToInt64(agentSession.SessionId), context);
                        if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                        bannerMessage = response.Result as string;

                        if (bannerMessage == string.Empty)
                        {
                            bannerMessage = errorMessage;
                        }

                    }
                    else
                    {
                        bannerMessage = errorMessage;
                    }
                }
                if (Session["IsTerminalSetup"] == null)
                {
                    Session["IsTerminalSetup"] = 0;
                    //throw new Exception("DMS is not setup correctly and Transactions are currently disabled. Please contact the System Administrator to help solve the problem.");
                }
                TerminalIdentifier.IdentifyTerminal(long.Parse(agentSession.SessionId), context);
                TempData["IsChooseLocation"] = TerminalIdentifier.IsTerminalAvailableForHostName(long.Parse(agentSession.SessionId),context);
                westernUnionBanner.BannerMessage = bannerMessage;
                return View("AgentBannerMessage", "_Menu", westernUnionBanner);
            }
            catch (Exception ex)
            {
                ViewBag.IsException = true; ;
                ViewBag.ExceptionMessage = ex.Message;
                westernUnionBanner.BannerMessage = errorMessage;
                VerifyException(ex);
                return View("AgentBannerMessage", "_Menu", westernUnionBanner);
            }
        }

        [CustomHandleError(ControllerName = "CustomerSearch", ActionName = "CustomerSearch", ResultType = "redirect")]
        public ActionResult GetWUCountriesList()
        {
            ZeoClient.ZeoContext context = GetZeoContext();

            ZeoClient.ZeoServiceClient serviceClient = new ZeoClient.ZeoServiceClient();

            ZeoClient.Response response = serviceClient.GetBlockedUnBlockedCountries(context);
            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

            var countries = (ZeoClient.WUCountry)response.Result;

            ViewBag.BlockedCountries = countries.BlockedCountries;

            ViewBag.UnblockedCountries = countries.UnblockedCountries;

            return View("WUCountriesList", "_Menu", new BaseModel());
        }

        [HttpPost]
        public JsonResult SaveBlockedCountries(List<string> blockedCountries)
        {
            ZeoClient.ZeoContext context = GetZeoContext();

            ZeoClient.ZeoServiceClient serviceClient = new ZeoClient.ZeoServiceClient();

            //If there are no Blocked Countries in the UI, Empty array is assigned from the JS, but in Action method, it will passed as "null" as a default behavior.
            //If the array is null, then create a object, so that it will have zero items in the list if there are no blocked countries instead of null.
            if (blockedCountries == null)
                blockedCountries = new List<string>();

            ZeoClient.Response response = serviceClient.SaveBlockedCountries(blockedCountries, context);
            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

            return Json(new { success = true}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveBlockedCountriesSuccessPopup()
        {
            return PartialView("_SuccessResponse");
        }
    }
}
