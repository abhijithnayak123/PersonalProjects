-- ============================================================
-- Author:		<Ashok Kumar G>
-- Create date: <07/30/2014>
-- Description:	<Made MICR column nullable in tTxn_Check_Stage 
--				& tTxn_Check_Stage_Aud tables>
-- Rally ID:	<DE3052>
-- ============================================================

IF EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE NAME = N'MICR' AND OBJECT_ID = OBJECT_ID(N'tTxn_Check_Stage'))
BEGIN
	ALTER TABLE tTxn_Check_Stage
	ALTER COLUMN MICR NVARCHAR(100) NULL
END
GO  


IF EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE NAME = N'MICR' AND OBJECT_ID = OBJECT_ID(N'tTxn_Check_Stage_Aud'))
BEGIN
	ALTER TABLE tTxn_Check_Stage_Aud
	ALTER COLUMN MICR NVARCHAR(100) NULL
END
GO  