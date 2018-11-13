-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <02/17/2014>
-- Description:	<Added additional columns for storing FushionGoShopping
--				 request and response in XML format>
-- Rally ID:	<DE2515 - TA4272>
-- ============================================================

IF NOT EXISTS
(
	SELECT 
		1 
	FROM 
		sys.columns 
    WHERE 
		Name = N'Request_XML' 
		AND Object_ID = Object_ID(N'tWUnion_BillPay_Trx')
)
BEGIN
	ALTER TABLE 
		tWUnion_BillPay_Trx
	ADD 
		Request_XML VARCHAR(MAX) NULL,
		Response_XML VARCHAR(MAX) NULL
END            
GO
