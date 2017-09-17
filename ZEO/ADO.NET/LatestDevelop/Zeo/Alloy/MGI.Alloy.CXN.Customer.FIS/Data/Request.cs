using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Alloy.CXN.Customer.FIS.Data
{
    public class Request
    {
        string _mtvnsvcver = "1.0";
        string _msguuid = Guid.NewGuid().ToString();

        public string MtvnSvcVer { get { return _mtvnsvcver; } }
        public string MsgUUID { get { return _msguuid; } }
        public string SrcID { get; set; }
        public string TestInd { get; set; }
        public SvcParms SvcParms { get; set; }
        public MsgData MsgData { get; set; }
    }

    public class SvcParms
    {
        public string ApplID { get; set; }
        public string SvcID { get; set; }
        public string SvcVer { get; set; }
        public Guid RqstUUID { get; set; }
        public string RoutingID { get; set; }
    }
    public class MsgData
    {
        public string ApplMsgApplId { get; set; }
        public string ApplMsgNbr { get; set; }
        public string ApplMsgTxt { get; set; }
        public string ApplMsgErrInd { get; set; }
        public string ApplMsgElement { get; set; } 
    }
}
