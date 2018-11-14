-- ==========================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <12/28/2015>
-- Description:	<As VisaDPS, update Primary Alias ID in Alloy for companion cardholder>
-- Rally ID:	<AL-2108>
-- ==========================================================================
IF NOT EXISTS
	(
		SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE 
		TABLE_NAME = 'tvisa_account' AND  COLUMN_NAME = 'PrimaryCardAliasId'
	)
BEGIN
	ALTER TABLE tvisa_account 
	ADD PrimaryCardAliasId VARCHAR(50)
END
GO