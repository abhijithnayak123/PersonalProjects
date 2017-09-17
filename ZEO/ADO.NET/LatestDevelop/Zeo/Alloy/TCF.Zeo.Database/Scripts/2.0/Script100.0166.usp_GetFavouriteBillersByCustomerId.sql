--- ===============================================================================
-- Author:		<Purna Pushkal>
-- Create date: <18-11-2016>
-- Description:	Get favourite billers by customer Id
-- Jira ID:		<AL-8320>
-- ================================================================================

IF OBJECT_ID('usp_GetFavouriteBillersByCustomerId') IS NOT NULL
BEGIN
	DROP PROCEDURE dbo.usp_GetFavouriteBillersByCustomerId
END
GO

CREATE PROCEDURE usp_GetFavouriteBillersByCustomerId
(
	@customerId BIGINT
)
AS
BEGIN
	BEGIN TRY
		SELECT 
			cp.ProductId,
			tmc.BillerName,
			tmc.BillerCode
		FROM 
			tCustomerPreferedProducts cp WITH (NOLOCK)
			INNER JOIN dbo.tMasterCatalog tmc ON cp.ProductId = tmc.MasterCatalogID
		WHERE 
			CustomerID = @customerId
			AND cp.Enabled = 1
	END TRY

	BEGIN CATCH
		EXECUTE dbo.usp_CreateErrorInfo
	END CATCH
END
GO
