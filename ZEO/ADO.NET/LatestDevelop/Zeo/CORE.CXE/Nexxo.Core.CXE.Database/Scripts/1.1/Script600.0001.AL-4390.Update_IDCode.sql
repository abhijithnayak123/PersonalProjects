-- ================================================================================
-- Author:		<Anisha Abraham>
-- Create date: <21/01/2016>
-- Description:	<TCF: IDKind value is Null for existing card customer>
-- Jira ID:		<AL-4390>
-- ================================================================================

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers' AND 'IDCode' IS NULL)
BEGIN
UPDATE [dbo].[tCustomers] set IDCode = (CASE WHEN LEFT(SSN,1) = 9  AND LEN(SSN) = 9 AND (SUBSTRING(SSN,4,2) BETWEEN 70 AND 88 
			OR SUBSTRING(SSN,4,2) BETWEEN 90 AND 92 OR SUBSTRING(SSN,4,2) BETWEEN 94 AND 99) THEN 'I' WHEN LEN(SSN) = 0 THEN ''
			ELSE 'S' END
			) WHERE  LEFT(SSN,1) != '*' 
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers_Aud' AND 'IDCode' IS NULL)
BEGIN
UPDATE [dbo].[tCustomers_Aud] set IDCode = (CASE WHEN LEFT(SSN,1) = 9  AND LEN(SSN) = 9 AND (SUBSTRING(SSN,4,2) BETWEEN 70 AND 88 
			OR SUBSTRING(SSN,4,2) BETWEEN 90 AND 92 OR SUBSTRING(SSN,4,2) BETWEEN 94 AND 99) THEN 'I' WHEN LEN(SSN) = 0 THEN ''
			ELSE 'S' END
			) WHERE  LEFT(SSN,1) != '*' 
END
GO