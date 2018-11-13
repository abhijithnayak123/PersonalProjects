-- ============================================================
-- Author:		<Rahul K>
-- Create date: <09/09/2014>
-- Description:	<Biller Limit table creation> 
-- Rally ID:	<NA>
-- ============================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tMGram_BillerLimit]') AND type in (N'U'))
DROP TABLE [dbo].[tMGram_BillerLimit]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tMGram_BillerLimit](
		[rowguid] [uniqueidentifier] NOT NULL,
	    [Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	    [AgentUnitOffice] VARCHAR(50) NULL,
		[AgentID] VARCHAR(50) NULL,
		[ModifiedDate] datetime NULL,
		[ModifiedUser] VARCHAR(100) NULL,
		[AmtFlag] Bit NULL,
		[MinimumAmount] decimal(18,2) NULL,
		[MaximumAmount] decimal(18,2) NULL,
		[MinimumAmountMessage] VARCHAR(max) NULL,
		[MaximumAmountMessage] VARCHAR(max) NULL,
		[MessageForAmtNotInList] VARCHAR(max) NULL,
		[ReceiveCode]  VARCHAR(50) NULL,	
		[TransactingAgentID] VARCHAR(100) NULL,
		[ChannelPartnerID] int NULL,

 CONSTRAINT [PK_tMGram_BillerLimit] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

