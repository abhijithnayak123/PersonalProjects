using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.Synovus.Contract
{
    interface ISynovusCustomer
    {
        // Can we pass MGI.Core.Partner.Data.Prospect Here?, instead of creating a new set of data contract?
        void Register(); 

        // What should be input params? Customer Id and Card Number?
        void AddGPRCard();
    }
}
