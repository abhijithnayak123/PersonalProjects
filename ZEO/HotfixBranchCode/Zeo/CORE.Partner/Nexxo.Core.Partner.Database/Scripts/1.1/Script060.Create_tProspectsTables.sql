CREATE TABLE [dbo].[tProspects](
	[id] [uniqueidentifier] NOT NULL,
	[PAN] [bigint] NULL,
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
	[Gender] [nvarchar](6) NULL,
	[Email] [nvarchar](320) NULL,
	[IsAccountHolder] [bit] NULL,
	[ReferralCode] [nvarchar](16) NULL,
	[PIN] [nvarchar](4) NULL,
	[IsMailingAddressDifferent] [bit] NULL,
	[MailingAddress1] [nvarchar](255) NULL,
	[MailingAddress2] [nvarchar](255) NULL,
	[MailingCity] [nvarchar](255) NULL,
	[MailingState] [nvarchar](255) NULL,
	[MailingZipCode] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[tProspectGovernmentIdDetails](
	[ProspectId] [uniqueidentifier] NOT NULL,
	[PAN] [bigint] NULL,
	[Identification] [nvarchar](255) NULL,
	[ExpirationDate] [datetime] NULL,
	[DTCreate] [datetime] NULL,
	[DTLastMod] [datetime] NULL,
	[IdTypePK] [uniqueidentifier] NULL,
	[IssueDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProspectId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tProspectGovernmentIdDetails]  WITH CHECK ADD  CONSTRAINT [FK_tProspectGovernmentIdDetails_tNexxoIdTypes] FOREIGN KEY([IdTypePK])
REFERENCES [dbo].[tNexxoIdTypes] ([rowguid])
GO

ALTER TABLE [dbo].[tProspectGovernmentIdDetails] CHECK CONSTRAINT [FK_tProspectGovernmentIdDetails_tNexxoIdTypes]
GO

ALTER TABLE [dbo].[tProspectGovernmentIdDetails]  WITH CHECK ADD  CONSTRAINT [FKBC9FBE4E30F18057] FOREIGN KEY([ProspectId])
REFERENCES [dbo].[tProspects] ([id])
GO

ALTER TABLE [dbo].[tProspectGovernmentIdDetails] CHECK CONSTRAINT [FKBC9FBE4E30F18057]
GO


CREATE TABLE [dbo].[tProspectEmploymentDetails](
	[ProspectId] [uniqueidentifier] NOT NULL,
	[PAN] [bigint] NULL,
	[Occupation] [nvarchar](255) NULL,
	[Employer] [nvarchar](255) NULL,
	[EmployerPhone] [nvarchar](255) NULL,
	[DTCreate] [datetime] NULL,
	[DTLastMod] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProspectId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tProspectEmploymentDetails]  WITH CHECK ADD  CONSTRAINT [FK23E166C730F18057] FOREIGN KEY([ProspectId])
REFERENCES [dbo].[tProspects] ([id])
GO

ALTER TABLE [dbo].[tProspectEmploymentDetails] CHECK CONSTRAINT [FK23E166C730F18057]
GO



