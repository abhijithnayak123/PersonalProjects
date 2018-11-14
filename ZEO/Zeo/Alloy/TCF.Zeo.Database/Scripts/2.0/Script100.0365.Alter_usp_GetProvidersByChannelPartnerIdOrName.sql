ALTER PROCEDURE [dbo].[usp_GetProvidersByChannelPartnerIdOrName]
(
@channelPartnerName NVARCHAR(100)= NULL,
	@channelPartnerId INT = NULL
)
AS
BEGIN
	BEGIN TRY
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
		FROM tChannelPartnerProductProcessorsMapping PM 
			left outer join tProductProcessorsMapping PPM on PM.ProductProcessorId=PPM.ProductProcessorsMappingID 
			left outer join tProducts P on PPM.ProductId=P.ProductsID 
			left outer join tProcessors PS on PPM.ProcessorId=PS.ProcessorsID 
		WHERE PM.ChannelPartnerId = (SELECT TOP 1 ChannelPartnerId FROM tChannelPartners 
		WHERE (Name = @channelPartnerName) OR (ChannelPartnerId= @channelPartnerId) ) 
		ORDER BY Sequence;

	END TRY
	BEGIN CATCH	        
	-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END