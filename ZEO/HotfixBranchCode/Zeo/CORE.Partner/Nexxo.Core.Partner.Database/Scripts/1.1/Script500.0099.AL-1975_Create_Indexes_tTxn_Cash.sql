-- ================================================================================
-- Author:		<Sunil Shetty>
-- Create date: <09/21/2015>
-- Description:	<Add indexing for txn_cash table >
-- Jira ID:		<AL-1975>
-- ================================================================================
/****** Object:  Index [IX_tAccount_ID]  ******/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_tTxnCash_TransactionID' AND object_id = OBJECT_ID('tTxn_Cash'))
BEGIN
CREATE NONCLUSTERED INDEX [IX_tTxnCash_TransactionID] ON tTxn_Cash
(
	[TransactionID] ASC
)
INCLUDE ( [CustomerSessionPK],
[AccountPK],
[DTServerCreate],
[DTServerLastModified])
END
GO