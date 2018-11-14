--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <18-11-2016>
-- Description:	 Get product Id by biller Info
-- Jira ID:		<AL-8320>
-- ================================================================================

IF OBJECT_ID('ufn_GetProductIdByBillerInfo') IS NOT NULL
BEGIN
	DROP FUNCTION dbo.ufn_GetProductIdByBillerInfo
END
GO


CREATE FUNCTION ufn_GetProductIdByBillerInfo 
(
	@billerNameOrCode VARCHAR(255), 
	@channelPartnerId INT
)
RETURNS BIGINT
AS
BEGIN
	DECLARE @productId BIGINT

	SELECT 
		@productId = tmc.MasterCatalogID
	FROM 
		tMasterCatalog tmc WITH (NOLOCK)
	WHERE 
		(
			tmc.BillerName = @billerNameOrCode
			OR 
			tmc.BillerCode = @billerNameOrCode
		)
		AND tmc.ChannelPartnerId = @channelPartnerId

	RETURN @productId
END
GO

