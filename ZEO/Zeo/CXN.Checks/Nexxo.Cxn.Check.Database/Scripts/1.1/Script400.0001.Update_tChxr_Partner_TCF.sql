-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <11/19/2014>
-- Description:	<Updated Ingo partner name for TCF bank>
-- Rally ID:	<US1610>
-- ============================================================

IF EXISTS
(
	SELECT 1 FROM tChxr_Partner WHERE (Id = 34)
)
BEGIN
	UPDATE    
		tChxr_Partner
	SET 
		Name = 'TCF National Bank'
	WHERE
		(Id = 34)
END
GO