-- ==========================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <01/05/2016>
-- Description:	<As VisaDPS, Trigger new customer registration for companion>
-- Rally ID:	<AL-2083>
-- ==========================================================================
IF NOT EXISTS
	(
		SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE 
		TABLE_NAME = 'tTxn_Funds' AND  COLUMN_NAME = 'AddOnCustomerId'
	)
BEGIN
	ALTER TABLE tTxn_Funds 
	ADD AddOnCustomerId BIGINT
END
GO