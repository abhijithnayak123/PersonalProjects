-- ==========================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <1/13/2016>
-- Description:	<Add new column PromoCode to tVisa_trx table>
-- Rally ID:	<AL-2153>
-- ==========================================================================
IF NOT EXISTS
	(
		SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE 
		TABLE_NAME = 'tVisa_trx' AND  COLUMN_NAME = 'PromoCode'
	)
BEGIN

	ALTER TABLE tVisa_trx ADD PromoCode VARCHAR(50)

END
