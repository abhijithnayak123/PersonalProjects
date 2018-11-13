﻿-- ================================================================================
-- Author:		<Anisha Abraham>
-- Create date: <21/01/2016>
-- Description:	<TCF: IDKind value is Null for existing card customer>
-- Jira ID:		<AL-4390>
-- ================================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCCIS_Account' AND 'IDCode' IS NULL)
BEGIN
UPDATE [dbo].[tCCIS_Account] set IDCode = (CASE WHEN LEFT(SSN,1) = 9  AND LEN(SSN) = 9 AND (SUBSTRING(SSN,4,2) BETWEEN 70 AND 88 
			OR SUBSTRING(SSN,4,2) BETWEEN 90 AND 92 OR SUBSTRING(SSN,4,2) BETWEEN 94 AND 99) THEN 'I' WHEN LEN(SSN) = 0 THEN ''
			ELSE 'S' END
			) WHERE  LEFT(SSN,1) != '*' 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCertegy_Account' AND 'IDCode' IS NULL)
BEGIN
UPDATE [dbo].[tCertegy_Account] set IDCode = (CASE WHEN LEFT(SSN,1) = 9  AND LEN(SSN) = 9 AND (SUBSTRING(SSN,4,2) BETWEEN 70 AND 88 
			OR SUBSTRING(SSN,4,2) BETWEEN 90 AND 92 OR SUBSTRING(SSN,4,2) BETWEEN 94 AND 99) THEN 'I' WHEN LEN(SSN) = 0 THEN ''
			ELSE 'S' END
			) WHERE  LEFT(SSN,1) != '*' 
END
GO

ALTER TABLE tChxr_Account DISABLE TRIGGER [trChxr_AccountAudit]
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account' AND 'IDCode' IS NULL)
BEGIN
UPDATE [dbo].[tChxr_Account] set IDCode = (CASE WHEN LEFT(SSN,1) = 9  AND LEN(SSN) = 9 AND (SUBSTRING(SSN,4,2) BETWEEN 70 AND 88 
			OR SUBSTRING(SSN,4,2) BETWEEN 90 AND 92 OR SUBSTRING(SSN,4,2) BETWEEN 94 AND 99) THEN 'I' WHEN LEN(SSN) = 0 THEN ''
			ELSE 'S' END
			) WHERE  LEFT(SSN,1) != '*' 
END
GO
ALTER TABLE tChxr_Account ENABLE TRIGGER [trChxr_AccountAudit]

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxr_Account_Aud' AND 'IDCode' IS NULL)
BEGIN
UPDATE [dbo].[tChxr_Account_Aud] set IDCode = (CASE WHEN LEFT(SSN,1) = 9  AND LEN(SSN) = 9 AND (SUBSTRING(SSN,4,2) BETWEEN 70 AND 88 
			OR SUBSTRING(SSN,4,2) BETWEEN 90 AND 92 OR SUBSTRING(SSN,4,2) BETWEEN 94 AND 99) THEN 'I' WHEN LEN(SSN) = 0 THEN ''
			ELSE 'S' END
			) WHERE  LEFT(SSN,1) != '*' 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChxrSim_Account' AND 'IDCode' IS NULL)
BEGIN
UPDATE [dbo].[tChxrSim_Account] set IDCode = (CASE WHEN LEFT(SSN,1) = 9  AND LEN(SSN) = 9 AND (SUBSTRING(SSN,4,2) BETWEEN 70 AND 88 
			OR SUBSTRING(SSN,4,2) BETWEEN 90 AND 92 OR SUBSTRING(SSN,4,2) BETWEEN 94 AND 99) THEN 'I' WHEN LEN(SSN) = 0 THEN ''
			ELSE 'S' END
			) WHERE  LEFT(SSN,1) != '*' 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tFIS_Account'AND 'IDCode' IS NULL)
BEGIN
UPDATE [dbo].[tFIS_Account] set IDCode = (CASE WHEN LEFT(SSN,1) = 9  AND LEN(SSN) = 9 AND (SUBSTRING(SSN,4,2) BETWEEN 70 AND 88 
			OR SUBSTRING(SSN,4,2) BETWEEN 90 AND 92 OR SUBSTRING(SSN,4,2) BETWEEN 94 AND 99) THEN 'I' WHEN LEN(SSN) = 0 THEN ''
			ELSE 'S' END
			) WHERE  LEFT(SSN,1) != '*' 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTCIS_Account' AND 'IDCode' IS NULL)
BEGIN
UPDATE [dbo].[tTCIS_Account] set IDCode = (CASE WHEN LEFT(SSN,1) = 9  AND LEN(SSN) = 9 AND (SUBSTRING(SSN,4,2) BETWEEN 70 AND 88 
			OR SUBSTRING(SSN,4,2) BETWEEN 90 AND 92 OR SUBSTRING(SSN,4,2) BETWEEN 94 AND 99) THEN 'I' WHEN LEN(SSN) = 0 THEN ''
			ELSE 'S' END
			) WHERE  LEFT(SSN,1) != '*' 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='tTSys_Account' AND 'IDCode' IS NULL)
BEGIN
UPDATE [dbo].[tTSys_Account] set IDCode = (CASE WHEN LEFT(SSN,1) = 9  AND LEN(SSN) = 9 AND (SUBSTRING(SSN,4,2) BETWEEN 70 AND 88 
			OR SUBSTRING(SSN,4,2) BETWEEN 90 AND 92 OR SUBSTRING(SSN,4,2) BETWEEN 94 AND 99) THEN 'I' WHEN LEN(SSN) = 0 THEN ''
			ELSE 'S' END
			) WHERE  LEFT(SSN,1) != '*' 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='tTSys_Account_Aud' AND 'IDCode' IS NULL)
BEGIN
UPDATE [dbo].[tTSys_Account_Aud] set IDCode = (CASE WHEN LEFT(SSN,1) = 9  AND LEN(SSN) = 9 AND (SUBSTRING(SSN,4,2) BETWEEN 70 AND 88 
			OR SUBSTRING(SSN,4,2) BETWEEN 90 AND 92 OR SUBSTRING(SSN,4,2) BETWEEN 94 AND 99) THEN 'I' WHEN LEN(SSN) = 0 THEN ''
			ELSE 'S' END
			) WHERE  LEFT(SSN,1) != '*' 
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_Account' AND 'IDCode' IS NULL)
BEGIN
UPDATE [dbo].[tVisa_Account] set IDCode = (CASE WHEN LEFT(SSN,1) = 9  AND LEN(SSN) = 9 AND (SUBSTRING(SSN,4,2) BETWEEN 70 AND 88 
			OR SUBSTRING(SSN,4,2) BETWEEN 90 AND 92 OR SUBSTRING(SSN,4,2) BETWEEN 94 AND 99) THEN 'I' WHEN LEN(SSN) = 0 THEN ''
			ELSE 'S' END
			) WHERE  LEFT(SSN,1) != '*' 
END
GO