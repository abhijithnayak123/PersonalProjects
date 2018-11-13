-- ============================================================
-- Author:		Sunil Shetty
-- Create date: <09/17/2014>
-- Description:	<Added new mapping check value for chexar type>
-- Rally ID:	<US12025>
-- ============================================================

IF EXISTS 
(
  SELECT 
	* 
  FROM   
	sys.columns 
  WHERE 
  object_id = OBJECT_ID(N'[dbo].[tChxr_CheckTypeMapping]')      
)
BEGIN         
	truncate table [dbo].[tChxr_CheckTypeMapping]
	
	insert into [dbo].[tChxr_CheckTypeMapping] values 
		(18, 'Check - Cashier''s/Official', 1),
		(19, 'Check - Two Party', 10),
		(21, 'Check - Payroll Handwritten', 6), 
		(22, 'Check - Money Order', 5), 
		(23, 'Check - Payroll Printed', 7), 
		(24, 'Check - Insurance/Attorney', 1), 
		(25, 'Check - Tax Refund -   U.S. Treasury', 2), 
		(26, 'Check - Govt -  U.S. Treasury', 2), 
		(27, 'Check - Govt State - Out of State', 3), 
		(41, 'Check - Free / Gratis', 1), 
		(42, 'Check - WU Fee', 1), 
		(108, 'Check - Promo Printed Payroll', 1), 
		(390, 'Check - Misc Check', 10), 
		(420, 'Check - Govt -  U.S. Treasury Recurring', 2), 
		(421, 'Check - Loan / RAL', 14), 
		(423, 'Check - Two Party Business', 10), 
		(424, 'Check - Govt - All Other', 3), 
		(425, 'Check - Govt - All Other  Recurring', 3), 
		(426, 'Check - Tax Refund - All Other', 3), 
		(444, 'Check - On Us', 1), 
		(452, 'Check - Loan', 1), 
		(455, 'Check - Unknown Check Type', 14), 
		(460, 'Check - Loan', 14), 
		(465, 'Check - RAC', 14)
END
GO