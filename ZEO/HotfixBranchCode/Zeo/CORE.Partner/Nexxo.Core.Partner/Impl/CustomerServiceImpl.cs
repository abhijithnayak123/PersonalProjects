using System;
using System.Collections.Generic;
using System.Linq;

using MGI.Common.DataAccess.Contract;

using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.Common.Util;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Core.Partner.Impl
{
	public class CustomerServiceImpl : ICustomerService
	{
		private IRepository<Customer> _ptnrCustomerRepo;
		public IRepository<Customer> PartnerCustomerRepo { set { _ptnrCustomerRepo = value; } }

		private IRepository<Prospect> _prospectRepo;
		public IRepository<Prospect> ProspectRepo { set { _prospectRepo = value; } }

		private IIDNumberBuilder _idBuilder;
		public IIDNumberBuilder IDBuilder { set { _idBuilder = value; } }

		public IRepository<IdentificationConfirmation> IdentityConfirmRepo { private get; set; }

		public IRepository<Account> AccountsRepo { private get; set; }
		public IRepository<CustomerGroupSetting> GroupSetting { private get; set; }
		public TLoggerCommon MongoDBLogger { private get; set; }
		public Customer Create(Customer customer)
		{
			if (customer.ChannelPartnerId == Guid.Empty)
				throw new PartnerCustomerException(PartnerCustomerException.CUSTOMER_CHANNELPARTNER_NOT_FOUND, "Channel Partner Not Found");

			_ptnrCustomerRepo.AddWithFlush(customer);

			return customer;
		}

		public void Update(Customer customer)
		{
			try
			{
                customer.DTServerLastModified = DateTime.Now;
				_ptnrCustomerRepo.UpdateWithFlush(customer);
			}
			catch (Exception ex)
			{
				throw new PartnerCustomerException(PartnerCustomerException.CUSTOMER_UPDATE_FAILED, ex);
			}
		}
		//Method to do lookup based on cxnAccountId and providerId
		public Customer LookupByCXNAccountId(long cxnAccountId, int providerId)
		{
			try
			{
				List<Account> accounts = AccountsRepo.FilterBy(c => c.CXNId == cxnAccountId && c.ProviderId == providerId).ToList();
				if (accounts.Count > 1)
					throw new PartnerCustomerException(PartnerCustomerException.CUSTOMER_MULTIPLE_ACCOUNT_FOUND);
				Account account = accounts[0];
				return _ptnrCustomerRepo.FindBy(x => x.Id == account.Customer.Id);
			}
			catch (Exception ex)
			{
				throw new PartnerCustomerException(PartnerCustomerException.CUSTOMER_NOT_FOUND, ex);
			}
		}

		public Customer Lookup(long Id)
		{
			try
			{
				return _ptnrCustomerRepo.FindBy(x => x.Id == Id);
			}
			catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(Id), "Lookup", AlloyLayerName.CORE, ModuleName.Customer,
				 "Error in Lookup -MGI.Core.Partner.Impl.CustomerServiceImpl", ex.Message, ex.StackTrace);
				
				throw new PartnerCustomerException(PartnerCustomerException.CUSTOMER_NOT_FOUND, ex);
			}
		}
		public Customer LookupByCxeId(long CxeId)
		{
			try
			{
				return _ptnrCustomerRepo.FindBy(x => x.CXEId == CxeId);
			}
			catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(CxeId), "LookupByCxeId", AlloyLayerName.CORE, ModuleName.Customer,
				"Error in LookupByCxeId -MGI.Core.Partner.Impl.CustomerServiceImpl", ex.Message, ex.StackTrace);
				
				throw new PartnerCustomerException(PartnerCustomerException.CUSTOMER_NOT_FOUND, ex);
			}
		}
		public Prospect LookupProspect(long alloyId)
		{
			Prospect prospect = null;
			try
			{
				prospect = _prospectRepo.FindBy(x => x.AlloyID == alloyId);
			}
			catch (Exception ex)
			{
				throw new PartnerCustomerException(PartnerCustomerException.PROSPECT_NOT_FOUND, ex);
			}
			return prospect;
		}

		public void SaveProspect(Prospect prospect)
		{
			// if prospect hasn't been saved yet, set DTCreate and assign AlloyID
			// otherwise, set DTLastMod
			if (prospect.id == null || prospect.id == Guid.Empty)
			{
				try
				{
					prospect.AlloyID = _idBuilder.NextPAN();
				}
				catch (Exception ex)
				{
					throw new PartnerCustomerException(PartnerCustomerException.ID_GENERATION_FAILED, ex);
				}
				prospect.DTServerCreate = DateTime.Now;
			}
			else
				prospect.DTServerLastModified = DateTime.Now;

			try
			{
				_prospectRepo.SaveOrUpdate(prospect);
				_prospectRepo.Flush();
			}
			catch (Exception ex)
			{
				throw new PartnerCustomerException(PartnerCustomerException.PROSPECT_SAVE_FAILED, ex);
			}
		}

		public string ConfirmIdentity(long agentId, long customerSessionId, bool status)
		{
			string returnString = string.Empty;
			try
			{
				IdentificationConfirmation identityConfirmed = new IdentificationConfirmation()
				{
					AgentID = agentId,
					CustomerSessionID = customerSessionId,
					ConfirmStatus = status,
					DateIdentified = DateTime.Now,
					DTServerCreate = DateTime.Now,
					rowguid = Guid.NewGuid()
				};

				IdentityConfirmRepo.AddWithFlush(identityConfirmed);
				returnString = identityConfirmed.Id.ToString();
			}
			catch (Exception ex)
			{
				throw new PartnerCustomerException(PartnerCustomerException.CUSTOMER_IDENTITY_RECORD_FAILED, ex);
			}
			return returnString;
		}
		public void SaveGroupSetting(CustomerGroupSetting _CustomerGroupSetting)
		{
			try
			{
				GroupSetting.AddWithFlush(_CustomerGroupSetting);				
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
	}
}
