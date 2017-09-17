using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Models;
using System.Net;
using System.Collections;
using TCF.Channel.Zeo.Web.Common;
//TODO Merge using System.Web.Http;
using Helper = TCF.Zeo.Common.Util.Helper;
#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
#endregion

namespace TCF.Channel.Zeo.Web.Controllers
{
    public class TerminalSetupController : BaseController
    {
        [HttpGet]
        [Authorize(Roles = "Manager, SystemAdmin, Tech")]
        [CustomHandleErrorAttribute(ViewName = "CustomerSearch", MasterName = "_Menu", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.CustomerSearch")]
        public ActionResult TerminalSetup(bool IsException = false, string ExceptionMessage = "")
        {
            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.Response alloyResponse = new ZeoClient.Response();

                ViewBag.IsException = IsException;
                ViewBag.ExceptionMessage = ExceptionMessage;
                long agentSessionId = GetAgentSessionId();
                string ipAddress = string.Empty;
                string terminalName = string.Empty;
                List<SelectListItem> locations = GetLocations();
                TerminalModel terminalModel = null;

                string yubiKey = string.Empty;

                if (TempData["Message"] != null)
                {
                    terminalModel = BuildTerminalModel(agentSessionId, locations, terminalModel);
                }

                if (terminalModel == null)
                {
                    terminalModel = new TerminalModel();
                    if (terminalModel.TIM == (short)Helper.TerminalIdentificationMechanism.YubiKey)
                    {
                        terminalName = GetHostNameFromYubiKey();
                        terminalModel = BuildTerminalModel(agentSessionId, alloyServiceClient, locations, terminalName);
                    }
                    else if (terminalModel.TIM == (short)Helper.TerminalIdentificationMechanism.Cookie)
                    {
                        if (Request.Cookies["TerminalCookie"] == null)
                        {
                            if (locations.Count > 0)
                            {
                                ipAddress = GetComputerInformation(string.Empty, out terminalName);
                            }

                            terminalModel = BuildNewTerminalModel(terminalName, locations);
                        }
                        else
                        {
                            HttpCookie terminalCookie = Request.Cookies["TerminalCookie"];
                            terminalName = terminalCookie.Values["TerminalIdentifier"].ToString();
                            terminalModel = BuildTerminalModel(agentSessionId, alloyServiceClient, locations, terminalName);
                        }
                    }
                    else if (terminalModel.TIM == (short)Helper.TerminalIdentificationMechanism.HostName)
                    {
                        terminalName = GetHostName();
                        terminalModel = BuildTerminalModel(agentSessionId, alloyServiceClient, locations, terminalName);
                        if (terminalModel == null)
                            terminalModel = BuildNewTerminalModel(terminalName, locations);
                    }
                }
                return View("TerminalSetup", terminalModel);
            }
            catch (Exception ex)
            {
                VerifyException(ex);
                return null;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Manager, SystemAdmin, Tech")]
        [CustomHandleErrorAttribute(ActionName = "TerminalSetup", ControllerName = "TerminalSetup", ResultType = "redirect", MasterName = "_Menu")]
        public ActionResult TerminalSetup(TerminalModel terminalModel, ZeoClient.ZeoContext context)
        {
            try
            {

                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.Response alloyResponse = new ZeoClient.Response();
                context = GetZeoContext();
                ZeoClient.Location location = null;
                ZeoClient.NpsTerminal npsTerminal = null;
                bool isSuccess = false;

                long agentSessionId = GetAgentSessionId();

                //This Code added for returning the model if any exception occur.
                terminalModel.Locations = GetLocations();

                if (!string.IsNullOrWhiteSpace(terminalModel.Location) || terminalModel.Location != "0")
                {
                    terminalModel.NpsTerminals = GetNpsTerminalsByLocationId(terminalModel.Location, Convert.ToInt64(terminalModel.NpsTerminal));
                }
                else if (terminalModel.Locations.Count() > 1)
                {
                    terminalModel.NpsTerminals = new List<SelectListItem>() { new SelectListItem() { Selected = true, Text = "Select", Value = "0" } };
                }

                string terminalName = terminalModel.TerminalName;
                long locationId = Convert.ToInt64(terminalModel.Location);
                long npsTerminalId = Convert.ToInt64(terminalModel.NpsTerminal);
                long terminalId = terminalModel.Id;

                ZeoClient.Response alloyresponse = alloyServiceClient.GetLocationById(locationId, context);
                if (WebHelper.VerifyException(alloyresponse)) throw new ZeoWebException(alloyresponse.Error.Details);
                List<ZeoClient.Location> locations = alloyresponse.Result as List<ZeoClient.Location>;
                location = locations.FirstOrDefault();

                alloyresponse = alloyServiceClient.GetNpsterminalByTerminalId(npsTerminalId, context);
                if (WebHelper.VerifyException(alloyresponse)) throw new ZeoWebException(alloyresponse.Error.Details);
                List<ZeoClient.NpsTerminal> npsTerminals = alloyresponse.Result as List<ZeoClient.NpsTerminal>;
                npsTerminal = npsTerminals.FirstOrDefault();
                if (npsTerminal.LocationId != location.LocationID)
                {
                    npsTerminal.LocationId = location.LocationID;
                    alloyServiceClient.UpdateNpsTerminal(npsTerminal, context);
                }


                if (terminalModel.TIM == (short)Helper.TerminalIdentificationMechanism.YubiKey)
                {
                    if (!string.IsNullOrWhiteSpace(terminalModel.TerminalName) && terminalModel.TerminalName.Length == 44)
                        terminalName = terminalName.Substring(0, 12).ToUpper();

                    isSuccess = UpdateTerminal(terminalModel, alloyServiceClient, location, npsTerminal);

                    if (!isSuccess)
                    {
                        isSuccess = AddTerminal(terminalModel, alloyServiceClient, location, npsTerminal, terminalName);
                    }

                    if (isSuccess)
                    {
                        long sessionId = Convert.ToInt64(Session["SessionId"]);
                        ZeoClient.ChannelPartner channelPartner = Session["ChannelPartner"] as ZeoClient.ChannelPartner;

                        alloyresponse = alloyServiceClient.GetTerminalByName(terminalName, context);
                        if (WebHelper.VerifyException(alloyresponse)) throw new ZeoWebException(alloyresponse.Error.Details);
                        ZeoClient.Terminal terminal = alloyresponse.Result as ZeoClient.Terminal;
                        if (isSuccess)
                        {
                            UpdateSession(terminal);
                        }
                    }

                    TempData["TerminalModel"] = terminalModel;
                    TempData["Message"] = "Terminal setup successful";
                    return RedirectToAction("TerminalSetup");
                }
                else if (terminalModel.TIM == (short)Helper.TerminalIdentificationMechanism.Cookie)
                {
                    // When cookie is not available
                    if (Request.Cookies["TerminalCookie"] == null)
                    {
                        ZeoClient.ChannelPartner channelPartner = Session["ChannelPartner"] as ZeoClient.ChannelPartner;
                        alloyresponse = alloyServiceClient.GetTerminalByName(terminalName, context);
                        if (WebHelper.VerifyException(alloyresponse)) throw new ZeoWebException(alloyresponse.Error.Details);
                        ZeoClient.Terminal terminal = alloyresponse.Result as ZeoClient.Terminal;
                        if (terminal != null)
                        {
                            TempData["Message"] = "Terminal Name is unavailable, please use different terminal name";
                            TempData["TerminalModel"] = terminalModel;
                            return RedirectToAction("TerminalSetup");
                        }
                        isSuccess = AddTerminal(terminalModel, alloyServiceClient, location, npsTerminal, terminalName);
                    }
                    else
                    {
                        isSuccess = UpdateTerminal(terminalModel, alloyServiceClient, location, npsTerminal);
                    }

                    if (isSuccess)
                    {
                        CreateTerminalCookie(terminalModel.TerminalName);
                    }

                    return RedirectToAction("Logout", "SSO");
                }
                else// if (terminalModel.TIM == (short)TerminalIdentificationMechanism.HostName)
                {
                    ZeoClient.ChannelPartner channelPartner = Session["ChannelPartner"] as ZeoClient.ChannelPartner;

                    alloyresponse = alloyServiceClient.GetTerminalByName(terminalName, context);
                    if (WebHelper.VerifyException(alloyresponse)) throw new ZeoWebException(alloyresponse.Error.Details);
                    ZeoClient.Terminal terminal = alloyresponse.Result as ZeoClient.Terminal;
                    if (terminal != null)
                    {
                        isSuccess = UpdateTerminal(terminalModel, alloyServiceClient, location, npsTerminal);
                    }
                    else
                    {
                        isSuccess = AddTerminal(terminalModel, alloyServiceClient, location, npsTerminal, terminalName);
                    }

                    return RedirectToAction("Logout", "SSO");
                }
            }
            catch (Exception ex)
            {
                VerifyException(ex);
                return null;
            }
        }

        [HttpGet]
        [Authorize(Roles = "Manager, SystemAdmin, Tech")]
        public ActionResult GetNpsTerminal(string locationId)
        {
            try
            {
                List<SelectListItem> npsTerminalCollection = GetNpsTerminalsByLocationId(locationId, 0);
                return Json(npsTerminalCollection, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex);
                return null;
            }
        }

        [Authorize(Roles = "Manager, SystemAdmin, Teller, ComplianceManager, Tech")]

        public ActionResult DisplayChooseLocation(long dt)
        {
            try
            {
                ZeoClient.Terminal terminal = new ZeoClient.Terminal();
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();
                var htSessions = (Hashtable)Session["HTSessions"];
                var agentSession = ((ZeoClient.AgentSession)(htSessions["TempSessionAgent"]));

                ZeoClient.Response alloyresponse = alloyServiceClient.GetTerminalById(Convert.ToInt64(terminal.TerminalId), context);
                if (WebHelper.VerifyException(alloyresponse)) throw new ZeoWebException(alloyresponse.Error.Details);
                terminal = alloyresponse.Result as ZeoClient.Terminal;

                TerminalModel terminalModel;
                //If Terminal Exists without Location
                if (terminal != null)
                {
                    terminalModel = new TerminalModel()
                    {
                        Id = terminal.TerminalId,
                        TerminalName = terminal.Name,
                        IpAddress = terminal.IpAddress,
                        Locations = GetLocations(),
                        Location = null,
                        NpsTerminal = terminal.PeripheralServerId
                    };
                }
                else
                { //If Terminal Not Exists				
                    terminalModel = new TerminalModel();
                    terminalModel.Id = 0;
                    terminalModel.TerminalName = Session["HostName"] == null ? "" : Session["HostName"].ToString();
                    terminalModel.IpAddress = "";
                    terminalModel.Locations = GetLocations();
                    terminalModel.Location = null;
                    terminalModel.NpsTerminal = null;
                }

                if (terminalModel.Locations != null)
                    terminalModel.Locations.First().Text = " Choose Location";

                return PartialView("_partialChooseTerminalLocation", terminalModel);
            }
            catch (Exception ex)
            {
                VerifyException(ex);
                return null;
            }

        }

        [HttpPost]
        [Authorize(Roles = "Manager, SystemAdmin, Teller, ComplianceManager, Tech")]
        [CustomHandleErrorAttribute(ActionName = "CustomerSearch", ControllerName = "CustomerSearch", ResultType = "redirect")]
        public ActionResult ChooseLocation(TerminalModel terminal)
        {

            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.Response alloyResponse = new ZeoClient.Response();
                ZeoClient.Terminal existingTerminal = new ZeoClient.Terminal();


                bool isUpdateSuccess = false;
                long agentSessionId = GetAgentSessionId();
                terminal.Locations = GetLocations();
                if (terminal.Locations != null)
                    terminal.Locations.First().Text = " Choose Location";

                if (terminal.Location != null && terminal.Location != "0")
                {
                    //Lookup for existing Terminal
                    ZeoClient.Response alloyresponse = alloyServiceClient.GetTerminalById(Convert.ToInt64(terminal.Id), context);
                    if (WebHelper.VerifyException(alloyresponse)) throw new ZeoWebException(alloyresponse.Error.Details);
                    existingTerminal = alloyresponse.Result as ZeoClient.Terminal;


                    alloyResponse = alloyServiceClient.GetLocationById(Convert.ToInt32(terminal.Location), context);
                    if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                    List<ZeoClient.Location> locations = alloyResponse.Result as List<ZeoClient.Location>;
                    ZeoClient.Location location = locations.FirstOrDefault();
                    Session["CurrentLocation"] = location.LocationName;

                    // getChannelPartnerName

                    if (existingTerminal.TerminalId != 0)
                    {
                        existingTerminal.LocationId = location.LocationID;
                        alloyResponse = alloyServiceClient.UpdateTerminal(existingTerminal, context);
                        if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                        isUpdateSuccess = Convert.ToBoolean(alloyResponse.Result);
                    }
                    else
                    {
                        existingTerminal = new ZeoClient.Terminal();
                        existingTerminal.Name = Session["HostName"] == null ? "" : Session["HostName"].ToString();
                        existingTerminal.LocationId = location.LocationID;
                        existingTerminal.ChannelPartnerId = context.ChannelPartnerId;
                        if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                        alloyResponse = alloyServiceClient.CreateTerminal(existingTerminal, context);
                        isUpdateSuccess = Convert.ToBoolean(alloyResponse.Result);
                    }

                    if (isUpdateSuccess)
                    {

                        alloyResponse = alloyServiceClient.GetTerminalByName(existingTerminal.Name, context);
                        if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                        existingTerminal = alloyResponse.Result as ZeoClient.Terminal;

                        long sessionId = Convert.ToInt64(Session["SessionId"]);

                        UpdateSession(existingTerminal);

                        return RedirectToAction("CustomerSearch", "CustomerSearch");
                    }
                }
            }

            catch (Exception ex)
            {
                VerifyException(ex);
                return null;
            }


            return PartialView("_partialChooseTerminalLocation", terminal);

        }

        [HttpGet]
        [Authorize(Roles = "Manager, SystemAdmin, Tech")]
        public ActionResult EpsonDiagnostics()
        {
            BaseModel baseModel = new BaseModel();
            return View("EpsonDiagnostics", baseModel);
        }

        [HttpGet]
        [Authorize(Roles = "Manager, SystemAdmin, Tech")]
        public ActionResult ScanCheckTest()
        {
            CheckImage cashacheck = new CheckImage();
            return View("ScanCheckTest", cashacheck);
        }
        [HttpPost]
        [Authorize(Roles = "Manager, SystemAdmin, Tech")]
        public ActionResult ScanCheckTestPost(CheckImage checkCash)
        {
            return View("TestCheckImage", checkCash);
        }

        [HttpGet]
        [Authorize(Roles = "Manager, SystemAdmin, Tech")]
        [CustomHandleErrorAttribute(ViewName = "MoneyOrderTest", MasterName = "_Menu", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.CheckImage")]
        public ActionResult MoneyOrderTest()
        {
            try
            {
                CheckImage moneyOrder = new CheckImage();
                ZeoClient.ZeoContext context = GetZeoContext();

                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.Response response = alloyServiceClient.GenerateMoneyOrderDiagnostics(context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                ZeoClient.MoneyOrderCheckPrint checkPrint = response.Result as ZeoClient.MoneyOrderCheckPrint;
                moneyOrder.PrintData = ShoppingCartHelper.PrepareCheckForPrinting(checkPrint.Lines);

                return View("MoneyOrderTest", moneyOrder);
            }
            catch (Exception ex)
            {
                VerifyException(ex);
                return null;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Manager, SystemAdmin, Tech")]
        public ActionResult MoneyOrderTest(CheckImage moneyOrder)
        {
            return View("TestMoneyOrderImage", moneyOrder);
        }

        [HttpGet]
        [Authorize(Roles = "Manager, SystemAdmin, Tech")]
        public ActionResult ReceiptPrintTest()
        {
            BaseModel model = new BaseModel();
            return View("ReceiptPrintTest", model);
        }
        #region Private Methods

        private string GetComputerInformation(string terminalName, out string terminalIdentifier)
        {
            terminalIdentifier = string.Empty;
            string ipAddress = Request.ServerVariables["REMOTE_ADDR"];
            try
            {
                IPHostEntry visitorIP = System.Net.Dns.GetHostEntry(ipAddress);
                if (visitorIP != null)
                {
                    terminalIdentifier = string.IsNullOrWhiteSpace(terminalName) ? visitorIP.HostName : terminalName;
                    IPAddress[] visiterIPAddressList = visitorIP.AddressList;
                    if (visiterIPAddressList.Length > 0)
                    {
                        ipAddress = visiterIPAddressList[visiterIPAddressList.Length - 1].ToString();
                    }
                }
            }
            catch
            {
                // ignore this for now.
            }
            return ipAddress;
        }

        private TerminalModel BuildNewTerminalModel(string terminalName, List<SelectListItem> locations)
        {
            List<SelectListItem> npsTerminals = new List<SelectListItem>(); ;

            string computerName = string.Empty;
            string ipAddress = GetComputerInformation(terminalName, out computerName);

            npsTerminals.Insert(0, new SelectListItem() { Selected = true, Text = "Select", Value = "0" });

            TerminalModel terminalModel = new TerminalModel()
            {
                TerminalName = terminalName,
                IpAddress = ipAddress,
                Locations = locations,
                NpsTerminals = npsTerminals
            };
            return terminalModel;
        }

        private TerminalModel BuildTerminalModel(long agentSessionId, List<SelectListItem> locations, TerminalModel terminalModel)
        {
            terminalModel = TempData["TerminalModel"] as TerminalModel;
            List<SelectListItem> npsTerminals = null;

            if (locations.Count == 1)
            {
                npsTerminals = GetNpsTerminalsByLocationId(locations.FirstOrDefault().Value, 0);
            }
            else
            {
                if (terminalModel.NpsTerminal != null && string.Compare(terminalModel.NpsTerminal, "select", true) != 0)
                {
                    npsTerminals = GetNpsTerminalsByLocationId(terminalModel.Location, Convert.ToInt64(terminalModel.NpsTerminal));
                    npsTerminals.Insert(0, new SelectListItem() { Selected = true, Text = "Select", Value = "0" });
                }
                else
                {
                    npsTerminals = new List<SelectListItem>() { new SelectListItem() { Selected = true, Text = "Select", Value = "0" } };
                }
            }
            terminalModel.Locations = locations;
            terminalModel.NpsTerminals = npsTerminals;

            ViewBag.Message = TempData["Message"].ToString();
            ViewBag.PeripheralUrl = GetPeripheralUrl(agentSessionId, Convert.ToInt64(terminalModel.NpsTerminal));
            return terminalModel;
        }

        private string GetPeripheralUrl(long agentSessionId, long npsTerminalId)
        {

            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.ZeoContext context = GetZeoContext();

            string peripheralUrl = string.Empty;
            ZeoClient.Response alloyresponse = alloyServiceClient.GetTerminalById(npsTerminalId, context);
            if (WebHelper.VerifyException(alloyresponse)) throw new ZeoWebException(alloyresponse.Error.Details);
            ZeoClient.NpsTerminal npsTerminal = alloyresponse.Result as ZeoClient.NpsTerminal;
            if (npsTerminal != null)
            {
                if (npsTerminal.PeripheralServiceUrl != null)
                {
                    peripheralUrl = npsTerminal.PeripheralServiceUrl;
                }
            }
            return peripheralUrl;
        }

        private TerminalModel BuildTerminalModel(long agentSessionId, ZeoClient.ZeoServiceClient alloyServiceClient, List<SelectListItem> locations, string terminalName)
        {
            ZeoClient.ZeoContext context = GetZeoContext();

            TerminalModel terminalModel = null;
            ZeoClient.ChannelPartner channelPartner = Session["ChannelPartner"] as ZeoClient.ChannelPartner;
            ZeoClient.Terminal terminal = null;
            if (!string.IsNullOrWhiteSpace(terminalName))
            {
                ZeoClient.Response alloyResponse = alloyServiceClient.GetTerminalByName(terminalName, context);
                if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                terminal = alloyResponse.Result as ZeoClient.Terminal;
            }

            if (terminal != null && terminal.TerminalId != 0)
            {
                List<SelectListItem> npsTerminals = null;
                long peripheralServerId = (terminal.PeripheralServerId != null) ? Convert.ToInt32(terminal.PeripheralServerId) : 0;
                npsTerminals = GetNpsTerminalsByLocationId(terminal.LocationId.ToString(), peripheralServerId);
                npsTerminals.Insert(0, new SelectListItem() { Selected = true, Text = "Select", Value = "0" });
                if (npsTerminals != null)
                {
                    terminalModel = new TerminalModel()
                    {
                        Id = terminal.TerminalId,
                        TerminalName = terminal.Name,
                        IpAddress = terminal.IpAddress,
                        Locations = locations,
                        Location = terminal.LocationId.ToString(),
                        NpsTerminal = peripheralServerId == 0 ? string.Empty : peripheralServerId.ToString(),
                        NpsTerminals = npsTerminals
                    };
                }
            }
            return terminalModel;
        }

        private bool UpdateTerminal(TerminalModel terminalModel, ZeoClient.ZeoServiceClient alloyServiceClient, ZeoClient.Location location, ZeoClient.NpsTerminal npsTerminal)
        {
            ZeoClient.ZeoContext context = GetZeoContext();
            bool isUpdateSuccess = false;
            ZeoClient.Terminal existingTerminal = null;
            long agentSessionId = GetAgentSessionId();
            if (terminalModel.TIM == (short)Helper.TerminalIdentificationMechanism.YubiKey)
            {
                ZeoClient.ChannelPartner channelPartner = Session["ChannelPartner"] as ZeoClient.ChannelPartner;
                ZeoClient.Response alloyresponse = alloyServiceClient.GetTerminalByName(terminalModel.TerminalName, context);
                if (WebHelper.VerifyException(alloyresponse)) throw new ZeoWebException(alloyresponse.Error.Details);
                existingTerminal = alloyresponse.Result as ZeoClient.Terminal;
            }
            else
            {
                ZeoClient.Response alloyresponse = alloyServiceClient.GetTerminalById(terminalModel.Id, context);
                if (WebHelper.VerifyException(alloyresponse)) throw new ZeoWebException(alloyresponse.Error.Details);
                existingTerminal = alloyresponse.Result as ZeoClient.Terminal;
            }

            if (existingTerminal != null)
            {
                existingTerminal.TerminalId = terminalModel.Id;
                existingTerminal.Name = terminalModel.TerminalName;
                existingTerminal.IpAddress = terminalModel.IpAddress;
                existingTerminal.LocationId = Convert.ToInt32(terminalModel.Location);

                ZeoClient.Response alloyresponse = alloyServiceClient.UpdateTerminal(existingTerminal, context);
                if (WebHelper.VerifyException(alloyresponse)) throw new ZeoWebException(alloyresponse.Error.Details);
                isUpdateSuccess = Convert.ToBoolean(alloyresponse.Result);
            }

            return isUpdateSuccess;
        }

        private bool AddTerminal(TerminalModel terminalModel, ZeoClient.ZeoServiceClient alloyServiceClient, ZeoClient.Location location, ZeoClient.NpsTerminal npsTerminal, string terminalName)
        {
            ZeoClient.ZeoContext context = GetZeoContext();

            bool isSuccess = false;
            if (Session["ChannelPartner"] != null)
            {
                ZeoClient.ChannelPartner channelPartner = Session["ChannelPartner"] as ZeoClient.ChannelPartner;
                ZeoClient.Terminal terminal = new ZeoClient.Terminal()
                {
                    Name = terminalName,
                    IpAddress = terminalModel.IpAddress,
                    ChannelPartnerId = context.ChannelPartnerId,
                    MacAddress = "",
                    LocationId = location.LocationID,
                    PeripheralServerId = Convert.ToString(npsTerminal.NpsTerminalId),
                    PeripheralServerUrl = npsTerminal.PeripheralServiceUrl,
                    
                };

                ZeoClient.Response response = alloyServiceClient.CreateTerminal(terminal, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                isSuccess = Convert.ToBoolean(response.Result);
            }
            return isSuccess;
        }

        private List<SelectListItem> GetNpsTerminalsByLocationId(string locationId, long selectedNpsTerminal)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.ZeoContext context = GetZeoContext();

            ZeoClient.Response alloyresponse = alloyServiceClient.GetNpsTerminalBylocationId(locationId, context);
            if (WebHelper.VerifyException(alloyresponse)) throw new ZeoWebException(alloyresponse.Error.Details);
            List<ZeoClient.NpsTerminal> npsTerminals = alloyresponse.Result as List<ZeoClient.NpsTerminal>;
            ZeoClient.ChannelPartner channelPartner = Session["ChannelPartner"] as ZeoClient.ChannelPartner;

            string TerminalName = GetHostName();

            alloyresponse = alloyServiceClient.GetNpsTerminalByName(TerminalName, context.ChannelPartnerId, context);
            if (WebHelper.VerifyException(alloyresponse)) throw new ZeoWebException(alloyresponse.Error.Details);
            ZeoClient.NpsTerminal npsterminal = alloyresponse.Result as ZeoClient.NpsTerminal;
            if (npsterminal != null && !npsTerminals.Any(x => x.Name == npsterminal.Name))
            {
                npsTerminals.Insert(0, npsterminal);
            }

            List<SelectListItem> npsTerminalCollection = new List<SelectListItem>();

            foreach (var npsTerminal in npsTerminals)
            {
                if (!string.IsNullOrWhiteSpace(npsTerminal.Name))
                {
                    npsTerminalCollection.Add(
                        new SelectListItem()
                        {
                            Text = npsTerminal.Name,
                            Value = npsTerminal.NpsTerminalId.ToString(),
                            Selected = (npsTerminal.NpsTerminalId == selectedNpsTerminal)
                        });
                }
            }
            return npsTerminalCollection;
        }

        private List<SelectListItem> GetLocations()
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.ZeoContext context = GetZeoContext();
            ZeoClient.Response alloyResponse = new ZeoClient.Response();

            List<SelectListItem> locations = new List<SelectListItem>();

            string channelPartner = Session["ChannelPartnerName"].ToString();
            if (!string.IsNullOrWhiteSpace(channelPartner))
            {
                alloyResponse = alloyServiceClient.GetLocationsByChannelPartnerId(context.ChannelPartnerId, context);
                if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                var availableLocations = alloyResponse.Result as List<ZeoClient.Location>;

                foreach (var location in availableLocations)
                {
                    locations.Add(new SelectListItem() { Text = location.LocationName, Value = location.LocationIdentifier.ToString() });
                }
                locations.Insert(0, new SelectListItem() { Text = "Select", Value = "0" });
            }
            return locations;
        }

        private void CreateTerminalCookie(string terminalIdentifier)
        {
            HttpCookie terminalCookie = new HttpCookie("TerminalCookie");
            terminalCookie.Expires = DateTime.Now.AddYears(1);
            /****************************Begin TA-50 Changes************************************************/
            //     User Story Number: TA-50 | ALL |   Developed by: Sunil Shetty     Date: 03.03.2015
            //     Purpose: CRLF Injection
            //				To Avoid CRLF injection we are replacing string with CarriageReturn and Line Feed
            terminalCookie.Values["TerminalIdentifier"] = HttpUtility.HtmlEncode(terminalIdentifier);
            Response.Cookies.Add(terminalCookie);
        }

        private string GetHostNameFromYubiKey()
        {
            string terminalIdentifier = string.Empty;
            if (Request.Cookies["Yubikeys"] != null)
            {
                string yubiKey = Request.Cookies["Yubikeys"].Value;
                if (!string.IsNullOrWhiteSpace(yubiKey))
                {
                    terminalIdentifier = yubiKey.Substring(0, 12).ToUpper();
                }
            }
            return terminalIdentifier;
        }

        private string GetHostName()
        {
            string hostName = string.Empty;

            if (Session["HostName"] != null)
            {
                hostName = Session["HostName"].ToString();
            }

            return hostName;
        }

        private void UpdateSession(ZeoClient.Terminal terminal)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.ZeoContext context = GetZeoContext();
            ZeoClient.Response alloyResponse = new ZeoClient.Response();

            Session["IsTerminalSetup"] = 1;
            Session["IsPeripheralServerSetUp"] = IsPeripheralServerSetUp(terminal) ? 1 : 0;

            var htSessions = (Hashtable)Session["HTSessions"];
            var agentSession = ((ZeoClient.AgentSession)(htSessions["TempSessionAgent"]));

            htSessions["TempSessionAgent"] = agentSession;
            this.Session["HTSessions"] = htSessions;
        }

        #endregion
    }
}
