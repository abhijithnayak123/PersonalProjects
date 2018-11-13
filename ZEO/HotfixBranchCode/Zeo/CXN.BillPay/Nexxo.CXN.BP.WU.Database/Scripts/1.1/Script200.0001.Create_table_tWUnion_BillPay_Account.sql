IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_BillPay_Account]') AND type in (N'U'))
DROP TABLE [dbo].[tWUnion_BillPay_Account]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO	

CREATE TABLE [dbo].[tWUnion_BillPay_Account](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[DateOfBirth] [datetime] NULL,
	[Address1] [varchar](250) NOT NULL,
	[Address2] [varchar](250) NULL,
	[City] [varchar](50) NULL,
	[Street] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[PostalCode] [varchar](20) NULL,
	[CardNumber] [varchar](50) NULL,
	[PreferredCustomerLevelCode] [varchar](250) NULL,
	[Email] [varchar](250) NULL,
	[ContactPhone] [varchar](50) NULL,
	[MobilePhone] [varchar](50) NULL,
	[SmsNotificationFlag] [varchar](250) NULL,
	CONSTRAINT [PK_tWUnion_BillPay_Account] PRIMARY KEY CLUSTERED 
	(
	[rowguid] ASC
	)
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO
