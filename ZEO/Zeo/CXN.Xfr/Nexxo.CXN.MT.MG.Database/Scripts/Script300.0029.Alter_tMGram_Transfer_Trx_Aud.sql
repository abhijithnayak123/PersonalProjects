
	--===========================================================================================
-- Auther:			Rahul K
-- Date Created:	1/12/2014
-- Description:		New columns related to Test question and Test Answer in tMGram_Transfer_Trx_Aud
--===========================================================================================
	ALTER TABLE tMGram_Transfer_Trx_Aud 
	ADD 
	[IsTestQusAndAnsRequired] [varchar] (10) NULL
	GO