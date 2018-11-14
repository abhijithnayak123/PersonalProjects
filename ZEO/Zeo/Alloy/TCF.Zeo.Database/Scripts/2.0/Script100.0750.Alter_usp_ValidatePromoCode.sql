--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <08-02-2016>
-- Description:	 Stored procedure to validate promotion code
-- Jira ID:		<AL-7926>
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_ValidatePromoCode'
)
BEGIN
	DROP PROCEDURE usp_ValidatePromoCode
END
GO

-- EXEC usp_ValidatePromoCode 1, '2018-02-08-00-00-0000', 'BOGO' 

CREATE PROCEDURE [dbo].[usp_ValidatePromoCode]
	@productId INT,
	@date DATE,
	@promoCode NVARCHAR(1000)
AS
BEGIN
	BEGIN TRY

	    DECLARE @isValid BIT = 0 
		
		IF EXISTS( 
		           SELECT 1 FROM tPromotions
		           WHERE 
					 ProductId = @productId 
					 AND Name = @promoCode
					 AND Status != 7 
					 AND @date BETWEEN StartDate AND EndDate
				  )
				  BEGIN
				     SET @isValid = 1
				  END

           SELECT @isValid as IsValid


	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END