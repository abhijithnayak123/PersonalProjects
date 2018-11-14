--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <01-31-2018>
-- Description:	 SP to update promotion status
-- Jira ID:		<B-12321>
-- ================================================================================

IF EXISTS(SELECT 1 FROM sys.objects WHERE name = 'usp_UpdatePromotionStatus')
BEGIN
	DROP PROCEDURE usp_UpdatePromotionStatus
END 
GO

CREATE PROCEDURE usp_UpdatePromotionStatus
(
	 @promotionId BIGINT,
	 @isActive BIT,
	 @serverLastModify DATETIME,
	 @terminalLastModify DATETIME
)
AS
BEGIN
	
	UPDATE 
		tPromotions
	SET 
		IsActive = @isActive,
		DTServerLastModified = @serverLastModify,
		DTTerminalLastModified = @terminalLastModify
	WHERE 
		PromotionId = @promotionId

END 