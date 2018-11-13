-- ============================================================
-- Author:		<Pamila Jose>
-- Create date: <07/May/2014>
-- Description:	<Added additional column 'CustomerProfileStatus' to 
--				store the combination of CXE Customer Status and CXN Customer Status (3rd Party)>
-- Rally ID:	<US1991>
-- ============================================================

IF NOT EXISTS(SELECT * FROM SYS.COLUMNS WHERE Name = N'CustomerProfileStatus' and OBJECT_ID = OBJECT_ID(N'tPartnerCustomers'))
BEGIN
	ALTER TABLE tPartnerCustomers
	ADD CustomerProfileStatus bit NULL
END
GO 
