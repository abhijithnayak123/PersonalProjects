-- ============================================================
-- Author:		Abhijith Nayak
-- Create date: <02/24/2015>
-- Description:	<Rename "Nexxo" to "MoneyGram" in processor field>
-- Rally ID:	<US2279>
-- ============================================================
UPDATE tMessageStore
SET Processor = 'MoneyGram'
WHERE Processor = 'Nexxo'