--- ===============================================================================
-- Author:		<Rizwana Shaik>
-- Create date: <1-12-2017>
-- Description:	Create procedure for fetching channel partner providers
-- Jira ID:		<AL-7580>
-- ================================================================================


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
			PPM.ProductProcessorsMappingPK, 
			PPM.ProductProcessorsMappingId, 
			PPM.ProductId, 
			PPM.ProcessorId, 
			PPM.Code, 
			PPM.IsSSNRequired, 
			PPM.IsSWBRequired, 
			PPM.CanParkReceiveMoney, 
			PPM.ReceiptCopies, 
			PPM.ReceiptReprintCopies, 
			PPM.DTServerCreate, 
			PPM.DTServerLastModified, 
			P.ProductsPK, 
			P.ProductsId, 
			P.Name AS ProductName, 
			P.DTServerCreate, 
			P.DTServerLastModified, 
			PS.ProcessorsPK, 
			PS.ProcessorsId, 
			PS.Name AS ProcessorName , 
			PS.DTServerCreate, 
			PS.DTServerLastModified 
		FROM tChannelPartnerProductProcessorsMapping PM 
			INNER JOIN tProductProcessorsMapping PPM on PM.ProductProcessorId=PPM.ProductProcessorsMappingID 
			INNER JOIN tProducts P on PPM.ProductId=P.ProductsPK 
			INNER JOIN tProcessors PS on PPM.ProcessorId=PS.ProcessorsPK 
			INNER JOIN tChannelPartners C on C.ChannelPartnerId=PM.ChannelPartnerId 
		WHERE 
			C.Name = ISNULL(@channelPartnerName, C.Name)
			AND C.ChannelPartnerId = ISNULL(@channelPartnerId, C.ChannelPartnerId)
		ORDER BY PM.Sequence;

	END TRY
	BEGIN CATCH	        
	-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END