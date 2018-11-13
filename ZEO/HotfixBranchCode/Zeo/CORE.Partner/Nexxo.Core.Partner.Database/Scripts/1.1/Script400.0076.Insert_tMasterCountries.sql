--===========================================================================================
-- Author:		<Rita Patel>
-- Create date: <April 24 2015>
-- Description:	<Script to insert MasterCountries list in the table>
-- Jira ID:	    <AL-419>
--===========================================================================================

IF NOT EXISTS(SELECT 1 FROM  tMasterCountries WHERE Name = 'LAOS')
BEGIN
	INSERT INTO tMasterCountries(rowguid, Name, Abbr2, Abbr3) VALUES
	('2B60C245-978F-4825-8BFC-8F74F67F2C59', 'LAOS', 'LA', 'LAO'),
	('52AC1FB4-5E6F-4246-8AE4-3B9E31189329', 'RUSSIA', 'RU', 'RUS')
END


