--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <07-03-2017>
-- Description:	Dropping the index for table - "tTxn_Cash". 
-- ================================================================================
IF EXISTS( SELECT 1 FROM sys.indexes WHERE name = 'IX_tTxn_Cash_CXNId' )
BEGIN
	DROP INDEX IX_tTxn_Cash_CXNId ON tTxn_Cash 
END
GO


