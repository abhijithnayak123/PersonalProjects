-- =============================================
-- Author:		Shwetha		
-- Create date: 10/18/2016	
-- Description:	To get visa shipping type by channelpartner Id
-- =============================================

IF OBJECT_ID(N'usp_GetShippingTypesByChannelPartnerId', N'P') IS NOT NULL
DROP PROC usp_GetShippingTypesByChannelPartnerId
GO

CREATE PROCEDURE usp_GetShippingTypesByChannelPartnerId
	(
		@channelpartnerId BIGINT
	)
AS
BEGIN
BEGIN TRY
	Select
		Code,Name
	From 
		tVisa_CardShippingTypes AS t1
		INNER JOIN tVisa_ChannelPartnerShippingTypeMapping AS t2 ON	t1.CardShippingTypeId = t2.CardShippingTypeId
	Where
		Active = 1 AND t2.ChannelPartnerId = @channelpartnerId
END TRY	 

BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH
END
GO
