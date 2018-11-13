/****** Object:  Table [dbo].[tXRecipientProfiles]    Script Date: 05/14/2013 17:15:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS(SELECT * FROM sys.objects where object_id = OBJECT_ID('tXRecipientProfiles') and TYPE in ('U'))
BEGIN
CREATE TABLE [dbo].[tXRecipientProfiles](
	[id] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[XProviderId] [int] NULL,
	[PAN] [bigint] NOT NULL,
	[Active] [bit] NOT NULL,
	[FName] [nvarchar](50) NULL,
	[MName] [nvarchar](50) NULL,
	[LName] [nvarchar](50) NULL,
	[LName2] [nvarchar](50) NULL,
	[Relationship] [nvarchar](20) NULL,
	[ResAddr1] [nvarchar](50) NULL,
	[ResAddr2] [nvarchar](50) NULL,
	[ResCity] [nvarchar](50) NULL,
	[ResState] [nvarchar](50) NULL,
	[ResCountryCode] [int] NULL,
	[ResPostalCode] [nvarchar](50) NULL,
	[ResTelephoneNumber] [nvarchar](50) NULL,
	[PickupMethodId] [int] NULL,
	[MTCountryId] [nvarchar](10) NULL,
	[MTStateId] [nvarchar](50) NULL,
	[MTCityId] [nvarchar](50) NULL,
	[MTBankId] [nvarchar](20) NULL,
	[MTBranchId] [nvarchar](50) NULL,
	[MTCurrencyId] [nvarchar](10) NULL,
	[RecCardNumber] [nvarchar](50) NULL,
	[RecAccountNumber] [nvarchar](50) NULL,
	[RecTypeOfAccount] [nvarchar](50) NULL,
	[Notes] [text] NULL,
	[DTCreate] [datetime] NULL,
	[DTLastMod] [datetime] NULL,
	[LastEditBy] [nvarchar](20) NULL,
	[FirstEditBy] [nvarchar](20) NULL,
	[PoPId] [uniqueidentifier] NULL,
	[Completed] [bit] NULL,
	[DOB] [datetime] NULL,
	[WatchList] [bit] NULL,
	[BeneficiaryId] [int] IDENTITY(1,1) NOT NULL,
	[FName_Soundex]  AS (soundex(replace([FName],' ',''))) PERSISTED,
	[MName_Soundex]  AS (soundex(replace([MName],' ',''))) PERSISTED,
	[LName_Soundex]  AS (soundex(replace([LName],' ',''))) PERSISTED,
	[LName2_Soundex]  AS (soundex(replace([LName2],' ',''))) PERSISTED,
	[Linked] [bit] NOT NULL,
	[RelationshipNote] [nvarchar](200) NULL,
	[CityId] [uniqueidentifier] NULL,
	[BirthCountryCode] [int] NULL,
	[Gender] [nvarchar](6) NULL,
	[Test] [bit] NOT NULL,
	[TaxpayerId] [nvarchar](20) NULL,
	[Lead] [bit] NULL,
 CONSTRAINT [PK_tXRecipientProfiles] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]



SET ANSI_PADDING OFF

ALTER TABLE [dbo].[tXRecipientProfiles] ADD  CONSTRAINT [DF_tXRecipientProfiles_id]  DEFAULT (newid()) FOR [id]

ALTER TABLE [dbo].[tXRecipientProfiles] ADD  CONSTRAINT [DF_tXRecipientProfiles_DTCreate]  DEFAULT (getdate()) FOR [DTCreate]

ALTER TABLE [dbo].[tXRecipientProfiles] ADD  CONSTRAINT [DF_tXRecipientProfiles_Completed]  DEFAULT ((1)) FOR [Completed]

ALTER TABLE [dbo].[tXRecipientProfiles] ADD  CONSTRAINT [DF__tXRecipie__Linke__3C7FD589]  DEFAULT ((1)) FOR [Linked]

ALTER TABLE [dbo].[tXRecipientProfiles] ADD  CONSTRAINT [DF__tXRecipien__Test__3D73F9C2]  DEFAULT ((0)) FOR [Test]

ALTER TABLE [dbo].[tXRecipientProfiles] ADD  CONSTRAINT [DF_tXRecipientProfiles_Lead]  DEFAULT ((0)) FOR [Lead]

END