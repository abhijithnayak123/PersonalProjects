using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MGI.Channel.DMS.Web.Models
{
    public class Product
    {
        public string Name { get; set; }
        public string ProcessorName { get; set; }
		public long ProcessorId { get; set; }
        public int Sequence { get; set; }
        public string ActionName { get { return Name; } }
        public string ControllerName { get { return ProcessorName + Name; } }
        public bool IsSSNRequired { get; set; }
		public bool IsSWBRequired { get; set; }
		public bool IsTnCForcePrintRequired { get; set; }
		public bool CanParkReceiveMoney { get; set; }
		public bool IsCertegy { get; set; }
		public int MinimumTransactAge { get; set; }
		public bool CanCustomerTransact { get; set; }
        public string ProductDisplayName { 
            get 
            {
                string channelPartnerName = (string)HttpContext.Current.Session["ChannelPartnerName"];
                string resourceValue = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.ResourceManager.GetString(channelPartnerName + Name);
                return resourceValue;
            }
        }
		public string ProcessorDisplayName
		{
			get
			{
				string resourceValue = !string.IsNullOrEmpty(ProcessorName) ? MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.ResourceManager.GetString(ProcessorName) : string.Empty;
				return resourceValue;
			}
		}
    }
}