
ALTER TABLE [dbo].[tTSys_Trx] ADD ChannelPartnerID bigint
GO

Update [dbo].[tTSys_Trx] set ChannelPartnerID=33
GO

ALTER TABLE [dbo].[tTSys_Trx] ALTER COLUMN ChannelPartnerID bigint NOT NULL
GO
