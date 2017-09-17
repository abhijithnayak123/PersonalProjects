--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <08-28-2017>
-- Description:	Get the RCIF credentials. 
-- Jira ID:		<B-08245 - Move the RCIF service url configuration to database layer>
-- ================================================================================

IF OBJECT_ID(N'usp_GetRCIFCredentials', N'P') IS NOT NULL
DROP PROC usp_GetRCIFCredentials
GO


CREATE PROCEDURE usp_GetRCIFCredentials
(
	@channelPartnerId SMALLINT
)
AS
BEGIN
	BEGIN TRY
		SELECT 
			ServiceUrl
			,ChannelPartnerId 
			,CertificateName
			,ThumbPrint
		FROM tRCIF_Credential 
		WHERE ChannelPartnerId = @channelPartnerId
	END TRY
	BEGIN CATCH
		EXECUTE usp_CreateErrorInfo
	END CATCH
END
GO