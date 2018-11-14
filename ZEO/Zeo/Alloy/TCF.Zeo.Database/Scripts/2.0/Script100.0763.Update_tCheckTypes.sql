--- ===============================================================================
-- Author:		<M.Pushkal>
-- Create date: <06-06-2018>
-- Description:	Adding a column "Display name"
-- ================================================================================


--======================== Updating the Name with the actual name ========================

IF EXISTS(SELECT 1 FROM tCheckTypes WHERE Name = 'Official Check or Money Order')
BEGIN
	UPDATE tCheckTypes 
	SET Name = 'OnUsOCMO'
	WHERE Name = 'Official Check or Money Order' AND ProductProviderCode = 202
END 

IF EXISTS(SELECT 1 FROM tCheckTypes WHERE Name = 'TCF Check')
BEGIN
	UPDATE tCheckTypes 
	SET Name = 'OnUsTRUE'
	WHERE Name = 'TCF Check' AND ProductProviderCode = 202
END 

IF EXISTS(SELECT 1 FROM tCheckTypes WHERE Name = 'OnUs Other')
BEGIN
	UPDATE tCheckTypes 
	SET Name = 'OnUsOTHER'
	WHERE Name = 'OnUs Other' AND ProductProviderCode = 202
END
GO
--================ Creating a New column "DisplayName" in the tCheckTypes table ==================

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'DisplayName'AND Object_ID = Object_ID(N'tCheckTypes'))
BEGIN
    ALTER TABLE tCheckTypes ADD DisplayName NVARCHAR(1000)
END
GO
--================ Updating the "DisplayName" column ============================

IF EXISTS(SELECT 1 FROM tCheckTypes WHERE Name = 'OnUsOCMO')
BEGIN
	UPDATE tCheckTypes 
	SET DisplayName = 'Official Check or Money Order'
	WHERE Name = 'OnUsOCMO' AND ProductProviderCode = 202
END 

IF EXISTS(SELECT 1 FROM tCheckTypes WHERE Name = 'OnUsTRUE')
BEGIN
	UPDATE tCheckTypes 
	SET DisplayName = 'TCF Check'
	WHERE Name = 'OnUsTRUE' AND ProductProviderCode = 202
END 

IF EXISTS(SELECT 1 FROM tCheckTypes WHERE Name = 'OnUsOTHER')
BEGIN
	UPDATE tCheckTypes 
	SET DisplayName = 'OnUs Other'
	WHERE Name = 'OnUsOTHER' AND ProductProviderCode = 202
END 
GO

--====================== Updating the Off Us check type display names =====================
	UPDATE tCheckTypes 
	SET DisplayName = Name
	WHERE  ProductProviderCode = 200
GO
