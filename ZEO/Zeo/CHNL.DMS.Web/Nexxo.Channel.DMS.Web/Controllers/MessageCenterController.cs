using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Models;
using TCF.Channel.Zeo.Web.Common;
#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
#endregion

namespace TCF.Channel.Zeo.Web.Controllers
{
    [SkipNoDirectAccess]
    public class MessageCenterController : BaseController
    {
        public JsonResult GetMessageData()
        {
            List<ZeoClient.AgentMessage> agentMessages = new List<ZeoClient.AgentMessage>();
            try
            {
                ZeoClient.ZeoContext context = GetZeoContext();
                JsonResult json = new JsonResult();
                ZeoClient.ZeoServiceClient alloyClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.Response getAgentResponse = alloyClient.GetAgentMessages(context);
                agentMessages = getAgentResponse.Result as List<ZeoClient.AgentMessage>;
            }
            catch { }
            return Json(agentMessages ?? new List<ZeoClient.AgentMessage>(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowChatPopup(long transactionId)
        {
            try
            {
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.ZeoServiceClient alloyClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.Response getMessageDetailsResponse = alloyClient.GetMessageDetails(transactionId, context);
                if (WebHelper.VerifyException(getMessageDetailsResponse)) throw new ZeoWebException(getMessageDetailsResponse.Error.Details);
                ZeoClient.AgentMessage messageDetails = getMessageDetailsResponse.Result as ZeoClient.AgentMessage;
                Chat chat = new Chat()
                {
                    CheckAmount = messageDetails.Amount,
                    CheckStatus = messageDetails.TransactionState,
                    CustomerName = messageDetails.CustomerFirstName + " " + messageDetails.CustomerLastName,
                    TicketNumber = messageDetails.TicketNumber,
                    TransactionId = messageDetails.TransactionId
                };

                return PartialView("_partialChatCenter", chat);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }

        }

        public ActionResult GetMessageDetails(long transactionId)
        {
            try
            {
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.ZeoServiceClient alloyClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.Response getMessageResponse = alloyClient.GetMessageDetails(transactionId, context);
                if (WebHelper.VerifyException(getMessageResponse)) throw new ZeoWebException(getMessageResponse.Error.Details);
                ZeoClient.AgentMessage agetMessage = getMessageResponse.Result as ZeoClient.AgentMessage;
                return Json(agetMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public ActionResult StatusChangedPopUp()
        {
            try
            {
                ViewBag.StatusChangedMessage = App_GlobalResources.Nexxo.StatusChangedMessage;
                return PartialView("_CheckStatusChanged");
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }
    }
}
