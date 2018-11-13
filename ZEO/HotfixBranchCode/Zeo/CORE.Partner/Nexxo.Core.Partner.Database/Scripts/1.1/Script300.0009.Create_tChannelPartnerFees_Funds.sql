CREATE TABLE [dbo].[tChannelPartnerFees_Funds](
	[rowguid] [uniqueidentifier] NOT NULL,
	[ChannelPartnerPK] [uniqueidentifier] NOT NULL,
	[FundsType] [int] NOT NULL,
	[Fee] [money] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tChannelPartnerFees_Funds] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tChannelPartnerFees_Funds] ADD  CONSTRAINT [DF_tChannelPartnerFees_Funds_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO

ALTER TABLE [dbo].[tChannelPartnerFees_Funds] ADD  CONSTRAINT [DF_tChannelPartnerFees_Funds_DTCreate]  DEFAULT (getdate()) FOR [DTCreate]
GO

ALTER TABLE [dbo].[tChannelPartnerFees_Funds]  WITH CHECK ADD  CONSTRAINT [FK_tChannelPartnerFees_Funds_tChannelPartners] FOREIGN KEY([ChannelPartnerPK])
REFERENCES [dbo].[tChannelPartners] ([rowguid])
GO

ALTER TABLE [dbo].[tChannelPartnerFees_Funds] CHECK CONSTRAINT [FK_tChannelPartnerFees_Funds_tChannelPartners]
GO

declare @PartnerPK uniqueidentifier,
		@FeeMin decimal

-- Centris 
select @PartnerPK = rowguid from tChannelPartners where id = 27

insert tChannelPartnerFees_Funds(ChannelPartnerPK, FundsType, Fee)
values
(@PartnerPK, 0, 2.00),
(@PartnerPK, 1, 2.00),
(@PartnerPK, 2, 0.00)

-- Synovus 
select @PartnerPK = rowguid from tChannelPartners where id = 33

insert tChannelPartnerFees_Funds(ChannelPartnerPK, FundsType, Fee)
values
(@PartnerPK, 0, 0.00),
(@PartnerPK, 1, 0.00),
(@PartnerPK, 2, 0.00)

-- Carver 
select @PartnerPK = rowguid from tChannelPartners where id = 28

insert tChannelPartnerFees_Funds(ChannelPartnerPK, FundsType, Fee)
values
(@PartnerPK, 0, 0.00),
(@PartnerPK, 1, 0.00),
(@PartnerPK, 2, 0.00)