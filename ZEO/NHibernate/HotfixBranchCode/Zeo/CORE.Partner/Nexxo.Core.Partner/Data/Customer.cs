using System;
using System.Collections.Generic;
using System.Linq;

using MGI.Common.DataAccess.Data;
using MGI.Common.Util;

namespace MGI.Core.Partner.Data
{
	public class Customer : NexxoModel
	{
		public Customer()
		{
			Accounts = new List<Account>();
			Groups = new List<CustomerGroupSetting>();
		}

		public virtual IList<Account> Accounts { get; set; }
		public virtual long CXEId { get; set; }
		public virtual bool IsPartnerAccountHolder { get; set; }
		public virtual string ReferralCode { get; set; }
		public virtual Guid ChannelPartnerId { get; set; }
		public virtual Guid AgentSessionId { get; set; }
		public virtual ProfileStatus CustomerProfileStatus { get; set; }
		public virtual IList<CustomerGroupSetting> Groups { get; set; }
		public virtual ChannelPartner ChannelPartner { get; set; }
		/// <summary>
		/// Add an account to a customer.
		/// </summary>
		/// <param name="account">The account to add.</param>
		//public virtual void AddAccount( Account account )
		//{
		//    account.Customer = this;
		//    account.DTCreate = DateTime.Now;
		//    Accounts.Add( account );
		//}

		public virtual Account AddAccount(int ProviderId, long CXEId)
		{
			return AddAccount(ProviderId, CXEId, long.MinValue);
		}

		public virtual Account AddAccount(int ProviderId, long CXEId, long CXNId)
		{
			Account account = new Account(ProviderId, CXEId, CXNId);
			account.Customer = this;
			Accounts.Add(account);
			return account;
		}

		public virtual Account GetAccount(int ProviderId)
		{
			return Accounts.FirstOrDefault<Account>(x => x.ProviderId == ProviderId);
		}

		public virtual bool IsAccountExists(long cxeId, int providerId)
		{
			return Accounts.Any(x => x.CXEId == cxeId && x.ProviderId == providerId);
		}
		public virtual Account GetAccount(int ProviderId, long CXNId)
		{
			return Accounts.FirstOrDefault<Account>(x => x.ProviderId == ProviderId && x.CXNId == CXNId);
		}

		/// <summary>
		/// Use to find a customer's account after starting a transaction in CXE layer.
		/// </summary>
		/// <param name="CXEId">CXE Account Id</param>
		/// <returns>Account</returns>
		public virtual Account FindAccountByCXEId(long CXEId)
		{
			return Accounts.First<Account>(x => x.CXEId == CXEId);
           // return Accounts.FirstOrDefault<Account>(x => x.CXEId == CXEId);
		}

		public virtual CustomerGroupSetting AddtoGroup(ChannelPartnerGroup g)
		{
			CustomerGroupSetting groupSetting = new CustomerGroupSetting(g);
			groupSetting.customer = this;
			Groups.Add(groupSetting);
			return groupSetting;
		}

	        /// <summary>
        	/// Use to find a customer's account after starting a transaction in CXE layer using cxeid and providerid mainly using in Bill Pay.
	        /// </summary>
        	/// <param name="CXEId">CXE Account Id</param>
	        /// <returns>Account</returns>
        	public virtual Account FindAccountByCXEId(long CXEId,int providerId)
        	{
            		return Accounts.First<Account>(x => x.CXEId == CXEId && x.ProviderId == providerId);
        	}

	}
}
