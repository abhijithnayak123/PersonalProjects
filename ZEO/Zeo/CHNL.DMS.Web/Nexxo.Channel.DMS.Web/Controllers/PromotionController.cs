using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Common;
using TCF.Channel.Zeo.Web.Models;
using TCF.Zeo.Common.Util;
#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
#endregion

namespace TCF.Channel.Zeo.Web.Controllers
{
    public class PromotionController : BaseController
    {
        [HttpGet]
        [CustomHandleError(ControllerName = "CustomerSearch", ActionName = "CustomerSearch", ResultType = "redirect")]
        public ActionResult GetPromotion(bool IsException = false, string ExceptionMessage = "")
        {
            try
            {
                Session["activeButton"] = "promotion";
                Session["Promotion"] = null;
                Session["PromotionSearch"] = null;

                PromotionSearch promotionsearch = new PromotionSearch();
                promotionsearch.ProductTypes = getProducts();
                ViewBag.IsException = IsException;
                ViewBag.ExceptionMessage = ExceptionMessage;

                return View("PromotionHistory", "_Menu", promotionsearch);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        [HttpPost]
        public ActionResult GetPromotion(PromotionSearch promotion)
        {
            try
            {
                Session["PromotionSearch"] = promotion;
                promotion.ProductTypes = getProducts();
                return View("PromotionHistory", "_Menu", promotion);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        [HttpPost]
        public ActionResult GetPromotionGrid(string sidx, string sord, int page = 1, int rows = 5)
        {
            var promotions = getPromotions();
            var totalRecords = promotions.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

            var data = (from s in promotions
                        select new
                        {
                            id = s.PromotionId.ToString(),
                            cell = new object[]
                            {
                                s.PromotionName,
                                string.IsNullOrWhiteSpace(s.PromotionDescription) ? "NA" : s.PromotionDescription,
                                s.Product?.Name,
                                string.IsNullOrWhiteSpace(s.Provider.Name) ? "NA" : s.Provider.Name,
                                s.Priority,
                                s.StartDate.Value.ToString("MM/dd/yyyy"),
                                s.EndDate.Value.ToString("MM/dd/yyyy"),
                                Convert.ToString(s.PromotionStatus)?.Replace("InProgress", "In Progress")?.Replace("InActive","In Active"),
                                (int)s.PromotionStatus                              
                            }
                        }).ToArray();

            var jsonData = new
            {
                display = true,
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = data.Skip((page - 1) * rows).Take(rows)
            };

            return Json(jsonData, "text/html", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [CustomHandleError(ControllerName = "Promotion", ActionName = "GetPromotion", ResultType = "redirect")]
        public ActionResult AddPromotion(string id, bool IsException = false, string ExceptionMessage = "")
        {
            try
            {
                Promotion promotion = new Promotion();
                promotion.ProductTypes = getProducts();
                promotion.PromotionStatus = Helper.PromotionStatus.InProgress.ToString(); // By default promotion should be in InProgress State
                ZeoClient.Promotion zeoPromotion = getPromotion();

                if (!string.IsNullOrWhiteSpace(id) && zeoPromotion.PromotionDetail == null)
                {
                    ZeoClient.ZeoServiceClient zeoServiceClient = new ZeoClient.ZeoServiceClient();
                    ZeoClient.ZeoContext context = GetZeoContext();
                    ZeoClient.Response response = zeoServiceClient.GetPromotion(long.Parse(id), context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    zeoPromotion = response.Result as ZeoClient.Promotion;
                    if(zeoPromotion != null && zeoPromotion.PromotionDetail != null)
                        zeoPromotion.PromotionDetail.FreeTxnCount = zeoPromotion?.PromotionDetail?.FreeTxnCount == 0 ? null : zeoPromotion?.PromotionDetail?.FreeTxnCount;
                    Session["Promotion"] = zeoPromotion;
                }

                if (zeoPromotion.PromotionDetail != null)
                {
                    promotion.PromotionId = zeoPromotion.PromotionDetail.PromotionId;
                    promotion.PromotionName = zeoPromotion.PromotionDetail.PromotionName;
                    promotion.PromoDescription = zeoPromotion.PromotionDetail.PromotionDescription;
                    promotion.ProductType = zeoPromotion.PromotionDetail.Product?.Id.ToString();
                    promotion.Provider = zeoPromotion.PromotionDetail.Provider?.Id.ToString();
                    promotion.Priority = zeoPromotion.PromotionDetail.Priority?.ToString();
                    promotion.StartDate = zeoPromotion.PromotionDetail.StartDate?.ToString("MM/dd/yyyy");
                    promotion.EndDate = zeoPromotion.PromotionDetail.EndDate?.ToString("MM/dd/yyyy");
                    promotion.PromotionStatus = zeoPromotion.PromotionDetail.PromotionStatus.ToString();
                    promotion.IsNextCustomerSession = zeoPromotion.PromotionDetail.IsNextCustomerSession;
                    promotion.IsOverridable = zeoPromotion.PromotionDetail.IsOverridable;
                    promotion.IsPrintable = zeoPromotion.PromotionDetail.IsPrintable;
                    promotion.IsSystemApplied = zeoPromotion.PromotionDetail.IsSystemApplied;
                    promotion.ProviderId = zeoPromotion.PromotionDetail.Provider?.Id.ToString();
                    if (zeoPromotion != null && zeoPromotion.PromotionDetail != null)
                        promotion.FreeTxnCount = zeoPromotion.PromotionDetail.FreeTxnCount == 0 ? null : zeoPromotion.PromotionDetail.FreeTxnCount;
                    promotion.Stackable = zeoPromotion.PromotionDetail.Stackable;
                    promotion.IsPromotionHidden = zeoPromotion.PromotionDetail.IsPromotionHidden;
                    if (zeoPromotion.PromotionDetail.Provider == null || string.IsNullOrEmpty(zeoPromotion.PromotionDetail.PromotionName))
                        ViewBag.Disabled = true;
                    else
                        ViewBag.Disabled = false;
                }
                else
                    ViewBag.Disabled = true;

                DisablingTrainStopRequired();
                ViewBag.IsException = IsException;
                ViewBag.ExceptionMessage = ExceptionMessage;

                return View("CreatePromotion", "_Menu", promotion);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        [HttpPost]
        [CustomHandleError(ControllerName = "Promotion", ActionName = "AddPromotion", ResultType = "redirect")]
        public ActionResult AddPromotion(Promotion promotion)
        {
            try
            {
                ZeoClient.ZeoServiceClient zeoServiceClient = new ZeoClient.ZeoServiceClient();

                ZeoClient.ZeoContext context = GetZeoContext();

                ZeoClient.PromotionDetail promoDetail = PromoDetailServiceMapper(promotion);

                ZeoClient.Response response = zeoServiceClient.SavePromoDetails(promoDetail, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                promotion.PromotionId = (long)response.Result;

                addPromotion(promotion, null, null);

                return RedirectToAction("AddQualifier");
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        [CustomHandleError(ControllerName = "Promotion", ActionName = "AddPromotion", ResultType = "redirect")]
        public ActionResult AddQualifier(string id, bool IsException = false, string ExceptionMessage = "")
        {
            try
            {
                Qualifier qualifier = new Qualifier();
                qualifier.ProductTypes = getProducts();
                ZeoClient.Promotion promo = getPromotion();
                qualifier.PromotionName = promo?.PromotionDetail?.PromotionName;
                qualifier.PromotionId = promo?.PromotionDetail?.PromotionId ?? 0;
                ViewBag.ChangeButtonText = "Add";

                if (!string.IsNullOrWhiteSpace(id))
                {
                    ZeoClient.Qualifier zeoQualifier = promo.Qualifiers.FirstOrDefault(x => x.QualifierId == long.Parse(id) || x.RowId == int.Parse(id));
                    qualifier.QualifierId = zeoQualifier.QualifierId;
                    qualifier.QualifierProduct = zeoQualifier.QualifierProduct?.Id.ToString();
                    qualifier.TransactionEndDate = zeoQualifier.TrxEndDate?.ToString("MM/dd/yyyy");
                    qualifier.TransactionAmount = zeoQualifier.TrxAmount;
                    qualifier.MinimumTrxCount = zeoQualifier.MinTrxCount;
                    qualifier.IsPaidFee = zeoQualifier.IsPaidFee;
                    qualifier.TrxStates = GetSelectedValues(qualifier.TrxStates, zeoQualifier.TransactionStates);
                    qualifier.RowId = zeoQualifier.RowId;
                    qualifier.PromotionId = zeoQualifier.PromotionId;
                    ViewBag.ChangeButtonText = "Update";
                }
                else
                {
                    qualifier.RowId = promo.Qualifiers.Count + 1;
                    qualifier.TransactionEndDate = promo.PromotionDetail.EndDate?.ToString("MM/dd/yyyy");
                }

                DisablingTrainStopRequired();
                ViewBag.PromoStartDate = promo?.PromotionDetail?.StartDate?.ToString("MM/dd/yy");
                ViewBag.PromoEndDate = promo?.PromotionDetail?.EndDate?.ToString("MM/dd/yy");
                ViewBag.IsException = IsException;
                ViewBag.ExceptionMessage = ExceptionMessage;

                return View("AddQualifier", "_Menu", qualifier);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        [HttpPost]
        [CustomHandleError(ControllerName = "Promotion", ActionName = "AddQualifier", ResultType = "redirect")]
        public ActionResult AddQualifier(Qualifier qualifier, string save)
        {
            try
            {
                ZeoClient.ZeoServiceClient zeoServiceClient = new ZeoClient.ZeoServiceClient();

                ZeoClient.ZeoContext context = GetZeoContext();

                ZeoClient.Qualifier qualifierDetail = QualifierServiceMapper(qualifier);

                ZeoClient.Response response = zeoServiceClient.SavePromoQualifier(qualifierDetail, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                qualifier.QualifierId = (long)response.Result;

                addPromotion(null, qualifier, null);

                if (!string.IsNullOrWhiteSpace(save))
                {
                    return RedirectToAction("AddQualifier");
                }
                return RedirectToAction("AddProvision");
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        [HttpPost]
        public ActionResult GetQualifiers(string sidx, string sord, int page = 1, int rows = 5)
        {
            ZeoClient.Promotion promotion = getPromotion();
            List<ZeoClient.Qualifier> qualifiers = promotion.Qualifiers.Where(x => x.IsActive == true)?.ToList();
            var totalRecords = qualifiers.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
            var allList = new Qualifier().TrxStates;


            var data = (from s in qualifiers
                        select new
                        {
                            id = s.QualifierId == 0 ? s.RowId : s.QualifierId,
                            cell = new object[]
                            {
                                promotion.PromotionDetail.PromotionName,
                                s.QualifierProduct?.Id == 0 ? "NA" : getProducts().FirstOrDefault(x => x.Value == s.QualifierProduct?.Id.ToString())?.Text,
                                string.IsNullOrWhiteSpace(s.TransactionStates) ? "NA" : string.Join(",",  (s.TransactionStates.Split(',').Join(allList, loc => loc, l => l.Value, (loc, l) => new { l.Text})).ToList().Select(x => x.Text)),
                                s.TrxEndDate?.ToString("MM/dd/yyyy"),
                                s.TrxAmount == null ? "NA" : s.TrxAmount?.ToString("C2"),
                                s.MinTrxCount == null ? "NA" : s.MinTrxCount.ToString(),
                                s.IsPaidFee == true ? "Yes" : "No",
                                ""
                            }
                        }).ToArray();

            var jsonData = new
            {
                display = true,
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = data.Skip((page - 1) * rows).Take(rows)
            };

            return Json(jsonData, "text/html", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowPopWarringQualifier(long qualifierId)
        {
            ViewBag.Id = qualifierId;
            return PartialView("_partialQualifierDeleteConform");
        }

        public JsonResult DeleteQualifier(long qualifierId)
        {
            ZeoClient.ZeoServiceClient zeoServiceClient = new ZeoClient.ZeoServiceClient();

            ZeoClient.ZeoContext context = GetZeoContext();

            ZeoClient.Promotion promo = getPromotion();

            ZeoClient.Qualifier qualifier = promo.Qualifiers.FirstOrDefault(x => x.QualifierId == qualifierId || x.RowId == (int)qualifierId);

            var index = promo.Qualifiers.FindIndex(x => x.QualifierId == qualifierId || x.RowId == (int)qualifierId);

            if (index >= 0)
            {
                promo.Qualifiers.RemoveAt(index);

                ZeoClient.Response response = zeoServiceClient.DeletePromoQualifier(qualifier.QualifierId, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                Session["Promotion"] = promo;
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [CustomHandleError(ControllerName = "Promotion", ActionName = "AddPromotion", ResultType = "redirect")]
        public ActionResult AddProvision(string id, bool IsException = false, string ExceptionMessage = "")
        {
            try
            {
                Provision provision = new Provision();
                ZeoClient.Promotion promo = getPromotion();
                ZeoClient.HelperProviderId providerId;
                Enum.TryParse(Convert.ToString(promo.PromotionDetail.Provider?.Id), out providerId);
                List<SelectListItem> checkTypes = promo.PromotionDetail.Product.Id == (int)ZeoClient.HelperTransactionType.ProcessCheck ? GetCheckTypes(providerId) : DefaultSelectList();
                checkTypes.RemoveAt(0);
                provision.CheckTypes = checkTypes;
                provision.Locations = GetStatesAndLocations();
                ViewBag.StateLocations = GetSelectedStatesAndLocations(provision.Locations, null);
                provision.PromotionName = promo?.PromotionDetail?.PromotionName;
                provision.PromotionId = promo?.PromotionDetail?.PromotionId ?? 0;
                provision.LGroup = getGroups();
                ViewBag.ChangeButtonText = "Add";
                if (!string.IsNullOrWhiteSpace(id))
                {
                    ZeoClient.Promotion promotion = promo;
                    ZeoClient.Provision zeoProvision = promotion.Provisions.FirstOrDefault(x => x.ProvisionId == long.Parse(id) || x.RowId == int.Parse(id));
                    provision.ProvisionId = zeoProvision.ProvisionId;
                    provision.MaxAmount = zeoProvision.MaxTrxAmount;
                    provision.MinAmount = zeoProvision.MinTrxAmount;
                    provision.Value = zeoProvision.Value;
                    provision.DiscountType = zeoProvision.DiscountType.ToString("D");
                    provision.CheckTypes = GetSelectedValues(provision.CheckTypes, zeoProvision.CheckTypeIds);
                    ViewBag.StateLocations = GetSelectedStatesAndLocations(provision.Locations, zeoProvision.LocationIds);
                    provision.LGroup = GetSelectedValues(provision.LGroup, zeoProvision.Groups);
                    provision.RowId = zeoProvision.RowId;
                    // provision.PromotionId = zeoProvision.PromotionId;
                    ViewBag.ChangeButtonText = "Update";
                }
                else
                {
                    provision.RowId = promo.Provisions.Count + 1;
                }

                DisablingTrainStopRequired();
                ViewBag.DisableCheckType = (promo.PromotionDetail.Product.Id != (int)Helper.Product.ProcessCheck);
                ViewBag.IsException = IsException;
                ViewBag.ExceptionMessage = ExceptionMessage;

                return View("AddProvision", "_Menu", provision);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        [HttpPost]
        [CustomHandleError(ControllerName = "Promotion", ActionName = "AddProvision", ResultType = "redirect")]
        public ActionResult AddProvision(Provision provision, string save)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ZeoClient.ZeoServiceClient zeoServiceClient = new ZeoClient.ZeoServiceClient();

                    ZeoClient.ZeoContext context = GetZeoContext();

                    ZeoClient.Provision provisionDetail = ProvisionServiceMapper(provision);

                    ZeoClient.Response response = zeoServiceClient.SavePromoProvision(provisionDetail, context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                    provision.ProvisionId = (long)response.Result;

                    addPromotion(null, null, provision);
                }

                if (string.IsNullOrWhiteSpace(save))
                    return RedirectToAction("PromotionSummary");
                else
                    return RedirectToAction("AddProvision");
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        [HttpPost]
        public ActionResult GetProvisions(string sidx, string sord, int page = 1, int rows = 5)
        {
            ZeoClient.Promotion promotion = getPromotion();

            List<ZeoClient.Provision> provisions = promotion.Provisions.Where(x => x.IsActive == true)?.ToList();

            var totalRecords = provisions.Count;
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
            var allLocations = GetStatesAndLocations();
            ZeoClient.HelperProviderId providerId;
            Enum.TryParse(Convert.ToString(promotion.PromotionDetail.Provider?.Id), out providerId);
            List<SelectListItem> allCheckTypes = promotion.PromotionDetail.Product.Id == (int)ZeoClient.HelperTransactionType.ProcessCheck ? GetCheckTypes(providerId) : DefaultSelectList();
            allCheckTypes.RemoveAt(0);
            var groups = getGroups();

            var data = (from s in provisions
                        select new
                        {
                            id = s.ProvisionId == 0 ? s.RowId : s.ProvisionId,
                            cell = new object[]
                            {
                                promotion.PromotionDetail.PromotionName,
                                string.IsNullOrWhiteSpace(s.LocationIds) ? "NA" : s.LocationIds.Split(',').Count() == allLocations.Count ? "All Locations" : (string.Join(", ",  (s.LocationIds.Split(',').Join(allLocations, loc => loc, l => Convert.ToString(l.Id), (loc, l) => new { l.Code})).ToList().Select(x => x.Code))),
                                string.IsNullOrWhiteSpace(s.CheckTypeIds) ? "NA" :s.CheckTypeIds.Split(',').Count() == allCheckTypes.Count ? "All CheckTypes" : (string.Join(", ",  (s.CheckTypeIds.Split(',').Join(allCheckTypes, ch => ch, ct => ct.Value, (ch, ct) => new { ct.Text})).ToList().Select(x => x.Text))),
                                string.IsNullOrWhiteSpace(s.Groups) ? "NA" : string.Join(", ",  (s.Groups.Split(',').Join(groups, g => g, gr => gr.Value, (g, gr) => new { gr.Text})).ToList().Select(x => x.Text)),
                                s.MinTrxAmount == null ? "NA" : s.MinTrxAmount?.ToString("C2"),
                                s.MaxTrxAmount == null ? "NA" : s.MaxTrxAmount?.ToString("C2"),
                                Convert.ToString(s.DiscountType) == Helper.DiscountType.Percentage.ToString() ? s.Value?.ToString() + "%" : Convert.ToDecimal(s.Value).ToString("C2"),
                                Convert.ToString(s.DiscountType).Replace("FlatRate","Flat Rate").Replace("FixedFee","Fixed Fee"),
                                ""
                            }
                        }).ToArray();

            var jsonData = new
            {
                display = true,
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = data.Skip((page - 1) * rows).Take(rows)
            };

            return Json(jsonData, "text/html", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowPopWarringProvision(long provisionId)
        {
            ViewBag.Id = provisionId;
            return PartialView("_partialProvisionDeleteConform");
        }

        public JsonResult DeleteProvision(long provisionId)
        {
            ZeoClient.ZeoServiceClient zeoServiceClient = new ZeoClient.ZeoServiceClient();

            ZeoClient.ZeoContext context = GetZeoContext();

            ZeoClient.Promotion promo = getPromotion();

            ZeoClient.Provision provision = promo.Provisions.FirstOrDefault(x => x.ProvisionId == provisionId || x.RowId == (int)provisionId);

            var index = promo.Provisions.FindIndex(x => x.ProvisionId == provisionId || x.RowId == (int)provisionId);

            if (index >= 0)
            {
                promo.Provisions.RemoveAt(index);

                ZeoClient.Response response = zeoServiceClient.DeletePromoProvision(provision.ProvisionId, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                Session["Promotion"] = promo;
            }

            DisablingTrainStopRequired();

            return Json(new { success = true, disableTrainStop = ViewBag.DisableTrainStop }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PromotionSummary(bool IsException = false, string ExceptionMessage = "")
        {
            Promotion promotion = new Promotion();

            ZeoClient.Promotion zeoPromotion = getPromotion();

            promotion.PromotionName = zeoPromotion.PromotionDetail.PromotionName;
            promotion.PromoDescription = zeoPromotion.PromotionDetail.PromotionDescription;
            promotion.ProductType = getProducts().FirstOrDefault(x => x.Value == zeoPromotion.PromotionDetail.Product.Id.ToString())?.Text;
            if (zeoPromotion.PromotionDetail.Provider != null && zeoPromotion.PromotionDetail.Provider.Id != 0)
            {
                promotion.Provider = getProviders(int.Parse(zeoPromotion.PromotionDetail.Product.Id.ToString())).FirstOrDefault(x => x.Value == zeoPromotion.PromotionDetail.Provider.Id.ToString())?.Text;
            }
            promotion.Priority = zeoPromotion.PromotionDetail.Priority?.ToString();
            promotion.StartDate = zeoPromotion.PromotionDetail.StartDate?.ToString("MM/dd/yyyy");
            promotion.EndDate = zeoPromotion.PromotionDetail.EndDate?.ToString("MM/dd/yyyy");
            promotion.PromotionStatus = zeoPromotion.PromotionDetail.PromotionStatus.ToString();
            promotion.IsNextCustomerSession = zeoPromotion.PromotionDetail.IsNextCustomerSession;
            promotion.IsOverridable = zeoPromotion.PromotionDetail.IsOverridable;
            promotion.IsPrintable = zeoPromotion.PromotionDetail.IsPrintable;
            promotion.IsSystemApplied = zeoPromotion.PromotionDetail.IsSystemApplied;
            promotion.FreeTxnCount = zeoPromotion.PromotionDetail.FreeTxnCount;
            promotion.Stackable = zeoPromotion.PromotionDetail.Stackable;
            promotion.IsPromotionHidden = zeoPromotion.PromotionDetail.IsPromotionHidden;

            ViewBag.IsException = IsException;
            ViewBag.ExceptionMessage = ExceptionMessage;

            return View("PromotionSummary", promotion);
        }

        [HttpPost]
        [CustomHandleError(ControllerName = "Promotion", ActionName = "PromotionSummary", ResultType = "redirect")]
        public ActionResult PromotionSummary()
        {
            try
            {
                ZeoClient.ZeoServiceClient zeoServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.Promotion promo = Session["Promotion"] as ZeoClient.Promotion;
                //When doing copy from existing the and click on save and continue buttons on all screen still summary page and click submit button on summary page need to verify all qualifier and provision get added or not if not add
                if (promo.Provisions.Count(x => x.ProvisionId == 0) > 0 || promo.Qualifiers.Count(x => x.QualifierId == 0) > 0)
                {
                    ZeoClient.Response zeoresponse = zeoServiceClient.CreateAndUpdatePromotion(promo, context);
                    if (WebHelper.VerifyException(zeoresponse)) throw new ZeoWebException(zeoresponse.Error.Details);
                }

                ZeoClient.Response response = zeoServiceClient.UpdatePromotionStatus(promo.PromotionDetail.PromotionId,ServiceClient.ZeoService.HelperPromotionStatus.Ready, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                return RedirectToAction("GetPromotion");
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public JsonResult ValidatePromoName(string promoName, long promotionId)
        {
            ZeoClient.ZeoServiceClient zeoServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.ZeoContext context = GetZeoContext();
            ZeoClient.Response response = zeoServiceClient.ValidatePromoName(promoName, promotionId, context);
            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
            var jsonData = new { success = (bool)response.Result };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetProviders(string productId)
        {
            try
            {
                List<SelectListItem> providers = new List<SelectListItem>();

                if (!string.IsNullOrWhiteSpace(productId))
                    providers = getProviders(Convert.ToInt32(productId));
                else
                    providers.Add(DefaultListItem());

                return Json(providers, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public ActionResult GetExistingPromotions(string promotionId)
        {
            ZeoClient.Promotion promo = getPromotion();
            promo.PromotionDetail.PromotionId = 0;
            promo.PromotionDetail.PromotionName = "";
            if (promo.PromotionDetail.StartDate < DateTime.Now)
            {
                promo.PromotionDetail.StartDate = null;
                promo.PromotionDetail.EndDate = null;
            }
            promo.PromotionDetail.PromotionStatus = ServiceClient.ZeoService.HelperPromotionStatus.InProgress; 
            int i = 1;
            promo.Qualifiers.ForEach(x => { x.QualifierId = 0; x.RowId = i; i++; });
            i = 1;
            promo.Provisions.ForEach(x => { x.ProvisionId = 0; x.RowId = i; i++; });
            Session["Promotion"] = promo;
            return RedirectToAction("AddPromotion");
        }

        public ActionResult ShowPromotionDetails(string id)
        {
            ZeoClient.ZeoServiceClient zeoServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.ZeoContext context = GetZeoContext();
            ZeoClient.Response response = zeoServiceClient.GetPromotion(long.Parse(id), context);
            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
            ZeoClient.Promotion zeoPromotion = response.Result as ZeoClient.Promotion;
            Session["Promotion"] = zeoPromotion;

            Promotion promotion = new Promotion();
            promotion.PromotionId = zeoPromotion.PromotionDetail.PromotionId;
            promotion.PromotionName = zeoPromotion.PromotionDetail.PromotionName;
            promotion.PromoDescription = zeoPromotion.PromotionDetail.PromotionDescription;
            promotion.ProductType = getProducts().FirstOrDefault(x => x.Value == zeoPromotion.PromotionDetail.Product.Id.ToString())?.Text;
            promotion.Provider = getProviders(int.Parse(zeoPromotion.PromotionDetail.Product.Id.ToString())).FirstOrDefault(x => x.Value == zeoPromotion.PromotionDetail.Provider.Id.ToString())?.Text;
            promotion.Priority = zeoPromotion.PromotionDetail.Priority?.ToString();
            promotion.StartDate = zeoPromotion.PromotionDetail.StartDate?.ToString("MM/dd/yyyy");
            promotion.EndDate = zeoPromotion.PromotionDetail.EndDate?.ToString("MM/dd/yyyy");
            promotion.PromotionStatus = zeoPromotion.PromotionDetail.PromotionStatus.ToString();
            promotion.IsNextCustomerSession = zeoPromotion.PromotionDetail.IsNextCustomerSession;
            promotion.IsOverridable = zeoPromotion.PromotionDetail.IsOverridable;
            promotion.IsPrintable = zeoPromotion.PromotionDetail.IsPrintable;
            promotion.IsSystemApplied = zeoPromotion.PromotionDetail.IsSystemApplied;
            if (zeoPromotion != null && zeoPromotion.PromotionDetail != null)
                promotion.FreeTxnCount = zeoPromotion.PromotionDetail.FreeTxnCount == 0 ? null : zeoPromotion.PromotionDetail.FreeTxnCount;
            promotion.Stackable = zeoPromotion.PromotionDetail.Stackable;
            promotion.IsPromotionHidden = zeoPromotion.PromotionDetail.IsPromotionHidden;
            return PartialView("_partialDisplayDetailsOfPromo", promotion);
        }

        public ActionResult ShowConfirmationPopUp(long promotionId)
        {
            ViewBag.PromotionId = promotionId;
            return PartialView("_partialConfirmationPopUp");
        }

        public JsonResult ClearSession()
        {
            Session["Promotion"] = null;
            return Json(new { sucess = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowPageLeavePopup(bool data)
        {
            ViewBag.IsQualifier = data;
            return PartialView("_PageLeavePopup");
        }

        public ActionResult RedirectToProvisionOrSummary(bool isQualifier)
        {
            try
            {
                ZeoClient.Promotion promo = Session["Promotion"] as ZeoClient.Promotion;
                if (isQualifier)
                {
                    if (promo.Qualifiers.Count(x => x.QualifierId == 0) > 0)
                    {
                        ZeoClient.Response response = new ZeoClient.ZeoServiceClient().AddUpdateQualifiers(promo.Qualifiers, promo.PromotionDetail.PromotionId, (DateTime)promo.PromotionDetail.StartDate, GetZeoContext());
                        if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                        List<ZeoClient.Qualifier> qualifiers = response.Result as List<ZeoClient.Qualifier>;
                        promo.Qualifiers = qualifiers;
                        Session["Promotion"] = promo;
                    }
                    return RedirectToAction("AddProvision", "Promotion");
                }
                else
                {
                    if (promo.Provisions.Count(x => x.ProvisionId == 0) > 0)
                    {
                        ZeoClient.Response response = new ZeoClient.ZeoServiceClient().AddUpdateProvisions(promo.Provisions, promo.PromotionDetail.PromotionId, GetZeoContext());
                        if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                        List<ZeoClient.Provision> qualifiers = response.Result as List<ZeoClient.Provision>;
                        promo.Provisions = qualifiers;
                        Session["Promotion"] = promo;
                    }
                    return RedirectToAction("PromotionSummary", "Promotion");
                }
            }
            catch (Exception ex) 
            {
                VerifyException(ex);
                if (isQualifier)
                    RedirectToAction("AddQualifier", "Promotion");
                else
                    RedirectToAction("AddProvision", "Promotion");
                return null;
            }
        }

        public ActionResult DeletePromotion(long promotionId)
        {
            try
            {
                ZeoClient.Response response = new ZeoClient.ZeoServiceClient().UpdatePromotionStatus(promotionId, ServiceClient.ZeoService.HelperPromotionStatus.Deleted, GetZeoContext());
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                Session["Promotion"] = null;

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public ActionResult UpdatePromotionStatus(long promotionId, int status)
        {
            try
            {
                ZeoClient.Response response = new ZeoClient.ZeoServiceClient().UpdatePromotionStatus(promotionId, (ServiceClient.ZeoService.HelperPromotionStatus)status, GetZeoContext());
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public ActionResult ShowConfirmationPopUpForStatus(long promotionId, int status)
        {
            ViewBag.StatusName =  status == 1 ? "enable" : "disable";
            ViewBag.PromotionId = promotionId;
            ViewBag.Status = status;
            return PartialView("_partialConfirmationPopUpStatus");
        }

        #region Private methods
        private List<SelectListItem> getLocations()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();

            if (Session["Locations"] != null)
            {
                List<ZeoClient.MasterData> locations = Session["Locations"] as List<ZeoClient.MasterData>;

                locations.ForEach(i => selectList.Add(new SelectListItem()
                {
                    Value = i.Code,
                    Text = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(i.Name.ToLower())
                }));
            }
            else
            {
                ZeoClient.ZeoServiceClient zeoServiceClient = new ZeoClient.ZeoServiceClient();

                ZeoClient.ZeoContext context = GetZeoContext();

                ZeoClient.Response response = zeoServiceClient.GetStateNamesAndIdByChannelPartnerId(context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                List<ZeoClient.MasterData> locations = response.Result as List<ZeoClient.MasterData>;

                Session["Locations"] = locations;

                locations.ForEach(i => selectList.Add(new SelectListItem()
                {
                    Value = i.Code,
                    Text = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(i.Name.ToLower())
                }));
            }

            return selectList;
        }

        private List<SelectListItem> getProducts()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            List<ZeoClient.ProductProviderDetails> collections = getProductProvider();
            collections = collections.GroupBy(c => c.ProductId).Select(c => c.First()).ToList();
            SelectListItem type = null;
            selectList.Add(DefaultListItem());
            if (collections.Count > 0)
            {
                foreach (var item in collections)
                {
                    //Checking whether the feature is disabled in the database by System Admin/Tech user.
                    bool isEnable = isFeatureEnable(item.ProductName);

                    if (!isEnable)
                        continue;

                    type = new SelectListItem() { Text = item.ProductName, Value = Convert.ToString(item.ProductId) };
                    selectList.Add(type);
                }
            }
            return selectList;
        }

        private List<SelectListItem> getGroups()
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();

            if (Session["ListOfGroups"] == null)
            {
                ZeoClient.ZeoServiceClient zeoServiceClient = new ZeoClient.ZeoServiceClient();

                ZeoClient.ZeoContext context = GetZeoContext();

                ZeoClient.Response response = zeoServiceClient.GetPartnerGroups(context.ChannelPartnerId, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                List<ZeoClient.KeyValuePair> groups = response.Result as List<ZeoClient.KeyValuePair>;

                selectListItems.AddRange(groups.Select(x => new SelectListItem { Value = Convert.ToString(x.Key), Text = Convert.ToString(x.Value) }).ToList());

                Session["ListOfGroups"] = groups;
            }
            else
            {
                List<ZeoClient.KeyValuePair> groups = Session["ListOfGroups"] as List<ZeoClient.KeyValuePair>;

                selectListItems.AddRange(groups.Select(x => new SelectListItem { Value = Convert.ToString(x.Key), Text = Convert.ToString(x.Value) }).ToList());
            }

            return selectListItems;
        }

        private List<SelectListItem> getProviders(int productId)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            List<ZeoClient.ProductProviderDetails> collections = getProductProvider().FindAll(x => x.ProductId == productId);
            SelectListItem type = null;
            selectList.Add(DefaultListItem());
            if (collections.Count > 0)
            {
                foreach (var item in collections)
                {
                    type = new SelectListItem() { Text = item.ProviderName, Value = Convert.ToString(item.ProviderId) };
                    selectList.Add(type);
                }
            }
            return selectList;
        }

        private List<ZeoClient.ProductProviderDetails> getProductProvider()
        {
            List<ZeoClient.ProductProviderDetails> productDetails = new List<ZeoClient.ProductProviderDetails>();
            if (Session["ProductDetails"] != null)
            {
                productDetails = Session["ProductDetails"] as List<ZeoClient.ProductProviderDetails>;
                return productDetails;
            }
            ZeoClient.ZeoServiceClient zeoServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.ZeoContext context = GetZeoContext();
            ZeoClient.Response response = zeoServiceClient.GetProductProviderDetails(context);
            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
            productDetails = response.Result as List<ZeoClient.ProductProviderDetails>;
            Session["ProductDetails"] = productDetails;

            return productDetails;
        }

        private ZeoClient.Promotion getPromotion()
        {
            ZeoClient.Promotion promo = new ZeoClient.Promotion();
            if (Session["Promotion"] != null)
            {
                promo = Session["Promotion"] as ZeoClient.Promotion;
            }
            return promo;
        }

        private SelectListItem DefaultListItem()
        {
            return new SelectListItem() { Value = string.Empty, Text = "Select", Selected = true };
        }

        private void addPromotion(Promotion promotion, Qualifier qualifier, Provision provision)
        {
            ZeoClient.Promotion promo = new ZeoClient.Promotion();
            if (Session["Promotion"] != null)
            {
                promo = Session["Promotion"] as ZeoClient.Promotion;
            }
            else
            {
                promo.Qualifiers = new List<ZeoClient.Qualifier>();
                promo.Provisions = new List<ZeoClient.Provision>();
            }
            if (promotion != null)
            {
                promo.PromotionDetail = PromoDetailServiceMapper(promotion);
            }
            if (qualifier != null)
            {
                ZeoClient.Qualifier zeoQualifier = QualifierServiceMapper(qualifier);

                if (promo.Qualifiers == null)
                {
                    promo.Qualifiers = new List<ZeoClient.Qualifier>();
                    promo.Qualifiers.Add(zeoQualifier);
                }
                else if (promo.Qualifiers.Any(x => x.QualifierId == qualifier.QualifierId) && qualifier.QualifierId != 0)
                {
                    var index = promo.Qualifiers.FindIndex(x => x.QualifierId == qualifier.QualifierId);
                    promo.Qualifiers[index] = zeoQualifier;
                }
                else if (promo.Qualifiers.Any(x => x.RowId == qualifier.RowId) & qualifier.RowId != 0)
                {
                    var index = promo.Qualifiers.FindIndex(x => x.RowId == qualifier.RowId);
                    promo.Qualifiers[index] = zeoQualifier;
                }
                else
                {
                    promo.Qualifiers.Add(zeoQualifier);
                }

            }
            if (provision != null)
            {
                ZeoClient.Provision zeoProvision = ProvisionServiceMapper(provision);

                if (promo.Provisions == null)
                {
                    promo.Provisions = new List<ZeoClient.Provision>();
                    promo.Provisions.Add(zeoProvision);
                }
                else if (promo.Provisions.Any(x => x.ProvisionId == provision.ProvisionId) && provision.ProvisionId != 0)
                {
                    var index = promo.Provisions.FindIndex(x => x.ProvisionId == provision.ProvisionId);
                    promo.Provisions[index] = zeoProvision;
                }
                else if (promo.Provisions.Any(x => x.RowId == provision.RowId & provision.RowId != 0))
                {
                    var index = promo.Provisions.FindIndex(x => x.RowId == provision.RowId);
                    promo.Provisions[index] = zeoProvision;
                }
                else
                {
                    promo.Provisions.Add(zeoProvision);
                }
            }
            DisablingTrainStopRequired();

            Session["Promotion"] = promo;
        }

        private List<ZeoClient.PromotionDetail> getPromotions()
        {
            List<ZeoClient.PromotionDetail> promos = new List<ZeoClient.PromotionDetail>();
            ZeoClient.ZeoServiceClient zeoServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.ZeoContext context = GetZeoContext();
            ZeoClient.Response response = zeoServiceClient.GetPromotions(getPromotionforSearch(), context);
            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
            promos = response.Result as List<ZeoClient.PromotionDetail>;
            return promos;
        }

        private ZeoClient.PromotionSearchCriteria getPromotionforSearch()
        {
            ZeoClient.PromotionSearchCriteria promoSearchCriteria = new ServiceClient.ZeoService.PromotionSearchCriteria();
            if (Session["PromotionSearch"] != null)
            {
                PromotionSearch promo = Session["PromotionSearch"] as PromotionSearch;
                promoSearchCriteria.PromotionName = promo.PromotionName;

                if (!string.IsNullOrWhiteSpace(promo.StartDate))
                    promoSearchCriteria.StartDate = Convert.ToDateTime(promo.StartDate);
                else
                    promoSearchCriteria.StartDate = null;

                if (!string.IsNullOrWhiteSpace(promo.EndDate))
                    promoSearchCriteria.EndDate = Convert.ToDateTime(promo.EndDate);
                else
                    promoSearchCriteria.EndDate = null;

                promoSearchCriteria.Product = new ZeoClient.MasterData()
                {
                    Id = string.IsNullOrWhiteSpace(promo.ProductType) ? 0 : long.Parse(promo.ProductType)
                };

                promoSearchCriteria.ShowExpired = promo.ShowExpired;

            }
            return promoSearchCriteria;
        }

        private IEnumerable<SelectListItem> GetSelectedValues(IEnumerable<SelectListItem> allItems, string selectedItems)
        {
            if (!string.IsNullOrWhiteSpace(selectedItems))
            {
                allItems.Join(selectedItems.Split(',').ToList(), (a) => a.Value, (sl) => sl, (a, sl) =>
                                                        {
                                                            a.Selected = true;
                                                            return a;
                                                        }).ToList();
            }
            return allItems;
        }

        private void DisablingTrainStopRequired()
        {
            ZeoClient.Promotion promo = getPromotion();

            ViewBag.DisableTrainStop = promo?.Provisions?.Count(x => x.IsActive == true) > 0 ? false : true;
        } 

        private ZeoClient.PromotionDetail PromoDetailServiceMapper(Promotion promotion)
        {
            ZeoClient.PromotionDetail promoDetail = new ZeoClient.PromotionDetail();
            promoDetail.PromotionId = promotion.PromotionId;
            promoDetail.PromotionName = promotion.PromotionName;
            promoDetail.PromotionDescription = promotion.PromoDescription;
            promoDetail.Product = new ZeoClient.MasterData();
            promoDetail.Product.Id = Convert.ToInt32(promotion.ProductType);
            if (!string.IsNullOrWhiteSpace(promotion.Priority))
            {
                promoDetail.Priority = int.Parse(promotion.Priority);
            }
            promoDetail.StartDate = Convert.ToDateTime(promotion.StartDate);
            promoDetail.EndDate = Convert.ToDateTime(promotion.EndDate);
            if (!string.IsNullOrWhiteSpace(promotion.Provider))
            {
                promoDetail.Provider = new ZeoClient.MasterData();
                promoDetail.Provider.Id = Convert.ToInt32(promotion.Provider);
            }
            promoDetail.IsNextCustomerSession = promotion.IsNextCustomerSession;
            promoDetail.IsOverridable = promotion.IsOverridable;
            promoDetail.IsPrintable = promotion.IsPrintable;
            promoDetail.IsSystemApplied = promotion.IsSystemApplied;
            promoDetail.PromotionStatus = (ZeoClient.HelperPromotionStatus)Enum.Parse(typeof(ZeoClient.HelperPromotionStatus),promotion.PromotionStatus);
            promoDetail.FreeTxnCount = promotion.FreeTxnCount;
            promoDetail.Stackable = promotion.Stackable;
            promoDetail.IsPromotionHidden = promotion.IsPromotionHidden;
            return promoDetail;
        }

        private ZeoClient.Provision ProvisionServiceMapper(Provision provision)
        {
            ZeoClient.Provision zeoProvision = new ZeoClient.Provision();
            zeoProvision.LocationIds = provision.SelectedLocations;
            zeoProvision.CheckTypeIds = provision.SelectedCheckTypes;
            zeoProvision.MinTrxAmount = provision.MinAmount;
            zeoProvision.MaxTrxAmount = provision.MaxAmount;
            zeoProvision.Value = provision.Value;
            zeoProvision.DiscountType = (ZeoClient.HelperDiscountType)Enum.Parse(typeof(ZeoClient.HelperDiscountType), provision.DiscountType);
            zeoProvision.ProvisionId = provision.ProvisionId;
            zeoProvision.Groups = provision.SelectedGroupNames;
            zeoProvision.IsActive = true;
            zeoProvision.RowId = provision.RowId;
            zeoProvision.PromotionId = provision.PromotionId;

            return zeoProvision;
        }

        private ZeoClient.Qualifier QualifierServiceMapper(Qualifier qualifier)
        {
            ZeoClient.Qualifier zeoQualifier = new ZeoClient.Qualifier();
            zeoQualifier.QualifierProduct = new ZeoClient.MasterData();
            zeoQualifier.QualifierProduct.Id = Convert.ToInt16(qualifier.QualifierProduct);

            if (!string.IsNullOrWhiteSpace(qualifier.TransactionEndDate))
                zeoQualifier.TrxEndDate = Convert.ToDateTime(qualifier.TransactionEndDate);
            else
                zeoQualifier.TrxEndDate = null;

            zeoQualifier.TrxAmount = qualifier.TransactionAmount;
            zeoQualifier.MinTrxCount = qualifier.MinimumTrxCount;
            zeoQualifier.QualifierId = qualifier.QualifierId;
            zeoQualifier.TransactionStates = qualifier.SelectedTxnStates;
            zeoQualifier.IsPaidFee = qualifier.IsPaidFee;
            zeoQualifier.IsActive = true;
            if(qualifier.SelectedTxnStates != null)
                zeoQualifier.ConsiderParkedTxns = qualifier.SelectedTxnStates.Contains("6-8-5") ? true : false;
            zeoQualifier.RowId = qualifier.RowId;
            zeoQualifier.PromotionId = qualifier.PromotionId;
            zeoQualifier.TrxStartDate = getPromotion()?.PromotionDetail?.StartDate;

            return zeoQualifier;
        }

        public ActionResult ValidatePromoCode(int type, decimal amount, string promoCode, int providerId)
        {
            ZeoClient.ZeoServiceClient zeoServiceClient = new ZeoClient.ZeoServiceClient();
            try
            {
                ZeoClient.ZeoContext context = GetZeoContext();
                long promotionId = 0;
                bool isPromoCodeEmpty = true;
                if (string.IsNullOrWhiteSpace(promoCode))
                {
                    context.PromotionCode = string.Empty;
                    context.ProviderId = 0;
                }
                else
                {
                    isPromoCodeEmpty = false;
                    context.PromotionCode = promoCode;
                    context.ProviderId = providerId;
                    ZeoClient.HelperTransactionType transactionType = (ZeoClient.HelperTransactionType)type;
                    ZeoClient.Response response = zeoServiceClient.ValidateProviderPromotion(transactionType, amount, context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    promotionId = (long)response.Result;
                } 
                return Json(
                        new
                        {
                            data = promotionId,
                            errorMessage = "",//TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.PromotionEligibilityMessage,
                            isPromoCodeEmpty = isPromoCodeEmpty
                        }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        private List<ZeoClient.MasterData> GetStatesAndLocations()
        {
            List<ZeoClient.MasterData> stateLocations = new List<ZeoClient.MasterData>();

            var statesLoc = Session["Locations"] != null ? Session["Locations"] as List<ZeoClient.MasterData> : null;

            if (statesLoc != null && statesLoc.Count > 0)
            {
                stateLocations = Session["Locations"] as List<ZeoClient.MasterData>;
            }
            else
            {
                ZeoClient.ZeoServiceClient zeoServiceClient = new ZeoClient.ZeoServiceClient();

                ZeoClient.ZeoContext context = GetZeoContext();

                ZeoClient.Response response = zeoServiceClient.GetStateNamesAndIdByChannelPartnerId(context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                stateLocations = response.Result as List<ZeoClient.MasterData>;

                Session["Locations"] = stateLocations;
            }

            stateLocations.ForEach(s => s.Name = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(s.Name?.ToLower()));
            stateLocations.ForEach(s => s.Code = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(s.Code?.ToLower()));

            return stateLocations;
        }

        private object GetSelectedStatesAndLocations(List<ZeoClient.MasterData> statesLocations, string selectedStateLocations)
        {
            List<ZeoClient.MasterData> stateLocations = new List<ZeoClient.MasterData>();

            stateLocations.AddRange(statesLocations);

            List<object> items = new List<object>();

            if (!string.IsNullOrWhiteSpace(selectedStateLocations))
            {
                foreach (var item in selectedStateLocations.Split(','))
                {
                    var selItem = stateLocations.Where(x => Convert.ToString(x.Id) == item);

                    if (selItem != null)
                    {
                        items.Add(selItem.Select(a => new { Name = a.Name, Id = a.Id, Code = a.Code, Selected = true }).FirstOrDefault());
                        int itemIndex = stateLocations.IndexOf(selItem.FirstOrDefault());
                        if (itemIndex >= 0)
                            stateLocations.RemoveAt(itemIndex);
                    }
                }
                stateLocations.Select(x => new { Name = x.Name, Id = x.Id, Code = x.Code, Selected = false }).ToList().ForEach(a => items.Add(a));
            }
            else
            {
                stateLocations.Select(x => new { Name = x.Name, Id = x.Id, Code = x.Code, Selected = false }).ToList().ForEach(a => items.Add(a));
            }
            return items;
        }

        #endregion
    }
}
