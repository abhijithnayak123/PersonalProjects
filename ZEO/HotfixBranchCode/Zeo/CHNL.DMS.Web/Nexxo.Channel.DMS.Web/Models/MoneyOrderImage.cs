using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MGI.Channel.DMS.Web.Models
{
    /// <summary>
    /// This class performs a MoneyOrderImage model.
    /// </summary>
    public class MoneyOrderImage : BaseModel
    {
        /// <summary>
        /// Gets or sets the MoneyOrderCheckFrontImage
        /// </summary>
        public string FrontImage { get; set; }

		/// <summary>
		/// Gets or sets the MoneyOrderCheckBackImage
		/// </summary>
		public string BackImage { get; set; }

        /// <summary>
        /// Gets or sets the MoneyOrderCheckNumber
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "MoneyOrderCheckNumberRequired")]
        public string CheckNumber { get; set; }

        /// <summary>
        /// Gets or sets the NpsId
        /// </summary>
        public string NpsId { get; set; }

		/// <summary>
		/// Gets or sets the RoutingNumber
		/// </summary>
		public string RoutingNumber { get; set; }

		/// <summary>
		/// Gets or sets the AccountNumber
		/// </summary>
		public string AccountNumber { get; set; }

		/// <summary>
		/// Gets or sets the MICR
		/// </summary>
		public string MICR { get; set; }
    }
}