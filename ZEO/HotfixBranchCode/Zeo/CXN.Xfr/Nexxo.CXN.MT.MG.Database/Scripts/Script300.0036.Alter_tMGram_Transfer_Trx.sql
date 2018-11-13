	--===========================================================================================
-- Auther:			Rahul K
-- Date Created:	17/12/2014
-- Description:		New column NickName added 
--===========================================================================================


    ALTER TABLE tMGram_Transfer_Trx 
	ADD 
	[ReceiverNickName] [nvarchar] (255) NULL
	GO
