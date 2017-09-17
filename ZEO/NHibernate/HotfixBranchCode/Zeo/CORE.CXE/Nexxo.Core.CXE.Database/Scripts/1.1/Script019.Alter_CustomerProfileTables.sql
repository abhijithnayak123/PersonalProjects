
DROP TABLE tCustomerGovernmentIdDetails
GO

DROP TABLE tCustomerEmploymentDetails
GO

drop table tCustomerProfiles
GO

CREATE TABLE [dbo].[tCustomerProfiles](
	[CustomerPK] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](255) NULL,
	[MiddleName] [nvarchar](255) NULL,
	[LastName] [nvarchar](255) NULL,
	[LastName2] [nvarchar](255) NULL,
	[MothersMaidenName] [nvarchar](255) NULL,
	[DOB] [datetime] NULL,
	[Address1] [nvarchar](255) NULL,
	[Address2] [nvarchar](255) NULL,
	[City] [nvarchar](255) NULL,
	[State] [nvarchar](255) NULL,
	[ZipCode] [nvarchar](255) NULL,
	[Phone1] [nvarchar](255) NULL,
	[Phone1Type] [nvarchar](255) NULL,
	[Phone1Provider] [nvarchar](255) NULL,
	[Phone2] [nvarchar](255) NULL,
	[Phone2Type] [nvarchar](255) NULL,
	[Phone2Provider] [nvarchar](255) NULL,
	[SSN] [nvarchar](255) NULL,
	[TaxpayerId] [nvarchar](255) NULL,
	[DoNotCall] [bit] NULL,
	[SMSEnabled] [bit] NULL,
	[MarketingSMSEnabled] [bit] NULL,
	[ChannelPartnerId] [int] NULL,
	[DTCreate] [datetime] NULL,
	[DTLastMod] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerPK] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tCustomerProfiles]  WITH CHECK ADD CONSTRAINT [FK_tCustomerProfiles_tCustomers] FOREIGN KEY([CustomerPK])
REFERENCES [dbo].[tCustomers] ([rowguid])
GO

ALTER TABLE [dbo].[tCustomerProfiles] CHECK CONSTRAINT [FK_tCustomerProfiles_tCustomers]
GO



CREATE TABLE [dbo].[tCustomerEmploymentDetails](
	[CustomerPK] [uniqueidentifier] NOT NULL,
	[Occupation] [nvarchar](255) NULL,
	[Employer] [nvarchar](255) NULL,
	[EmployerPhone] [nvarchar](255) NULL,
	[DTCreate] [datetime] NULL,
	[DTLastMod] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerPK] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tCustomerEmploymentDetails]  WITH CHECK ADD  CONSTRAINT [FK2AF0DFC27B3856E] FOREIGN KEY([CustomerPK])
REFERENCES [dbo].[tCustomers] ([rowguid])
GO

ALTER TABLE [dbo].[tCustomerEmploymentDetails] CHECK CONSTRAINT [FK2AF0DFC27B3856E]
GO



CREATE TABLE [dbo].[tCustomerGovernmentIdDetails](
	[CustomerPK] [uniqueidentifier] NOT NULL,
	[IdTypeId] [bigint] NULL,
	[Identification] [nvarchar](255) NULL,
	[ExpirationDate] [datetime] NULL,
	[DTCreate] [datetime] NULL,
	[DTLastMod] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerPK] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tCustomerGovernmentIdDetails]  WITH CHECK ADD  CONSTRAINT [FK47342B047B3856E] FOREIGN KEY([CustomerPK])
REFERENCES [dbo].[tCustomers] ([rowguid])
GO

ALTER TABLE [dbo].[tCustomerGovernmentIdDetails] CHECK CONSTRAINT [FK47342B047B3856E]
GO

