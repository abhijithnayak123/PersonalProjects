-- ============================================================
-- Author:		<Rita Patel>
-- Create date: <06/08/2015>
-- Description:	<Script to Upadet records to tNexxoIdTypes table>
-- Jira ID:		<AL-544>
-- ============================================================

UPDATE 
	[dbo].[tNexxoIdTypes]
SET 
	[HasExpirationDate] = 0
WHERE 
	Country = 'UNITED STATES' 
	AND NAME ='U.S. STATE IDENTITY CARD' 
	AND STATE='Texas'
GO

UPDATE 
	[dbo].[tNexxoIdTypes]
SET 
    [HasExpirationDate] = 0
WHERE 
	Country = 'UNITED STATES' 
	AND NAME ='U.S. STATE IDENTITY CARD' 
	AND STATE='Arizona'
GO

UPDATE 
	[dbo].[tNexxoIdTypes]
SET 
    [HasExpirationDate] = 0
WHERE 
	Country = 'UNITED STATES' 
	AND NAME ='U.S. STATE IDENTITY CARD' 
	AND STATE='Wyoming'
GO

UPDATE 
	[dbo].[tNexxoIdTypes]
SET 
    [HasExpirationDate] = 0
WHERE 
	Country = 'UNITED STATES' 
	AND NAME ='U.S. STATE IDENTITY CARD' 
	AND STATE='Tennessee'
GO


