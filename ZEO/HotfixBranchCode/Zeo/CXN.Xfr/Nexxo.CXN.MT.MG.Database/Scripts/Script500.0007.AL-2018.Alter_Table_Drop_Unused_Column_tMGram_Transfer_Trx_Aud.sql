--===========================================================================================
-- Author:		<Shwetha Mohan>
-- Created date: <September 25 2015>
-- Description:	<Scripts for dropping unused Column from tMGram_Transfer_Trx_Aud>           
-- Jira ID:	<AL-2018>
--===========================================================================================
IF EXISTS 
(
  SELECT 
	1 
  FROM   
	SYS.COLUMNS 
  WHERE  
	OBJECT_ID = OBJECT_ID(N'[dbo].[tMGram_Transfer_Trx_Aud]') 
    AND name = 'TimeStamp'
)
BEGIN
	ALTER TABLE tMGram_Transfer_Trx_Aud DROP COLUMN [TimeStamp]
END
GO