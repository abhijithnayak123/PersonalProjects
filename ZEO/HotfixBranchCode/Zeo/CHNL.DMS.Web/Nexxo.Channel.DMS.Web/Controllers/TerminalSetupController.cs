using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.DMS.Server.Data;
using System.Net;
using System.Collections;
using MGI.Channel.Shared.Server.Data;
//TODO Merge using System.Web.Http;

namespace MGI.Channel.DMS.Web.Controllers
{
	public class TerminalSetupController : BaseController
	{
		[HttpGet]
		[Authorize(Roles = "Manager, SystemAdmin, Tech")]
		[CustomHandleErrorAttribute(ViewName = "CustomerSearch", MasterName = "_Menu", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.CustomerSearch")]
		public ActionResult TerminalSetup()
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			Desktop desktop = new Desktop();
            long agentSessionId = GetAgentSessionId();
			string ipAddress = string.Empty;
			string terminalName = string.Empty;
			List<SelectListItem> locations = GetLocations(mgiContext);
			TerminalModel terminalModel = null;

			string yubiKey = string.Empty;

			if (TempData["Message"] != null)
			{
				terminalModel = BuildTerminalModel(agentSessionId, locations, terminalModel, mgiContext);
			}

			if (terminalModel == null)
			{
				terminalModel = new TerminalModel();
				if (terminalModel.TIM == (short)TerminalIdentificationMechanism.YubiKey)
				{
					terminalName = GetHostNameFromYubiKey();
					terminalModel = BuildTerminalModel(agentSessionId, desktop, locations, terminalName, mgiContext);
				}
				else if (terminalModel.TIM == (short)TerminalIdentificationMechanism.Cookie)
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
						terminalModel = BuildTerminalModel(agentSessionId, desktop, locations, terminalName, mgiContext);
					}
				}
				else if (terminalModel.TIM == (short)TerminalIdentificationMechanism.HostName)
				{
					terminalName = GetHostName();
					terminalModel = BuildTerminalModel(agentSessionId, desktop, locations, terminalName, mgiContext);
					if (terminalModel == null)
						terminalModel = BuildNewTerminalModel(terminalName, locations);
				}
			}
			return View("TerminalSetup", terminalModel);
		}

		[HttpPost]
		[Authorize(Roles = "Manager, SystemAdmin, Tech")]
		[CustomHandleErrorAttribute(ViewName = "TerminalSetup", MasterName = "_menu")]
		public ActionResult TerminalSetup(TerminalModel terminalModel)
		{
			Desktop desktop = new Desktop();
			Server.Data.Location location = null;
			Server.Data.NpsTerminal npsTerminal = null;
			bool isSuccess = false;

			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new DMS.Server.Data.MGIContext();
            long agentSessionId = GetAgentSessionId();

			//This Code added for returning the model if any exception occur.
			terminalModel.Locations = GetLocations(mgiContext);

			if (!string.IsNullOrWhiteSpace(terminalModel.Location) || terminalModel.Location != "0")
			{
				terminalModel.NpsTerminals = GetNpsTerminalsByLocationId(terminalModel.Location, Convert.ToInt64(terminalModel.NpsTerminal), mgiContext);
			}
			else if (terminalModel.Locations.Count() > 1)
			{
				terminalModel.NpsTerminals = new List<SelectListItem>() { new SelectListItem() { Selected = true, Text = "Select", Value = "0" } };
			}

			string terminalName = terminalModel.TerminalName;
			long locationId = Convert.ToInt64(terminalModel.Location);
			long npsTerminalId = Convert.ToInt64(terminalModel.NpsTerminal);

			location = desktop.GetLocationDetailsForEdit(Convert.ToString(agentSessionId), locationId, mgiContext);
            npsTerminal = desktop.LookupNpsTerminal(agentSessionId, npsTerminalId, mgiContext);

			if (npsTerminal.Location.Id != location.Id)
			{
				npsTerminal.Location = location;
				desktop.UpdateNpsTerminal(agentSessionId, npsTerminal, mgiContext);
			}


			if (terminalModel.TIM == (short)TerminalIdentificationMechanism.YubiKey)
			{
				if (!string.IsNullOrWhiteSpace(terminalModel.TerminalName) && terminalModel.TerminalName.Length == 44)
					terminalName = terminalName.Substring(0, 12).ToUpper();

				isSuccess = UpdateTerminal(terminalModel, desktop, location, npsTerminal, mgiContext);

				if (!isSuccess)
				{
					isSuccess = AddTerminal(terminalModel, desktop, location, npsTerminal, terminalName, mgiContext);
				}

				if (isSuccess)
				{
					long sessionId = Convert.ToInt64(Session["SessionId"]);
					ChannelPartner channelPartner = Session["ChannelPartner"] as ChannelPartner;
					Terminal terminal = desktop.LookupTerminal(agentSessionId, terminalName, Convert.ToInt32(channelPartner.Id), mgiContext);

					if (isSuccess)
					{
						isSuccess = desktop.UpdateSession(sessionId, terminal, mgiContext);
						UpdateSession(terminal);
					}
				}

				TempData["TerminalModel"] = terminalModel;
				TempData["Message"] = "Terminal setup successful";
				return RedirectToAction("TerminalSetup");
			}
			else if (terminalModel.TIM == (short)TerminalIdentificationMechanism.Cookie)
			{
				// When cookie is not available
				if (Request.Cookies["TerminalCookie"] == null)
				{
					ChannelPartner channelPartner = Session["ChannelPartner"] as ChannelPartner;
					Terminal terminal = desktop.LookupTerminal(agentSessionId, terminalName, Convert.ToInt32(channelPartner.Id), mgiContext);
					if (terminal != null)
					{
						TempData["Message"] = "Terminal Name is unavailable, please use different terminal name";
						TempData["TerminalModel"] = terminalModel;
						return RedirectToAction("TerminalSetup");
					}
					isSuccess = AddTerminal(terminalModel, desktop, location, npsTerminal, terminalName, mgiContext);
				}
				else
				{
					isSuccess = UpdateTerminal(terminalModel, desktop, location, npsTerminal, mgiContext);
				}

				if (isSuccess)
				{
					CreateTerminalCookie(terminalModel.TerminalName);
				}

				return RedirectToAction("Logout", "SSO");
			}
			else// if (terminalModel.TIM == (short)TerminalIdentificationMechanism.HostName)
			{
				ChannelPartner channelPartner = Session["ChannelPartner"] as ChannelPartner;
				Terminal terminal = desktop.LookupTerminal(agentSessionId, terminalName, Convert.ToInt32(channelPartner.Id), mgiContext);
				if (terminal != null)
				{
					isSuccess = UpdateTerminal(terminalModel, desktop, location, npsTerminal, mgiContext);
				}
				else
				{
					isSuccess = AddTerminal(terminalModel, desktop, location, npsTerminal, terminalName, mgiContext);
				}

				return RedirectToAction("Logout", "SSO");
			}
		}

