-- ================================================================================
-- Author:		<Namit Khandelwal>
-- Create date: <08/06/2015>
-- Description:	<As Alloy, Expiration Date field should be made optional for AZ, WY, TN and TX states >
-- Jira ID:		<AL-912>
-- ================================================================================


UPDATE 
	tNexxoIdTypes
SET 
	HasExpirationDate = 1
WHERE 
	Country = 'UNITED STATES' 
	AND NAME ='U.S. STATE IDENTITY CARD' 
	AND STATE='Texas'
GO

UPDATE 
	tNexxoIdTypes
SET 
    HasExpirationDate = 1
WHERE 
	Country = 'UNITED STATES' 
	AND NAME ='U.S. STATE IDENTITY CARD' 
	AND STATE='Arizona'
GO

UPDATE 
	tNexxoIdTypes
SET 
    HasExpirationDate = 1
WHERE 
	Country = 'UNITED STATES' 
	AND NAME ='U.S. STATE IDENTITY CARD' 
	AND STATE='Wyoming'
GO

UPDATE 
	 tNexxoIdTypes
SET 
    HasExpirationDate = 1
WHERE 
	Country = 'UNITED STATES' 
	AND NAME ='U.S. STATE IDENTITY CARD' 
	AND STATE='Tennessee'
GO
