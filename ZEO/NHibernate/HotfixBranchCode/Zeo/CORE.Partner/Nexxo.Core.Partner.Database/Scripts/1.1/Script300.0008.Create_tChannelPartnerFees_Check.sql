CREATE TABLE [dbo].[tChannelPartnerFees_Check](
	[rowguid] [uniqueidentifier] NOT NULL,
	[ChannelPartnerPK] [uniqueidentifier] NOT NULL,
	[CheckType] [int] NOT NULL,
	[FeeRate] [money] NOT NULL,
	[FeeMinimum] [money] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tChannelPartnerFees_Check] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tChannelPartnerFees_Check] ADD  CONSTRAINT [DF_tChannelPartnerFees_Check_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO

ALTER TABLE [dbo].[tChannelPartnerFees_Check] ADD  CONSTRAINT [DF_tChannelPartnerFees_Check_DTCreate]  DEFAULT (getdate()) FOR [DTCreate]
GO

ALTER TABLE [dbo].[tChannelPartnerFees_Check]  WITH CHECK ADD  CONSTRAINT [FK_tChannelPartnerFees_Check_tChannelPartners] FOREIGN KEY([ChannelPartnerPK])
REFERENCES [dbo].[tChannelPartners] ([rowguid])
GO

ALTER TABLE [dbo].[tChannelPartnerFees_Check] CHECK CONSTRAINT [FK_tChannelPartnerFees_Check_tChannelPartners]
GO

declare @PartnerPK uniqueidentifier,
		@FeeMin decimal

-- Centris 
select @PartnerPK = rowguid from tChannelPartners where id = 27
set @FeeMin = 2.00

insert tChannelPartnerFees_Check(ChannelPartnerPK, CheckType, FeeRate, FeeMinimum)
values 
(@PartnerPK, 1, 0.060, @FeeMin),
(@PartnerPK, 2, 0.020, @FeeMin),
(@PartnerPK, 3, 0.020, @FeeMin),
(@PartnerPK, 4, 0.015, @FeeMin),
(@PartnerPK, 5, 0.060, @FeeMin),
(@PartnerPK, 6, 0.040, @FeeMin),
(@PartnerPK, 7, 0.020, @FeeMin),
(@PartnerPK, 8, 0.020, @FeeMin),
(@PartnerPK, 9, 0.020, @FeeMin),
(@PartnerPK, 10, 0.060, @FeeMin),
(@PartnerPK, 11, 0.060, @FeeMin),
(@PartnerPK, 12, 0.060, @FeeMin),
(@PartnerPK, 13, 0.060, @FeeMin),
(@PartnerPK, 14, 0.034, @FeeMin),
(@PartnerPK, 15, 0.060, @FeeMin),
(@PartnerPK, 16, 0.060, @FeeMin),
(@PartnerPK, 17, 0.034, @FeeMin)

-- Synovus 
select @PartnerPK = rowguid from tChannelPartners where id = 33
set @FeeMin = 1.00

insert tChannelPartnerFees_Check(ChannelPartnerPK, CheckType, FeeRate, FeeMinimum)
values 
(@PartnerPK, 1, 0.029, @FeeMin),
(@PartnerPK, 2, 0.015, @FeeMin),
(@PartnerPK, 3, 0.015, @FeeMin),
(@PartnerPK, 4, 0.015, @FeeMin),
(@PartnerPK, 5, 0.049, @FeeMin),
(@PartnerPK, 6, 0.029, @FeeMin),
(@PartnerPK, 7, 0.015, @FeeMin),
(@PartnerPK, 8, 0.015, @FeeMin),
(@PartnerPK, 9, 0.015, @FeeMin),
(@PartnerPK, 10, 0.029, @FeeMin),
(@PartnerPK, 11, 0.029, @FeeMin),
(@PartnerPK, 12, 0.029, @FeeMin),
(@PartnerPK, 13, 0.029, @FeeMin),
(@PartnerPK, 14, 0.029, @FeeMin),
(@PartnerPK, 15, 0.029, @FeeMin),
(@PartnerPK, 16, 0.029, @FeeMin),
(@PartnerPK, 17, 0.029, @FeeMin)

-- Carver 
select @PartnerPK = rowguid from tChannelPartners where id = 28
set @FeeMin = 3.00

insert tChannelPartnerFees_Check(ChannelPartnerPK, CheckType, FeeRate, FeeMinimum)
values 
(@PartnerPK, 1, 0.0186, @FeeMin),
(@PartnerPK, 2, 0.0186, @FeeMin),
(@PartnerPK, 3, 0.0186, @FeeMin),
(@PartnerPK, 4, 0.0186, @FeeMin),
(@PartnerPK, 5, 0.0186, @FeeMin),
(@PartnerPK, 6, 0.0186, @FeeMin),
(@PartnerPK, 7, 0.0186, @FeeMin),
(@PartnerPK, 8, 0.0186, @FeeMin),
(@PartnerPK, 9, 0.0186, @FeeMin),
(@PartnerPK, 10, 0.0186, @FeeMin),
(@PartnerPK, 11, 0.0186, @FeeMin),
(@PartnerPK, 12, 0.0186, @FeeMin),
(@PartnerPK, 13, 0.0186, @FeeMin),
(@PartnerPK, 14, 0.0186, @FeeMin),
(@PartnerPK, 15, 0.0186, @FeeMin),
(@PartnerPK, 16, 0.0186, @FeeMin),
(@PartnerPK, 17, 0.0186, @FeeMin)