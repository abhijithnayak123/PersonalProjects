--===========================================================================================
-- Auther:			Rahul
-- Date Created:	16/10/2014
-- Description:		Alter Table_tMGram_Transfer 
--===========================================================================================

 ALTER TABLE tMGram_Transfer_Trx
 ALTER COLUMN ExchangeRate DECIMAL(18, 4) NULL
 GO
 ALTER TABLE tMGram_Transfer_Trx
 ALTER COLUMN ExchangeRateApplied  DECIMAL(18, 4) NULL