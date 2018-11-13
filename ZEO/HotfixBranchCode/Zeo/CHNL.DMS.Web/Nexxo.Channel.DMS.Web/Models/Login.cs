using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using MGI.Channel.DMS.Web.Models;
using System.Web.Mvc;

namespace MGI.Channel.DMS.Web.Models
{
    /// <summary>
    /// Something about what the <c>MySomeFunction</c> does
    /// with some of the sample like
    /// <code>
    /// Some more code statement to comment it better
    /// </code>
    /// For more information seee <see cref="http://www.me.com"/>
    /// </summary>
    /// <param name="someObj">What the input to the function is</param>
    /// <returns>What it returns</returns>
    public class Login : BaseModel
    {
        [Required(ErrorMessage="User name is required.")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage="Password is required.")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(16, MinimumLength = 8, ErrorMessage="Password should be between 8 and 16 characters.")]
        public string Password { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }

        [Display(Name = "Alert")]
        public string Message { get; set; }

		public string HostName { get; set; }

        public IEnumerable<SelectListItem> LLocation { get; set; }

    }
}