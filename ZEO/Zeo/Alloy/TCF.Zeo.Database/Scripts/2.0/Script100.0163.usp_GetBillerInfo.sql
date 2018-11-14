--- ===============================================================================
-- Author:		<Purna Pushkal>
-- Create date: <18-11-2016>
-- Description:	Get biller info by name or code
-- Jira ID:		<AL-8320>
-- ================================================================================

IF OBJECT_ID('usp_GetBillerInfo') IS NOT NULL
BEGIN
	DROP PROCEDURE usp_GetBillerInfo
END
GO

CREATE PROCEDURE usp_GetBillerInfo
(
	@billerNameOrCode VARCHAR(255),																	 
	@customerId       BIGINT,
	@channelPartnerId INT
)
AS
BEGIN
	BEGIN TRY
		SELECT 
			tc.AccountNumber,
			tmc.BillerName,
			tmc.ProviderId,
			tmc.MasterCatalogID
		FROM 
			tMasterCatalog tmc WITH (NOLOCK)
			LEFT JOIN dbo.tCustomerPreferedProducts tc WITH (NOLOCK) ON tmc.MasterCatalogID = tc.ProductId
			AND tc.CustomerID = @customerId
		WHERE
		(
			tmc.BillerName = @billerNameOrCode
			OR 
			tmc.BillerCode = @billerNameOrCode
		)
		AND tmc.ChannelPartnerId = @channelPartnerId
		AND tc.Enabled = 1
	END TRY

	BEGIN CATCH
		EXECUTE dbo.usp_CreateErrorInfo
	END CATCH
END