--===========================================================================================
-- Author:		<Ashok Kumar G>
-- Created date: <June 3rd 2015>
-- Description:	<Alter view vODS_CustomerIdInfo>           
-- Jira ID:	<AL-522>
--===========================================================================================

DROP VIEW [dbo].[vODS_CustomerIdInfo]
GO

CREATE VIEW [dbo].[vODS_CustomerIdInfo]
AS
SELECT tNexxoIdTypes.Name AS IdName, tStates.Name AS StateName, tMasterCountries.Name AS CountryName, tNexxoIdTypes.NexxoIdTypeID as Id
FROM tNexxoIdTypes 
LEFT OUTER JOIN tMasterCountries ON tNexxoIdTypes.CountryPK = tMasterCountries.rowguid
LEFT OUTER JOIN tStates ON tNexxoIdTypes.StatePK = tStates.StatePK
GO