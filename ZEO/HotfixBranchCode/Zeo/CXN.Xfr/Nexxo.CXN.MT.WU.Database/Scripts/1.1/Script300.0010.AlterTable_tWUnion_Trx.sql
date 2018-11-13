-- ============================================================
-- Author:	    <Swarnalakshmi>
-- Create date: <Nov/12/2014>
-- Description:	<Altering the tWunion_trx for counterId>
-- Rally ID: US2028	
-- ============================================================
IF NOT EXISTS 
(
SELECT 
	1 
  FROM   
	sys.columns 
  WHERE  
	object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]') 
    AND name = 'CounterId'
)
BEGIN        
	ALTER TABLE tWUnion_Trx 
	ADD CounterId VARCHAR(100)
END
GO