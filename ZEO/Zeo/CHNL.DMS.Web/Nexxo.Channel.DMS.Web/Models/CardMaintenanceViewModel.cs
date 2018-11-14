using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCF.Channel.Zeo.Web.Models
{
	////AL-324 VISA Prepaid Service
	public class CardMaintenanceViewModel : BaseModel
	{

		public string CardHolderName { get; set; }

		public string CardNumber { get; set; }

		public CardBalance CardBalance { get; set; }

		public string PrepaidAction { get; set; }

		public string MaskCardNumber { get; set; }

		public string CVV { get; set; }

		public IEnumerable<SelectListItem> ActionForCardReplace { get; set; }

		public string CardShippingType { get; set; }

		public IEnumerable<SelectListItem> ShippingTypes { get; set; }
	}
}