
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tIdentificationConfirmation]') AND type in (N'U'))
DROP TABLE [dbo].[tIdentificationConfirmation]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tIdentificationConfirmation](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[AgentID] [nvarchar](100) NOT NULL,
	[CustomerSessionID] [nvarchar](100) NOT NULL,
	[DateIdentified] [datetime] NULL,
	[ConfirmStatus] bit NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	CONSTRAINT [PK_tRecordIdentityConfirmed] PRIMARY KEY CLUSTERED 
	(
		[rowguid] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
) ON [PRIMARY]
GO
