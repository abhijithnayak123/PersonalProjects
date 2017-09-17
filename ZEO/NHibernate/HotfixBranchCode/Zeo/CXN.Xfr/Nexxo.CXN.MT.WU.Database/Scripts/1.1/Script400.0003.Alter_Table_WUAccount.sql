--===========================================================================================
-- Auther:			Shwetha
-- Date Created:	14/05/2014
-- Description:		Alter tWUnion_Account for adding send MiddleName and SecondaLastName 
-- Jira ID:			AL-502 
--===========================================================================================

IF NOT EXISTS 
(
	SELECT 
		1 
	FROM   
		sys.columns 
	WHERE  
		object_id = OBJECT_ID(N'[dbo].[tWUnion_Account]') 
		AND name = 'MiddleName'
)
BEGIN 
	ALTER TABLE tWUnion_Account 
	ADD MiddleName VARCHAR(250)
END
GO

IF NOT EXISTS 
(
	SELECT 
		1 
	FROM   
		sys.columns 
	WHERE  
		object_id = OBJECT_ID(N'[dbo].[tWUnion_Account]') 
		AND name = 'SecondLastName'
)
BEGIN 
	ALTER TABLE tWUnion_Account 
	ADD SecondLastName VARCHAR(250)
END
GO
