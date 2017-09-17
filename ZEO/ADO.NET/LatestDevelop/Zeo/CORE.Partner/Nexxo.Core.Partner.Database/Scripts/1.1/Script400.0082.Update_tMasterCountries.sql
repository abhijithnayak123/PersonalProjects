--===========================================================================================
-- Author:		<Rita Patel>
-- Create date: <May 13 2015>
-- Description:	<Script to Update MasterCountries list in the table>
-- Jira ID:	    <AL-499>
--===========================================================================================

UPDATE tMasterCountries SET Name = 'SLOVAKIA' WHERE Name = 'SLOVAKIA (Slovak Republic)'
UPDATE tMasterCountries SET Name = 'BOSNIA AND HERZEGOVINA' WHERE Name = 'BOSNIA AND HERZEGOWINA'
UPDATE tMasterCountries SET Name = 'COTE DIVOIRE' WHERE Name = 'COTE D''IVOIRE'
UPDATE tMasterCountries SET Name = 'GUINEA BISSAU' WHERE Name = 'GUINEA-BISSAU'
UPDATE tMasterCountries SET Name = 'LIBYA' WHERE Name = 'LIBYAN ARAB JAMAHIRIYA'
UPDATE tMasterCountries SET Name = 'ST KITTS AND NEVIS' WHERE Name = 'SAINT KITTS AND NEVIS'
UPDATE tMasterCountries SET Name = 'ST LUCIA' WHERE Name = 'SAINT LUCIA'
UPDATE tMasterCountries SET Name = 'ST VINCENT AND THE GRENADINES' WHERE Name = 'SAINT VINCENT AND THE GRENADINES'
UPDATE tMasterCountries SET Name = 'SINT MAARTEN' WHERE Name = 'SINT MAARTEN (DUTCH SIDE)'
UPDATE tMasterCountries SET Name = 'VIETNAM' WHERE Name = 'VIET NAM'
UPDATE tMasterCountries SET Name = 'VIRGIN ISLANDS-BRITISH' WHERE Name = 'VIRGIN ISLANDS (BRITISH)'
UPDATE tMasterCountries SET Name = 'VIRGIN ISLANDS-US' WHERE Name = 'VIRGIN ISLANDS (U.S.)'
GO
