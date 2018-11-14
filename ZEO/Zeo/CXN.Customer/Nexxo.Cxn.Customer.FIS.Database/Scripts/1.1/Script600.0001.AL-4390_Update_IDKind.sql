-- ================================================================================
-- Author:		<Anisha Abraham>
-- Create date: <01/02/2016>
-- Description:	<TCF: IDKind value is Null for existing card customer>
-- Jira ID:		<AL-4390>
-- ================================================================================

BEGIN
UPDATE [dbo].[tFIS_Account] set IDCode = (CASE WHEN LEFT(SSN,1) = 9  AND LEN(SSN) = 9 AND (SUBSTRING(SSN,4,2) BETWEEN 70 AND 88 
			OR SUBSTRING(SSN,4,2) BETWEEN 90 AND 92 OR SUBSTRING(SSN,4,2) BETWEEN 94 AND 99) THEN 'I' WHEN LEN(SSN) = 0 THEN ''
			ELSE 'S' END
			) WHERE  LEFT(SSN,1) != '*' AND IDCode IS NULL
END
GO