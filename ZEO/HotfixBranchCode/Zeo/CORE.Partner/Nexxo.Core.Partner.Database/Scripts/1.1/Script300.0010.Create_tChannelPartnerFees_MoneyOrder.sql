CREATE TABLE [dbo].[tChannelPartnerFees_MoneyOrder](
	[rowguid] [uniqueidentifier] NOT NULL,
	[ChannelPartnerPK] [uniqueidentifier] NOT NULL,
	[Fee] [money] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tChannelPartnerFees_MoneyOrder] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tChannelPartnerFees_MoneyOrder] ADD  CONSTRAINT [DF_tChannelPartnerFees_MoneyOrder_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO

ALTER TABLE [dbo].[tChannelPartnerFees_MoneyOrder] ADD  CONSTRAINT [DF_tChannelPartnerFees_MoneyOrder_DTCreate]  DEFAULT (getdate()) FOR [DTCreate]
GO

ALTER TABLE [dbo].[tChannelPartnerFees_MoneyOrder]  WITH CHECK ADD  CONSTRAINT [FK_tChannelPartnerFees_MoneyOrder_tChannelPartners] FOREIGN KEY([ChannelPartnerPK])
REFERENCES [dbo].[tChannelPartners] ([rowguid])
GO

ALTER TABLE [dbo].[tChannelPartnerFees_MoneyOrder] CHECK CONSTRAINT [FK_tChannelPartnerFees_MoneyOrder_tChannelPartners]
GO


declare @PartnerPK uniqueidentifier

-- Synovus 
select @PartnerPK = rowguid from tChannelPartners where id = 33

insert tChannelPartnerFees_MoneyOrder(ChannelPartnerPK, Fee)
values (@PartnerPK, 2.00)

-- Carver 
select @PartnerPK = rowguid from tChannelPartners where id = 28

insert tChannelPartnerFees_MoneyOrder(ChannelPartnerPK, Fee)
values (@PartnerPK, 0.50)
