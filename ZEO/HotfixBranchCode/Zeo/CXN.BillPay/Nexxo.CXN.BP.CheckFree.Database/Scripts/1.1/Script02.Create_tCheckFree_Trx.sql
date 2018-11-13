IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tCheckFree_Trx_tCheckFree_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[tCheckFree_Trx]'))
ALTER TABLE [dbo].[tCheckFree_Trx] DROP CONSTRAINT [FK_tCheckFree_Trx_tCheckFree_Account]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tCheckFree_Trx]') AND type in (N'U'))
DROP TABLE [dbo].[tCheckFree_Trx]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tCheckFree_Trx](
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[ProcessorReferenceId] [varchar](50) NOT NULL,
	[processorId] [int] NOT NULL,
	[Amount] [int] NOT NULL,
	[CustomerAccountNo] [varchar](50) NULL,
	[BillID] [int] NOT NULL,
	[BillChoiceData] [varchar](500) NULL,
	[BillerZip] [varchar](20) NULL,
	[BillerListID] [int] NOT NULL,
	[ProductName] [varchar](50) NULL,
	[DataElementList] [varchar](100) NULL,
	[ReturnCode] [varchar](50) NULL,
	[ReturnCodeText] [varchar](100) NULL,
	[ReturnCodeDetail] [varchar](100) NULL,
	[MessageCode] [varchar](20) NULL,
	[MessageText] [varchar](100) NULL,
	[MessageDetail] [varchar](100) NULL,
	[ConsumerMessage] [varchar](100) NULL,
	[DataPrompts] [varchar](max) NULL,
	[CheckFreeAccountPK] [uniqueidentifier] NOT NULL,
	[CheckFreeStatus] [nvarchar](50) NULL,
	[DTCreate] [datetime] NOT NULL,
 CONSTRAINT [PK_tCheckFree_Trx_ID] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tCheckFree_Trx]  WITH CHECK ADD  CONSTRAINT [FK_tCheckFree_Trx_tCheckFree_Account] FOREIGN KEY(CheckFreeAccountPK)
REFERENCES [dbo].tCheckFree_Account(rowguid)
GO

ALTER TABLE [dbo].[tCheckFree_Trx] CHECK CONSTRAINT [FK_tCheckFree_Trx_tCheckFree_Account]
GO

SET ANSI_PADDING OFF
GO