
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[PK_tMessageStore]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tMessageStore] DROP CONSTRAINT [PK_tMessageStore]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[UQ_tMessageStore_1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tMessageStore] DROP CONSTRAINT [UQ_tMessageStore_1]
END

GO


/****** Object:  Table [dbo].[tMessageStore]    Script Date: 05/26/2013 15:04:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tMessageStore]') AND type in (N'U'))
DROP TABLE [dbo].[tMessageStore]
GO


CREATE TABLE [dbo].[tMessageStore]
	(
	[rowguid] [uniqueidentifier] NOT NULL ROWGUIDCOL,
	[Id] [bigint] NOT NULL IDENTITY (1000000000, 1),
	[MessageKey] [nvarchar](20) NOT NULL,
    [PartnerPK] [bigint] NULL,
	[Language] [nvarchar](4) NOT NULL,
	[Content] [nvarchar] (4000) NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL
	)  ON [PRIMARY]
GO
ALTER TABLE [dbo].[tMessageStore] ADD CONSTRAINT
	PK_tMessageStore PRIMARY KEY CLUSTERED 
	(
	rowguid
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tMessageStore] ADD CONSTRAINT
	UQ_tMessageStore_1 UNIQUE ([MessageKey], [PartnerPK], [Language])
GO


ALTER TABLE tAccounts SET (LOCK_ESCALATION = TABLE)
GO