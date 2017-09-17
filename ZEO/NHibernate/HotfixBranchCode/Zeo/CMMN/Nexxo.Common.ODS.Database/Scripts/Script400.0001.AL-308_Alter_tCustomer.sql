-- ============================================================
-- Author:        <Ashok Kumar G>
-- Create date:   <4/17/2015>
-- Description:   <Added AlloyAgentIdentifier, ClientAgentIdentifier Columns in 'tCustomer' for ODS> 
-- Rally ID:      <AL-308>
-- ============================================================
IF NOT EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE NAME = N'AlloyAgentIdentifier' AND OBJECT_ID = OBJECT_ID(N'tCustomer'))
BEGIN
	ALTER TABLE tCustomer ADD
	AlloyAgentIdentifier nvarchar(50)
END
GO


IF NOT EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE NAME = N'ClientAgentIdentifier' AND OBJECT_ID = OBJECT_ID(N'tCustomer'))
BEGIN
	ALTER TABLE tCustomer ADD
	ClientAgentIdentifier nvarchar(20)
END
GO
