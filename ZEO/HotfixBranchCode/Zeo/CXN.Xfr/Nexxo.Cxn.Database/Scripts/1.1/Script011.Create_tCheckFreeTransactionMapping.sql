IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tBillPayTransactionMapping]') AND type in (N'U'))
	DROP TABLE [dbo].[tBillPayTransactionMapping]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tCheckFreeTransactionMapping]') AND type in (N'U'))
	DROP TABLE [dbo].[tCheckFreeTransactionMapping]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tCheckFreeTransactionMapping](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[BillPayId] [bigint] NOT NULL,
	[ProcessorReferenceId] [varchar](50) NOT NULL,
	[processorId] [int] NOT NULL,
	[Amount] [int] NOT NULL,
	[CustomerFirstName] [varchar](50) NOT NULL,
	[CustomerLastName] [varchar](50) NULL,
	[CustomerPhoneNumber] [varchar](50) NULL,
	[CustomerCity] [varchar](50) NULL,
	[CustomerState] [varchar](50) NULL,
	[CustomerZip] [varchar](50) NULL,
	[CustomerAccount] [varchar](50) NULL,
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
	[DTCreate] [datetime] NOT NULL,
 CONSTRAINT [PK_tCheckFreeTransactionMapping_ID] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO