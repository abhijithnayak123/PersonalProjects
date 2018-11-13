using System;
using System.Collections.Generic;

using MGI.Biz.Customer.Contract;
using BizCustomerService = MGI.Biz.Customer.Contract.ICustomerService;
using MGI.Biz.Customer.Data;
using CustomerSearchCriteriaDTO = MGI.Biz.Customer.Data.CustomerSearchCriteria;

using MGI.Core.CXE.Data;
using PTNRProspect = MGI.Core.Partner.Data.Prospect;

using MGI.Core.CXE.Contract;
using ICXECustomerService = MGI.Core.CXE.Contract.ICustomerService;
using CXECustomer = MGI.Core.CXE.Data.Customer;
using CXECustomerSearchCriteria = MGI.Core.CXE.Data.CustomerSearchCriteria;
using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;

using IPtnrCustomerService = MGI.Core.Partner.Contract.ICustomerService;
using MGI.Common.Util;

namespace MGI.Biz.Customer.Impl
{
	public class CustomerServiceValidatingWrapper : BizCustomerService
	{
		private BizCustomerService _innerService;
		public BizCustomerService InnerService { set { _innerService = value; } }

		private INexxoDataStructuresService _cxeIdTypeService;
		public INexxoDataStructuresService CXEIdTypeService { set { _cxeIdTypeService = value; } }

		public IShoppingCartService ShoppingCartSvc { private get; set; }

		public IPtnrCustomerService PartnerCustomerService { private get; set; }

		public ICXECustomerService CXECustomerService { private get; set; }

		public IAgentSessionService AgentSessionService { private get; set; }

		public void Register(long agentSessionId, SessionContext sessionContext, long alloyId, MGIContext mgiContext)
		{
			PTNRProspect PTNRProspect = PartnerCustomerService.LookupProspect(alloyId);

			//US1800 Referral promotions – Free check cashing to referrer and referee 
			//Validating Referal Code			
			if (ValidateReferalCode(long.Parse(sessionContext.ChannelPartnerId.ToString()), PTNRProspect.ReferralCode))
				_innerService.Register(agentSessionId, sessionContext, alloyId, mgiContext);
		}

		public List<CustomerSearchResult> Search(long agentSessionId, CustomerSearchCriteriaDTO criteria, MGIContext mgiContext)
		{
			if (!mgiContext.IsCompanionSearch)
			{
				ValidateCriteria(criteria);
			}
			return _innerService.Search(agentSessionId, criteria, mgiContext);
		}

		public MGI.Biz.Customer.Data.CustomerSession InitiateCustomerSession(long agentSessionId, long alloyId, MGIContext mgiContext)
		{
			return _innerService.InitiateCustomerSession(agentSessionId, alloyId, mgiContext);
		}

		public bool CanChangeProfileStatus(long agentSessionId, long userId, string profileStatus, MGIContext mgiContext)
		{
			return _innerService.CanChangeProfileStatus(agentSessionId, userId, profileStatus, mgiContext);
		}

		public Data.Customer GetCustomer(long customerSessionId, long alloyId, MGIContext mgiContext)
		{
			return _innerService.GetCustomer(customerSessionId, alloyId, mgiContext);
		}

		public void SaveCustomer(long agentSessionId, long alloyId, Data.Customer customer, MGIContext mgiContext)
		{
			//US1800 Referral promotions – Free check cashing to referrer and referee 
			//Validating Referal Code
			AgentSession agentsession = AgentSessionService.Lookup(agentSessionId);

			if (ValidateReferalCode(long.Parse(agentsession.Terminal.Location.ChannelPartnerId.ToString()), customer.Profile.ReferralCode))
				_innerService.SaveCustomer(agentSessionId, alloyId, customer, mgiContext);
		}

		public Data.Customer Get(long agentSessionId, string Phone, string PIN, MGIContext mgiContext)
		{
			return _innerService.Get(agentSessionId, Phone, PIN, mgiContext);
		}

		public Data.Customer GetCustomerForCardNumber(long agentSessionId, string CardNumber, MGIContext mgiContext)
		{
			return _innerService.GetCustomerForCardNumber(agentSessionId, CardNumber, mgiContext);
		}

		public bool ValidateSSN(long agentSessionId, string SSN, MGIContext mgiContext)
		{
			return _innerService.ValidateSSN(agentSessionId, SSN, mgiContext);
		}

