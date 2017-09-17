	--===========================================================================================
-- Auther:			Rahul K
-- Date Created:	20/11/2014
-- Description:		Alter Table_tMGram_Transfer_Aud
--===========================================================================================


	ALTER TABLE tMGram_Transfer_Trx_Aud 
	ADD 
	[OriginalTransactionID] [bigint] NULL,
    [TransactionSubType] [varchar](20) NULL
	