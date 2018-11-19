
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TBL_TrustAdvisorLookup_Exception]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TBL_TrustAdvisorLookup_Exception](
	[SubContactID] [int] NULL,
	[ContactID] [int] NULL,
	[RoleCode] [int] NULL,
	[TrustAdvisorID] [int] NULL,	
	[PartyID] [int] NULL,
	[ParticipantID] int null
) ON [PRIMARY]
END
GO
