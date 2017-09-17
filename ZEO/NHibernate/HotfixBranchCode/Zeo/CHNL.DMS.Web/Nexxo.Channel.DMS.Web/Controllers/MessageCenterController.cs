using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.ServiceClient.DMSService;
using MGI.Channel.DMS.Server.Data;
using System.Collections;
using System.Web.Script.Serialization;
using MGI.Channel.DMS.Web.Common;
using System.ServiceModel;
using MGI.Common.Sys;
using MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.DMS.Web.Controllers
{
	[SkipNoDirectAccess]
	public class MessageCenterController : BaseController
	{
		public JsonResult GetMessageData()
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			JsonResult json = new JsonResult();
			Desktop desktop = new Desktop();
			System.Collections.Hashtable htSessions = (System.Collections.Hashtable)Session["HTSessions"];
			MGI.Channel.DMS.Server.Data.AgentSession agentSession = ((MGI.Channel.DMS.Server.Data.AgentSession)(htSessions["TempSessionAgent"]));
			List<AgentMessage> agentMessages = desktop.GetAgentMessages(long.Parse(agentSession.SessionId), mgiContext);
			return Json(agentMessages, JsonRequestBehavior.AllowGet);
		}

        public ActionResult ShowChatPopup(long transactionId)
        {
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            Desktop client = new Desktop();
            System.Collections.Hashtable htSessions = (System.Collections.Hashtable)Session["HTSessions"];
            AgentMessage messageDetails = client.GetMessageDetails(long.Parse(((MGI.Channel.DMS.Server.Data.AgentSession)(htSessions["TempSessionAgent"])).SessionId), transactionId, mgiContext);

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

        public ActionResult GetMessageDetails(long transactionId)
        {
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            Desktop client = new Desktop();
            System.Collections.Hashtable htSessions = (System.Collections.Hashtable)Session["HTSessions"];
            AgentMessage agetMessage = client.GetMessageDetails(long.Parse(((MGI.Channel.DMS.Server.Data.AgentSession)(htSessions["TempSessionAgent"])).SessionId), transactionId, mgiContext);
            return Json(agetMessage, JsonRequestBehavior.AllowGet);
        }
	}
}
