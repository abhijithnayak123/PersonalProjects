-- ============================================================
-- Author:		<Shwetha Mohan>
-- Create date: <08/11/2015>
-- Description:	<Added additional column 'TransalatedDeliveryServiceName' 
--				 to tWUnion_Trx
-- Jira ID:		<AL-648>
-- ============================================================

IF NOT EXISTS
(
	SELECT 
		1 
	FROM
		SYS.COLUMNS 
	WHERE
		Name = N'TransalatedDeliveryServiceName' 
		AND OBJECT_ID = OBJECT_ID(N'tWUnion_Trx')
)
BEGIN
	ALTER TABLE dbo.tWUnion_Trx 
	ADD TransalatedDeliveryServiceName VARCHAR(200) NULL
END
GO 