--- ===============================================================================
-- Author:		<M.Purna Pushkal>
-- Create date: <01-19-2018>
-- Description:	 SP to get the promotion types
-- Jira ID:		<B-12321>
-- ================================================================================

IF EXISTS(SELECT 1 FROM sys.objects WHERE name = 'usp_GetPromotionTypes')
BEGIN
	DROP PROCEDURE usp_GetPromotionTypes
END 
GO 

CREATE PROCEDURE usp_GetPromotionTypes
AS 
BEGIN
	SELECT
		PromotionTypeId,  
		Name AS PromoTypeName
	FROM
		tPromotionTypes
END