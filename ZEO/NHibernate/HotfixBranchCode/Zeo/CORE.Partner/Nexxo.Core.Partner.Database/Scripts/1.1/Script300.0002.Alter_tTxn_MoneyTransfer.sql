-- ============================================================
-- Author:		Swarnalakshmi
-- Create date: <03/11/2014>
-- Description:	<Added a New Column TransactionSubType, 
--				in 'tTxn_MoneyTransfer' to persist TransactionSubType
--				in Shopping Cart>
-- Rally ID:	<US1685>
-- ============================================================

IF NOT EXISTS 
(
  SELECT 
	* 
  FROM   
	sys.columns 
  WHERE 
  name = N'TransactionSubType' 
	AND object_id = OBJECT_ID(N'[dbo].[tTxn_MoneyTransfer]')      
)
BEGIN         
	ALTER TABLE tTxn_MoneyTransfer 
	ADD TransactionSubType VARCHAR(20) NULL	
END
GO

IF NOT EXISTS 
(
  SELECT 
	* 
  FROM   
	sys.columns 
  WHERE 
  name = N'OriginalTransactionID' 
	AND object_id = OBJECT_ID(N'[dbo].[tTxn_MoneyTransfer]')      
)
BEGIN         
	ALTER TABLE tTxn_MoneyTransfer 
	ADD OriginalTransactionID bigint
END
GO