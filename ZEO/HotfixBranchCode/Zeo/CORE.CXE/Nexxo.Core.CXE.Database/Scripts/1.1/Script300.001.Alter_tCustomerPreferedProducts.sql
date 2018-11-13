-- =======================================================================================
-- Author:		Sudhir Baregar
-- Create date: 02/28/2014
-- Description:	<Added additional column 'tReceiverIndexNo' in tCustomerPreferedProducts 
--				 to persist ReceiverIndexNo. Added for US1646>
-- Rally ID:	<US1646 - TA4313>
-- =======================================================================================

IF NOT EXISTS(SELECT * FROM SYS.COLUMNS WHERE NAME = N'ReceiverIndexNo' AND OBJECT_ID = OBJECT_ID(N'tCustomerPreferedProducts'))
BEGIN
	ALTER TABLE dbo.tCustomerPreferedProducts 
	ADD ReceiverIndexNo VARCHAR(5) NULL
END
GO 