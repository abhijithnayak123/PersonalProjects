using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
    public class UserSearchCriteria
    {
       public virtual string FirstName{ get; set; }
       public virtual string LastName{ get; set; }
       public virtual string UserName { get; set; }
	   public virtual string LocationName { get; set; }
	   public virtual long ChannelPartnerId { get; set; }
    }  
}
