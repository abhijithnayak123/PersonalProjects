-- =============================================
-- Author:		Kaushik Sakala		
-- Create date: 10/11/2016	
-- Description:	To get card class by state code
-- =============================================

IF OBJECT_ID(N'usp_GetLocationStateCodeAndCardExpPeriod', N'P') IS NOT NULL
DROP PROC usp_GetLocationStateCodeAndCardExpPeriod
GO

CREATE PROCEDURE usp_GetLocationStateCodeAndCardExpPeriod 
	@channelPartnerId BIGINT,
	@locationId BIGINT
AS
BEGIN
BEGIN TRY
	SELECT 
		cpp.CardExpiryPeriod,l.[State]
	FROM 
		tChannelPartnerProductProcessorsMapping cpp
		INNER JOIN tProductProcessorsMapping ppm ON cpp.ProductProcessorId = ppm.ProductProcessorsMappingPK
		INNER JOIN tProcessors p ON p.ProcessorsPK = ppm.ProcessorId
		INNER JOIN tProducts pr ON pr.ProductsPK = ppm.ProductId
		INNER JOIN tChannelPartners cp ON cp.ChannelPartnerPK = cpp.ChannelPartnerId
		INNER JOIN tLocations l ON L.ChannelPartnerId = CP.ChannelPartnerId
	WHERE 
		cp.ChannelPartnerId = @channelPartnerId 
		AND pr.ProductsID = 6 
		AND p.ProcessorsID = 4 
		AND l.LocationID = @locationId
END TRY

BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH
END
GO
