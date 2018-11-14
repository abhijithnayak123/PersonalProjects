using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using TCF.Zeo.Common.Util;
using TCF.Channel.Zeo.Web.Common;

#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
#endregion

namespace TCF.Channel.Zeo.Web.Models
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
            GetCustomerProfileSession();
            GetCustomerInfo();
            GetProviderDetails();
            GetEnabledFeatures();
        }

        public string ChannelPartnerName { get; set; }
        public int CardLength { get; set; }
        public ZeoClient.ChannelPartner channelPartner { get; set; }
        private Customer customer { get; set; }

        //New Property added for AO implementation.
        public ZeoClient.CustomerSession CustomerSession { get; set; }
        private List<Product> _providers;

        public List<ZeoClient.ChannelPartnerProductProvider> ProductProvider { get; set; }

        public List<ZeoClient.FeatureDetails> FeatureList { get; set; }
        public List<Product> Providers
        {
            get
            {
                _providers = ProductProvider.Select(c => new Product()
                {
                    Name = c.ProductName,
                    ProcessorName = c.ProcessorName,
                    ProcessorId = c.ProcessorId,
                    IsSSNRequired = c.IsSSNRequired,
                    IsSWBRequired = c.IsSWBRequired,
                    IsTnCForcePrintRequired = c.IsTnCForcePrintRequired,
                    CanParkReceiveMoney = c.CanParkReceiveMoney,
                    MinimumTransactAge = c.MinimumTransactAge,
                    CanCustomerTransact = Helper.GetCustomerAgeByDateOfBirth(customer != null ? Convert.ToDateTime(customer.PersonalInformation.DateOfBirth) : DateTime.Now) >= c.MinimumTransactAge
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
                this.channelPartner = (ZeoClient.ChannelPartner)HttpContext.Current.Session["ChannelPartner"];
                this.TIM = ((ZeoClient.ChannelPartner)HttpContext.Current.Session["ChannelPartner"]).TIM;
                //This value should come from service
                this.CardLength = 16;//this.ChannelPartnerName.ToLower() == "centris" ? 8 : 16;
            }
            else if (HttpContext.Current.Session["ChannelPartnerName"] != null)
            {
                ZeoClient.ZeoServiceClient serviceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.Response response = new ZeoClient.Response();
                ZeoClient.ZeoContext context = new ZeoClient.ZeoContext();

                this.ChannelPartnerName = HttpContext.Current.Session["ChannelPartnerName"].ToString();
                response = serviceClient.ChannelPartnerConfigByName(this.ChannelPartnerName, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                this.channelPartner = response.Result as ZeoClient.ChannelPartner;
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
        }

        private void GetProviderDetails()
        {
            if (HttpContext.Current.Session["ChannelPartnerName"] != null)
            {
                if (HttpContext.Current.Session["ChannelPartnerProvider"] == null)
                {
                    ZeoClient.ZeoServiceClient serviceClient = new ZeoClient.ZeoServiceClient();
                    ZeoClient.Response response = new ZeoClient.Response();
                    ZeoClient.ZeoContext context = new ZeoClient.ZeoContext();
                    this.ChannelPartnerName = HttpContext.Current.Session["ChannelPartnerName"].ToString();
                    response = serviceClient.GetProvidersbyChannelPartnerName(this.ChannelPartnerName, context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    this.ProductProvider = response.Result as List<ZeoClient.ChannelPartnerProductProvider>;
                    HttpContext.Current.Session["ChannelPartnerProvider"] = this.ProductProvider;
                }
                else
                {
                    this.ProductProvider = HttpContext.Current.Session["ChannelPartnerProvider"] as List<ZeoClient.ChannelPartnerProductProvider>;
                }
            }
        }


        //      //TODO - Abhi - Remove this method as we are returning the CustomerSession from ZeoClient now.
        //      // Not changing now because it will get affected in all the places.
        //      // Please don't use this method for the new implementation.
        private void GetCustomerSession()
        {
            if (HttpContext.Current.Session["CustomerSession"] != null)
            {
                this.CustomerSession = (ZeoClient.CustomerSession)HttpContext.Current.Session["CustomerSession"];
            }
            else
            {
                this.CustomerSession = null;
            }
        }

        private void GetCustomerInfo()
        {
            if (this.CustomerSession != null)
            {
                if (HttpContext.Current.Session["CustomerAO"] != null)
                {
                    this.CustomerSession.Customer = (ZeoClient.CustomerProfile)HttpContext.Current.Session["CustomerAO"];
                }
                else
                {
                    ZeoClient.ZeoContext context = new ZeoClient.ZeoContext() { CustomerId = this.CustomerSession.CustomerId };

                    ZeoClient.ZeoServiceClient client = new ZeoClient.ZeoServiceClient();
                    ZeoClient.Response response = client.GetCustomer(context);

                    if (WebHelper.VerifyException(response))
                    {
                        this.CustomerSession.Customer = null;
                    }
                    else
                    {
                        CustomerSession.Customer = (ZeoClient.CustomerProfile)response.Result;
                        HttpContext.Current.Session["CustomerAO"] = CustomerSession.Customer;
                    }
                }
            }
        }


        private void GetCustomerProfileSession()
        {
            customer = null;
            if (HttpContext.Current.Session["Customer"] != null)
            {
                this.customer = (Customer)HttpContext.Current.Session["Customer"];
            }
        }

        private void GetEnabledFeatures()
        {
            if(HttpContext.Current.Session["FeatureList"] == null)
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = new ZeoClient.ZeoContext();

                ZeoClient.Response response = alloyServiceClient.GetFeatures(context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                List<ZeoClient.FeatureDetails> features = response.Result as List<ZeoClient.FeatureDetails>;

                HttpContext.Current.Session["FeatureList"] = FeatureList = features;
            }
            else
            {
                this.FeatureList = HttpContext.Current.Session["FeatureList"] as List<ZeoClient.FeatureDetails>;
            }
        }
    }
}
