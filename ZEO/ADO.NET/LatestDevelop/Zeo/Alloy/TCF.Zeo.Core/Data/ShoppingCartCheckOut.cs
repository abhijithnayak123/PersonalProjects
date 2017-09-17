using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class ShoppingCartCheckOut
    {
        public long CartId { set; get; }

        public bool IsCashOverCounter { set; get; }

        public List<ShoppingCartTransaction> transactions { set; get; }

        public ShoppingCartCheckOut()
        {
            transactions = new List<ShoppingCartTransaction>();
        }

    }
}
