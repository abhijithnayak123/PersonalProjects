-- ============================================================
-- Author:		Abhijith
-- Create date: <11/19/2014>
-- Description:	<Script for Altering tCustomerEmploymentDetails table>
-- Rally ID:	<US2169>
-- ============================================================

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tCustomerEmploymentDetails' AND COLUMN_NAME = 'OccupationDescription')
BEGIN
	ALTER TABLE dbo.tCustomerEmploymentDetails
	ADD OccupationDescription NVARCHAR(255) NULL 
END
GO