SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tChannelPartnerConfig]
(
	[ChannelPartnerID] [uniqueidentifier] NOT NULL,
	[DisableWithdrawCNP] [bit] NOT NULL,
	[CashOverCounter] [bit] NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tChannelPartnerConfig] 
ADD  CONSTRAINT [DF_tChannelPartnerConfig_DisableWithdraw]  DEFAULT ((0)) FOR [DisableWithdrawCNP]
GO

ALTER TABLE [dbo].[tChannelPartnerConfig] 
ADD  CONSTRAINT [DF_tChannelPartnerConfig_CashOverCounter]  DEFAULT ((0)) FOR [CashOverCounter]
GO

