-- Create date: 10/18/2016	
-- Description:	To get visa shipping fee by shipping type
-- =============================================

IF OBJECT_ID(N'USP_GetVisaFeeByChannelPartnerIdandVisaFeeType', N'P') IS NOT NULL
DROP PROC USP_GetVisaFeeByChannelPartnerIdandVisaFeeType
GO

CREATE PROCEDURE USP_GetVisaFeeByChannelPartnerIdandVisaFeeType

		@channelPartnerId BIGINT,
		@code BIGINT

AS
BEGIN
BEGIN TRY
	SELECT
		 tvf.Fee,
		 tvf.Feecode,
		 tvf.StockId
	From
		tVisa_FeeTypes AS tvft 
	INNER JOIN tVisa_ChannelPartnerFeeTypeMapping AS cpft
		ON cpft.VisaFeeTypeID = tvft.VisaFeeTypeID
	INNER JOIN tVisa_Fee AS tvf
	    ON tvf.ChannelPartnerFeeTypeId = cpft.ChannelPartnerFeeTypeId
	Where 
		ChannelPartnerId = @channelPartnerId AND tvft.VisaFeeTypeId = @code
END TRY

BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH

END
GO
