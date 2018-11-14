-- ============================================================
-- Author:		<Swarnalakshmi>
-- Create date: <07/03/2014>
-- Description:	<Table for MoneyGram Transfer Account> 
-- Rally ID:	<NA>
-- ============================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tMGram_Account]') AND type in (N'U'))
	DROP TABLE [dbo].[tMGram_Account]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tMGram_Account](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[FirstName] [varchar](250) NULL,
	[LastName] [varchar](250) NULL,
	[Address] [varchar](250) NULL,
	[City] [varchar](250) NULL,
	[State] [varchar](250) NULL,
	[PostalCode] [varchar](250) NULL,
	[LoyaltyCardNumber] [varchar](250) NULL,
	[Email] [varchar](250) NULL,
	[ContactPhone] [varchar](250) NULL,
	[MobilePhone] [varchar](250) NULL,
	[SmsNotificationFlag] [varchar](250) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tMGram_Account] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
