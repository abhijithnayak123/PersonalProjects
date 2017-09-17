using System;
using System.Data.Linq.Mapping;

namespace Reporting.X9
{
	[Table(Name = "tChannelPartner_X9_Audit_Header")]
	class X9AuditHeader
	{
		[Column(Name = "AuditHeaderID", IsPrimaryKey = true, IsDbGenerated=true, AutoSync=AutoSync.OnInsert)]
		public int AuditHeaderID;
		[Column(Name="ChannelPartnerID")]
		public Guid ChannelPartnerID;
		[Column(Name = "DateGenerated")]
		public DateTime DateGenerated;
		[Column(Name = "RecordCount")]
		public int RecordCount;
		[Column(Name = "FileSpec")]
		public string FileSpec;
		[Column(Name = "FileType")]
		public string FileType;
		[Column(Name = "DTServerCreate")]
		public DateTime DTServerCreate;
		[Column(Name = "DTServerLastModified")]
		public DateTime DTServerLastModified;
	}
}
