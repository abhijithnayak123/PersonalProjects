--===========================================================================================
-- Auther:			Pamila Jose
-- Date Created:	11/04/2014
-- Description:		Alter Table tMGram_Transfer & Audit
--===========================================================================================

	EXEC sp_RENAME 'tMGram_Transfer_Trx.ReceiveReferenceNumber', 'ReceiveMoneySearchRefNo', 'COLUMN'

	EXEC sp_RENAME 'tMGram_Transfer_Trx_Aud.ReceiveReferenceNumber', 'ReceiveMoneySearchRefNo', 'COLUMN'
