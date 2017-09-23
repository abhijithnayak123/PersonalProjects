using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFBasicTutorials
{
    interface IEntityObjectState
    {
        EntityObjectState ObjectState { get; set; }
    }

   public enum EntityObjectState
    { 
        Added,
        Modified,
        Deleted,
        Unchanged
    }
}
