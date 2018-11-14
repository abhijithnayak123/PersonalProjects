--===========================================================================================
-- Auther:			Bijo
-- Date Created:	21/10/2014
-- Description:		Alter Table_tMGram_Transfer_Aud 
--===========================================================================================

ALTER TABLE tMGram_Transfer_Trx_Aud
ALTER COLUMN [PromotionErrorCodeMessage_PrimaryLanguage] [varchar](1000) NULL
GO
ALTER TABLE tMGram_Transfer_Trx_Aud
ALTER COLUMN [PromotionalMessage_PrimaryLanguage] [varchar](1000) NULL
GO
ALTER TABLE tMGram_Transfer_Trx_Aud
ALTER COLUMN [PromotionalMessage_SecondaryLanguage] [varchar](1000) NULL
GO
ALTER TABLE tMGram_Transfer_Trx_Aud
ALTER COLUMN [ReceiptTextInfo_PrimaryLanguage] [varchar](1000) NULL
GO
ALTER TABLE tMGram_Transfer_Trx_Aud
ALTER COLUMN [ReceiptTextInfo_SecondaryLanguage] [varchar](1000) NULL
GO
ALTER TABLE tMGram_Transfer_Trx_Aud
ALTER COLUMN [PromotionErrorCodeMessage_SecondaryLanguage] [varchar](1000) NULL
GO
ALTER TABLE tMGram_Transfer_Trx_Aud
ALTER COLUMN [DisclosureText_PrimaryLanguage] [varchar](1000) NULL
GO
ALTER TABLE tMGram_Transfer_Trx_Aud
ALTER COLUMN [DisclosureText_SecondaryLanguage] [varchar](1000) NULL

