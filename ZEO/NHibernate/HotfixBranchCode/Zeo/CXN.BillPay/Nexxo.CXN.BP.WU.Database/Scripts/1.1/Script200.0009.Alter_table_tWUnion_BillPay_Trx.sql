-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <27/02/2014>
-- Description:	<Made WesternUnionCardNumber column nullable 
--				as now card number is not mandatory to perform 
--				Western Union transaction>
-- ============================================================

IF EXISTS 
(
	SELECT 
		1 
	FROM   
		sys.columns 
	WHERE  
		object_id = OBJECT_ID(N'[dbo].[tWUnion_BillPay_Trx]') 
		AND name = 'WesternUnionCardNumber'
)
BEGIN
	ALTER TABLE 
		tWUnion_billpay_Trx
	ALTER COLUMN 
		WesternUnionCardNumber VARCHAR(15) NULL
END
GO
