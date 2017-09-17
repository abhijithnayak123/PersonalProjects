-- =============================================
-- Author:		Shwetha		
-- Create date: 10/18/2016	
-- Description:	To get  visa credential by channel partner id
-- =============================================

IF OBJECT_ID(N'usp_GetVisaCredentialsByChannelPartnerId', N'P') IS NOT NULL
DROP PROC usp_GetVisaCredentialsByChannelPartnerId
GO

CREATE PROCEDURE usp_GetVisaCredentialsByChannelPartnerId
	(
		@channelPartnerId BIGINT
	)
AS
BEGIN
BEGIN TRY
	SELECT 
		ServiceUrl,
		CertificateName,
		UserName,
		Password,
		ClientNodeId,
		CardProgramNodeId,
		SubClientNodeId,
		StockId
	 FROM 
		tVisa_Credential
	WHERE
	ChannelPartnerId = @channelPartnerId
END TRY

BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH

END
GO
