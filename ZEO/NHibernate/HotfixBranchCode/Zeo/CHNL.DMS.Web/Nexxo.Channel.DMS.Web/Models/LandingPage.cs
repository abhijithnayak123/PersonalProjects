using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
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
    public class LandingPage : BaseModel 
    {
        public LandingPage()
        {
           
        }

        [Display(Name = "Loggedin Username:")]
        public string UserName { get; set; }
        [Display(Name = "Last Login Date:")]
        public string LastLoginDatetime { get; set; }
        [Display(Name = "ExpDayMessage:")]
        public string ExpDaysMessage { get; set; }
    }
}