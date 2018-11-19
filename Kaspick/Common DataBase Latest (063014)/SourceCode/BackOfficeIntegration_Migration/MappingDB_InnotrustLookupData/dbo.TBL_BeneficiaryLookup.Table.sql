
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TBL_BeneficiaryLookup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TBL_BeneficiaryLookup](
	[ContactID] [int] NOT NULL,
	[CustomerAccountNumber] [char](14) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[RoleCode] [int] NOT NULL,	
	[PaidForContactID] INT NULL,
	BeneficiaryDistributionID INT null,
	
	[BeneficiaryID] [int] NOT NULL,
	[K1BeneficiaryID] [int] NULL,
	
	[ParticipantID] [int] NOT NULL,
	[AccountID] [int] NULL,
	
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
