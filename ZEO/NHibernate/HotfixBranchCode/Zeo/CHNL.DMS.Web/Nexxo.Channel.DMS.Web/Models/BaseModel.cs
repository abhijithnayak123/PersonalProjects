using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ServiceModel;

using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;

namespace MGI.Channel.DMS.Web.Models
{
	public class BaseModel
	{
		public BaseModel()
		{
			if (HttpContext.Current.Session["RefChannelPartnerName"] != null && HttpContext.Current.Session["ChannelPartnerName"].ToString() != HttpContext.Current.Session["RefChannelPartnerName"].ToString())
				HttpContext.Current.Session["ChannelPartnerName"] = HttpContext.Current.Session["RefChannelPartnerName"];
			HttpContext.Current.Session["RefChannelPartnerName"] = HttpContext.Current.Session["ChannelPartnerName"];

			GetChannelPartner();
			GetCustomerSession();
		}

		public string ChannelPartnerName { get; set; }
		public int CardLength { get; set; }
		public ChannelPartner channelPartner { get; set; }
		public CustomerSession customerSession { get; set; }
		private List<Product> _providers;
		public List<Product> Providers
		{
			get
			{
				_providers = channelPartner.Providers.Select(c => new Product()
				{
					Name = c.ProductName,
					ProcessorName = c.ProcessorName,
					ProcessorId = c.ProcessorId,
					IsSSNRequired = c.IsSSNRequired,
					IsSWBRequired = c.IsSWBRequired,
					IsTnCForcePrintRequired = c.IsTnCForcePrintRequired,
					CanParkReceiveMoney = c.CanParkReceiveMoney,
					MinimumTransactAge = c.MinimumTransactAge,
					CanCustomerTransact = NexxoUtil.GetCustomerAgeByDateOfBirth(customerSession != null ? customerSession.Customer.PersonalInformation.DateOfBirth : DateTime.Now) >= c.MinimumTransactAge
				}).ToList();
				return _providers;
			}
		}
		public short TIM { get; set; }

		private void GetChannelPartner()
		{
			if (HttpContext.Current.Session["ChannelPartner"] != null)
			{
				this.ChannelPartnerName = HttpContext.Current.Session["ChannelPartnerName"].ToString();
				this.channelPartner = (ChannelPartner)HttpContext.Current.Session["ChannelPartner"];
				this.TIM = ((ChannelPartner)HttpContext.Current.Session["ChannelPartner"]).TIM;
				//This value should come from service
				this.CardLength = 16;//this.ChannelPartnerName.ToLower() == "centris" ? 8 : 16;
			}
			else if (HttpContext.Current.Session["ChannelPartnerName"] != null)
			{
				MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
				this.ChannelPartnerName = HttpContext.Current.Session["ChannelPartnerName"].ToString();
				this.channelPartner = new Desktop().GetChannelPartner(this.ChannelPartnerName, mgiContext);

				//US1115 - Start - Saving channel partner config details 
				HttpContext.Current.Session["DisableWithdrawCNP"] = this.channelPartner.DisableWithdrawCNP;
				HttpContext.Current.Session["CashOverCounter"] = this.channelPartner.CashOverCounter;
				//US1115 - End 

				this.TIM = this.channelPartner.TIM;
				if (channelPartner.AuthenticationType.ToLower().Contains("card"))
				{
					this.channelPartner.AuthenticationType = "Card";
				}
				HttpContext.Current.Session["ChannelPartner"] = channelPartner;

				//This value should come from service
				this.CardLength = 16;// this.ChannelPartnerName.ToLower() == "centris" ? 8 : 16;
			}

			//Just for testing
			//this.channelPartner.AuthenticationType = "Card";
		}

		private void GetCustomerSession()
		{
			if (HttpContext.Current.Session["CustomerSession"] != null)
			{
				this.customerSession = (CustomerSession)HttpContext.Current.Session["CustomerSession"];
			}
			else
			{
				this.customerSession = null;
			}
		}
	}
}
