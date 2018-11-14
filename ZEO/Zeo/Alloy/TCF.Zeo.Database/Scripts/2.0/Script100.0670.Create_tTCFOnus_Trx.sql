--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <05/25/2018>
-- Description: Create tTCFOnus_Trx table.
-- ================================================================================


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tTCFOnus_Trx]') AND type in (N'U'))
DROP TABLE [dbo].[tTCFOnus_Trx]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tTCFOnus_Trx](
	[TCFOnusTrxID] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[Amount] [money] NOT NULL,
	[TCFOnusAmount] [money] NULL,
	[TCFOnusFee] [money] NULL,
	[CheckDate] [datetime] NULL,
	[CheckNumber] [nvarchar](20) NULL,
	[RoutingNumber] [nvarchar](20) NULL,
	[AccountNumber] [nvarchar](20) NULL,
	[Micr] [nvarchar](50) NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
	--[InvoiceId] [int] NULL,
	--[TicketId] [int] NULL,
	--[WaitTime] [nvarchar](50) NULL,
	--[Status] [int] NOT NULL,
	[TCFOnusStatus] [nvarchar](50) NOT NULL,
	--[DeclineCode] [int] NULL,
	[Message] [nvarchar](255) NULL,
	[Location] [nvarchar](50) NOT NULL,
	[DTTerminalCreate] [datetime] NOT NULL,
	[DTTerminalLastModified] [datetime] NULL,
	--[SubmitType] [int] NULL,
	--[ReturnType] [int] NULL,
	[ChannelPartnerID] [int] NULL,
	[DTServerCreate] [datetime] NULL,
	[DTServerLastModified] [datetime] NULL,
	[IsCheckFranked] [bit] NOT NULL DEFAULT ((0)),
	[TellerTraceCode] VARCHAR(50) NULL,
	[CurrentBalance] [money] NULL,
	[AvailableBalance] [money] NULL,
	[TCFOnusAccountId] [bigint] NOT NULL
 CONSTRAINT [PK_tTCFOnus_Trx] PRIMARY KEY CLUSTERED 
(
	[TCFOnusTrxID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tTCFOnus_Trx]  WITH CHECK ADD  CONSTRAINT [FK_tTCFOnus_Trx_tTCFOnus_Account] FOREIGN KEY([TCFOnusAccountID])
REFERENCES [dbo].[tTCFOnus_Account] ([TCFOnusAccountID])
GO

ALTER TABLE [dbo].[tTCFOnus_Trx] CHECK CONSTRAINT [FK_tTCFOnus_Trx_tTCFOnus_Account]
GO