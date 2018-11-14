--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <01-31-2018>
-- Description:	 SP to validate promotion name 
-- Jira ID:		<B-12321>
-- ================================================================================

IF EXISTS(SELECT 1 FROM sys.objects WHERE name = 'usp_ValidatePromotionName')
BEGIN
	DROP PROCEDURE usp_ValidatePromotionName
END 
GO

CREATE PROCEDURE usp_ValidatePromotionName
(
	 @promotionName NVARCHAR(100) = NULL,
	 @promotionId BIGINT
)
AS
BEGIN

	SELECT 
		CASE 
			WHEN COUNT(1) > 0  THEN CAST(0 AS BIT)
			ELSE CAST(1 AS BIT) 
			END AS IsValid
	FROM 
		tpromotions
	WHERE 
		(Name = @promotionName and  (@promotionId = 0 or PromotionId != @promotionId))
END 