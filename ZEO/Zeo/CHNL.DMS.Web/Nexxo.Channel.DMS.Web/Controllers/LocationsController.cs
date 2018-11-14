using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Models;
using TCF.Channel.Zeo.Web.Common;
#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using System.ServiceModel;
using TCF.Zeo.Common.Util;
#endregion

namespace TCF.Channel.Zeo.Web.Controllers
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
        [CustomHandleErrorAttribute(ViewName = "CustomerSearch", MasterName = "_Menu", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.CustomerSearch")]
        public ActionResult EnterNewLocation(string message = "", bool IsException = false, string ExceptionMessage = "")
        {
            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();

                Session["activeButton"] = "Locations";
                Locations newLocation = new Locations();

                ZeoClient.Response response = alloyServiceClient.GetUSStates(context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                newLocation.LLocationUSStates = ExtensionHelper.GetSelectListItems(response.Result).ToList();
                newLocation.LLocationStatus = GetLocationStatus();
                newLocation.LTimeZone = TimeZoneHelper.GetTimeZones();
                newLocation.AddEdit = "Enter New Locations";

                ViewBag.IsException = IsException;
                ViewBag.ExceptionMessage = ExceptionMessage;
                ViewBag.Navigation = Resources.NexxoSiteMap.ManageLocations;
                return View("EnterNewLocation", "_Menu", newLocation);
            }
            catch (Exception ex)
            {
                VerifyException(ex);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [CustomHandleErrorAttribute(ActionName = "EnterNewLocation", ControllerName = "Locations", ResultType = "redirect")]
        public ActionResult ViewLocationSummary(bool IsException = false, string ExceptionMessage = "")
        {
            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();


                Locations locations = new Locations();
                ZeoClient.Response response = alloyServiceClient.GetLocationsByChannelPartnerId(context.ChannelPartnerId, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                locations.LocationLists = response.Result as List<ZeoClient.Location>;

                locations.AddEdit = "Enter New Locations";

                ViewBag.IsException = IsException;
                ViewBag.ExceptionMessage = ExceptionMessage;

                return View("LocationSummary", "_Menu", locations);
            }
            catch (Exception ex)
            {
                VerifyException(ex);
                return null;
            }
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
            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.Response alloyResponse = new ZeoClient.Response();

                long Id = 0;
                long agentSessionId = GetAgentSessionId();

                ZeoClient.Response USStatesResponse = alloyServiceClient.GetUSStates(context);
                if (WebHelper.VerifyException(USStatesResponse)) throw new ZeoWebException(USStatesResponse.Error.Details);
                locations.LLocationUSStates = ExtensionHelper.GetSelectListItems(USStatesResponse.Result).ToList();

                locations.LLocationStatus = GetLocationStatus();
                locations.LTimeZone = TimeZoneHelper.GetTimeZones();

                locations.AddEdit = (locations.Source == "Save" ? "Edit Location" : "Enter New Locations");

                ZeoClient.Response response = alloyServiceClient.GetLocationById(locations.Id, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                List<ZeoClient.Location> existingLocations = response.Result as List<ZeoClient.Location>;
                ZeoClient.Location existingLocation = existingLocations.FirstOrDefault();
                ZeoClient.Location newLocation = new ZeoClient.Location();
                newLocation.BankID = locations.BankID;
                newLocation.BranchID = locations.BranchID;
                newLocation.LocationIdentifier = locations.LocationIdentifier;
                newLocation.LocationName = locations.LocationName;
                newLocation.IsActive = locations.LocationStatus == "Active";
                newLocation.Address1 = locations.Address1;
                newLocation.Address2 = locations.Address2;
                newLocation.City = AlloyUtil.TrimString(locations.City);
                newLocation.State = locations.LocationUSState;
                newLocation.ZipCode = locations.ZipCode;
                newLocation.TimezoneID = string.IsNullOrWhiteSpace(locations.TimeZone) ? "Select" : locations.TimeZone;
                newLocation.ChannelPartnerId = Convert.ToInt32(context.ChannelPartnerId);
                newLocation.PhoneNumber = string.IsNullOrWhiteSpace(locations.Phone) ? string.Empty : locations.Phone.Replace("-", "");
                newLocation.NoOfCounterIDs = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["NoOfCounterIDs"]);

                if (existingLocation == null)
                {
                    response = alloyServiceClient.ValidateLocation(newLocation, context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    context.TimeZone = newLocation.TimezoneID;
                    alloyResponse = alloyServiceClient.CreateLocation(newLocation, context);
                    if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                    Id = Convert.ToInt64(alloyResponse.Result);
                }
                else
                {
                    newLocation.LocationID = Id = existingLocation.LocationID;
                    alloyResponse = alloyServiceClient.UpdateLocation(newLocation, context);
                    if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);

                }

                return RedirectToAction("GetProcessorCredentials", "Locations", new { Id = Id });
            }
            catch (Exception ex)
            {
                VerifyException(ex);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationName"></param>
        /// <returns></returns>
        [CustomHandleErrorAttribute(ActionName = "ViewLocationSummary", ControllerName = "Locations", ResultType = "redirect")]
        public ActionResult EditLocation(long locationId)
        {
            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();

                long agentSessionId = GetAgentSessionId();
                Locations editLocation = new Locations();
                editLocation.LLocationStatus = GetLocationStatus();
                editLocation.LTimeZone = TimeZoneHelper.GetTimeZones();

                ZeoClient.Response response = alloyServiceClient.GetLocationById(locationId, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                List<ZeoClient.Location> locations = response.Result as List<ZeoClient.Location>;
                ZeoClient.Location location = locations.FirstOrDefault();
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
                    editLocation.LocationUSState = location.State;
                    ZeoClient.Response USStatesResponse = alloyServiceClient.GetUSStates(context);
                    if (WebHelper.VerifyException(USStatesResponse)) throw new ZeoWebException(USStatesResponse.Error.Details);
                    editLocation.LLocationUSStates = ExtensionHelper.GetSelectListItems(USStatesResponse.Result).ToList();
                    editLocation.LocationUSState = editLocation.LLocationUSStates.Where(c => c.Value == location.State).ElementAt(0).Value;

                }

                editLocation.ZipCode = location.ZipCode;
                editLocation.TimeZone = location.TimezoneID;
                editLocation.Phone = location.PhoneNumber;
                editLocation.AddEdit = "Edit Location";
                editLocation.Id = location.LocationID;


                return View("EnterNewLocation", "_Menu", editLocation);
            }
            catch (Exception ex)
            {
                VerifyException(ex);
                return null;
            }
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
        [CustomHandleErrorAttribute(ActionName = "EnterNewLocation", ControllerName = "Locations", ResultType = "redirect")]
        public ActionResult GetProcessorCredentials(long Id = 0, bool IsException = false, string ExceptionMessage = "")
        {
            try
            {
                ViewBag.IsException = IsException;
                ViewBag.ExceptionMessage = ExceptionMessage;


                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.Response alloyResponse = new ZeoClient.Response();

                ProcessorCredentialViewModel vwModel = new ProcessorCredentialViewModel();
                List<ProcessorCrendential> processorcredentials = new List<ProcessorCrendential>();
                List<ZeoClient.LocationProcessorCredentials> credentials = new List<ZeoClient.LocationProcessorCredentials>();

                if (Id > 0)
                {
                    ZeoClient.Response response = alloyServiceClient.GetLocationProcessorCredentials(Id, context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    credentials = response.Result as List<ZeoClient.LocationProcessorCredentials>;

                    if (credentials.Count > 0)
                    {
                        processorcredentials = credentials.Select(x => new ProcessorCrendential()
                        {
                            Identifier = x.Identifier,
                            Identifier2 = x.Identifier2,
                            UserName = x.UserName,
                            Password = x.Password,
                            ProviderID = x.ProviderId,
                            LocationID = x.locationId
                        }).ToList();
                    }
                }

                vwModel.LocationID = Id;
                vwModel.Credentials = processorcredentials;

                return View("ProcessorCredentials", "_Menu", vwModel);
            }
            catch (Exception ex)
            {
                VerifyException(ex);
                return null;
            }

        }

        [HttpPost]
        [CustomHandleErrorAttribute(ActionName = "GetProcessorCredentials", ControllerName = "Locations", ResultType = "redirect")]
        public ActionResult SaveProcessorCredential(ProcessorCrendential proccred)
        {
            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.Response alloyResponse = new ZeoClient.Response();

                ZeoClient.LocationProcessorCredentials locationModelCredential = new ZeoClient.LocationProcessorCredentials()
                {
                    locationId = proccred.LocationID,
                    Identifier = proccred.Identifier,
                    UserName = proccred.UserName,
                    Password = proccred.Password,
                    ProviderId = proccred.ProviderID,
                    Identifier2 = proccred.Identifier2
                };
                ZeoClient.Response response = alloyServiceClient.SaveLocationProcessorCredential(locationModelCredential, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                List<ZeoClient.LocationProcessorCredentials> credentials = response.Result as List<ZeoClient.LocationProcessorCredentials>;
                return RedirectToAction("GetProcessorCredentials", new { Id = proccred.LocationID });
            }
            catch (Exception ex)
            {
                VerifyException(ex);
                return null;
            }
        }

        public ActionResult WarningPopUp(string name)
        {
            return PartialView("_WarningPopup", name);
        }

        public List<SelectListItem> GetLocationStatus()
        {
            List<SelectListItem> locationStatus = new List<SelectListItem>();
            locationStatus.Add(DefaultListItem());
            locationStatus.Add(new SelectListItem() { Text = "Active", Value = "Active" });
            locationStatus.Add(new SelectListItem() { Text = "Inactive", Value = "Inactive" });
            return locationStatus;
        }

        private SelectListItem DefaultListItem()
        {
            return new SelectListItem() { Value = string.Empty, Text = "Select", Selected = true };
        }

    }
}
