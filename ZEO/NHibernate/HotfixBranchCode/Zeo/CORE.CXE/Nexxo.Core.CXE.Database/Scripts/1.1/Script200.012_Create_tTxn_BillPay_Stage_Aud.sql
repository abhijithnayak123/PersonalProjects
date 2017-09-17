IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tTxn_BillPay_Stage_Aud]') AND type in (N'U'))
DROP TABLE [dbo].[tTxn_BillPay_Stage_Aud]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tTxn_BillPay_Stage_Aud](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[Amount] [money] NOT NULL,
	[Fee] [money] NOT NULL,
	[AccountPK] [uniqueidentifier] NOT NULL,
	[Status] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[AccountNumber] [nvarchar](100) NULL,
	[ConfirmationNumber] [nvarchar](50) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	[DTServerCreate] [datetime] NULL,
	[DTServerLastMod] [datetime] NULL,
	[RevisionNo] [BIGINT] NOT NULL,
	[AuditEvent] [SMALLINT] NOT NULL,
	[DTAudit] [DATETIME] NOT NULL
) ON [PRIMARY]

GO


