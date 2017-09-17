-- ============================================================
-- Author:		<Sunil Shetty>
-- Create date: <21/03/2014>
-- Description:	<Added new column to table 'tPartnerCustomers'>
-- Rally ID:	<US1919 - TA4567: Add Teller/User Username and ID to ODS>
-- ============================================================
IF NOT EXISTS
(
	SELECT 
		1 
	FROM 
		SYS.COLUMNS 
	WHERE 
		Name = N'AgentSessionId' 
		AND Object_ID = Object_ID(N'tPartnerCustomers')
)
BEGIN
   ALTER TABLE tPartnerCustomers 
   ADD AgentSessionId uniqueidentifier NULL
END
GO
