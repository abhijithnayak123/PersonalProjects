-- ============================================================
-- Author:		<Rogy Eapen>
-- Create date: <03/03/2014>
-- Description:	<Added a New Column in 'tWUnion_Trx' 
--				for Modify Send Money Transaction> 
-- Rally ID:	<US1686 - TA4405>
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

IF NOT EXISTS 
(
  SELECT 
	1 
  FROM   
	sys.columns 
  WHERE  
	object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]') 
    AND name = 'ReasonCode'
)
BEGIN         
	ALTER TABLE tWUnion_Trx 
	ADD	ReasonCode VARCHAR(20) NULL  
END
GO

IF NOT EXISTS 
(
  SELECT 
	1 
  FROM   
	sys.columns 
  WHERE  
	object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]') 
    AND name = 'ReasonDescription'
)
BEGIN         
	ALTER TABLE tWUnion_Trx 
	ADD	ReasonDescription VARCHAR(50) NULL
END
GO

IF NOT EXISTS 
(
  SELECT 
	1 
  FROM   
	sys.columns 
  WHERE  
	object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]') 
	AND name = 'Comments'
)
BEGIN         
	ALTER TABLE tWUnion_Trx 
	ADD Comments  VARCHAR(50) NULL    
END
GO