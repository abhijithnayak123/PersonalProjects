-- ============================================================
-- Author:		<Rogy Eapen>
-- Create date: <03/03/2014>
-- Description:	<Added a New Column OriginalTransactionID 
--				in 'tWUnion_Trx' to persist Transaction Sub Type
--				for Modify Send Money Transaction> 
-- Rally ID:	<US1685 - TA4321>
-- ============================================================

IF NOT EXISTS 
(
  SELECT 
	1 
  FROM   
	sys.columns 
  WHERE  
	object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]') 
    AND name = 'TransactionSubType'
)
BEGIN         
	ALTER TABLE tWUnion_Trx 
	ADD TransactionSubType varchar(20)    
END
GO