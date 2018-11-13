-- ==================================================================================
-- Author:		Sudhir Baregar
-- Create date: 02/28/2014
-- Description:	<Added additional column 'tReceiverIndexNo' in tWUnion_Receiver 
--				 to persist ReceiverIndexNo for User Story US#1645>
-- Rally ID:	<US1645 - TA4313>
-- ==================================================================================

IF NOT EXISTS(SELECT * FROM SYS.COLUMNS WHERE NAME = N'ReceiverIndexNo' 
AND OBJECT_ID = OBJECT_ID(N'tWUnion_Receiver'))
BEGIN
	ALTER TABLE dbo.tWUnion_Receiver 
	ADD ReceiverIndexNo VARCHAR(5) NULL
END
GO
