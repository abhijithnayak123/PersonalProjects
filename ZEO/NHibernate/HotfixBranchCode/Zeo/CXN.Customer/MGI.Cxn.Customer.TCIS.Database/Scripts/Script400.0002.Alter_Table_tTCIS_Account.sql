-- ============================================================
-- Author:		<Abhijith>
-- Create date: <03/07/2015>
-- Description:	<script for adding new column "TcfCustInd" to tTCIS_Account table.>
-- Rally ID:	<AL-80>
-- ============================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tTCIS_Account' AND COLUMN_NAME = 'TcfCustInd')
BEGIN
	ALTER TABLE tTCIS_Account
	ADD TcfCustInd BIT
END
GO

