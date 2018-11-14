--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <06-07-2018>
-- Description:	 Adding new column to validate Onus check transaction send to INGO
-- ================================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_Check' AND COLUMN_NAME = 'InitiatedProviderId')
BEGIN
    ALTER TABLE tTxn_Check 
	ADD InitiatedProviderId INT NULL 
END
GO