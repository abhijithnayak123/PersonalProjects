-- ============================================================
-- Author:		<SwarnaLakshmi>
-- Create date: <18/03/2014>
-- Description:	<Added a New Column in 'tWUnion_Trx' 
--				for Modify Send Money Transaction> 
-- Rally ID:	<US1685>
-- ============================================================

IF NOT EXISTS 
(
  SELECT 
	1 
  FROM   
	sys.columns 
  WHERE  
	object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]') 
    AND name = 'DeliveryOption'
)
BEGIN         
	ALTER TABLE tWUnion_Trx 
	ADD DeliveryOption varchar(20)  NULL
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
    AND name = 'DeliveryOptionDesc'
)
BEGIN         
	ALTER TABLE tWUnion_Trx 
	ADD	DeliveryOptionDesc VARCHAR(100) NULL  
END
GO



