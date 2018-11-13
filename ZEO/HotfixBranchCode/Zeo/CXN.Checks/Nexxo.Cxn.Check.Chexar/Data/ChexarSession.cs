// -----------------------------------------------------------------------
// <copyright file="ChexarSession.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MGI.Cxn.Check.Chexar.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ChexarSession
    {
        public virtual Guid rowguid { get; set; }

		public virtual string Location { get; set; }
		public virtual string CompanyToken { get; set; }
		public virtual int EmployeeId { get; set; }
		public virtual int BranchId { get; set; }

		public virtual ChexarPartner Partner { get; set; }

        public virtual DateTime DTTerminalCreate { get; set; }
        public virtual DateTime? DTTerminalLastModified { get; set; }
    }
}
