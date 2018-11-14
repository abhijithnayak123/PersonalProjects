--- =====================================================================================
-- Author:		<Rizwana Shaik>
-- Modified By : <Kaushik Sakala>
-- Create Date: <12-01-2017>
-- Modified Date : <06-02-2017>
-- Description:	Avoiding the page load delay, when multiple users access the application.
-- =======================================================================================


IF OBJECT_ID(N'usp_GetProvidersByChannelPartnerIdOrName', N'P') IS NOT NULL
DROP PROCEDURE usp_GetProvidersByChannelPartnerIdOrName   -- Drop the existing procedure.
GO

CREATE PROCEDURE [dbo].[usp_GetProvidersByChannelPartnerIdOrName]
(
	@channelPartnerName NVARCHAR(100)= NULL,
	@channelPartnerId INT = NULL
)
AS
BEGIN
BEGIN TRY
		SELECT @channelPartnerId = ChannelPartnerId FROM tChannelPartners WITH (NOLOCK) 
		WHERE (Name = @channelPartnerName) OR (ChannelPartnerId= @channelPartnerId) 

		SELECT 
			PM.ChannelPartnerId, 
			PM.ChannelPartnerProductProcessorsMappingID, 
			PM.ChannelPartnerId, PM.ProductProcessorId, 
			PM.Sequence,
			PM.IsTnCForcePrintRequired, 
			PM.CheckEntryType, 
			PM.MinimumTransactAge, 
			PM.CardExpiryPeriod, 
			PM.DTServerCreate, 
			PM.DTServerLastModified, 
			PPM.ProductProcessorsMappingId, 
			PPM.ProductId, 
			PPM.ProcessorId, 
			PPM.Code AS Code, 
			PPM.IsSSNRequired, 
			PPM.IsSWBRequired, 
			PPM.CanParkReceiveMoney, 
			PPM.ReceiptCopies, 
			PPM.ReceiptReprintCopies, 
			PPM.DTServerCreate, 
			PPM.DTServerLastModified, 
			P.ProductsId, 
			P.Name AS ProductName, 
			P.DTServerCreate, 
			P.DTServerLastModified, 
			PS.ProcessorsId, 
			PS.Name AS ProcessorName , 
			PS.DTServerCreate, 
			PS.DTServerLastModified 
		FROM tChannelPartnerProductProcessorsMapping PM WITH (NOLOCK)
			INNER JOIN tProductProcessorsMapping PPM WITH (NOLOCK) on PM.ProductProcessorId=PPM.ProductProcessorsMappingID 
			INNER JOIN tProducts P WITH (NOLOCK) on PPM.ProductId=P.ProductsID 
			INNER JOIN tProcessors PS WITH (NOLOCK) on PPM.ProcessorId=PS.ProcessorsID 
		WHERE PM.ChannelPartnerId = @channelPartnerId  
		ORDER BY Sequence; 
  
	END TRY
	BEGIN CATCH	        
	-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END