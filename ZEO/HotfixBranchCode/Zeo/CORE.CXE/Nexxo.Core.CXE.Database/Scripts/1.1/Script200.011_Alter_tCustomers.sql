-- ============================================================
-- Author:		<Komal Kant Dube>
-- Create date: <02/18/2014>
-- Description:	<Added additional column 'CountryOfBirth' to 
--				persist country of birth from Customer Profile 
--				identification train stop >
-- Rally ID:	<US1847 - TA4258>
-- ============================================================

IF NOT EXISTS(SELECT * FROM SYS.COLUMNS WHERE NAME = N'CountryOfBirth' AND OBJECT_ID = OBJECT_ID(N'tCustomers'))
BEGIN
	ALTER TABLE dbo.tCustomers 
	ADD CountryOfBirth VARCHAR(5) NULL
END
GO 