-- ============================================================
-- Author:		<Sunil Shetty>
-- Create date: <05/05/2014>
-- Description:	<Added a New Column in 'tWUnion_Trx' 
--				for Send Money Promotion Sequence Number> 
-- Rally ID:	<N/A>
-- ============================================================

IF NOT EXISTS 
(
  SELECT 
	1 
  FROM   
	sys.columns 
  WHERE  
	object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]') 
    AND name = 'PromotionSequenceNo'
)
BEGIN         
	ALTER TABLE tWUnion_Trx 
	ADD PromotionSequenceNo varchar(20)  NULL
END
GO

