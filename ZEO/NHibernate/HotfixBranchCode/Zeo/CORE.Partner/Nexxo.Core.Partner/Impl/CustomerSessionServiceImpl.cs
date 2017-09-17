using System;

using MGI.Common.DataAccess.Contract;

using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.Core.Partner.Data.Transactions;
using System.Linq;
using System.Linq.Expressions;
using MGI.Common.Util;
using MGI.Common.TransactionalLogging.Data;


namespace MGI.Core.Partner.Impl
{
    public class CustomerSessionServiceImpl : ICustomerSessionService
    {
        private IRepository<CustomerSession> _sessionRepo;
        public IRepository<CustomerSession> CustomerSessionRepo { set { _sessionRepo = value; } }
        public NLoggerCommon NLogger { get; set; }
		public TLoggerCommon MongoDBLogger { get; set; }

        public CustomerSession Create(AgentSession agentSession, Customer customer, bool cardPresent, string TimezoneID)
        {
            try
            {
                CustomerSession cs = new CustomerSession
                {
                    AgentSession = agentSession,
                    Customer = customer,
					CardPresent = cardPresent,
                    TimezoneID = TimezoneID,
                    DTStart = DateTime.Now,
                    DTServerCreate = DateTime.Now,
					DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(agentSession.Terminal.Location.TimezoneID)
                };
                _sessionRepo.AddWithFlush(cs);
                return cs;
            }
            catch (Exception ex)
            {
                throw new PartnerCustomerException(PartnerCustomerException.CUSTOMER_SESSION_CREATE_FAILED, ex);
            }
        }

        public CustomerSession Lookup(long id)
        {
            try
            {
                return _sessionRepo.FindBy(x => x.Id == id);               
            }
            catch (Exception ex)
            {
                NLogger.Error("MOSAKHA-CUSTOMER_SESSION_NOT_FOUND" + ex.InnerException);
				
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(id), "Lookup", AlloyLayerName.CORE, ModuleName.Customer,
				"Error in Lookup -MGI.Core.Partner.Impl.CustomerSessionServiceImpl", ex.Message, ex.StackTrace);
				
                throw new PartnerCustomerException(PartnerCustomerException.CUSTOMER_SESSION_NOT_FOUND, ex);
            }
        }

        public void Update(CustomerSession customerSession)
        {
            try
            {
                customerSession.DTServerLastModified = DateTime.Now;
				customerSession.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(customerSession.AgentSession.Terminal.Location.TimezoneID);
                _sessionRepo.UpdateWithFlush(customerSession);
            }             
            catch (Exception ex)
            {
                throw new PartnerCustomerException(PartnerCustomerException.CUSTOMER_SESSION_UPDATE_FAILED, ex); 
            }
        }

        public void Save(CustomerSession customerSession)
        {
            try
            {
                // flush for any changes made to the customer session (like shopping cart additions)
                _sessionRepo.Flush();
            }
            catch (Exception ex)
            {
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<CustomerSession>(customerSession, "Save", AlloyLayerName.CORE, ModuleName.Customer,
				"Error in Save -MGI.Core.Partner.Impl.CustomerSessionServiceImpl", ex.Message, ex.StackTrace);
				
                throw new PartnerCustomerException(PartnerCustomerException.CUSTOMER_SESSION_SAVE_FAILED, ex);
            }
        }

        public void End(CustomerSession customerSession)
        {
            try
            {
                customerSession.DTEnd = DateTime.Now;
                if (customerSession.HasActiveShoppingCart())
                    customerSession.ActiveShoppingCart.CloseShoppingCart();
                Update(customerSession);
            }
            catch (Exception ex)
            {
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<CustomerSession>(customerSession, "End", AlloyLayerName.CORE, ModuleName.Customer,
								"Error in End - MGI.Core.Partner.Impl.CustomerSessionServiceImpl", ex.Message, ex.StackTrace);

                throw new PartnerCustomerException(PartnerCustomerException.CUSTOMER_SESSION_END_SESSION_FAILED, ex);
            }
        }

        // US1488 Parking Transaction Changes
        /// <summary>
        /// Get Previous Session Parking ShoppingCart as Active ShoppingCart
        /// </summary>
        /// <param name="customerSession"></param>
        /// <returns></returns>
        public ShoppingCart GetParkingShoppingCart(CustomerSession customerSession)
        {

            var query = (from s in _sessionRepo.FilterBy(x => x.Customer == customerSession.Customer && x.ShoppingCarts.Any(y => y.IsParked == true))
                        .OrderByDescending(x => x.DTServerCreate)
                         select s).AsEnumerable().Take(1);

            CustomerSession custsession = null;
          
            if (query.Any())            
                custsession = query.First();

                if (custsession != null && custsession.ParkingShoppingCart != null && custsession.ParkingShoppingCart.ShoppingCartTransactions.Count > 0)
                {
                    string timezone = customerSession.AgentSession.Terminal.Location.TimezoneID;
                    customerSession.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
                    customerSession.DTServerLastModified = DateTime.Now;

                    customerSession.AddShoppingCart(custsession.ParkingShoppingCart, timezone);

                    foreach (var transaction in customerSession.ActiveShoppingCart.ShoppingCartTransactions)
                    {
                        transaction.Transaction.CustomerSession = customerSession;
                    };
                        
                    _sessionRepo.UpdateWithFlush(customerSession);
                }  

            return customerSession.ActiveShoppingCart;
        }    
    
    }
}
