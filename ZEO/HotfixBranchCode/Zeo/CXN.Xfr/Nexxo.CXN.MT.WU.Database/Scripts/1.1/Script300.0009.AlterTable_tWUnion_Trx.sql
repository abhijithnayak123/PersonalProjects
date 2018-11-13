-- ============================================================
-- Author:	    <Sunil Shetty>
-- Create date: <06/23/2014>
-- Description:	<Altering the width of ReasonDescription in tWunion_trx>
-- Rally ID:	
-- ============================================================
IF EXISTS 
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
	ALTER COLUMN ReasonDescription VARCHAR(255)
END
GO