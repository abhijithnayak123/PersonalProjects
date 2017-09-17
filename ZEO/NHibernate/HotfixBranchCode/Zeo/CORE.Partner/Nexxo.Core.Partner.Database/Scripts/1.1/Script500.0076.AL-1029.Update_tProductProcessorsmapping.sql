-- ============================================================
-- Author:	<Sunil Shetty>
-- Create date: <03/08/2015>
-- Description:	<Removing the MGiAlloy requirement that all WU transactions require an SSN>
-- Rally ID:	<AL-1029>
-- ============================================================
UPDATE 
	TPP 
SET 
	TPP.IsSSNRequired = 0 
FROM 
	tProductProcessorsMapping AS TPP
	INNER JOIN tProcessors AS TPR ON TPP.ProcessorId = TPR.rowguid
WHERE 
	TPR.Name = 'WesternUnion'
GO
