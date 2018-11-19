/****** Object:  Table [dbo].[TBL_TrustParticipantLookup]    Script Date: 04/11/2014 11:36:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TBL_TrustParticipantLookup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TBL_TrustParticipantLookup](
	[ContactId] [int] NOT NULL,
	[ParticipantID] [int] NOT NULL,
	[ClientBriefName] [varchar](6) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
