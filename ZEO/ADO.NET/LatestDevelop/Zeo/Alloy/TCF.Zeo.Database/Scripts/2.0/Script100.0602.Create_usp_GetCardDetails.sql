-- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-28-2017>
-- Description:	Get Card Details
-- Jira ID:		<B-06199>
-- ================================================================================

-- EXEC usp_GetCardDetails

IF OBJECT_ID(N'usp_GetCardDetails', N'P') IS NOT NULL
DROP PROC usp_GetCardDetails
GO

CREATE PROCEDURE usp_GetCardDetails
AS
BEGIN
	BEGIN TRY

	   SELECT 
	      CardBIN,
		  CardType
	   FROM
		  tCardDetails

	END TRY
		
	BEGIN CATCH
		EXECUTE usp_CreateErrorInfo
	END CATCH
END


