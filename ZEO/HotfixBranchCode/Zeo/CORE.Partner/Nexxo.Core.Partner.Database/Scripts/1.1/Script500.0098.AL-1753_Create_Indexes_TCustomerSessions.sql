-- ================================================================================
-- Author:		<Sunil Shetty>
-- Create date: <09/14/2015>
-- Description:	<UAT 5.0.1 Alloy displaying error message "NHibernate.Exceptions.GenericADOException">
-- Jira ID:		<AL-1753>
-- ================================================================================
/****** Object:  Index [IX_tCustomerSessions_ID] ******/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_tCustomerSessions_ID' AND object_id = OBJECT_ID('tCustomerSessions'))
BEGIN
CREATE NONCLUSTERED INDEX [IX_tCustomerSessions_ID] ON [dbo].[tCustomerSessions] 
(
	[CustomerSessionID] ASC
)
INCLUDE ([CustomerSessionPK],
[AgentSessionPK],
[DTServerCreate],
[DTServerLastModified],
[DTStart],
[DTEnd],
[CardPresent])
END
GO
