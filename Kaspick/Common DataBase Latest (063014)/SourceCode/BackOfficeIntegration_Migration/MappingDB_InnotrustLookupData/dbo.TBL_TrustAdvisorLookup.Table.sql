
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TBL_TrustAdvisorLookup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TBL_TrustAdvisorLookup](
	[SubContactId] [int] NOT NULL,
	[ContactId] [int] NOT NULL,
	[RoleCode] [int] NOT NULL,
	[TrustAdvisorID] [int]  NULL,
	[PartyID] [int] not null,
	[ParticipantID] int not null
) ON [PRIMARY]
END
GO


