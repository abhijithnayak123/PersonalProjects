using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.DataAccess.Data;

namespace MGI.Core.CXE.Data
{
    public class CustomerPreferedProduct : NexxoModel 
    {
		public virtual long AlloyID { get; set; }       
        public virtual long ProductId { get; set; }
        public virtual string AccountNumber { get; set; }
        public virtual System.Nullable<long> PhoneNumber { get; set; }
        public virtual string AccountPIN { get; set; }
        public virtual System.Nullable<System.DateTime> AccountDOB { get; set; }
        public virtual string AccountName { get; set; }
        public virtual System.Nullable<bool> Enabled { get; set; }     
        public virtual string Operator { get; set; }
        public virtual System.Nullable<int> AgentId { get; set; }
        public virtual string TenantId { get; set; }
        public virtual string BillerCode { get; set; }


        //ReceiverIndex has been added to identify the receiver for Biller by Index Number. Added for User Story # US1646.
        public virtual string ReceiverIndexNo { get; set; }
    }
}
