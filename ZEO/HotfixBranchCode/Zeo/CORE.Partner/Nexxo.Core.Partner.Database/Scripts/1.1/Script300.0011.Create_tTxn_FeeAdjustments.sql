CREATE TABLE [dbo].[tTxn_FeeAdjustments](
	[rowguid] [uniqueidentifier] NOT NULL,
	[TxnPK] [uniqueidentifier] NOT NULL,
	[FeeAdjustmentPK] [uniqueidentifier] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
 CONSTRAINT [PK_tTxn_FeeAdjustments] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tTxn_FeeAdjustments]  WITH CHECK ADD  CONSTRAINT [FK_tTxn_FeeAdjustments_tChannelPartnerFeeAdjustments] FOREIGN KEY([FeeAdjustmentPK])
REFERENCES [dbo].[tChannelPartnerFeeAdjustments] ([rowguid])
GO

ALTER TABLE [dbo].[tTxn_FeeAdjustments] CHECK CONSTRAINT [FK_tTxn_FeeAdjustments_tChannelPartnerFeeAdjustments]
GO
