--===========================================================================================
-- Auther:			Bijo
-- Date Created:	17/10/2014
-- Description:		Alter Table_tMGram_Transfer 
--===========================================================================================

 ALTER TABLE tMGram_Transfer_Trx_Aud
 ALTER COLUMN ExchangeRate DECIMAL(18, 4) NULL
 GO
 ALTER TABLE tMGram_Transfer_Trx_Aud
 ALTER COLUMN ExchangeRateApplied  DECIMAL(18, 4) NULL