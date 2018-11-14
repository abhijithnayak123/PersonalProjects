CREATE TABLE [dbo].[tFeeAdjustmentConditions](
	[rowguid] [uniqueidentifier] NOT NULL,
	[FeeAdjustmentPK] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](100) NULL,
	[ConditionType] [int] NOT NULL,
	[CompareType] [int] NOT NULL,
	[ConditionValue] [nvarchar](1000) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tFeeAdjustmentConditions] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tFeeAdjustmentConditions]  WITH CHECK ADD  CONSTRAINT [FK_tFeeAdjustmentConditions_tChannelPartnerFeeAdjustments] FOREIGN KEY([FeeAdjustmentPK])
REFERENCES [dbo].[tChannelPartnerFeeAdjustments] ([rowguid])
GO

ALTER TABLE [dbo].[tFeeAdjustmentConditions] CHECK CONSTRAINT [FK_tFeeAdjustmentConditions_tChannelPartnerFeeAdjustments]
GO

ALTER TABLE [dbo].[tFeeAdjustmentConditions]  WITH CHECK ADD  CONSTRAINT [FK_tFeeAdjustmentConditions_tFeeAdjustmentConditionTypes] FOREIGN KEY([ConditionType])
REFERENCES [dbo].[tFeeAdjustmentConditionTypes] ([Id])
GO

ALTER TABLE [dbo].[tFeeAdjustmentConditions] CHECK CONSTRAINT [FK_tFeeAdjustmentConditions_tFeeAdjustmentConditionTypes]
GO



