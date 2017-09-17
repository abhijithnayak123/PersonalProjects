using System;
using System.Data.Linq.Mapping;

namespace Reporting.X9
{
	[Table(Name = "tChannelPartner_X9_Audit_Detail")]
	class X9AuditDetail
	{

		[Column(Name = "AuditDetailID", IsPrimaryKey = true, IsDbGenerated = true, AutoSync=AutoSync.OnInsert)]
		public int AuditDetailID;
		[Column(Name = "AuditHeaderID")]
		public int AuditHeaderID;
		[Column(Name = "ItemType")]
		public string ItemType;
		[Column(Name = "ItemPK")]
		public Guid ItemPK;
		[Column(Name = "DTServerCreate")]
		public DateTime DTServerCreate;
		[Column(Name = "DTServerLastModified")]
		public DateTime DTServerLastModified;
	}
}
