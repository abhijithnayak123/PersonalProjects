-- ============================================================
-- Author:		<Rogy Eapen>
-- Create date: <02/21/2014>
-- Description:	<Added a New Column OriginalTransactionID 
--				in 'tWUnion_Trx' to persist Original Transaction ID
--				for Modify Send Money Transaction> 
-- Rally ID:	<US1685 - TA4291>
-- ============================================================

IF NOT EXISTS 
(
  SELECT 
	1 
  FROM   
	sys.columns 
  WHERE  
	object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]') 
    AND name = 'OriginalTransactionID' 
)
BEGIN         
	ALTER TABLE tWUnion_Trx 
	ADD OriginalTransactionID bigint    
END
GO