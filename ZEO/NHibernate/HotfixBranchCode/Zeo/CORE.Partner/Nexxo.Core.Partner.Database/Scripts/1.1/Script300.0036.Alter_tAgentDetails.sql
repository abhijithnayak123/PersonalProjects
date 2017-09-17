-- Author:		<Ashok Kumar>
-- Create date: <12/05/2014>
-- Description:	<Alter tAgentDetails, UserName column length to 255 characters>
-- Rally ID:	<TA6299>
-- ============================================================

IF EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE NAME = N'UserName' AND OBJECT_ID = OBJECT_ID(N'tAgentDetails'))
BEGIN
	ALTER TABLE tAgentDetails
	ALTER COLUMN UserName nvarchar(255)
END
GO