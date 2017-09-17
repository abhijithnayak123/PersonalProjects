-- ============================================================
-- Author:		<Sunil Shetty>
-- Create date: <02/03/2014>
-- Description:	<Added a New Column WUCard_TotalPointsEarned 
--				in 'tWUnion_BillPay_Trx' to store Total Earned Values. 
--				Rally ID: US 1706>
-- ============================================================
IF NOT EXISTS 
(
  SELECT 
	1 
  FROM   
	sys.columns 
  WHERE  
	object_id = OBJECT_ID(N'[dbo].[tWUnion_BillPay_Trx]') 
    AND name = 'WUCard_TotalPointsEarned'
)
BEGIN 
ALTER TABLE tWUnion_BillPay_Trx
ADD WUCard_TotalPointsEarned VARCHAR(50) NULL
END
GO