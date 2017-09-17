using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MGI.Channel.DMS.Web.Models
{
    public class User : BaseModel
    {
        public User(int userId, string firstname, string lastname, string status, string location)
        {
            this.Id = userId;
            this.FirstName = firstname;
            this.LastName = lastname;
            this.Status = status;
            this.Location = location;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public int Id { get; set; }
    }
}