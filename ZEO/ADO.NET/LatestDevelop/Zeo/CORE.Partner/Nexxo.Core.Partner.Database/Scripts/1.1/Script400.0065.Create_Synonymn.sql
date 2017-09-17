--===========================================================================================
-- Author:		<Prince Bajaj>
-- Create date: <Mar 31 2015>
-- Description:	<Script to Create Synonymn for FIS and CCIS >
-- Jira ID:	    <AL-265>
--===========================================================================================
IF EXISTS (SELECT * FROM sys.synonyms WHERE name = N'stCCISAccount')
 DROP SYNONYM [dbo].[stCCISAccount]
 GO
 CREATE SYNONYM [dbo].[stCCISAccount] FOR [$CXNDATABASE$].[dbo].[tCCIS_Account]
 GO

 IF EXISTS (SELECT * FROM sys.synonyms WHERE name = N'stFISAccount')
 DROP SYNONYM [dbo].[stFISAccount]
 GO
 CREATE SYNONYM [dbo].[stFISAccount] FOR [$CXNDATABASE$].[dbo].[tFIS_Account]
 GO
