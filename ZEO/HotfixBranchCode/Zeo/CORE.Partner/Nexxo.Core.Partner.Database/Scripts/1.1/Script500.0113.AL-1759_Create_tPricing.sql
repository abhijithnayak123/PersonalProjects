--- ================================================================================
-- Author:		<Abhijith>
-- Create date: <10/28/2015>
-- Description:	<As Alloy, a configurable pricing engine should be built to apply specific pricing at the right level for each product and apply to that level>
-- Jira ID:		<AL-1759>
-- ================================================================================

CREATE TABLE [dbo].[tPricing](
    [PricingPK] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL IDENTITY (1000000000, 1),
	[PricingGroupPK] [uniqueidentifier] NOT NULL,
	[CompareTypePK] [tinyint] NULL,
	[MinimumAmount] decimal(18, 2) NULL,
	[MaximumAmount] decimal(18, 2) NULL,
	[MinimumFee] decimal(18, 2) NULL,
	[Value] decimal(18, 2) NULL,
	[IsPercentage] bit NULL,
    [DTServerCreate] [datetime] NOT NULL,
	[DTServerLastModified] [datetime] NULL,
	[DTTerminalCreate] [datetime] NOT NULL,
	[DTTerminalLastModified] [datetime] NULL,
	CONSTRAINT [PK_tPricing] PRIMARY KEY CLUSTERED 
	(
		[PricingPK] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tPricing]  WITH CHECK ADD CONSTRAINT [FK_tPricing_tPricingGroups] FOREIGN KEY([PricingGroupPK])
REFERENCES [dbo].[tPricingGroups] ([PricingGroupPK])
GO

ALTER TABLE [dbo].[tPricing]  WITH CHECK ADD CONSTRAINT [FK_tPricing_tFeeAdjustmentCompareTypes] FOREIGN KEY([CompareTypePK])
REFERENCES [dbo].[tFeeAdjustmentCompareTypes] ([CompareTypePK])
GO
	