		[HttpGet]
		[Authorize(Roles = "Manager, SystemAdmin, Tech")]
		public ActionResult GetNpsTerminal(string locationId)
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			List<SelectListItem> npsTerminalCollection = GetNpsTerminalsByLocationId(locationId, 0, mgiContext);
			return Json(npsTerminalCollection, JsonRequestBehavior.AllowGet);
		}

		[Authorize(Roles = "Manager, SystemAdmin, Teller, ComplianceManager, Tech")]
		public ActionResult DisplayChooseLocation(long dt)
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			var htSessions = (Hashtable)Session["HTSessions"];
			var agentSession = ((Server.Data.AgentSession)(htSessions["TempSessionAgent"]));
			Terminal terminal = agentSession.Terminal;
			TerminalModel terminalModel;
			//If Terminal Exists without Location
			if (terminal != null)
			{
				terminalModel = new TerminalModel()
				{
					Id = terminal.Id,
					TerminalName = terminal.Name,
					IpAddress = terminal.IpAddress,
					Locations = GetLocations(mgiContext),
					Location = null,
					NpsTerminal = terminal.PeripheralServer.Id.ToString()
				};
			}
			else
			{ //If Terminal Not Exists				
				terminalModel = new TerminalModel();
				terminalModel.Id = 0;
				terminalModel.TerminalName = Session["HostName"] == null ? "" : Session["HostName"].ToString();
				terminalModel.IpAddress = "";
				terminalModel.Locations = GetLocations(mgiContext);
				terminalModel.Location = null;
				terminalModel.NpsTerminal = null;
			}

			if (terminalModel.Locations != null)
				terminalModel.Locations.First().Text = " Choose Location";

			return PartialView("_partialChooseTerminalLocation", terminalModel);
		}

		[HttpPost]
		[Authorize(Roles = "Manager, SystemAdmin, Teller, ComplianceManager, Tech")]
		public ActionResult ChooseLocation(TerminalModel terminal)
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			Desktop client = new Desktop();
			bool isUpdateSuccess = false;
            long agentSessionId = GetAgentSessionId();
			terminal.Locations = GetLocations(mgiContext);
			if (terminal.Locations != null)
				terminal.Locations.First().Text = " Choose Location";

			try
			{
				if (terminal.Location != null && terminal.Location != "0")
				{
					//Lookup for existing Terminal
					Terminal existingTerminal = client.LookupTerminal(terminal.Id, mgiContext);

					Server.Data.Location location = client.GetLocationDetailsForEdit(Convert.ToString(agentSessionId), long.Parse(terminal.Location), mgiContext);
					Session["CurrentLocation"] = location.LocationName;

					ChannelPartner channelPartner = client.GetChannelPartner(Session["ChannelPartnerName"].ToString(), mgiContext);

					if (existingTerminal != null)
					{
						//IF Terminal Exists update with Location
						existingTerminal.Location = location;
						existingTerminal.PeripheralServer.Location = location;

						isUpdateSuccess = client.UpdateTerminal(agentSessionId, existingTerminal, mgiContext);
					}
					else
					{
						//If Terminal not Exists Create New Terminal
						existingTerminal = new Terminal();
						existingTerminal.Location = location;
						existingTerminal.Name = Session["HostName"] == null ? "" : Session["HostName"].ToString();
						existingTerminal.ChannelPartner = channelPartner;

						isUpdateSuccess = client.CreateTerminal(agentSessionId, existingTerminal, mgiContext);
					}

					if (isUpdateSuccess)
					{
						existingTerminal = client.LookupTerminal(agentSessionId, existingTerminal.Name, Convert.ToInt32(channelPartner.Id), mgiContext);
						long sessionId = Convert.ToInt64(Session["SessionId"]);
						if (client.UpdateSession(sessionId, existingTerminal, mgiContext))
							UpdateSession(existingTerminal);

						return RedirectToAction("CustomerSearch", "CustomerSearch");
					}
				}
			}
			catch (Exception ex)
			{
				ViewBag.IsException = true;
				ViewBag.ExceptionMsg = System.Web.HttpUtility.JavaScriptStringEncode(ex.Message);
				Login loginModel = new Login();
				return View("Login", loginModel);
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
		public ActionResult MoneyOrderTest()
		{
			CheckImage moneyOrder = new CheckImage();
			Desktop desktop = new Desktop();
			MGIContext mgiContext = new MGIContext();
            CheckPrint checkPrint = desktop.GenerateMoneyOrderDiagnostics(GetAgentSessionId(), mgiContext);
			moneyOrder.PrintData = MGI.Channel.DMS.Web.Common.ShoppingCartHelper.PrepareCheckForPrinting(checkPrint);

			return View("MoneyOrderTest", moneyOrder);
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

		private TerminalModel BuildTerminalModel(long agentSessionId, List<SelectListItem> locations, TerminalModel terminalModel, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			terminalModel = TempData["TerminalModel"] as TerminalModel;
			List<SelectListItem> npsTerminals = null;

			if (locations.Count == 1)
			{
				npsTerminals = GetNpsTerminalsByLocationId(locations.FirstOrDefault().Value, 0, mgiContext);
			}
			else
			{
				if (terminalModel.NpsTerminal != null && string.Compare(terminalModel.NpsTerminal, "select", true) != 0)
				{
					npsTerminals = GetNpsTerminalsByLocationId(terminalModel.Location, Convert.ToInt64(terminalModel.NpsTerminal), mgiContext);
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
			ViewBag.PeripheralUrl = GetPeripheralUrl(agentSessionId, Convert.ToInt64(terminalModel.NpsTerminal), mgiContext);
			return terminalModel;
		}

		private string GetPeripheralUrl(long agentSessionId, long npsTerminalId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			Desktop desktop = new Desktop();
			string peripheralUrl = string.Empty;
            NpsTerminal npsTerminal = desktop.LookupNpsTerminal(agentSessionId, npsTerminalId, mgiContext);
			if (npsTerminal != null)
			{
				if (npsTerminal.PeripheralServiceUrl != null)
				{
					peripheralUrl = npsTerminal.PeripheralServiceUrl;
				}
			}
			return peripheralUrl;
		}

		private TerminalModel BuildTerminalModel(long agentSessionId, Desktop desktop, List<SelectListItem> locations, string terminalName, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			TerminalModel terminalModel = null;
			ChannelPartner channelPartner = Session["ChannelPartner"] as ChannelPartner;
			Terminal terminal = null;

			if (!string.IsNullOrWhiteSpace(terminalName))
			{
				terminal = desktop.LookupTerminal(agentSessionId, terminalName, Convert.ToInt32(channelPartner.Id), mgiContext);
			}

			if (terminal != null)
			{
				List<SelectListItem> npsTerminals = null;
				long peripheralServerId = (terminal.PeripheralServer != null) ? terminal.PeripheralServer.Id : 0;
				npsTerminals = GetNpsTerminalsByLocationId(terminal.Location.Id.ToString(), peripheralServerId, mgiContext);
				npsTerminals.Insert(0, new SelectListItem() { Selected = true, Text = "Select", Value = "0" });
				if (npsTerminals != null)
				{
					terminalModel = new TerminalModel()
					{
						Id = terminal.Id,
						TerminalName = terminal.Name,
						IpAddress = terminal.IpAddress,
						Locations = locations,
						Location = terminal.Location.Id.ToString(),
						NpsTerminal = peripheralServerId == 0 ? string.Empty : peripheralServerId.ToString(),
						NpsTerminals = npsTerminals
					};
				}
			}
			return terminalModel;
		}

		private bool UpdateTerminal(TerminalModel terminalModel, Desktop desktop, Server.Data.Location location, Server.Data.NpsTerminal npsTerminal, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			bool isUpdateSuccess = false;
			Terminal existingTerminal = null;
            long agentSessionId = GetAgentSessionId();
			if (terminalModel.TIM == (short)TerminalIdentificationMechanism.YubiKey)
			{
				ChannelPartner channelPartner = Session["ChannelPartner"] as ChannelPartner;
				existingTerminal = desktop.LookupTerminal(agentSessionId, terminalModel.TerminalName, Convert.ToInt32(channelPartner.Id), mgiContext);
			}
			else
			{
				existingTerminal = desktop.LookupTerminal(terminalModel.Id, mgiContext);
			}

			if (existingTerminal != null)
			{
				existingTerminal.Name = terminalModel.TerminalName;
				existingTerminal.Location = location;
				existingTerminal.PeripheralServer = npsTerminal;

				isUpdateSuccess = desktop.UpdateTerminal(agentSessionId, existingTerminal, mgiContext);
			}

			return isUpdateSuccess;
		}

		private bool AddTerminal(TerminalModel terminalModel, Desktop desktop, Server.Data.Location location, Server.Data.NpsTerminal npsTerminal, string terminalName, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			bool isSuccess = false;
			if (Session["ChannelPartner"] != null)
			{
				ChannelPartner channelPartner = Session["ChannelPartner"] as ChannelPartner;
				Terminal terminal = new Terminal()
				{
					Name = terminalName,
					IpAddress = terminalModel.IpAddress,
					MacAddress = "",
					ChannelPartner = channelPartner,
					Location = location,
					PeripheralServer = npsTerminal,
				};

                isSuccess = desktop.CreateTerminal(GetAgentSessionId(), terminal, mgiContext);
			}
			return isSuccess;
		}

		private List<SelectListItem> GetNpsTerminalsByLocationId(string locationId, long selectedNpsTerminal, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			Desktop desktop = new Desktop();
			long agentSessionId = Convert.ToInt64(Session["sessionId"].ToString());
            List<NpsTerminal> npsTerminals = desktop.LookupNpsTerminalByLocationID(agentSessionId, Convert.ToInt64(locationId), mgiContext);
			ChannelPartner channelPartner = Session["ChannelPartner"] as ChannelPartner;

			string TerminalName = GetHostName();

			NpsTerminal npsterminal = desktop.LookupNpsTerminalByName(agentSessionId, TerminalName, channelPartner, mgiContext);

			if ( npsterminal != null && !npsTerminals.Any(x => x.Name == npsterminal.Name))
			{
				//npsTerminals.Add(npsterminal);
				//npsTerminals = npsTerminals.OrderBy(x => x.Name).ToList();
				npsTerminals.Insert(0, npsterminal);
			}

			List<SelectListItem> npsTerminalCollection = new List<SelectListItem>();

			foreach (var npsTerminal in npsTerminals)
			{
				npsTerminalCollection.Add(
					new SelectListItem()
					{
						Text = npsTerminal.Name,
						Value = npsTerminal.Id.ToString(),
						Selected = (npsTerminal.Id == selectedNpsTerminal)
					});
			}
			return npsTerminalCollection;
		}

		private List<SelectListItem> GetLocations(MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			Desktop desktop = new Desktop();
			List<SelectListItem> locations = new List<SelectListItem>();

			string channelPartner = Session["ChannelPartnerName"].ToString();
			if (!string.IsNullOrWhiteSpace(channelPartner))
			{
				long channelPartnerId = desktop.GetChannelPartner(channelPartner, mgiContext).Id;
				var availableLocations = desktop.GetAllLocationNames().FindAll(loc => loc.ChannelPartnerId == channelPartnerId);

				foreach (var location in availableLocations)
				{
					locations.Add(new SelectListItem() { Text = location.LocationName, Value = location.Id.ToString() });
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

		private void UpdateSession(Terminal terminal)
		{
			Session["IsTerminalSetup"] = 1;
			Session["IsPeripheralServerSetUp"] = IsPeripheralServerSetUp(terminal) ? 1 : 0;

			var htSessions = (Hashtable)Session["HTSessions"];
			var agentSession = ((Server.Data.AgentSession)(htSessions["TempSessionAgent"]));
			agentSession.Terminal = terminal;

			htSessions["TempSessionAgent"] = agentSession;
			this.Session["HTSessions"] = htSessions;
		}



		#endregion
	}
}
