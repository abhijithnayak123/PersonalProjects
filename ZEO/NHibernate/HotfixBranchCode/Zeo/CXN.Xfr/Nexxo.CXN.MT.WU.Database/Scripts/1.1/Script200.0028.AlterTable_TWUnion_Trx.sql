-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <01/16/2014>
-- Description:	<Added new column to table 'tWUnion_Trx' 
--				 part of compliance implementation>
-- Rally ID:	<US1776 - TA3926>
-- ============================================================
IF NOT EXISTS
(
	SELECT 
		1 
	FROM 
		SYS.COLUMNS 
	WHERE 
		Name = N'Sender_ComplianceDetails_ComplianceData_Buffer' 
		AND Object_ID = Object_ID(N'tWUnion_Trx')
)
BEGIN
   ALTER TABLE tWUnion_Trx 
   ADD Sender_ComplianceDetails_ComplianceData_Buffer VARCHAR(500)
END
GO