/****** Object:  Table [dbo].[tFView_Card]    Script Date: 05/24/2013 20:40:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tFView_Card_DTCreate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tFView_Card] DROP CONSTRAINT [DF_tFView_Card_DTCreate]
END
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tFView_Card_DTLastMod]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tFView_Card] DROP CONSTRAINT [DF_tFView_Card_DTLastMod]
END
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[PK_tFView_Card]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tFView_Card] DROP CONSTRAINT [PK_tFView_Card]
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tFView_Card]') AND type in (N'U'))
DROP TABLE [dbo].[tFView_Card]
GO

CREATE TABLE [dbo].[tFView_Card](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(2000000000,1) NOT NULL,
	[CardNumber] [nvarchar](30) NULL,
	[AccountNumber] [nvarchar] (30) NULL,
	[BSAccountNumber] [nvarchar] (30) NULL,
	[NameAsOnCard] [nvarchar](50) NULL,
	[FirstName] [nvarchar] (50) NULL,
	[MiddleName] [nvarchar] (50) NULL,
	[LastName] [nvarchar] (50) NULL,
	[DateOfBirth] [datetime] NULL,
	[SSNNumber] [nvarchar] (12) NULL,
	[GovernmentID] [nvarchar] (50) NULL,
	[IDNumber] [nvarchar] (50) NULL,
	[GovtIdExpirationDate] [datetime] NULL,
	[GovtIDIssueCountry] [nvarchar] (50) NULL,
	[GovtIDIssueDate] [datetime] NULL,
	[GovtIDIssueState] [nvarchar] (50) NULL,
	[AddressLine1] [nvarchar] (100) NULL,
	[AddressLine2] [nvarchar] (100) NULL,
	[City] [nvarchar] (50) NULL,
	[State] [nvarchar] (50) NULL,
	[PostalCode] [nvarchar] (20) NULL,
	[HomePhoneNumber] [nvarchar] (20) NULL,
	[ShippingContactName] [nvarchar] (200) NULL,
	[ShippingAddressLine1] [nvarchar] (100) NULL,
	[ShippingAddressLine2] [nvarchar] (100) NULL,
	[ShippingCity] [nvarchar] (50) NULL,
	[ShippingState] [nvarchar] (50) NULL,
	[ShippingZipCode] [nvarchar] (20) NULL,
	[ExpiryDate] [datetime] NULL,
	[IsActive] [bit] NULL,
	[DTActivated] [datetime] NULL,
	[ActivatedBy] [int] NULL,
	[DTDeactivated] [datetime] NULL,
	[DeactivatedBy] [int] NULL,
	[DeactivatedReason] [nvarchar](100) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tFView_Card] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tFView_Card] ADD  CONSTRAINT [DF_tFView_Card_DTCreate]  DEFAULT (getdate()) FOR [DTCreate]
GO

ALTER TABLE [dbo].[tFView_Card] ADD  CONSTRAINT [DF_tFView_Card_DTLastMod]  DEFAULT (getdate()) FOR [DTLastMod]
GO