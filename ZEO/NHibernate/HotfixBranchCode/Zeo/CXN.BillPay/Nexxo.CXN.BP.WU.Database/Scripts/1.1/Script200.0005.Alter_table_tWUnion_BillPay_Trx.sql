-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <01/14/2014>
-- Description:	<Modified datatype from BIGINT to DECIMAL(18, 2) for 
--				all financial fields in 'tWUnion_BillPay_Trx'. 
--				Rally ID: DE2443>
-- ============================================================
ALTER TABLE tWUnion_BillPay_Trx
ALTER COLUMN Financials_Fee DECIMAL(18, 2)
GO

ALTER TABLE tWUnion_BillPay_Trx
ALTER COLUMN Financials_OriginatorsPrincipalAmount DECIMAL(18, 2) NULL
GO

ALTER TABLE tWUnion_BillPay_Trx
ALTER COLUMN Financials_DestinationPrincipalAmount DECIMAL(18, 2) NULL
GO

ALTER TABLE tWUnion_BillPay_Trx
ALTER COLUMN Financials_Fee DECIMAL(18, 2) NULL
GO

ALTER TABLE tWUnion_BillPay_Trx
ALTER COLUMN Financials_GrossTotalAmount DECIMAL(18, 2) NULL
GO

ALTER TABLE tWUnion_BillPay_Trx
ALTER COLUMN Financials_Total DECIMAL(18, 2) NULL
GO

ALTER TABLE tWUnion_BillPay_Trx
ALTER COLUMN Financials_UndiscountedCharges DECIMAL(18, 2) NULL
GO

ALTER TABLE tWUnion_BillPay_Trx
ALTER COLUMN Financials_DiscountedCharges DECIMAL(18, 2) NULL
GO