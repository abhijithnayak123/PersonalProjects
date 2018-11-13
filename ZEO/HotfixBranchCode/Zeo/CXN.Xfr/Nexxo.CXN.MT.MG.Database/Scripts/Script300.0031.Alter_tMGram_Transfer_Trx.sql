	--===========================================================================================
-- Auther:			Sandip Vilekar
-- Date Created:	03/12/2014
-- Description:		Change datatype tMGram_Transfer_Trx 
--===========================================================================================


ALTER TABLE [tMGram_Transfer_Trx]
ALTER COLUMN 
	SendReversalType int NULL

GO

ALTER TABLE [tMGram_Transfer_Trx]
ALTER COLUMN 
	SendReversalReason int NULL