using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.WU.Common.Data
{
	public class DeliveryServices
	{
		public string Code;
		public string Routing_Code;
		public string Information;
		public string Flags;
		public WUEnums.yes_no Phone_delivery_available;
		public bool Phone_delivery_availableSpecified;
		public WUEnums.yes_no physical_delivery_available;
		public bool Physical_Delivery_AvailableSpecified;
		public string Test_Question_Available;
		public Phone_Notification Phone_Notification;
		public Identification_question Identification_question;

        //Added for US#1684 (Delivery Services Data)

		public string[] personalMessages { get; set; }
        public string Personal_Messages;

	}
	public class Phone_Notification
	{
		public string Phone;
        
	}	
	public class Identification_question
	{
		public string Question;        
        public string Answer;
        
	}	
}