		private void ValidateCriteria(MGI.Biz.Customer.Data.CustomerSearchCriteria criteria)
		{
			if (criteria != null)
			{
				if (string.IsNullOrEmpty(criteria.CardNumber)
					&& (BoolToInt(criteria.DateOfBirth > DateTime.MinValue) + BoolToInt(!string.IsNullOrEmpty(criteria.FirstName))
						 + BoolToInt(!string.IsNullOrEmpty(criteria.LastName)) + BoolToInt(!string.IsNullOrEmpty(criteria.PhoneNumber))
						 + BoolToInt(!string.IsNullOrEmpty(criteria.GovernmentId))
						 + BoolToInt(!string.IsNullOrEmpty(criteria.SSN))) < 2)
					throw new BizCustomerException(BizCustomerException.INVALID_CUSTOMER_SEARCH_NOT_ENOUGH_CRITERIA_PROVIDED);
			}
			else
				throw new BizCustomerException(BizCustomerException.INVALID_CUSTOMER_SEARCH_NO_CRITERIA_PROVIDED);
		}

		/// <summary>
		/// US1800 Referral promotions – Free check cashing to referrer and referee (DMS Promotions Wave 3)		
		/// Validating Referal Code
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="referalCode"></param>
		/// <returns></returns>
		private bool ValidateReferalCode(long channelPartnerId, string referalCode)
		{
			if (referalCode == null || referalCode == string.Empty)
				return true;

			// check here if it is proper referal code
			// 1. Is it a proper AlloyID No
			// 2. Is a shoppingcart checkout isreferral=true for that AlloyID No		

			MGI.Core.CXE.Data.Customer customer = CXECustomerService.Lookup(referalCode);

			if (customer == null)
				throw new BizCustomerException(BizCustomerException.INVALID_REFERALCODE, "Invalid Referal Code");
			else
			{
				if (customer.ChannelPartnerId != channelPartnerId)
					throw new BizCustomerException(BizCustomerException.INVALID_REFERALCODE, "Invalid Referal Code");
			}

			long alloyId = customer.Id;

			List<MGI.Core.Partner.Data.ShoppingCart> shoppingCarts = ShoppingCartSvc.LookupForCustomer(alloyId);
			if (shoppingCarts.Count == 0)
				throw new BizCustomerException(BizCustomerException.INVALID_REFERALCODE, "Invalid Referal Code");
			else if (!(shoppingCarts.Exists(x => x.IsReferral == true)))
				throw new BizCustomerException(BizCustomerException.INVALID_REFERALCODE, "Invalid Referal Code");
			return true;
		}

		private int BoolToInt(bool flag)
		{
			return flag ? 1 : 0;
		}

		public long GetAnonymousUserPAN(long agentSessionId, long channelPartnerId, string firstName, string lastName, MGIContext mgiContext)
		{
			return _innerService.GetAnonymousUserPAN(agentSessionId, channelPartnerId, firstName, lastName, mgiContext);
		}

		public List<Data.Customer> CustomerLookUp(long agentSessionId, Dictionary<string, object> customerLookUpCriteria, MGIContext mgiContext)
		{
			return _innerService.CustomerLookUp(agentSessionId, customerLookUpCriteria, mgiContext);
		}

		public void ValidateCustomerStatus(long agentSessionId, long alloyId, MGIContext mgiContext)
		{
			_innerService.ValidateCustomerStatus(agentSessionId, alloyId, mgiContext);
		}

		public void RegisterToClient(long agentSessionId, long alloyId, MGIContext mgiContext)
		{
			_innerService.RegisterToClient(agentSessionId, alloyId, mgiContext);
		}

		public void UpdateCustomerToClient(long agentSessionId, long alloyId, MGIContext mgiContext)
		{
			_innerService.UpdateCustomerToClient(agentSessionId, alloyId, mgiContext);
		}

		public void CustomerSyncInFromClient(long agentSessionId, long alloyId, MGIContext mgiContext)
		{
			_innerService.CustomerSyncInFromClient(agentSessionId, alloyId, mgiContext);
		}

		public ProfileStatus GetClientProfileStatus(long agentSessionId, long alloyId, MGIContext mgiContext)
		{
			return _innerService.GetClientProfileStatus(agentSessionId, alloyId, mgiContext);
		}

		public bool IsValidSSN(long customerSessionId, string last4DigitsOfSSN, MGIContext mgiContext)
		{
			return _innerService.IsValidSSN(customerSessionId, last4DigitsOfSSN, mgiContext);
		}

		public bool ValidateCustomer(long agentSessionId, Biz.Customer.Data.Customer customer, MGIContext mgiContext)
		{
			return _innerService.ValidateCustomer(agentSessionId, customer, mgiContext);
		}
	}
}
