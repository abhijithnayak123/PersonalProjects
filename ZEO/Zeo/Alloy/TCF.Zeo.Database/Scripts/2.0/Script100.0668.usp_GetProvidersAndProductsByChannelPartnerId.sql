--- ===============================================================================
-- Author:		<M.Purna Pushkal>
-- Create date: <01-19-2018>
-- Description:	 SP to get the providers and products based on the channel partner Id
-- Jira ID:		<B-12321>
-- ================================================================================

IF EXISTS(SELECT 1 FROM sys.objects WHERE name = 'usp_GetProvidersAndProductsByChannelPartnerId')
BEGIN
	DROP PROCEDURE usp_GetProvidersAndProductsByChannelPartnerId
END 
GO

CREATE PROCEDURE usp_GetProvidersAndProductsByChannelPartnerId
(
	@channelPartnerId smallint
)
AS
BEGIN
	SELECT 
		CASE 
		  WHEN tp.Name = 'ProcessCheck' THEN 'Check Cashing'
		  WHEN tp.Name = 'BillPayment' THEN 'Bill Payment'
		  WHEN tp.Name = 'MoneyTransfer' THEN 'Send Money'
		  WHEN tp.Name = 'ProductCredential' THEN 'ZEO Card' 
		  WHEN tp.Name = 'MoneyOrder' THEN 'Money Order'
		  ELSE tp.Name 
		END AS ProductName,
		Code AS ProviderId,
		CASE 
		  WHEN tpr.Name = 'WesternUnion' THEN 'Western Union'
		  WHEN tpr.Name = 'INGO' THEN 'Ingo'
		  WHEN tpr.Name = 'VISA' THEN 'Visa'
		  ELSE tpr.Name 
		END AS ProviderName,
		tp.ProductsID AS ProductId
	FROM 
		tProducts tp WITH (NOLOCK)
		INNER JOIN tProductProcessorsMapping tppm WITH (NOLOCK) ON tp.ProductsID = tppm.ProductId
		INNER JOIN tProcessors tpr WITH (NOLOCK) ON tpr.ProcessorsID = tppm.ProcessorId
		INNER JOIN tChannelPartnerProductProcessorsMapping tcpm WITH (NOLOCK) ON tcpm.ProductProcessorId = tppm.ProductProcessorsMappingID
	WHERE 
		tcpm.ChannelPartnerID = @channelPartnerId AND tp.Name != 'ReceiveMoney'
	
END 
