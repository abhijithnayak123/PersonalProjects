-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <01/24/2014>
-- Description:	<Added additional columns as we identified 
--				 few information are not persisted>
-- Rally ID:	<US1820 - TA4086>
-- ============================================================

IF NOT EXISTS
(
	SELECT 
		1 
	FROM 
		sys.columns 
    WHERE 
		Name = N'Financials_PlusChargesAmount' 
		AND Object_ID = Object_ID(N'tWUnion_BillPay_Trx')
)
BEGIN
	ALTER TABLE 
		tWUnion_BillPay_Trx
	ADD 
		Financials_PlusChargesAmount DECIMAL(18, 2) NULL,
		Financials_TotalDiscount DECIMAL(18, 2) NULL
END            
GO

IF EXISTS
(
	SELECT 
		1 
	FROM 
		sys.columns 
    WHERE 
		Name = N'PaymentDetails_ExchangeRate' 
		AND Object_ID = Object_ID(N'tWUnion_BillPay_Trx')
)
BEGIN
	ALTER TABLE 
		tWUnion_BillPay_Trx
	ALTER COLUMN 
		PaymentDetails_ExchangeRate DECIMAL(18, 2) NULL
END            
GO	