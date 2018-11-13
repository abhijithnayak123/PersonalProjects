--===========================================================================================
-- Auther:			Raviraj
-- Date Created:	1/13/2014
-- Description:		Script for Create NYCHA CXN Transaction Tables
--===========================================================================================
--Create [tNYCHA_Account] table
CREATE TABLE [dbo].[tNYCHA_BillPay_Account](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[SSN] [nvarchar](20) NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[Address1] [nvarchar](100) NOT NULL,
	[Address2] [nvarchar](50) NULL,
	[City] [nvarchar](50) NOT NULL,
	[State] [nvarchar](2) NOT NULL,
	[Zip] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](320) NULL,
	[Phone] [nvarchar](20) NULL,
	[TenantID] [nvarchar](20) NOT NULL,
	[AccountNumber] [nvarchar](20) NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tNYCHA_BillPay_Account] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--Create [tNYCHA_BillPay_Trx] table
CREATE TABLE [dbo].[tNYCHA_BillPay_Trx](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[Amount] [money] NOT NULL,
	[Fee] [money] NOT NULL,
	[TenantID] [nvarchar](20) NOT NULL,
	[AccountNumber] [nvarchar](20) NOT NULL,
	[ProductId] [bigint] NOT NULL,
	[BillerName] [varchar](255) NOT NULL,
	[ProviderId] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[ChannelParterId] [smallint] NOT NULL,	
	[LocationId] [uniqueidentifier] NULL,
	[NYCHAAccountPK] [uniqueidentifier] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	[DTServerCreate] [datetime] NULL,
	[DTServerLastMod] [datetime] NULL,
 CONSTRAINT [PK_tNYCHA_BillPay_Trx] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tNYCHA_BillPay_Trx]  WITH CHECK ADD  CONSTRAINT [FK_tNYCHA_BillPay_Trx_tNYCHA_BillPay_Account] FOREIGN KEY([NYCHAAccountPK])
REFERENCES [dbo].[tNYCHA_BillPay_Account] ([rowguid])
GO

ALTER TABLE [dbo].[tNYCHA_BillPay_Trx] CHECK CONSTRAINT [FK_tNYCHA_BillPay_Trx_tNYCHA_BillPay_Account]
GO
--===========================================================================================
