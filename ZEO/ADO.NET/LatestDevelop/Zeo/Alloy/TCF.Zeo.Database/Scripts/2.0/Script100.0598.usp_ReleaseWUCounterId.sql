-- ================================================================================
-- Author:		  <M.Purna Pushkal>
-- Create date:   <19/08/2017>
-- Description:   <Making the counter Id's available>
-- Jira ID:		  <B-06839>
-- ================================================================================

IF EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE name = 'usp_ReleaseWUCounterId' )
BEGIN
	 DROP PROCEDURE usp_ReleaseWUCounterId
END
GO

CREATE PROCEDURE usp_ReleaseWUCounterId
AS
BEGIN

	 DECLARE @date DATETIME = GETDATE()

	 UPDATE tLocationCounterIdDetails
	 SET
		  IsAvailable = 1,
		  DTServerLastModified = @date,
		  DTTerminalLastModified = @date
END