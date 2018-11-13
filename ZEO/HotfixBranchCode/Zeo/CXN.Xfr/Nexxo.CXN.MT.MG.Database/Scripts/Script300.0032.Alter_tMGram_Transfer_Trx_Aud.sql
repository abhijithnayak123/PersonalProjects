	--===========================================================================================
-- Auther:			Sandip Vilekar
-- Date Created:	03/12/2014
-- Description:		Change datatype tMGram_Transfer_Trx_Aud 
--===========================================================================================


ALTER TABLE [tMGram_Transfer_Trx_Aud]
ALTER COLUMN 
	SendReversalType int NULL

GO

ALTER TABLE [tMGram_Transfer_Trx_Aud]
ALTER COLUMN 
	SendReversalReason int NULL