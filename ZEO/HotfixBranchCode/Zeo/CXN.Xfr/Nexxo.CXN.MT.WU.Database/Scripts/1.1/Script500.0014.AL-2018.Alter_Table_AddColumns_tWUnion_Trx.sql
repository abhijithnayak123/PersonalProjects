--===========================================================================================
-- Author:		<Shwetha Mohan>
-- Created date: <September 25 2015>
-- Description:	<Scripts for adding columns form tWUnion_Trx_Aud tables>           
-- Jira ID:	<AL-2018>
--===========================================================================================

IF NOT EXISTS 
(
  SELECT 
	1 
  FROM   
	SYS.COLUMNS
  WHERE  
	object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx_Aud]') 
    AND name = 'CounterId'
)
BEGIN        
	ALTER TABLE tWUnion_Trx_Aud 
	ADD CounterId VARCHAR(100) NULL
END
GO

IF NOT EXISTS
(
	SELECT 
		1 
	FROM
		SYS.COLUMNS 
	WHERE
		Name = N'TransalatedDeliveryServiceName' 
		AND OBJECT_ID = OBJECT_ID(N'tWUnion_Trx_Aud')
)
BEGIN
	ALTER TABLE dbo.tWUnion_Trx_Aud 
	ADD TransalatedDeliveryServiceName VARCHAR(200) NULL
END
GO 
