using TCF.Zeo.Common.Data;

namespace TCF.Zeo.Cxn.Check.Chexar.Data
{
    public class ChexarSession : ZeoModel
    {
        public virtual string Location { get; set; }
        public virtual string CompanyToken { get; set; }
        public virtual int EmployeeId { get; set; }
        public virtual int BranchId { get; set; }
        public virtual long ChexarPartnerId { get; set; }
        public virtual string URL { get; set; }
    }
}
