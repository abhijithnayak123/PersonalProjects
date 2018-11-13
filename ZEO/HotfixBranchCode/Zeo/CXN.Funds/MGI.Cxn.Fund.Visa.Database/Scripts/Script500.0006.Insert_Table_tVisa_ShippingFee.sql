-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <06/25/2014>
-- Description:	<DDL script to insert seed data tVisa_ShippingFee table>
-- Rally ID:	<AL-327>
-- ============================================================

IF  NOT EXISTS (SELECT 1 FROM  tVisa_ShippingFee)
BEGIN
	INSERT INTO tVisa_ShippingFee (VisaShippingFeePK, ShippingType, Fee, DTServerCreate, DTServerLastMod) VALUES
	(NEWID(), 0, 24.95, GETDATE(), NULL), 
	(NEWID(), 2, 0.00,  GETDATE(), NULL)
END
GO
