--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <10-09-2017>
-- Description:	Drop the PK columns/Unused columns from the table
-- ================================================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_FeeAdjustments' AND COLUMN_NAME = 'TxnFeeAdjPK')
BEGIN
    ALTER TABLE tTxn_FeeAdjustments DROP COLUMN TxnFeeAdjPK 
END
GO 

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_FeeAdjustments' AND COLUMN_NAME = 'TxnPK'  )
BEGIN
    ALTER TABLE tTxn_FeeAdjustments DROP COLUMN TxnPK 
END
GO 

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_FeeAdjustments' AND COLUMN_NAME = 'FeeAdjustmentPK'  )
BEGIN
    ALTER TABLE tTxn_FeeAdjustments DROP COLUMN FeeAdjustmentPK 
END
GO 

