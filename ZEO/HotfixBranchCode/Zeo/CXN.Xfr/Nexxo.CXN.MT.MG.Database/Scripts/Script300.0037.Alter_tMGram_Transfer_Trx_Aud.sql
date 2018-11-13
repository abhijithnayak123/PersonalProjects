	--===========================================================================================
-- Auther:			Rahul K
-- Date Created:	17/12/2014
-- Description:		New column NickName added in tMGram_Transfer_Trx_Aud 
--===========================================================================================


    ALTER TABLE tMGram_Transfer_Trx_Aud 
	ADD 
	[ReceiverNickName] [nvarchar] (255) NULL
	GO
