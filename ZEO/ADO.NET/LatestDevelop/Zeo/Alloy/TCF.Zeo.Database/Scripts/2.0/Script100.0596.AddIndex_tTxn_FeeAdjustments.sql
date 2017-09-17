-- ================================================================================
-- Author:		  <Manikandan Govindraj>
-- Create date:   <17/08/2017>
-- Description:   <Added non clustered index to 'transactionId' column in 'tTxn_FeeAdjustments' table.>
-- Jira ID:		  <B-06196>
-- ================================================================================

IF NOT EXISTS(SELECT 1 FROM SYS.INDEXES WHERE TYPE_DESC = 'NONCLUSTERED' AND object_name(object_id) = 'tTxn_FeeAdjustments' AND Name = 'IX_tTxn_FeeAdjustments_trx')
BEGIN

    CREATE NONCLUSTERED INDEX [IX_tTxn_FeeAdjustments_trx] ON [tTxn_FeeAdjustments]
	(
	     [IsActive],
		 [FeeAdjustmentId],
		 [TransactionId]
	)
    INCLUDE
	(
         [TransactionFeeAdjustmentId]
	)
END
GO
