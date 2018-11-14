--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <03-05-2018>
-- Description:	 Script for Job to update the Promotion status to Active if the 
-- Jira ID:		<B-12321>
-- ================================================================================

DECLARE @todayDate DATE = CONVERT (DATE, GETDATE())

UPDATE tPromotions
SET Status = 1
WHERE DATEDIFF(DAY, @todayDate, StartDate) = 0 AND Status = 5

UPDATE tPromotions
SET Status = 3
WHERE DATEDIFF(DAY, EndDate, @todayDate) > 0 AND Status = 1
GO

