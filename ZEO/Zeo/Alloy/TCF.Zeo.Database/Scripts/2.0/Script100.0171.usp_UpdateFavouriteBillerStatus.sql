--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <18-11-2016>
-- Description:	 Update favourite biller state
-- Jira ID:		<AL-8320>
-- ================================================================================

IF OBJECT_ID('usp_UpdateFavouriteBillerStatus') IS NOT NULL
BEGIN
	DROP PROCEDURE dbo.usp_UpdateFavouriteBillerStatus
END
GO

CREATE PROCEDURE usp_UpdateFavouriteBillerStatus 
(
	 @customerId         BIGINT,																 
	 @productId          BIGINT,
     @isEnabled          BIT,	 
	 @dtServerModified   DATETIME,																 
	 @dtTerminalModified DATETIME
)
AS
BEGIN
	BEGIN TRY
		UPDATE 
			dbo.tCustomerPreferedProducts
		SET
			Enabled = @isEnabled,
			DTTerminalLastModified = @dtTerminalModified,
			DTServerLastModified = @dtServerModified
		WHERE 
			CustomerID = @customerId
			AND ProductId = @productId
	END TRY

	BEGIN CATCH
		EXECUTE dbo.usp_CreateErrorInfo;
	END CATCH
END
GO
