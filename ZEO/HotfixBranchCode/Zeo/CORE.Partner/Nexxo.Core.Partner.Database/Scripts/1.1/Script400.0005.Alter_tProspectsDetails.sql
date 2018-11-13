-- ============================================================
-- Author:		Abhijith
-- Create date: <10/20/2014>
-- Description:	<Added columns for Client ID, Legal Code, 
--				Primary Country of Citizenship, Secondary Country of Citizenship>
-- Rally ID:	<US2157>
-- ============================================================

IF NOT EXISTS 
(
  SELECT 
	* 
  FROM   
	sys.columns 
  WHERE 
	name = N'ClientID' 
	AND object_id = OBJECT_ID(N'[dbo].[tProspects]')      
)
BEGIN         
	ALTER TABLE tProspects 
	ADD ClientID VARCHAR(15) NULL
END
GO

IF NOT EXISTS 
(
  SELECT 
	* 
  FROM   
	sys.columns 
  WHERE 
	name = N'LegalCode' 
	AND object_id = OBJECT_ID(N'[dbo].[tProspects]')      
)
BEGIN         
	ALTER TABLE tProspects 
	ADD LegalCode CHAR(1) NULL
END
GO


IF NOT EXISTS 
(
  SELECT 
	* 
  FROM   
	sys.columns 
  WHERE 
	name = N'PrimaryCountryCitizenShip' 
	AND object_id = OBJECT_ID(N'[dbo].[tProspects]')      
)
BEGIN         
	ALTER TABLE tProspects 
	ADD PrimaryCountryCitizenShip VARCHAR(5) NULL
END
GO


IF NOT EXISTS 
(
  SELECT 
	* 
  FROM   
	sys.columns 
  WHERE 
	name = N'SecondaryCountryCitizenShip' 
	AND object_id = OBJECT_ID(N'[dbo].[tProspects]')      
)
BEGIN         
	ALTER TABLE tProspects 
	ADD SecondaryCountryCitizenShip VARCHAR(5) NULL
END
GO