using System;
using MGI.Common.DataAccess.Data;
using MGI.Cxn.Check.Data;

namespace MGI.Cxn.Check.Certegy.Data
{
	public class CheckTypeMapping 
	{
		public virtual int CertegyTypePK { get; set; }
		public virtual string Name { get; set; }		
		public virtual CheckType CheckType { get; set; }
	}
}
