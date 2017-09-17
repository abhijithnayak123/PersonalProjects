using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Biz.Partner.Data;

namespace MGI.Biz.Partner.Contract
{
    public interface ICustomerProspectService
    {
        /// <summary>
        /// Save initial registration data.
        /// </summary>
        /// <param name="agentSessionId">This is the unique identifier for agent session</param>
        /// <param name="sessionContext">A transient  instance of SessionContext[Class]</param>
        /// <param name="prospect">A transient instance Prospect[Class] contains customer details</param>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>The Alloy Id</returns>
        string SaveProspect(long agentSessionId, SessionContext sessionContext, Prospect prospect, MGI.Common.Util.MGIContext mgiContext);
           
        /// <summary>
        /// This method to Save Customer Details. In PNTR Database.
        /// </summary>
        /// <param name="agentSessionId">This is the unique identifier for agent session</param>
        /// <param name="sessionContext">A transient  instance of SessionContext[Class]</param>
        /// <param name="alloyId">This is the unique identifier for customer</param>
        /// <param name="prospect">A transient instance Prospect[Class] contains customer details</param>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        void SaveProspect(long agentSessionId, SessionContext sessionContext, long alloyId, Prospect prospect, MGI.Common.Util.MGIContext mgiContext);
        
        /// <summary>
        /// To get the customer details from PNTR Database.
        /// </summary>
        /// <param name="agentSessionId">This is the unique identifier for agent session</param>
        /// <param name="sessionContext">A transient  instance of SessionContext[Class]</param>
        /// <param name="alloyId">This is the unique identifier for customer</param>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Prospect Details.</returns>
        Prospect GetProspect(long agentSessionId, SessionContext sessionContext, long alloyId, MGI.Common.Util.MGIContext mgiContext);

        /// <summary>
        /// This method is to save the confirmation identification
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agent Session</param>
        /// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
        /// <param name="status">Conformation Status</param>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Unique Identifier for Conformation Identification</returns>
        string ConfirmIdentity(long agentSessionId, long customerSessionId, bool status, MGI.Common.Util.MGIContext mgiContext);
    }
}
