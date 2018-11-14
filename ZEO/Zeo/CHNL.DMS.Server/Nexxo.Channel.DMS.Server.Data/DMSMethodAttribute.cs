using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MGI.Channel.DMS.Server.Data
{
	public enum DMSFunctionalArea
	{
		Check,
		Cash,
		Funds,
		MoneyOrder,
		MoneyTransfer,
		BillPay,
		Customer,
		Agent,
		Other
	}

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class DMSMethodAttribute : Attribute
	{
		private DMSFunctionalArea functionalArea;
		private string description;

		public DMSMethodAttribute(DMSFunctionalArea functionalArea, string description)
		{
			this.functionalArea = functionalArea;
			this.description = description;
		}

		public DMSFunctionalArea FunctionalArea
		{
			get { return functionalArea; }
		}

		public string Description
		{
			get { return description; }
		}
	}
}