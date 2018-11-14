--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <01-31-2018>
-- Description:	 SP to update promotion status
-- Version One:		<B-12321>
-- ================================================================================

IF EXISTS(SELECT 1 FROM sys.objects WHERE name = 'usp_UpdatePromotionStatus')
BEGIN
	DROP PROCEDURE usp_UpdatePromotionStatus
END 
GO

CREATE PROCEDURE usp_UpdatePromotionStatus
(
	 @promotionId BIGINT,
	 @status INT,
	 @dtServerDate DATETIME,
	 @dtTerminalDate DATETIME,
	 @promoStatus INT OUTPUT
)
AS
BEGIN
BEGIN TRY
BEGIN TRANSACTION
		UPDATE p
		SET 
			Status = CASE 
						WHEN @status = 6 THEN 6  -- 6 : Deleted 5 : Ready, 4 : InProgress, 3 : Expired, 2 : InActive, 1 : Active, 0 : None
						WHEN ((DATEADD(day, -7,  p.StartDate )) > CAST(@dtServerDate AS DATE)) -- Check the promotion start date, lesser than 7 day make promotion to active
						THEN 5 
						ELSE 1 
					 END,
			DTServerLastModified = @dtServerDate,
			DTTerminalLastModified = @dtTerminalDate
		FROM tPromotions p
		WHERE 
			PromotionId = @promotionId		

	    SELECT 
			@promoStatus = Status
		FROM tPromotions tp
		WHERE PromotionId = @promotionId

COMMIT
END TRY
BEGIN CATCH
	ROLLBACK
	EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
END CATCH

END 