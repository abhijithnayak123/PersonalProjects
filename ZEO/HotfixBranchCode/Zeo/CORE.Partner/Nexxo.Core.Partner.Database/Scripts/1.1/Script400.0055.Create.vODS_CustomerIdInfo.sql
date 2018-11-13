--===========================================================================================
-- Author:		Rogy Eapen
-- Create date: Mar 04 2015
-- Description:	<Script for vODS_CustomerIdInfo>
-- Jira ID:	AL-123
--===========================================================================================

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vODS_CustomerIdInfo]'))
DROP VIEW [dbo].[vODS_CustomerIdInfo]
GO
CREATE VIEW [dbo].[vODS_CustomerIdInfo]
AS
SELECT tNexxoIdTypes.Name AS IdName, tStates.Name AS StateName, tCountries.Name AS CountryName, tNexxoIdTypes.Id as Id
FROM tNexxoIdTypes 
LEFT OUTER JOIN tCountries ON tNexxoIdTypes.CountryId = tCountries.id 
LEFT OUTER JOIN tStates ON tNexxoIdTypes.StateId = tStates.id AND tCountries.id = tStates.CountryId
GO