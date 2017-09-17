--- ================================================================================
-- Author:		<Abhijith>
-- Create date: <10/28/2015>
-- Description:	<As Alloy, a configurable pricing engine should be built to apply specific pricing at the right level for each product and apply to that level>
-- Jira ID:		<AL-1759>
-- ================================================================================

CREATE TABLE [dbo].[tPricingGroups](
    [PricingGroupPK] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL IDENTITY (1000000000, 1),
	[PricingGroupName] [nvarchar](100) NULL,
	[DTServerCreate] [datetime] NOT NULL,
	[DTServerLastModified] [datetime] NULL,
	[DTTerminalCreate] [datetime] NOT NULL,
	[DTTerminalLastModified] [datetime] NULL,
	CONSTRAINT [PK_tPricingGroups] PRIMARY KEY CLUSTERED 
	(
		[PricingGroupPK] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
