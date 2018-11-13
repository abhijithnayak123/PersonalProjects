using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.Common;
using MGI.Channel.DMS.Web.ServiceClient.DMSService;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;
using NexxoSOAPFault = MGI.Common.Sys.NexxoSOAPFault;
using SharedData = MGI.Channel.Shared.Server.Data;


namespace MGI.Channel.DMS.Web.ServiceClient
{
    public partial class Desktop
    {
        public const string UNITED_STATES = "UNITED STATES";
        //DMS Backend Service
        public DesktopServiceClient DesktopService { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Desktop()
        {
            DesktopService = new DesktopServiceClient();
        }
        public List<ReceiptData> GetSummaryReceipt(long customerSessionId, long cartId, MGIContext mgiContext)
        {
            try
            {

                return DesktopService.GetSummaryReceipt(customerSessionId, cartId, mgiContext).ToList();
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }
        }

        #region CHECK PROCESSING RELATED METHODS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="checkId"></param>
        /// <returns></returns>
        public Check GetCheckStatus(string customerSessionId, string checkId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetCheckStatus(long.Parse(customerSessionId), checkId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        public Check SubmitCheck(string customerSessionId, CheckSubmission check, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.SubmitCheck(long.Parse(customerSessionId), check, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="checkId"></param>
        /// <returns></returns>
        public void CancelCheck(string customerSessionId, string checkId, MGIContext mgiContext)
        {
            try
            {
                DesktopService.CancelCheck(long.Parse(customerSessionId), checkId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkId"></param>
        /// <returns></returns>
        public bool CanResubmit(long customerSessionId, string checkId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.CanResubmit(customerSessionId, checkId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public List<SelectListItem> GetCheckTypes(long customerSessionId, MGIContext mgiContext)
        {
            try
            {
                List<SelectListItem> list = new List<SelectListItem>();
                list.Add(DefaultListItem());

                string[] checkTypes = DesktopService.GetCheckTypes(customerSessionId, mgiContext);

                foreach ( var val in checkTypes )
                {
                    list.Add(new SelectListItem() { Text = val, Value = val });
                }

                return list;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }


        public TransactionFee GetCheckFee(string customerSessionId, CheckSubmission checkSubmit, MGIContext mgiContext)
        {

            try
            {
                return DesktopService.GetCheckFee(long.Parse(customerSessionId), checkSubmit, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }

            //return decimal.Round(.02m * amount, 2);  // hardcoded to 2% for now

        }

        public CheckProcessorInfo GetCheckProcessorInfo(string agentSessionId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetCheckProcessorInfo(long.Parse(agentSessionId), mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }
        public AgentMessage GetMessageDetails(long agentSessionId, long transactionId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetMessageDetails(agentSessionId, transactionId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }


        public bool RemoveCheck(long customerSessionId, long checkId, MGIContext mgiContext, bool isParkedTransaction = false)
        {
            try
            {
                return DesktopService.RemoveCheck(customerSessionId, checkId, isParkedTransaction, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        #endregion

        #region BILL PAY RELATED METHODS

        /// <summary>
        /// This will return the list of billers (actually products)
        /// </summary>
        /// <param name="channelPartnerID"></param>
        /// <param name="searchTerm"></param>
        /// <param name="context">context should contain LocationRegionID which is a guid</param>
        /// <returns></returns>
        public List<string> GetBillers(long customerSessionId, long channelPartnerID, string searchTerm, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetBillers(customerSessionId, channelPartnerID, searchTerm, mgiContext).ToList();
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// Get biller information by ID
        /// </summary>
        /// <param name="billerID"></param>
        /// <returns></returns>
        public Product GetBiller(long customerSessionId, long billerID, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetBiller(customerSessionId, billerID, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="billerName"></param>
        /// <returns></returns>
        public Product GetBiller(long customerSessionId, long channelPartnerID, string billerNameOrCode, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetBillerByName(customerSessionId, channelPartnerID, billerNameOrCode, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// CFP implementation
        /// </summary>
        /// <param name="customerAccountNo"></param>
        /// <param name="customerSessionId"></param>
        /// <returns></returns>
        /// <param name="alloyId"></param>
        public List<Product> GetFrequentBillers(long customerSessionId, long alloyId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetFrequentBillers(customerSessionId, alloyId, mgiContext).ToList();
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public List<Product> GetAllBillers(long customerSessionId, long channelPartnerID, Guid locationRegionID, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetAllBillers(customerSessionId, channelPartnerID, locationRegionID, mgiContext).ToList();
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public BillPayLocation GetLocations(long customerSessionId, string billerName, string accountNumber, decimal amount, MGIContext mgiContext)
        {
            try
            {
                BillPayLocation locations = DesktopService.GetLocations(customerSessionId, billerName, accountNumber, amount, mgiContext);
                return locations;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public BillFee GetFee(long customerSessionId, string billerNameOrCode, string accountNumber, decimal amount, BillerLocation location, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetFee(customerSessionId, billerNameOrCode, accountNumber, amount, location, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public BillerInfo GetBillerInfo(long customerSessionId, string billerNameOrCode, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetBillerInfo(customerSessionId, billerNameOrCode, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public List<Field> GetProviderAttributes(long customerSessionId, string billerNameOrCode, string location, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetProviderAttributes(customerSessionId, billerNameOrCode, location, mgiContext).ToList();
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public decimal GetCustomerBillFee(long productID)
        {
            return 2.0m;
        }

        public List<SelectListItem> DefaultSelectList()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            selectList.Add(DefaultListItem());
            return selectList;
        }

        public FavoriteBiller GetFavoriteBiller(long customerSessionId, string billerNameOrCode, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetFavoriteBiller(customerSessionId, billerNameOrCode, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public void StageBillPayment(long customerSessionId, long transactionID, MGIContext mgiContext)
        {
            try
            {
                DesktopService.StageBillPayment(customerSessionId, transactionID, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }

        }

        public long ValidateBillPayment(long customerSessionId, BillPayment payment, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.ValidateBillPayment(customerSessionId, payment, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public void CancelBillPayment(long customerSessionId, long transactionId, MGIContext mgiContext)
        {
            try
            {
                //DesktopService.CancelBillPayment(customerSessoinID, transactionId);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public Shared.Server.Data.CardInfo GetCardInfo(long customerSessionId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetCardInfo(customerSessionId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public void AddPastBillers(long customerSessionId, string cardNumber, MGIContext mgiContext)
        {
            try
            {
                DesktopService.AddPastBillers(customerSessionId, cardNumber, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }
        //Begin TA-191 Changes
        //       User Story Number: TA-191 | Web |   Developed by: Sunil Shetty     Date: 21.04.2015
        //       Purpose: The user / teller will have the ability to delete a specific biller from the Favorite Billers list. we are sending Customer session id and biller id.
        // the customer session id will get AlloyID/Pan ID and biller ID will help us to disable biller from tCustomerPreferedProducts table
        public void DeleteFavoriteBiller(long customerSessionId, long billerID, MGIContext mgiContext)
        {
            try
            {

                DesktopService.DeleteFavoriteBiller(customerSessionId, billerID, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }
        //End TA-191 Changes
        #endregion

        #region CUSTOMER RELATED METHODS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentSessionId"></param>
        /// <param name="customerAuthentication"></param>
        /// <returns></returns>
        public CustomerSession InitiateCustomerSession(string agentSessionId, long alloyId, int cardPresentedType, MGIContext mgiContext)
        {
            try
            {
                //Service method shoud be modified to accept only AlloyID instead of CustomerAuthentication object as second param.
                CustomerAuthentication customerAuthentication = new CustomerAuthentication() { AlloyID = alloyId };
                mgiContext.CardPresentedType = cardPresentedType;
                return DesktopService.InitiateCustomerSession(long.Parse(agentSessionId), customerAuthentication, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="customerSessionId"></param>
        /// <param name="IdentificationStatus"></param>
        /// <returns></returns>
        public string RecordIdentificationConfirmation(string agentId, string customerSessionId, bool IdentificationStatus, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.RecordIdentificationConfirmation(long.Parse(customerSessionId), agentId, IdentificationStatus, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentSessionId"></param>
        /// <param name="prospect"></param>
        /// <returns></returns>
        public string GeneratePAN(string agentSessionId, Prospect prospect, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.Create(long.Parse(agentSessionId), prospect, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentSessionId"></param>
        /// <param name="alloyId"></param>
        /// <param name="prospect"></param>
        public void SaveCustomerProfile(string agentSessionId, long alloyId, Prospect prospect, MGIContext mgiContext, bool editMode = false)
        {
            try
            {
                DesktopService.Save(long.Parse(agentSessionId), alloyId, prospect, mgiContext);
                if ( editMode )
                    DesktopService.UpdateCustomer(long.Parse(agentSessionId), alloyId, prospect, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentSessionId"></param>
        /// <param name="alloyId"></param>
        /// <returns></returns>
        public Prospect GetCustomerProfile(string agentSessionId, long alloyId, MGIContext mgiContext)
        {
            try
            {

                return DesktopService.GetProspect(long.Parse(agentSessionId), alloyId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public CustomerSearchResult[] SearchCustomers(string agentSessionId, CustomerSearchCriteria searchCriteria, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.SearchCustomers(long.Parse(agentSessionId), searchCriteria, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw ex;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fundPaymentId"></param>
        /// <returns></returns>
        /// <param name="sessionId"></param>
        public string[] GetReceiptData(string fundPaymentId, string sessionId, MGIContext mgiContext)
        {
            try
            {
                Dictionary<string, string> context = new Dictionary<string, string>();
                //return DesktopService.GetReceiptData(sessionId, fundPaymentId, context);
                return new string[0];
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelPartner"></param>
        /// <returns></returns>
        public string[] GetChannelLocations(long agentSessionId, string channelPartner, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.Locations(agentSessionId, channelPartner, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public AgentSession AuthenticateSSO(AgentSSO ssoAgent, string channelPartner, string terminalName, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.AuthenticateSSO(ssoAgent, channelPartner, terminalName, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public bool UpdateSession(long sessionId, Terminal terminal, MGIContext mgiContext)
        {
            return DesktopService.UpdateSession(sessionId, terminal, mgiContext);
        }

        public List<AgentMessage> GetAgentMessages(long agentSessionId, MGIContext mgiContext)
        {

            var msgs = DesktopService.GetAgentMessages(agentSessionId, mgiContext);
            List<AgentMessage> lst = new List<AgentMessage>();
            foreach ( var msg in msgs )
            {
                lst.Add(
                    new AgentMessage()
                    {
                        CustomerFirstName = msg.CustomerFirstName.Trim(),
                        CustomerLastName = msg.CustomerLastName.Trim(),
                        Amount = Convert.ToDecimal(msg.Amount).ToString("F"),
                        TransactionState = msg.TransactionState.Trim(),
                        TransactionId = msg.TransactionId,
                        TicketNumber = msg.TicketNumber,
                        DeclineMessage = msg.DeclineMessage
                    });
            }
            return lst;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelPartnerName"></param>
        /// <returns></returns>
        public ChannelPartner GetChannelPartner(string channelPartnerName, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.ChannelPartnerConfig(channelPartnerName, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public ChannelPartnerCertificate GetChannelPartnerCertificateInfo(long channelPartnerId, string issuer, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetChannelPartnerCertificateInfo(channelPartnerId, issuer, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Customer GetCustomerByUserName(string userName)
        {
            try
            {
                Customer customer = new Customer();
                customer.ID = new Identification();

                return customer;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentSessionId"></param>
        /// <param name="alloyId"></param>
        /// <returns></returns>
        public void NexxoActivate(string agentSessionId, long alloyId, MGIContext mgiContext)
        {
            try
            {
                DesktopService.NexxoActivate(long.Parse(agentSessionId), alloyId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentSessionId"></param>
        /// <param name="alloyId"></param>
        /// <returns></returns>
        public void ClientActivate(string agentSessionId, long alloyId, MGIContext mgiContext)
        {
            try
            {
                DesktopService.ClientActivate(long.Parse(agentSessionId), alloyId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public void UpdateCustomerToClient(string agentSessionId, long alloyId, MGIContext mgiContext)
        {
            try
            {
                DesktopService.UpdateCustomerToClient(long.Parse(agentSessionId), alloyId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }

        }

        public void CustomerSyncInFromClient(string agentSessionId, long alloyId, MGIContext mgiContext)
        {
            try
            {
                DesktopService.CustomerSyncInFromClient(long.Parse(agentSessionId), alloyId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }

        }

        public bool ValidateSSN(string agentSessionId, string SSN, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.ValidateSSN(long.Parse(agentSessionId), SSN, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// AL-231
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="purse"></param>
        /// <returns></returns>
        public bool CanChangeProfileStatus(long agentSessionId, long userId, string profileStatus, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.CanChangeProfileStatus(agentSessionId, userId, profileStatus, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> exception )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(exception));
            }
        }

        public bool ValidateCustomer(string agentSessionId, Prospect prospect, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.ValidateCustomer(long.Parse(agentSessionId), prospect, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// Get Customers Details By Search Parameters
        /// </summary>
        /// <param name="agentSessionId">Session Id</param>
        /// <param name="customerLookUpCriteria">Search Parameters</param>
        /// <returns></returns>
        public Prospect[] CustomerLookUp(string agentSessionId, Dictionary<string, object> customerLookUpCriteria, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            try
            {
                return DesktopService.CustomerLookUp(long.Parse(agentSessionId), customerLookUpCriteria, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw ex;
            }

        }

        /// <summary>
        /// Validate the Customer for InitiateCustomerSession
        /// </summary>
        /// <param name="agentSessionId"></param>
        /// <param name="alloyId"></param>
        public void ValidateCustomerStatus(string agentSessionId, long alloyId, MGIContext mgiContext)
        {
            try
            {

                DesktopService.ValidateCustomerStatus(long.Parse(agentSessionId), alloyId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public bool UpdateCounterId(long customerSessionId, MGIContext mgiContext)
        {
            return DesktopService.UpdateCounterId(customerSessionId, mgiContext);
        }
        #endregion

        #region MASTER DATA RELATED METHODS
        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="idType"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public IDRequirement GetIdRequirements(long agentSessionId, long channelPartnerId, string countryId, string idType, string state, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.IdRequirements(agentSessionId, channelPartnerId, countryId, idType, state, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CountryId"></param>
        /// <param name="GovtIdType"></param>
        /// <returns></returns>
        public List<SelectListItem> GetStates(long agentSessionId, long channelPartnerId, MGIContext mgiContext, string CountryId = null, string GovtIdType = null)
        {
            try
            {
                List<SelectListItem> StateList = new List<SelectListItem>();
                StateList.Add(DefaultListItem());
                if ( CountryId != null && GovtIdType != null )
                {
                    string[] WebStates = DesktopService.IdStates(agentSessionId, channelPartnerId, CountryId, GovtIdType, mgiContext);

                    Array.Sort(WebStates);

                    if ( WebStates != null &&  WebStates.Length > 0 )
                    {
                        StateList.AddRange(WebStates.Select(s => new SelectListItem() { Value = s, Text = s }));
                    }
                }
                return StateList;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        public List<MasterCountry> GetMasterCountries(long agentSessionId, long channelPartnerId, MGIContext mgiContext)
        {
            try
            {
                List<MasterCountry> mastercountries = new List<MasterCountry>();
                mastercountries = DesktopService.MasterCountries(agentSessionId, channelPartnerId, mgiContext).ToList();
                var topCountries = new[] { "UNITED STATES", "CANADA", "MEXICO" };
                var orderByCountryName = mastercountries.OrderByDescending(c => topCountries.Contains(c.Name.ToUpper())).ThenBy(c => c.Name).ToList();
                var topItem = orderByCountryName.Single(x => x.Name.ToUpper() == UNITED_STATES);
                var masterCountryList = new List<MasterCountry>();
                masterCountryList.Add(topItem);
                masterCountryList = masterCountryList.Concat(orderByCountryName.Where(c => c.Name.ToUpper() != UNITED_STATES)).ToList();
                return masterCountryList;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public List<SelectListItem> GetCountries(long agentSessionId, long channelPartnerId, MGIContext mgiContext)
        {
            try
            {
                List<SelectListItem> countries = new List<SelectListItem>();
                countries.Add(DefaultListItem());
                string[] WebCountries = DesktopService.IdCountries(agentSessionId, channelPartnerId, mgiContext);
                var topCountries = new[] { "UNITED STATES", "CANADA", "MEXICO" };
                var orderByCountryName = WebCountries.OrderByDescending(c => topCountries.Contains(c.ToUpper())).ThenBy(x => x);
                var topItem = orderByCountryName.Single(x => x.ToUpper() == UNITED_STATES);
                var masterCountryList = new List<string>();
                masterCountryList.Add(topItem);
                masterCountryList = masterCountryList.Concat(orderByCountryName.Where(p => p.ToUpper() != UNITED_STATES)).ToList();
                if ( masterCountryList != null )
                {
                    foreach ( var val in masterCountryList )
                    {
                        countries.Add(new SelectListItem() { Text = val, Value = val });
                    }
                }
                return countries;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetCountryOfBirth(long agentSessionId, string countryISOCode, MGIContext mgiContext)
        {
            string countryName = string.Empty;
            try
            {
                var country = DesktopService.GetMasterCountryByCode(agentSessionId, countryISOCode, mgiContext);

                if ( country != null )
                {
                    countryName = country.Name;
                }
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            return countryName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public List<SelectListItem> GetGovtIdType(long agentSessionId, long channelPartnerId, MGIContext mgiContext, string countryId = null)
        {
            try
            {
                List<SelectListItem> GovtIdList = new List<SelectListItem>();
                GovtIdList.Add(DefaultListItem());
                if ( countryId != null )
                {
                    string[] WebIdCountries = DesktopService.IdTypes(agentSessionId, channelPartnerId, countryId, mgiContext);
                    if ( WebIdCountries != null )
                    {
                        foreach ( var val in WebIdCountries )
                        {
                            GovtIdList.Add(new SelectListItem() { Text = val, Value = val });
                        }
                    }
                }
                return GovtIdList;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetLegalCodes(long agentSessionId, MGIContext mgiContext)
        {
            try
            {
                List<SelectListItem> LegalcodeList = new List<SelectListItem>();
                LegalcodeList.Add(DefaultListItem());

                LegalCode[] WebIdLegalCodes = DesktopService.GetLegalCodes(agentSessionId, mgiContext);
                if ( WebIdLegalCodes != null )
                {
                    foreach ( var val in WebIdLegalCodes )
                    {
                        LegalcodeList.Add(new SelectListItem() { Text = val.Name, Value = val.Code });
                    }
                }

                return LegalcodeList;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetOccupations(long agentSessionId, MGIContext mgiContext)
        {
            try
            {
                List<SelectListItem> OccupationList = new List<SelectListItem>();
                OccupationList.Add(DefaultListItem());

                Occupation[] Occupations = DesktopService.GetOccupations(agentSessionId, mgiContext);
                if ( Occupations != null )
                {
                    foreach ( var val in Occupations )
                    {
                        OccupationList.Add(new SelectListItem() { Text = val.Name, Value = val.Code });
                    }
                }

                return OccupationList;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> PhoneType(long agentSessionId, MGIContext mgiContext)
        {
            try
            {
                List<SelectListItem> PhoneTypes = new List<SelectListItem>();
                PhoneTypes.Add(DefaultListItem());

                string[] WebPhoneTypes = DesktopService.PhoneTypes(agentSessionId, mgiContext);
                if ( WebPhoneTypes != null )
                {
                    foreach ( var val in WebPhoneTypes )
                    {
                        PhoneTypes.Add(new SelectListItem() { Text = val, Value = val });
                    }
                }
                return PhoneTypes;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> USStates(long agentSessionId, MGIContext mgiContext)
        {
            try
            {
                List<SelectListItem> States = new List<SelectListItem>();
                States.Add(DefaultListItem());

                string[] WebStates = DesktopService.USStates(agentSessionId, mgiContext);

                Array.Sort(WebStates);

                if ( WebStates != null )
                {
                    foreach ( var val in WebStates )
                    {
                        States.Add(new SelectListItem() { Text = val, Value = val });
                    }
                }
                return States;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> PhoneProvider(long agentSessionId, MGIContext mgiContext)
        {
            try
            {
                List<SelectListItem> MobileProviders = new List<SelectListItem>();
                MobileProviders.Add(DefaultListItem());

                string[] WebMobileProviders = DesktopService.MobileProviders(agentSessionId, mgiContext);

                if ( WebMobileProviders != null )
                {
                    foreach ( var val in WebMobileProviders )
                    {
                        MobileProviders.Add(new SelectListItem() { Text = val, Value = val });
                    }
                }
                return MobileProviders;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }

        }
        /// <summary>
        /// Get List of Groups
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetGroups(string channelPartner, MGIContext mgiContext)
        {
            try
            {

                List<SelectListItem> Groups = new List<SelectListItem>();
                Groups.Add(DefaultListItem());
                var list = DesktopService.GetPartnerGroups(channelPartner, mgiContext);
                if ( list.Length > 0 )
                {
                    foreach ( var val in list )
                    {
                        Groups.Add(new SelectListItem() { Text = val, Value = val });
                    }
                }
                return Groups;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetRecieptLanguages()
        {
            try
            {
                List<SelectListItem> ReceiptLanguages = new List<SelectListItem>();

                Array itemNames = Enum.GetNames(typeof(Language));

                for ( int i = 0; i < itemNames.Length; i++ )
                {
                    ReceiptLanguages.Add(new SelectListItem() { Text = itemNames.GetValue(i).ToString(), Value = itemNames.GetValue(i).ToString() });
                }

                //ReceiptLanugages.Add(new SelectListItem() { Text = "English", Value = "English", Selected = true });
                //ReceiptLanugages.Add(new SelectListItem() { Text = "Spanish", Value = "Spanish" });

                return ReceiptLanguages;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="cultureInfo"></param>
        /// <param name="channelPartner"></param>
        /// <returns></returns>
        public string GetTipsAndOffersForChannelPartner(long agentSessionId, string viewName, string cultureInfo, string channelPartner, string optionalFilter, MGIContext mgiContext)
        {

            try
            {
                string tipsandOffers = null;
                TipsAndOffers[] TipsAndOffersArr = DesktopService.GetTipsAndOffers(agentSessionId, channelPartner, cultureInfo, viewName, mgiContext);

                List<TipsAndOffers> TipsAndOffersList = TipsAndOffersArr.ToList();


                if ( TipsAndOffersList.Count > 0 && optionalFilter != null )

                    tipsandOffers = TipsAndOffersList.Where(x => x.OptionalFilter == optionalFilter).FirstOrDefault().TipsAndOffersValue;

                else if ( TipsAndOffersList.Count > 0 )

                    tipsandOffers = TipsAndOffersList.Where(x => x.OptionalFilter == null || x.OptionalFilter == "").FirstOrDefault().TipsAndOffersValue;


                if ( tipsandOffers != null && tipsandOffers.Trim().Length > 0 )
                    return tipsandOffers;
            }
            catch ( Exception )
            {
                return string.Empty;
            }

            return string.Empty; ;
        }

        #endregion

        #region REPORTS RELATED METHODS
        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public CashDrawer CashDrawerReport(long agentSessionId, int agentId, long locationId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.CashDrawerReport(agentSessionId, agentId, locationId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }
        #endregion

        #region Shopping Cart Methods

        public List<SharedData.ParkedTransaction> GetAllParkedTransactions()
        {
            return DesktopService.GetAllParkedShoppingCartTransactions().ToList();
        }

        public SharedData.Receipts GenerateReceiptsForShoppingCart(long customerSessionId, long shoppingCartId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GenerateReceiptsForShoppingCart(customerSessionId, shoppingCartId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public void CloseShoppingCart(long customerSessionId, MGIContext mgiContext)
        {
            try
            {

                DesktopService.CloseShoppingCart(customerSessionId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        #endregion

        #region PRIVATE METHODS

        /// </summary>
        /// <returns></returns>
        private SelectListItem DefaultListItem()
        {
            return new SelectListItem() { Value = string.Empty, Text = "Select", Selected = true };
        }
        #endregion

        #region SEND MONEY METHODS

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetStatus()
        {
            try
            {
                List<SelectListItem> status = new List<SelectListItem>();
                status.Add(DefaultListItem());

                List<string> stringList = new List<string>();
                stringList.Add("Active");
                stringList.Add("Inactive");

                foreach ( var val in stringList )
                {
                    status.Add(new SelectListItem() { Text = val, Value = val });
                }

                return status;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetGender()
        {
            try
            {
                List<SelectListItem> gender = new List<SelectListItem>();
                gender.Add(DefaultListItem());

                List<string> stringList = new List<string>();
                stringList.Add("Male");
                stringList.Add("Female");
                stringList.Sort();
                foreach ( var val in stringList )
                {
                    gender.Add(new SelectListItem() { Text = val, Value = val });
                }
                return gender;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetRelationShip()
        {
            try
            {
                List<SelectListItem> relationship = new List<SelectListItem>();
                relationship.Add(DefaultListItem());

                List<string> stringList = new List<string>();
                stringList.Add("Father");
                stringList.Add("Mother");

                foreach ( var val in stringList )
                {
                    relationship.Add(new SelectListItem() { Text = val, Value = val });
                }
                return relationship;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetCities()
        {
            try
            {
                List<SelectListItem> cities = new List<SelectListItem>();
                cities.Add(DefaultListItem());

                List<string> stringList = new List<string>();
                stringList.Add("Banglore");
                stringList.Add("Hyderabad");
                stringList.Add("Vijayawada");
                stringList.Add("Chennai");
                foreach ( var val in stringList )
                {
                    cities.Add(new SelectListItem() { Text = val, Value = val });
                }
                return cities;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetPayment()
        {
            try
            {
                List<SelectListItem> payment = new List<SelectListItem>();
                payment.Add(DefaultListItem());

                List<string> stringList = new List<string>();
                stringList.Add("Check");
                stringList.Add("Cash");
                stringList.Add("Balancetransfer");

                foreach ( var val in stringList )
                {
                    payment.Add(new SelectListItem() { Text = val, Value = val });
                }
                return payment;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetAmountType()
        {
            try
            {
                List<SelectListItem> amountType = new List<SelectListItem>();
                amountType.Add(DefaultListItem());

                List<string> stringList = new List<string>();
                stringList.Add("Check");
                stringList.Add("Cash");
                stringList.Add("Balancetransfer");

                foreach ( var val in stringList )
                {
                    amountType.Add(new SelectListItem() { Text = val, Value = val });
                }
                return amountType;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetDeliverMethod()
        {
            try
            {
                List<SelectListItem> deliveryMethod = new List<SelectListItem>();
                deliveryMethod.Add(DefaultListItem());

                List<string> stringList = new List<string>();
                stringList.Add("Cashon delivery");
                stringList.Add("Online");

                foreach ( var val in stringList )
                {
                    deliveryMethod.Add(new SelectListItem() { Text = val, Value = val });
                }
                return deliveryMethod;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetDeliverOption()
        {
            try
            {
                List<SelectListItem> deliveryOption = new List<SelectListItem>();
                deliveryOption.Add(DefaultListItem());

                List<string> stringList = new List<string>();
                stringList.Add("Delivery at home");
                stringList.Add("Delivery at workplace");

                foreach ( var val in stringList )
                {
                    deliveryOption.Add(new SelectListItem() { Text = val, Value = val });
                }
                return deliveryOption;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public long SaveReceiver(long customerSessionId, Receiver receiver, MGIContext mgiContext)
        {
            try
            {

                DesktopServiceClient desktopserviceclient = new DesktopServiceClient();
                return DesktopService.AddReceiver(customerSessionId, receiver, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public long UpdateReceiver(long customerSessionId, Receiver receiver, MGIContext mgiContext)
        {
            try
            {
                DesktopServiceClient desktopserviceclient = new DesktopServiceClient();
                return DesktopService.EditReceiver(customerSessionId, receiver, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=" "></param>
        public List<Receiver> GetReceivers(long customerSessionId, string searchTerm, MGIContext mgiContext)
        {
            // Has to chk why earlier AlloyID is used, if needed customer sessionid has to be sent to filter instead of AlloyID
            try
            {

                DesktopServiceClient client = new DesktopServiceClient();
                List<Receiver> receivers = new List<Receiver>();
                Receiver[] receiverArray = client.GetReceivers(customerSessionId, searchTerm, mgiContext);

                foreach ( var receiver in receiverArray )
                {
                    receivers.Add(receiver);
                }
                return receivers;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=" "></param>
        public List<Receiver> GetFrequentReceivers(long customerSessionId, MGIContext mgiContext)
        {
            try
            {

                List<Receiver> receivers = new List<Receiver>();
                Receiver[] receiverArray = DesktopService.GetFrequentReceivers(customerSessionId, mgiContext);

                foreach ( var receiver in receiverArray )
                {
                    receiver.Address = receiver.Address == null ? string.Empty : receiver.Address.Replace("\r\n", "<br>");

                    receivers.Add(receiver);
                }
                return receivers;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=" "></param>
        public Receiver GetReceiverDetails(long customerSessionId, long receiverId, MGIContext mgiContext)
        {
            try
            {

                return DesktopService.GetReceiver(customerSessionId, receiverId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public Receiver GetReceiverByFullName(long customerSessionId, string fullName, MGIContext mgiContext)
        {
            try
            {

                return DesktopService.GetReceiverByFullName(customerSessionId, fullName, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=" "></param>
        public Receiver GetReceiverDetailsForEdit(long customerSessionId, long receiverId, MGIContext mgiContext) // This method is needed ?? Has to be removed
        {
            try
            {

                return DesktopService.GetReceiver(customerSessionId, receiverId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        ///  AL-3502
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="receiverId"></param>
        /// <param name="mgiContext"></param>
        public void DeleteFavoriteReceiver(long customerSessionId, long receiverId, MGIContext mgiContext)
        {
            try
            {
                DesktopService.DeleteFavoriteReceiver(customerSessionId, receiverId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=" "></param>
        public DoddFrankConfirmationClient GetDoddFrankConfirmationDetails(string receiverId)
        {
            DoddFrankConfirmationClient receiver = new DoddFrankConfirmationClient()
            {
                ReceiverName = "Ashok kumar",
                PickupLocation = "Anil",
                PickupMethod = "Active",
                PickupOptions = "Yes",
                CurrencyType = "India",
                TransferFee = 5,
                TransferTax = 5,
                ExchangeRate = 5,
                TransferAmount = 10,
                TotalAmount = 5,
                OtherFees = 5,
                OtherTaxes = 5,
                TotalToRecipient = 5
            };
            return receiver;
        }

        #endregion

        #region WUReceiver MaterData

        public List<SelectListItem> GetXfrCountries(long customerSessionId, MGIContext mgiContext)
        {
            try
            {
                List<SelectListItem> countries = new List<SelectListItem>();
                countries.Add(DefaultListItem());
                var countryResult = DesktopService.GetXfrCountries(customerSessionId, mgiContext).ToList<XferMasterData>();
                var topCountries = new[] { "UNITED STATES", "CANADA", "MEXICO" };
                var orderByCountryName = countryResult.OrderByDescending(c => topCountries.Contains(c.Name.ToUpper())).ThenBy(c => c.Name).ToList();
                var topItem = orderByCountryName.Single(x => x.Name.ToUpper() == UNITED_STATES);
                var masterCountryList = new List<XferMasterData>();
                masterCountryList.Add(topItem);
                masterCountryList = masterCountryList.Concat(orderByCountryName.Where(c => c.Name.ToUpper() != UNITED_STATES)).ToList();

                foreach ( var item in masterCountryList )
                {
                    countries.Add(new SelectListItem() { Value = item.Code, Text = item.Name });
                }
                return countries;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public List<SelectListItem> GetXfrStates(long customerSessionId, string countryCode, MGIContext mgiContext)
        {
            try
            {
                List<SelectListItem> states = new List<SelectListItem>();
                states.Add(DefaultListItem());
                var stateResult = DesktopService.GetXfrStates(customerSessionId, countryCode, mgiContext).ToList<XferMasterData>().OrderBy(c => c.Name);

                foreach ( var item in stateResult )
                {
                    states.Add(new SelectListItem() { Value = item.Code, Text = item.Name });
                }
                return states;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public List<SelectListItem> GetXfrCities(long customerSessionId, string stateCode, MGIContext mgiContext)
        {
            try
            {
                List<SelectListItem> cities = new List<SelectListItem>();
                cities.Add(DefaultListItem());
                var cityResult = DesktopService.GetXfrCities(customerSessionId, stateCode, mgiContext).ToList<XferMasterData>().OrderBy(c => c.Name);

                foreach ( var item in cityResult )
                {
                    cities.Add(new SelectListItem() { Value = item.Name, Text = item.Name });
                }
                return cities;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public List<DeliveryService> GetDeliveryServices(long customerSessionId, DeliveryServiceRequest request, MGIContext mgiContext)
        {
            try
            {
                List<DeliveryService> deliveryServices = DesktopService.GetDeliveryServices(customerSessionId, request, mgiContext).ToList();
                deliveryServices.Insert(0, new DeliveryService() { Name = "Select", Code = string.Empty });
                return deliveryServices;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public List<Field> GetXfrProviderAttributes(long customerSessionId, AttributeRequest attributeRequest, MGIContext mgiContext)
        {
            return DesktopService.GetXfrProviderAttributes(customerSessionId, attributeRequest, mgiContext).ToList();
        }

        public List<SelectListItem> DefaultSelectListItem()
        {
            List<SelectListItem> defaultList = new List<SelectListItem>();
            defaultList.Add(DefaultListItem());

            return defaultList;
        }

        public List<SelectListItem> GetRefundReasons(long customerSessionId, ReasonRequest request, MGIContext mgiContext)
        {
            try
            {
                List<SelectListItem> pickupOptions = new List<SelectListItem>();
                pickupOptions.Add(DefaultListItem());
                var pickupOptionsResult = DesktopService.GetRefundReasons(customerSessionId, request, mgiContext).ToList<SharedData.MoneyTransferReason>().OrderBy(c => c.Name);

                foreach ( var item in pickupOptionsResult )
                {
                    pickupOptions.Add(new SelectListItem() { Value = item.Code, Text = item.Name });
                }
                return pickupOptions;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }


        public string WUGetAgentBannerMessage(long agentSessionId, MGIContext mgiContext)
        {
            try
            {
                List<SelectListItem> message = new List<SelectListItem>();
                var messageResult = DesktopService.WUGetAgentBannerMessage(agentSessionId, mgiContext).ToList<XferMasterData>().OrderBy(c => c.Code);
                string messageFormat = "";

                foreach ( var item in messageResult )
                {
                    messageFormat += item.Name + " ";
                }
                return messageFormat.TrimEnd();
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public bool UpdateCustomerProfile(long customerSessionId, string WUGoldCardNumber, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.UpdateWUAccount(customerSessionId, WUGoldCardNumber, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }
        #endregion

        #region MONEY TRANSFER

        /// <summary>
        /// Get MoneyTransfer Fee
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="feeRequest"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public FeeResponse GetMoneyTransferFee(long customerSessionId, FeeRequest feeRequest, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetMoneyTransferFee(customerSessionId, feeRequest, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xSender"></param>
        /// <param name="xBeneficiary"></param>
        /// <param name="xPay"></param>
        /// <returns></returns>
        public ValidateResponse ValidateTransfer(long customerSessionId, ValidateRequest validateRequest, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.ValidateTransfer(customerSessionId, validateRequest, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        public string GetCurrencyCode(long customerSessionId, string countryCode, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetCurrencyCode(customerSessionId, countryCode, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        public List<SelectListItem> GetCurrencyCodeList(long customerSessionId, string countryCode, MGIContext mgiContext)
        {
            try
            {
                List<SelectListItem> currencyList = new List<SelectListItem>();
                XferMasterData[] currencies = DesktopService.GetCurrencyCodeList(customerSessionId, countryCode, mgiContext);
                if ( currencies != null )
                {
                    currencies = currencies.OrderBy(c => c.Name).ToArray();
                    foreach ( var val in currencies )
                    {
                        currencyList.Add(new SelectListItem() { Text = val.Name, Value = val.Code });
                    }
                }
                return currencyList;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public MoneyTransferTransaction GetReceiveTransaction(long customerSessionId, ReceiveMoneyRequest request, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.ReceiveMoneySearch(customerSessionId, request, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public void UpdateFundAmount(long customerSessionId, long cxeFundTrxId, decimal amount, FundType fundType, MGIContext mgiContext)
        {
            try
            {
                DesktopService.UpdateFundAmount(customerSessionId, cxeFundTrxId, amount, fundType, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        //AL-2729 user story for updating the cash-in transaction
        public void UpdateCash(long customerSessionId, long trxId, decimal amount, MGIContext mgiContext)
        {
            try
            {
                DesktopService.UpdateCash(customerSessionId, trxId, amount, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public SharedData.CardDetails WUCardEnrollment(long customerSessionId, XferPaymentDetails paymentDetails, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.WUCardEnrollment(customerSessionId, paymentDetails, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }
        public List<WUCustomerGoldCardResult> WUCardLookup(long customerSessionId, CardLookupDetails wucardlookupreq, MGIContext mgiContext)
        {
            try
            {
                WUCustomerGoldCardResult[] wuGoldCards = DesktopService.WUCardLookup(customerSessionId, wucardlookupreq, mgiContext);
                return wuGoldCards.ToList();
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }
        public bool GetWUCardAccount(long customerSessionId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetWUCardAccount(customerSessionId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public SharedData.Account DisplayWUCardAccountInfo(long customerSessionId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.DisplayWUCardAccountInfo(customerSessionId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public void CancelXfer(long customerSessionId, long ptnrTransactionId, MGIContext mgiContext)
        {
            try
            {
                DesktopService.CancelXfer(customerSessionId, ptnrTransactionId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }
        public void AddPastReceivers(long customerSessionId, string cardNumber, MGIContext mgiContext)
        {
            try
            {
                DesktopService.AddPastReceivers(customerSessionId, cardNumber, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public List<SelectListItem> GetActBeHalfList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Selected = true, Value = "0", Text = "Select" });
            items.Add(new SelectListItem { Value = "1", Text = "Yes" });
            items.Add(new SelectListItem { Value = "2", Text = "No" });
            return items;
        }

        #endregion

        #region  Send Money Refund

        public long StageRefundSendMoney(long customerSessionId, RefundSendMoneyRequest moneyTransferRefund, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.StageRefundSendMoney(customerSessionId, moneyTransferRefund, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw ex;
            }
        }

        public string MoneyTransferRefund(long customerSessionId, RefundSendMoneyRequest moneyTransferRefund, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.SendMoneyRefund(customerSessionId, moneyTransferRefund, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw ex;
            }
        }

        #endregion

        #region  Send Money Modify

        public string GetStatus(long customerSessionId, string confirmationNumber, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetStatus(customerSessionId, confirmationNumber, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public SendMoneySearchResponse SendMoneySearch(long customerSessionId, SendMoneySearchRequest request, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.SendMoneySearch(customerSessionId, request, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public ModifySendMoneyResponse StageModifySendMoney(long customerSessionId, ModifySendMoneyRequest moneyTransferModify, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.StageModifySendMoney(customerSessionId, moneyTransferModify, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw ex;
            }
        }

        public MoneyTransferTransaction GetMoneyTransferDetailsTransaction(long customerSessionId, long transactionId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetMoneyTransferDetailsTransaction(customerSessionId, transactionId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }
        }

        public ModifySendMoneyResponse AuthorizeSendMoneyModify(long customerSessionId, ModifySendMoneyRequest modifySendMoneyRequest, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.AuthorizeModifySendMoney(customerSessionId, modifySendMoneyRequest, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw ex;
            }
        }

        #endregion

        public Shared.Server.Data.CardInfo GetCardInfoXfer(long customerSessionId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetCardInfoXfer(customerSessionId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }



        #region PREPAID CARD

        public decimal GetMinimumLoadAmount(long customerSessionId, bool initialLoad, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetMinimumLoadAmount(customerSessionId, initialLoad, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AlloyID"></param>
        /// <param name="fundAccount"></param>
        /// <param name="agentSessionId"></param>
        /// <param name="customerSessionId"></param>
        /// <returns></returns>
        public long RegisterCard(FundsProcessorAccount fundAccount, string customerSessionId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.AddFundsAccount(GetCustomerSessionId(customerSessionId), fundAccount, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="CardNumber"></param>
        /// <returns></returns>
        public bool AuthenticateCard(string customerSessionId, string cardNumber, string pin, MGIContext mgiContext)
        {
            try
            {

                return DesktopService.AuthenticateCard(GetCustomerSessionId(customerSessionId), cardNumber, pin, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AlloyID"></param>
        /// <param name="CardNumber"></param>
        /// <param name="agentSessionId"></param>
        /// <param name="customerSessionId"></param>
        /// <returns></returns>
        public Server.Data.CardInfo GetCardBalance(string customerSessionId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetFundsBalance(GetCustomerSessionId(customerSessionId), mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="funds"></param>
        /// <returns></returns>
        public long Load(string customerSessionId, Funds funds, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.LoadFunds(GetCustomerSessionId(customerSessionId), funds, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }
        /// <summary>
        /// Record the activation fee transaction
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="funds"></param>
        /// <returns></returns>
        public long ActivateGPRCard(string customerSessionId, Funds funds, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.ActivateGPRCard(GetCustomerSessionId(customerSessionId), funds, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="funds"></param>
        /// <returns></returns>
        public long Withdraw(string customerSessionId, Funds funds, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.WithdrawFunds(GetCustomerSessionId(customerSessionId), funds, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AlloyID"></param>
        /// <param name="CardNumber"></param>
        /// <param name="amount"></param>
        /// <param name="fee"></param>
        /// <param name="agentSessionId"></param>
        /// <param name="customerSessionId"></param>
        /// <returns></returns>

        /// <summary>
        /// Retrieves the fee for fund transactions based on load, withdraw or activate
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="fundType"></param>
        /// <returns></returns>
        public TransactionFee GetFundsFee(long customerSessionId, decimal amount, FundType fundType, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetFeeForFunds(customerSessionId, amount, fundType, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// This is a dummy method. Where do we get the card defaults?
        /// </summary>
        /// <returns></returns>
        public GPRCardDefaults GetGPRCardDefaults(long customerSessionId)
        {
            GPRCardDefaults objGPRCardDefaults = new GPRCardDefaults();
            return objGPRCardDefaults;
        }

        public List<CardTransactionHistory> GetTransactionHistory(long customerSessionId, TransactionHistoryRequest request, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetCardTransactionHistory(customerSessionId, request, mgiContext).ToList();
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }

        }

        public bool CloseAccount(long customerSessionId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.CloseAccount(customerSessionId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public bool UpdateCardStatus(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.UpdateCardStatus(customerSessionId, cardMaintenanceInfo, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public bool ReplaceCard(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.ReplaceCard(customerSessionId, cardMaintenanceInfo, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        #endregion

        # region ManageUser

        public UserDetails GetUser(long agentSessionId, int userId, MGIContext mgiContext)
        {
            try
            {
                UserDetails user = DesktopService.GetUser(agentSessionId, userId, mgiContext);
                return user;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }

        }

        public int SaveUser(UserDetails userDetails, SaveMode mode, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.SaveUser(userDetails, mode, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public List<SelectListItem> GetUsers(long agentSessionId, long locationId, MGIContext mgiContext)
        {
            try
            {
                List<SelectListItem> userDetails = new List<SelectListItem>();
                userDetails.Add(DefaultListItem());

                UserDetails[] userDetailsArray = DesktopService.GetUsers(agentSessionId, locationId, mgiContext);

                foreach ( var item in userDetailsArray )
                {
                    userDetails.Add(new SelectListItem() { Text = item.FullName, Value = item.Id.ToString() });
                }

                return userDetails;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        #endregion

        #region Location

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetLocationStatus()
        {
            try
            {
                List<SelectListItem> locationStatus = new List<SelectListItem>();
                locationStatus.Add(DefaultListItem());

                List<string> stringList = new List<string>();
                stringList.Add("Active");
                stringList.Add("Inactive");

                foreach ( var val in stringList )
                {
                    locationStatus.Add(new SelectListItem() { Text = val, Value = val });
                }
                return locationStatus;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<MGI.Channel.DMS.Server.Data.Location> GetAllLocationNames()
        {
            List<MGI.Channel.DMS.Server.Data.Location> locationList = new List<MGI.Channel.DMS.Server.Data.Location>();
            try
            {
                MGI.Channel.DMS.Server.Data.Location[] locations = DesktopService.GetAll();
                foreach ( var location in locations )
                {
                    locationList.Add(location);
                }
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            return locationList;
        }

        public List<MGI.Channel.DMS.Server.Data.Location> GetAllLocationNames(long agentSessionId, MGIContext mgiContext)
        {
            List<MGI.Channel.DMS.Server.Data.Location> locationList = new List<MGI.Channel.DMS.Server.Data.Location>();
            try
            {

                MGI.Channel.DMS.Server.Data.Location[] locations = DesktopService.GetAllLocationByChannelPartnerId(agentSessionId, mgiContext);
                foreach ( var location in locations )
                {
                    locationList.Add(location);
                }
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            return locationList;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public long SaveLocation(long agentSessionId, MGI.Channel.DMS.Server.Data.Location location, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.CreateLocation(agentSessionId, location, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public bool UpdateLocation(long agentSessionId, MGI.Channel.DMS.Server.Data.Location location, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.UpdateLocation(agentSessionId, location, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }


        public List<ProcessorCredential> GetLocationProcessorCredentials(long agentSessionId, long locationId, MGIContext mgiContext)
        {
            DesktopServiceClient client = new DesktopServiceClient();
            return client.GetLocationProcessorCredentials(agentSessionId, locationId, mgiContext).ToList();
        }

        public void SaveLocationProcessorCredentials(long agentSessionId, long locationId, ProcessorCredential processorCredentials, MGIContext mgiContext)
        {
            DesktopServiceClient client = new DesktopServiceClient();
            client.SaveProcessorCredentials(agentSessionId, locationId, processorCredentials, mgiContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationName"></param>
        /// <returns></returns>
        public MGI.Channel.DMS.Server.Data.Location GetLocationDetailsForEdit(long agentSessionId, string locationName, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetByName(agentSessionId, locationName, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public MGI.Channel.DMS.Server.Data.Location GetLocationDetailsForEdit(string agentSessionId, long locationId, MGIContext mgiContext)
        {
            try
            {

                return DesktopService.LookupLocationById(agentSessionId, locationId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetUSStates(long agentSessionId, MGIContext mgiContext)
        {
            try
            {
                List<SelectListItem> States = new List<SelectListItem>();
                States.Add(DefaultListItem());

                string[] WebStates = DesktopService.USStates(agentSessionId, mgiContext);

                Array.Sort(WebStates);

                if ( WebStates != null )
                {
                    foreach ( var val in WebStates )
                    {
                        States.Add(new SelectListItem() { Text = val, Value = val });
                    }
                }
                return States;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        #endregion

        #region NpsTerminal

        public bool CreateNpsTerminal(long agentSessionId, NpsTerminal npsTerminal, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.CreateNpsTerminal(agentSessionId, npsTerminal, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }
        }

        public bool UpdateNpsTerminal(long agentSessionId, NpsTerminal npsTerminal, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.UpdateNpsTerminal(agentSessionId, npsTerminal, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }
        }

        public NpsTerminal LookupNpsTerminal(long agentSessionId, long terminalId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.LookupNpsTerminalById(agentSessionId, terminalId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }
        }

        public NpsTerminal LookupNpsTerminal(long agentSessionId, string ipAddress, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.LookupNpsTerminalByIpAddress(agentSessionId, ipAddress, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }
        }

        public List<NpsTerminal> LookupNpsTerminalByLocationID(long agentSessionId, long locationId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.LookupNpsTerminalByLocationID(agentSessionId, locationId, mgiContext).ToList();
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }
        }

        public NpsTerminal LookupNpsTerminalByName(long agentSessionId, string Name, ChannelPartner channelPartner, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.LookupNpsTerminalByName(agentSessionId, Name, channelPartner, mgiContext);
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }
        }

        #endregion

        #region Terminal Setup

        public Terminal LookupTerminal(long Id, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.LookupTerminalByGuid(Id, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }
        }

        public Terminal LookupTerminal(long agentSessionId, string terminalName, int channelPartnerId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.LookupTerminalByChannelPartner(agentSessionId, terminalName, channelPartnerId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }
        }

        public bool CreateTerminal(long agentSessionId, Terminal terminal, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.CreateTerminal(agentSessionId, terminal, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }
        }

        public bool UpdateTerminal(long agentSessionId, Terminal terminal, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.UpdateTerminal(agentSessionId, terminal, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }
        }
        #endregion

        #region Cash

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <returns></returns>
        public long CashIn(long customerSessionId, decimal amount, MGIContext mgiContext)
        {
            return DesktopService.CashIn(customerSessionId, amount, mgiContext);
        }
        /// <param name="cxeTxnId"></param>
        /// <returns></returns>


        /// <summary>
        /// Cancel CashIn
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="cxeTrxnId"></param>
        public void CancelCashIn(long customerSessionId, long cashInId)
        {
            DesktopService.RemoveCashIn(customerSessionId, cashInId, new MGIContext());
        }

        #endregion

        #region Transaction History

        public List<TransactionHistory> GetTransactionHistory(long customerSessionId, long customerId, string transactionType, string location, DateTime dateRange, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetTransactionHistory(customerSessionId, customerId, transactionType, location, dateRange, mgiContext).ToList();
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }
        }

        public List<TransactionHistory> GetTransactionHistory(long agentSessionId, long? agentId, string transactionType, string location, bool showAll, long transactionId, int duration, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetAgentTransactionHistory(agentSessionId, agentId, transactionType, location, showAll, transactionId, duration, mgiContext).ToList();
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }
        }

        public FundTransaction GetFundTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
        {

            try
            {
                return DesktopService.GetFundTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }
        }

        public CheckTransactionDetails GetCheckTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
        {

            try
            {
                return DesktopService.GetCheckTranasactionDetails(agentSessionId, customerSessionId, transactionId.ToString(), mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }
        }

        public MoneyOrderTransaction GetMoneyOrderTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetMoneyOrderTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }
        }

        public MoneyTransferTransaction GetMoneyTransferTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetMoneyTransferTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }
        }

        public Server.Data.BillPayTransaction GetBillPayTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetBillPayTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }
        }

        public CashTransaction GetCashTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetCashTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }
        }

        public List<ReceiptData> GetFundReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
        {
            try
            {


                return DesktopService.GetFundsReceipt(agentSessionId, customerSessionId, transactionId, isReprint, mgiContext).ToList();
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }
        }

        public List<ReceiptData> GetCheckReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
        {


            try
            {
                return DesktopService.GetCheckReceipt(agentSessionId, customerSessionId, transactionId, isReprint, mgiContext).ToList();
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }

        }

        public List<ReceiptData> GetBillPayReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
        {
            try
            {


                return DesktopService.GetBillPayReceipt(agentSessionId, customerSessionId, transactionId, isReprint, mgiContext).ToList();
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }
        }

        public List<ReceiptData> GetSummaryReceiptForReprint(long agentSessionId, long customerSessionId, long transactionId, string transactiontype, MGIContext mgiContext)
        {
            try
            {


                return DesktopService.GetSummaryReceiptForReprint(agentSessionId, customerSessionId, transactionId, transactiontype, mgiContext).ToList();
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }
        }

        public List<ReceiptData> GetMoneyOrderReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
        {
            try
            {


                return DesktopService.GetMoneyOrderReceipt(agentSessionId, customerSessionId, transactionId, isReprint, mgiContext).ToList();
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }
        }

        public List<ReceiptData> GetMoneyTransferReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
        {
            try
            {


                return DesktopService.GetMoneyTransferReceipt(agentSessionId, customerSessionId, transactionId, isReprint, mgiContext).ToList();
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }

        }

        public List<ReceiptData> GetDoddfrankReceipt(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetDoddFrankReceipt(agentSessionId, customerSessionId, transactionId, mgiContext).ToList();
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }

        }

        /// <summary>
        /// US1800 Referral promotions – Free check cashing to referrer and referee 
        /// Added method to get Coupon Code Receipt 
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <returns></returns>
        public List<ReceiptData> GetCouponCodeReceipt(long customerSessionId, MGIContext mgiContext)
        {
            try
            {


                return DesktopService.GetCouponCodeReceipt(customerSessionId, mgiContext).ToList();
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }
        }

        public Customer GetCustomer(string customerSessionId, long alloyId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.Lookup(long.Parse(customerSessionId), alloyId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }
        }

        public string GetCheckFrankData(long customerSessionId, long transactionId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetCheckFrankingData(customerSessionId, transactionId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }

        }

        public void UpdateCheckTransactionFranked(long customerSessionId, long transactionId, MGIContext mgiContext)
        {
            try
            {


                DesktopService.UpdateTransactionFranked(customerSessionId, transactionId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }

        }

        #endregion

        #region Private method
        private long GetCustomerSessionId(string customerSessionId)
        {
            //need to change the customer session id to long everywhere. This will be a change in InitiateCustomerSession as well
            //currently it returns string which needs to be changed.
            long custSessionId = 0;
            long.TryParse(customerSessionId, out custSessionId);
            return custSessionId;
        }

        #endregion

        #region MONEY ORDER RELATED METHODS
        public TransactionFee GetMoneyOrderFee(long customerSessionId, MoneyOrderData moneyOrderData, MGIContext mgiContext)
        {
            try
            {

                return DesktopService.GetMoneyOrderFee(customerSessionId, moneyOrderData, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public MoneyOrder PurchaseMoneyOrder(long customerSessionId, MoneyOrderPurchase moneyOrderPurchase, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.PurchaseMoneyOrder(customerSessionId, moneyOrderPurchase, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public void UpdateMoneyOrder(long customerSessionId, MoneyOrderTransaction moneyOrderTransaction, long moneyOrderId, MGIContext mgiContext)
        {
            try
            {
                DesktopService.UpdateMoneyOrder(customerSessionId, moneyOrderTransaction, moneyOrderId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public void UpdateMoneyOrderStatus(long customerSessionId, long moneyOrderId, int newMoneyOrderStatus, MGIContext mgiContext)
        {
            try
            {
                DesktopService.UpdateMoneyOrderStatus(customerSessionId, moneyOrderId, newMoneyOrderStatus, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public Server.Data.CheckPrint GenerateCheckPrintForMoneyOrder(long moneyOrderId, long customerSessionId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GenerateCheckPrintForMoneyOrder(moneyOrderId, customerSessionId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public Server.Data.CheckPrint GenerateMoneyOrderDiagnostics(long agentSessionId, MGIContext mgiContext)
        {
            try
            {

                return DesktopService.GenerateMoneyOrderDiagnostics(agentSessionId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public MoneyOrder GetMoneyOrderStage(long customerSessionId, long moneyOrderId, MGIContext mgiContext)
        {
            try
            {

                return DesktopService.GetMoneyOrderStage(customerSessionId, moneyOrderId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        #endregion

        public long GetAnonymousUserPAN(long customerSessionId, long channelPartnerId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetAnonymousUserPAN(customerSessionId, channelPartnerId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public decimal GetBillPayFee(long customerSessionId, string providerName, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetBillPayFee(customerSessionId, providerName, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }

        }

        public IList<SelectListItem> GetDefaultList()
        {
            IList<SelectListItem> defaultList = new List<SelectListItem>();
            defaultList.Add(DefaultListItem());
            return defaultList;
        }

        public bool IsSWBStateXfer(long customerSessionId, string locationState, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.IsSWBStateXfer(customerSessionId, locationState, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw ex;
            }
        }

        public CashierDetails GetAgentXfer(long agentSessionId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetAgentXfer(agentSessionId, mgiContext);

            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw ex;
            }
        }

        public CashierDetails GetAgent(long agentSessionId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetAgent(agentSessionId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw ex;
            }
        }

        public void PostFlush(long customerSessionId, MGIContext mgiContext)
        {
            try
            {
                DesktopService.PostFlush(customerSessionId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw ex;
            }
        }

        public List<SelectListItem> GetProfilestatus(long agentSessionId, MGIContext mgiContext)
        {
            try
            {
                List<SelectListItem> profileStatus = new List<SelectListItem>();

                string[] Status = DesktopService.ProfileStatus(agentSessionId, mgiContext);

                foreach ( var val in Status )
                {
                    profileStatus.Add(new SelectListItem() { Text = val, Value = val });
                }
                return profileStatus;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public CheckLogin GetCheckLogin(long customerSessionId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetCheckSession(customerSessionId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public void RemoveCheckFromCart(long customersessionId, long checkId, MGIContext mgiContext)
        {
            try
            {
                DesktopService.RemoveCheckFromCart(customersessionId, checkId, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public List<ReceiptData> GetCheckDeclinedReceiptData(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetCheckDeclinedReceiptData(agentSessionId, customerSessionId, transactionId, mgiContext).ToList();
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
            catch ( Exception ex )
            {
                throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
            }

        }

        public List<KeyValuePair<string, string>> GetPrepaidActions(string cardStatus)
        {
            try
            {
                //List<string> items = new List<string>();
                List<KeyValuePair<string, string>> items = new List<KeyValuePair<string, string>>();

                switch ( cardStatus )
                {
                    case "active":
                        items.Add(new KeyValuePair<string, string>("Report Lost & Replace", "5"));
                        items.Add(new KeyValuePair<string, string>("Report Stolen & Replace", "6"));
                        items.Add(new KeyValuePair<string, string>("Suspend Card(Do not replace)", "3"));
                        items.Add(new KeyValuePair<string, string>("Replace Card", "14"));
                        break;
                    case "suspended":
                        items.Add(new KeyValuePair<string, string>("Activate Card", "0"));
                        items.Add(new KeyValuePair<string, string>("Report Lost & Replace", "5"));
                        items.Add(new KeyValuePair<string, string>("Report Stolen & Replace", "6"));
                        items.Add(new KeyValuePair<string, string>("Replace Card", "14"));
                        break;
                    case "cardissued":
                        items.Add(new KeyValuePair<string, string>("Report Lost & Replace", "5"));
                        items.Add(new KeyValuePair<string, string>("Report Stolen & Replace", "6"));
                        items.Add(new KeyValuePair<string, string>("Replace Card", "14"));
                        break;
                    case "lostcard":
                        items.Add(new KeyValuePair<string, string>("Replace Card", "14"));
                        break;
                    case "stolencard":
                        items.Add(new KeyValuePair<string, string>("Replace Card", "14"));
                        break;
                }
                return items;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public List<KeyValuePair<string, string>> GetShippingTypes(long customerSessionId, MGIContext mgiContext)
        {
            try
            {
                List<ShippingTypes> shippingTypes = DesktopService.GetShippingTypes(customerSessionId, mgiContext).ToList();
                List<KeyValuePair<string, string>> items = new List<KeyValuePair<string, string>>();

                foreach ( var shippingType in shippingTypes )
                {
                    items.Add(new KeyValuePair<string, string>(shippingType.Name, shippingType.Code));
                }
                return items;
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }
        public double GetShippingFee(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetShippingFee(customerSessionId, cardMaintenanceInfo, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public long AssociateCard(long customerSessionId, FundsProcessorAccount fundsProcessorAccount, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.AssociateCard(customerSessionId, fundsProcessorAccount, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public double GetFundFee(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.GetFundFee(customerSessionId, cardMaintenanceInfo, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }
        public long RequestAddOnCard(long customerSessionId, Funds fund, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.IssueAddOnCard(customerSessionId, fund, mgiContext);
            }
            catch ( FaultException<NexxoSOAPFault> nexxoFault )
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        //public void Info(string message)
        //{
        //   DesktopService.Info(message);
        //}
        //public void Warn(string message)
        //{
        //    DesktopService.Warn(message);
        //} 
        //public void Debug(string message)
        //{
        //    DesktopService.Debug(message);
        //}
        //public void Trace(string message)
        //{
        //    DesktopService.Trace(message);
        //}
        //public void TraceException(string message,Exception ex)
        //{
        //    DesktopService.TraceException(message, ex);
        //}
        //public void Error(string message)
        //{
        //    DesktopService.Error(message);
        //}
        //public void ErrorException(string message, Exception x)
        //{
        //    DesktopService.ErrorException(message, x);
        //}
        //public void Fatal(string message)
        //{
        //    DesktopService.Fatal(message);
        //}
        //public void FatalException(string message,Exception x)
        //{
        //    DesktopService.FatalException(message, x);
        //}
    }
}


public enum UserRoles
{
    [Description("Teller")]
    Teller = 1,
    [Description("Manager")]
    Manager = 2,
    [Description("Compliance Manager")]
    ComplianceManager = 3,
    [Description("System Admin")]
    SystemAdmin = 4,
    [Description("Tech")]
    Tech = 5
}

public enum TerminalIdentificationMechanism
{
    YubiKey = 1,
    Cookie,
    HostName
}

public class MngUserSearch
{

    public MngUserSearch(string firstname, string lastname, string status, string location)
    {
        this.FirstName = firstname;
        this.LastName = lastname;
        this.Status = status;
        this.Location = location;
    }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Status { get; set; }
    public string Location { get; set; }
}

public class SearchReceiver
{
    public string ReceiverID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Status { get; set; }
    public string Country { get; set; }
    public string State { get; set; }
    public string City { get; set; }
}

public class DoddFrankConfirmationClient
{
    public string ReceiverId { get; set; }

    public string ReceiverName { get; set; }

    public string PickupLocation { get; set; }

    public string PickupMethod { get; set; }

    public string PickupOptions { get; set; }

    public string CurrencyType { get; set; }

    public int TransferFee { get; set; }

    public int TransferTax { get; set; }

    public int TotalAmount { get; set; }

    public int ExchangeRate { get; set; }

    public int TransferAmount { get; set; }

    public int OtherFees { get; set; }

    public int OtherTaxes { get; set; }

    public int TotalToRecipient { get; set; }
}

public class GPRCardDefaults
{
    public decimal ActivationFee { set; get; }

    public decimal LoadFee { get; set; }

    public decimal WithdrawFee { get; set; }

    public decimal MinimumLoad { get; set; }

    public decimal MaximumLoad { get; set; }

    public decimal MinimumWithdraw { get; set; }

    public decimal MaximumWithdraw { get; set; }
}

public class Department
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; }

    public bool IsActive { get; set; }
}

public class ManageUser
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string UserName { get; set; }
    public string PrimaryLocation { get; set; }
    public string Department { get; set; }
    public string Manager { get; set; }
    public string UserRole { get; set; }
    public string UserStatus { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string TempPassword { get; set; }
}

public class Location : NexxoModel
{
    // public int LocationId { get; set; }
    public string LocationType { get; set; }

    public string ParentLocation { get; set; }

    public string LocationName { get; set; }

    public string LocationStatus { get; set; }

    public string Address1 { get; set; }

    public string Address2 { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string ZipCode { get; set; }
}

public class SetupLocationType
{
    public string LocationType { get; set; }

    public bool Enabled { get; set; }
}


