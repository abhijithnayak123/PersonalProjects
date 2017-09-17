-- =============================================
-- Author:		Shwetha		
-- Create date: 10/18/2016	
-- Description:	To get visa shipping fee by shipping type
-- =============================================

IF OBJECT_ID(N'usp_GetVisaShippingFeeByShippingType', N'P') IS NOT NULL
DROP PROC usp_GetVisaShippingFeeByShippingType
GO

CREATE PROCEDURE usp_GetVisaShippingFeeByShippingType
	@code VARCHAR(10),
	@channelPartnerId INT
AS
BEGIN
BEGIN TRY
	SELECT 
		 Fee,
		 FeeCode,*
	FROM
		tVisa_ShippingFee tvst
	INNER JOIN  tVisa_ChannelPartnerShippingTypeMapping AS cpst
		ON	tvst.ChannelPartnerShippingTypeID = cpst.ChannelPartnerShippingTypeID
	INNER JOIN tVisa_CardShippingTypes AS vcst ON vcst.CardShippingTypeId = cpst.CardShippingTypeId
	WHERE 
		CODE = @code AND vcst.Active = 1 AND ChannelPartnerId = @channelPartnerId
END TRY

BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH
END
GO
