-- =======================================================================================
-- Author:		<Ashok Kumar G>
-- Create date: <05/14/2014>
-- Description:	<Created table tWUnion_ImportBillers>
-- Rally ID:    <N/A>
-- =======================================================================================

IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE OBJECT_ID = OBJECT_ID(N'[dbo].[tWUnion_ImportBillers]') AND type in (N'U'))
BEGIN
CREATE TABLE tWUnion_ImportBillers
(
	rowguid uniqueidentifier NOT NULL PRIMARY KEY,
	Id BIGINT IDENTITY(1000000000,1) NOT NULL,
	BillerName VARCHAR(500) NOT NULL,
	AccountNumber VARCHAR(50),
	CardNumber VARCHAR(20) NOT NULL,
	WUIndex int NOT NULL,
	WUAccount UNIQUEIDENTIFIER FOREIGN KEY REFERENCES tWUnion_BillPay_Account(ROWGUID) NOT NULL,
	AgentSessionId BIGINT NOT NULL,
	CustomerSessionId BIGINT NOT NULL,
	DTCreate datetime NOT NULL,
	DTLastModified datetime NULL,
)
END