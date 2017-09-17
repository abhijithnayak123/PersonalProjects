--- ===============================================================================
-- Author:		<Kiranmaie>
-- Create date: <11-15-2016>
-- Description:	Get the WU credentials. 
-- Jira ID:		<AL-8325>
-- ================================================================================
IF OBJECT_ID(N'usp_GetWUnionCredentials', N'P') IS NOT NULL
DROP PROC usp_GetWUnionCredentials
GO


CREATE PROCEDURE usp_GetWUnionCredentials
(
	@channerlPartnerId BIGINT
)
AS
BEGIN
	BEGIN TRY
		SELECT 
			WUCredentialID
			,WUServiceUrl
			,WUClientCertificateSubjectName
			,AccountIdentifier
			,CounterId
			,ChannelName
			,ChannelVersion
			,ChannelPartnerId 
		FROM tWUnion_Credential 
		WHERE ChannelPartnerId=@ChannerlPartnerId
	END TRY
	BEGIN CATCH
		EXECUTE usp_CreateErrorInfo
	END CATCH
END
GO


