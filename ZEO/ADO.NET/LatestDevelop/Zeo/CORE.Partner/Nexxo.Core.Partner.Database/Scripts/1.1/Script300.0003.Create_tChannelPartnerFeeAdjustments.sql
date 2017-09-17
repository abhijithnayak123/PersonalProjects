CREATE TABLE [dbo].[tChannelPartnerFeeAdjustments](
	[rowguid] [uniqueidentifier] NOT NULL,
	[ChannelPartnerPK] [uniqueidentifier] NOT NULL,
	[TransactionType] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](100) NULL,
	[DTStart] [datetime] NOT NULL,
	[DTEnd] [datetime] NULL,
	[SystemApplied] [bit] NOT NULL,
	[AdjustmentRate] [money] NOT NULL,
	[AdjustmentAmount] [money] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tChannelPartnerFeeAdjustments] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tChannelPartnerFeeAdjustments] ADD  CONSTRAINT [DF_tChannelPartnerFeeAdjustments_AdjustmentRate]  DEFAULT ((0)) FOR [AdjustmentRate]
GO

ALTER TABLE [dbo].[tChannelPartnerFeeAdjustments] ADD  CONSTRAINT [DF_tChannelPartnerFeeAdjustments_AdjustmentAmount]  DEFAULT ((0)) FOR [AdjustmentAmount]
GO

ALTER TABLE [dbo].[tChannelPartnerFeeAdjustments] ADD  CONSTRAINT [DF_tChannelPartnerFeeAdjustments_DTCreate]  DEFAULT (getdate()) FOR [DTCreate]
GO

ALTER TABLE [dbo].[tChannelPartnerFeeAdjustments]  WITH CHECK ADD  CONSTRAINT [FK_tChannelPartnerFeeAdjustments_tChannelPartners] FOREIGN KEY([ChannelPartnerPK])
REFERENCES [dbo].[tChannelPartners] ([rowguid])
GO

ALTER TABLE [dbo].[tChannelPartnerFeeAdjustments] CHECK CONSTRAINT [FK_tChannelPartnerFeeAdjustments_tChannelPartners]
GO
