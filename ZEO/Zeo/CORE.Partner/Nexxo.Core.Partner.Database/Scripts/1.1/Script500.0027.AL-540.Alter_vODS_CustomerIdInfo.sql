--===========================================================================================
-- Author:		<Ashok Kumar G>
-- Created date: <May 21th 2015>
-- Description:	<Script to alter vODS_CustomerIdInfo>           
-- Jira ID:	<AL-540>
--===========================================================================================

DROP VIEW vODS_CustomerIdInfo
GO

CREATE VIEW [dbo].[vODS_CustomerIdInfo]
AS
SELECT tNexxoIdTypes.Name AS IdName, tStates.Name AS StateName, tCountries.Name AS CountryName, tNexxoIdTypes.NexxoIdTypeID as Id
FROM tNexxoIdTypes 
LEFT OUTER JOIN tCountries ON tNexxoIdTypes.CountryPK = tCountries.CountryPK
LEFT OUTER JOIN tStates ON tNexxoIdTypes.StatePK = tStates.StatePK AND tCountries.CountryPK = tStates.CountryPK
GO
