--===========================================================================================
-- Author:		<Shwetha Mohan>
-- Created date: <September 25 2015>
-- Description:	<Scripts for dropping unused column  from tChxr_Trx_Aud table>           
-- Jira ID:	<AL-2018>
--===========================================================================================

IF  EXISTS 
(
  SELECT 
	1 
  FROM   
	SYS.COLUMNS
  WHERE  
	object_id = OBJECT_ID(N'[dbo].[tChxr_Trx_Aud]') 
    AND name = 'Type'
)
BEGIN
	ALTER TABLE tChxr_Trx_Aud DROP  COLUMN [Type]
END
GO

IF EXISTS 
(
  SELECT 
	1 
  FROM   
	SYS.COLUMNS 
  WHERE  
	object_id = OBJECT_ID(N'[dbo].[tChxr_Trx_Aud]') 
    AND name = 'ChexarType'
)
BEGIN
	ALTER TABLE tChxr_Trx_Aud DROP  COLUMN ChexarType
END
GO

