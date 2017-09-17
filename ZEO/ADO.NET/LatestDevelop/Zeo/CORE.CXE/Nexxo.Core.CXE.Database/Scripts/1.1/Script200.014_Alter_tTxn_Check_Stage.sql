-- ============================================================
-- Author:		<Bineesh Ragahvan>
-- Create date: <05/08/2014>
-- Description:	<Made MICR column nullable in tTxn_Check_Stage 
--				& tTxn_Check_Stage_Aud tables>
-- Rally ID:	<US1830 - TA4962>
-- ============================================================

IF EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE NAME = N'MICR' AND OBJECT_ID = OBJECT_ID(N'tTxn_Check_Commit'))
BEGIN
	ALTER TABLE tTxn_Check_Commit
	ALTER COLUMN MICR NVARCHAR(100) NULL
END
GO  