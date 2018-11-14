using CXNDATA = MGI.Alloy.CXN.Customer.Data;

namespace MGI.Alloy.CXN.Customer.FIS.Data
{
    internal class FISCustomerProfile : CXNDATA.CustomerProfile
    {
        //Added for User Story - AL-3715
        //Description - Property - "IsPREPDSuccess" is made as nullable as this should be null
        //when customer registration is done. And updated on card activation.
        internal bool IsCISSuccess { get; set; }
        internal bool? IsCNECTSuccess { get; set; }
        internal bool? IsPREPDSuccess { get; set; }
    }
}
