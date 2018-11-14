-- ============================================================
-- Author:		<Sunil Shetty>
-- Create date: <18/04/2014>
-- Description:	<Added a New Column QPCompany_Department 
--				in 'tWUnion_BillPay_Trx' to store Attention Field Val. 
-- Rally ID:	<US 1980>
-- ============================================================
IF NOT EXISTS 
(
  SELECT 
	1 
  FROM   
	sys.columns 
  WHERE  
	object_id = OBJECT_ID(N'[dbo].[tWUnion_BillPay_Trx]') 
    AND name = 'QPCompany_Department'
)
BEGIN 
	ALTER TABLE tWUnion_BillPay_Trx
	ADD QPCompany_Department VARCHAR(100) NULL
END
GO