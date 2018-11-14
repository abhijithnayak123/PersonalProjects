-- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-21-2017>
-- Description:	Inserting On-us Check Types in CheckType table. 
-- Jira ID:		<B-08674>
-- ================================================================================

IF NOT EXISTS (SELECT 1 FROM tCheckTypes WHERE Name in('OnUsOCMO','OnUsTRUE','OnUsOTHER'))
BEGIN

	DECLARE @maxCheckId INT = (SELECT MAX(CheckTypeId) FROM tCheckTypes)

	 INSERT INTO tCheckTypes 
	 (
	 	CheckTypeId,
	 	Name,
		ProductProviderCode
	 )
	 VALUES 
	 (@maxCheckId + 1, 'OnUsOCMO', 202),
	 (@maxCheckId + 2, 'OnUsTRUE', 202),
	 (@maxCheckId + 3, 'OnUsOTHER',202)

END
GO