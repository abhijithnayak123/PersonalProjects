--===========================================================================================
-- Author:		<Rogy Eapen>
-- Created date: <01/19/2016>
-- Description:	<Alter view vODS_CustomerIdInfo>           
-- Jira ID:	<AL-4485>
--===========================================================================================

IF EXISTS (SELECT name FROM sys.views
   WHERE name = 'vODS_CustomerIdInfo')
BEGIN
   DROP VIEW vODS_CustomerIdInfo
END
GO

CREATE VIEW [dbo].[vODS_CustomerIdInfo]
AS
SELECT 
	tNexxoIdTypes.Name AS IdName,
	tStates.Name AS StateName,
	tMasterCountries.Name AS CountryName,
	tNexxoIdTypes.NexxoIdTypeID as Id
FROM 
	tNexxoIdTypes 
	LEFT OUTER JOIN tMasterCountries ON tNexxoIdTypes.CountryPK = tMasterCountries.MasterCountriesPK
	LEFT OUTER JOIN tStates ON tNexxoIdTypes.StatePK = tStates.StatePK
GO