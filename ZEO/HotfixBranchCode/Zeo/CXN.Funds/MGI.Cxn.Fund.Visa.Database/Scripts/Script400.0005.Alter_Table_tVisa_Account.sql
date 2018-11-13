-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <02/04/2015>
-- Description:	<Dropping unused column from tVisa_Account table>
-- Rally ID:	<DE3611>
-- ============================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tVisa_Account' AND COLUMN_NAME = 'PhoneType')
BEGIN
	ALTER TABLE tVisa_Account
	DROP COLUMN PhoneType
END
GO
