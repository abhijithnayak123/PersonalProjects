-- ============================================================
-- Author:		Adwait Ullal
-- Create date: <01/09/2015>
-- Description:	<Create a new table tChannelPartner_X9_Audit_Header, 
--				to persist X9 audit information> 
-- Rally ID:	<US1685>
-- ============================================================


/****** Object:  Table [dbo].[tChannelPartner_X9_Audit_Header]    Script Date: 12/17/2014 3:27:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tChannelPartner_X9_Audit_Header]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tChannelPartner_X9_Audit_Header]
(
	[AuditHeaderID]				[int]					IDENTITY(1,1)	NOT NULL,
	[ChannelPartnerID]			[uniqueidentifier]						NOT NULL,
	[DateGenerated]				[datetime]								NOT NULL,
	[RecordCount]				[int]									NOT NULL,
	[FileSpec]					[varchar](255)							NOT NULL,
	[FileType]					[varchar](20)							NOT NULL
			CONSTRAINT [CK_tChannelPartner_X9_Audit_Header_FileType]
				CHECK (FileType = 'OffUs' OR FileType = 'OnUs' OR FileType = 'MoneyOrder'),
	[DTCreate]					[datetime]								NOT NULL
			CONSTRAINT [CK_tChannelPartner_X9_Audit_Header_DTCreate]
				DEFAULT (GETDATE()),
	[DTLastMod]					[datetime]								NOT NULL
			CONSTRAINT [CK_tChannelPartner_X9_Audit_Header_DTLastMod]
				DEFAULT (GETDATE()),
	CONSTRAINT [PK_tChannelPartner_X9_Audit_Header] PRIMARY KEY CLUSTERED 
	(
		[AuditHeaderID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	CONSTRAINT [CK_tChannelPartner_X9_Audit_Header_ChannelPartnerID_tChannelPartner_RowGuid]
		FOREIGN KEY (ChannelPartnerID)
			REFERENCES tChannelPartners(RowGuid),

) ON [PRIMARY]
END
GO

SET ANSI_PADDING OFF
GO