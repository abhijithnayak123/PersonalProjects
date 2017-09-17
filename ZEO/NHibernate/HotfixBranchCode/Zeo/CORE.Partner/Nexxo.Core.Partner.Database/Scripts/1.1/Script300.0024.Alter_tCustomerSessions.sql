--===========================================================================================
-- Author:		<Raviraja>
-- Create date: <07/15/2014>
-- Description: <MVA - Allow to initiate customer session withought agent session>
--				<MVA - Add TimezoneID to tCustomerSessions>
-- Rally ID:	<US2039>

--===========================================================================================
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tCustomerSessions' AND COLUMN_NAME = 'AgentSessionPK')
BEGIN
	ALTER TABLE tCustomerSessions 
	ALTER COLUMN AgentSessionPK [uniqueidentifier] NULL
END
GO
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tCustomerSessions' AND COLUMN_NAME = 'TimezoneID')
BEGIN
 ALTER TABLE tCustomerSessions 
 ADD TimezoneID [varchar](100) NULL
END
GO


