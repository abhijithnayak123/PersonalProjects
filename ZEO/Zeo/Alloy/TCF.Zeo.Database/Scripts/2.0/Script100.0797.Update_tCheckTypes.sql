--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <08-28-2018>
-- Description:	Updating the Check Type Name.
-- ================================================================================

UPDATE tCheckTypes 
SET Name = 'OnUsTrue'
WHERE Name = 'OnUsTRUE' AND ProductProviderCode = 202


UPDATE tCheckTypes 
SET Name = 'OnUsOther'
WHERE Name = 'OnUsOTHER' AND ProductProviderCode = 202

GO
