--- ===============================================================================
-- Author     :	 M.Purna Pushkal
-- Description:  Creating the new table for SupportInformation details
-- Creatd Date:  12-02-2018
-- Story Id   :  B-12630
-- ================================================================================

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tSupportInformation')
BEGIN
	CREATE TABLE tSupportInformation
	(
		 SupportInformationId BIGINT IDENTITY (1000000000,1) NOT NULL
		,Email NVARCHAR(500)
		,Phone1 NVARCHAR(100)
		,Phone2 NVARCHAR(100)
		,StateCode NVARCHAR(10)
		,DTServerCreate DATETIME NOT NULL
		,DTServerLastModified DATETIME NULL
		,CONSTRAINT PK_tSupportInformation PRIMARY KEY CLUSTERED
		(
			SupportInformationId ASC
		)
	)

END
GO
