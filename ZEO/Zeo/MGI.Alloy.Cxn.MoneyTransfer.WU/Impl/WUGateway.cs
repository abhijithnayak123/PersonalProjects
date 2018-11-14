using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGI.Alloy.Cxn.MoneyTransfer.WU.Impl
{
    public class WUGateway
    {
        #region Dependencies
        public ProcessorDAL ProcessorDAL;
        public bool IsHardCodedCounterId { get; set; } = Convert.ToBoolean(ConfigurationManager.AppSettings["BranchUserNamePrefix"].ToString());
        public IIO WUBillPayIO = new IO();

        public IWUCommonIO WUCommonIO;
        IMapper mapper;

        #endregion
    }
}
