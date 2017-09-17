-- ============================================================
-- Author:        <Chinar Kulkarni>
-- Create date:   <07/24/2015>
-- Description:   <Changed the datatype of LocationId in tCustomer table to match the type of LocationIdentifier column in PTNR.dbo.tLocations table> 
-- Rally ID:      <AL-590>
-- ============================================================

IF EXISTS(SELECT 1
			FROM INFORMATION_SCHEMA.COLUMNS
			WHERE TABLE_NAME = 'tCustomer'
				AND COLUMN_NAME = 'LocationId')
BEGIN
	ALTER TABLE tCustomer
	ALTER COLUMN LocationId varchar(50)
END
