-- ============================================================
-- Author:	<Ashok Kumar G>
-- Create date: <06/02/2015>
-- Description:	<Alter Script to remove  LocationIdentifier column in tLocations table>
-- Rally ID:	<US2321>
-- ============================================================

IF EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE Name = N'LocationIdentifier' AND OBJECT_ID = OBJECT_ID(N'tLocations'))
BEGIN
	ALTER TABLE tLocations
	DROP COLUMN LocationIdentifier
END