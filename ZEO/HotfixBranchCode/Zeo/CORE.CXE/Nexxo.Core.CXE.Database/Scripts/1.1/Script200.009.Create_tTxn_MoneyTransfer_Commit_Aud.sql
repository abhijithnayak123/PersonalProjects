
/****** Object:  Table [dbo].[tTxn_MoneyTransfer_Commit_Aud]    Script Date: 12/10/2013 18:39:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tTxn_MoneyTransfer_Commit_Aud]') AND type in (N'U'))
DROP TABLE [dbo].[tTxn_MoneyTransfer_Commit_Aud]
GO

/****** Object:  Table [dbo].[tTxn_MoneyTransfer_Commit_Aud]    Script Date: 12/10/2013 18:39:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tTxn_MoneyTransfer_Commit_Aud](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[Amount] [money] NOT NULL,
	[Fee] [money] NOT NULL,
	[AccountPK] [uniqueidentifier] NOT NULL,
	[Status] [int] NOT NULL,
	[ConfirmationNumber] [nvarchar](50) NULL,
	[ReceiverName] [nvarchar](100) NULL,
	[Destination] [nvarchar](200) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	[DTServerCreate] [datetime] NULL,
	[DTServerLastMod] [datetime] NULL,
	[AuditEvent] [smallint] NOT NULL,
	[DTAudit] [datetime] NOT NULL,
	[RevisionNo] [bigint] NULL
) ON [PRIMARY]

GO


