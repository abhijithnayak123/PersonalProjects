using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.DMS.Server.Data;
using System.Collections;
using MGI.Channel.DMS.Web.Common;
using MGI.Common.Util;
//TODO Merge using System.Web.Http;

namespace MGI.Channel.DMS.Web.Controllers
{
    [Authorize(Roles = "SystemAdmin, Tech")]
    public class LocationsController : BaseController
    {
        //
        // GET: /Locations/
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [CustomHandleErrorAttribute(ViewName = "CustomerSearch", MasterName = "_Menu", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.CustomerSearch")]
        public ActionResult EnterNewLocation(string message = "", bool IsException = false, string ExceptionMsg = "")
        {
            Session["activeButton"] = "Locations";
            Locations newLocation = new Locations();
            Desktop desktop = new Desktop();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            newLocation.LLocationUSStates = desktop.GetUSStates(GetAgentSessionId(), mgiContext);
            newLocation.LLocationStatus = desktop.GetLocationStatus();
            newLocation.LTimeZone = TimeZoneHelper.GetTimeZones();
            newLocation.AddEdit = "Enter New Locations";
           
            ViewBag.IsException = IsException;
            ViewBag.ExceptionMsg = ExceptionMsg;
            ViewBag.Navigation = Resources.NexxoSiteMap.ManageLocations;
            return View("EnterNewLocation", "_Menu", newLocation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [CustomHandleErrorAttribute(ActionName = "EnterNewLocation", ControllerName = "Locations", ResultType = "redirect")]
        public ActionResult ViewLocationSummary(bool IsException = false, string ExceptionMsg = "")
        {
            Desktop desktop = new Desktop();
            Locations locations = new Locations();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            //locations.LocationLists = desktop.GetAllLocationNames();
			locations.LocationLists = desktop.GetAllLocationNames(long.Parse(Session["sessionId"].ToString()), mgiContext);
                  
            locations.AddEdit = "Enter New Locations";       

            ViewBag.IsException = IsException;
            ViewBag.ExceptionMsg = ExceptionMsg;

            return View("LocationSummary", "_Menu", locations);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locations"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomHandleErrorAttribute(ViewName = "EnterNewLocation", MasterName = "_Menu")]
        public ActionResult SaveNewLocation(Locations locations)
        {
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            Desktop desktop = new Desktop();
			long Id = 0;
            long agentSessionId = GetAgentSessionId();

            locations.LLocationUSStates = desktop.GetUSStates(agentSessionId, mgiContext);
            locations.LLocationStatus = desktop.GetLocationStatus();
            locations.LTimeZone = TimeZoneHelper.GetTimeZones();

            locations.AddEdit = (locations.Source == "Save" ? "Edit Location" : "Enter New Locations");

            MGI.Channel.DMS.Server.Data.Location dmsLocation = desktop.GetLocationDetailsForEdit(agentSessionId, locations.LocationName, mgiContext);
            if (dmsLocation != null && dmsLocation.Id != locations.Id)
                throw new Exception("Location Name already exists in DMS");

            MGI.Channel.DMS.Server.Data.Location existingLocation = desktop.GetLocationDetailsForEdit(Convert.ToString(agentSessionId), locations.Id, mgiContext);
            MGI.Channel.DMS.Server.Data.Location newLocation = new MGI.Channel.DMS.Server.Data.Location();
            newLocation.BankID = locations.BankID;
            newLocation.BranchID = locations.BranchID;
			newLocation.LocationIdentifier = locations.LocationIdentifier;
            newLocation.LocationName = locations.LocationName;
            newLocation.IsActive = locations.LocationStatus == "Active";
            newLocation.Address1 = locations.Address1;
            newLocation.Address2 = locations.Address2;
            newLocation.City = NexxoUtil.TrimString(locations.City);
            newLocation.State = locations.LocationUSState;
            newLocation.ZipCode = locations.ZipCode;
            newLocation.TimezoneID = string.IsNullOrWhiteSpace(locations.TimeZone) ? "Select" : locations.TimeZone;
            newLocation.ChannelPartnerId = ((MGI.Channel.DMS.Web.Models.BaseModel)(locations)).channelPartner.Id;
            newLocation.PhoneNumber = string.IsNullOrWhiteSpace(locations.Phone) ? string.Empty : locations.Phone.Replace("-", "");
			newLocation.NoOfCounterIDs = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["NoOfCounterIDs"]);

			if (existingLocation == null)
			{
                Id = desktop.SaveLocation(agentSessionId, newLocation, mgiContext);
			}
			else
			{
				newLocation.Id = Id = existingLocation.Id;
				newLocation.RowGuid = existingLocation.RowGuid;
				desktop.UpdateLocation(agentSessionId, newLocation, mgiContext);
			}

			return RedirectToAction("GetProcessorCredentials", "Locations", new { Id = Id});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationName"></param>
        /// <returns></returns>
        [CustomHandleErrorAttribute(ActionName = "ViewLocationSummary", ControllerName = "Locations", ResultType = "redirect")]
        public ActionResult EditLocation(string locationId)
        {
            long agentSessionId = GetAgentSessionId();
            Locations editLocation = new Locations();
            Desktop desktop = new Desktop();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            editLocation.LLocationStatus = desktop.GetLocationStatus();
			editLocation.LTimeZone = TimeZoneHelper.GetTimeZones();

            MGI.Channel.DMS.Server.Data.Location location = desktop.GetLocationDetailsForEdit(Convert.ToString(agentSessionId), Convert.ToInt64(locationId), mgiContext);
            editLocation.BankID = location.BankID;
            editLocation.BranchID = location.BranchID;
			editLocation.LocationIdentifier = location.LocationIdentifier;
            editLocation.LocationName = location.LocationName;
            editLocation.LocationStatus = (location.IsActive == true) ? "Active" : "Inactive";
            editLocation.Address1 = location.Address1;
            editLocation.Address2 = location.Address2;
            editLocation.City = location.City;           

            if (string.IsNullOrWhiteSpace(location.State) == false)
            {
                editLocation.LLocationUSStates = desktop.GetUSStates(agentSessionId, mgiContext);
                editLocation.LocationUSState = editLocation.LLocationUSStates.Where(c => c.Value == location.State).ElementAt(0).Value;
            }
            editLocation.ZipCode = location.ZipCode;
            editLocation.TimeZone = location.TimezoneID;
            editLocation.Phone = location.PhoneNumber;
            editLocation.AddEdit = "Edit Location";
            editLocation.Id = location.Id;
            editLocation.rowguid = location.RowGuid;
            
            return View("EnterNewLocation", "_Menu", editLocation);
        }

		public ActionResult SaveLocationConfirmation()
		{
			return PartialView("_SaveLocationConfirmationMessage");
		}

		//
		// GET: /Locations/
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		//[CustomHandleErrorAttribute(ActionName = "GetProcessorCredentials", ControllerName = "Locations", ResultType = "redirect")]
		public ActionResult GetProcessorCredentials(long Id = 0)
		{
			Desktop desktop = new Desktop();
			ProcessorCredentialViewModel vwModel = new ProcessorCredentialViewModel();
			List<ProcessorCrendential> processorcredentials = new List<ProcessorCrendential>();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			List<MGI.Channel.DMS.Server.Data.ProcessorCredential> credentials = new List<MGI.Channel.DMS.Server.Data.ProcessorCredential>();

			if (Id > 0)
			{
				credentials = desktop.GetLocationProcessorCredentials(GetAgentSessionId(), Id, mgiContext);

				if (credentials.Count > 0)
				{
					processorcredentials = credentials.Select(x => new ProcessorCrendential()
					{
						Identifier = x.Identifier,
						UserName = x.UserName,
						Password = x.Password,
						ProcessorID = x.Id,
						ProviderID = x.ProviderId
					}).ToList();
				}
			}
			
			vwModel.LocationID = Id;
			vwModel.Credentials = processorcredentials;

			return View("ProcessorCredentials", "_Menu", vwModel);
		}

		[HttpPost]
		[CustomHandleErrorAttribute(ViewName = "ProcessorCredentials", MasterName = "_Menu")]
		public ActionResult SaveProcessorCredential(ProcessorCrendential proccred)
		{
			Desktop desktop = new Desktop();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			MGI.Channel.DMS.Server.Data.ProcessorCredential credential = new ProcessorCredential()
			{
				Id = proccred.ProcessorID,
				Identifier = proccred.Identifier,
				UserName = proccred.UserName,
				Password = proccred.Password,
				ProviderId = proccred.ProviderID
			};

            desktop.SaveLocationProcessorCredentials(GetAgentSessionId(), proccred.LocationID, credential, mgiContext);
			return RedirectToAction("GetProcessorCredentials", new { Id = proccred.LocationID });
		}

		public ActionResult WarningPopUp(string name)
		{
			return PartialView("_WarningPopup", name);
		}

    }
}
