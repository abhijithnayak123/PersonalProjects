--- ===============================================================================
-- Author:		<Rizwana Shaik>
-- Create date: <1-12-2017>
-- Description:	Create procedure for fetching channel partner certificates
-- Jira ID:		<AL-7580>
-- ================================================================================


IF OBJECT_ID(N'usp_GetChannelPartnerCertificateByPartnerIdAndIssuerName', N'P') IS NOT NULL
DROP PROCEDURE usp_GetChannelPartnerCertificateByPartnerIdAndIssuerName   -- Drop the existing procedure.
GO

CREATE PROCEDURE [dbo].[usp_GetChannelPartnerCertificateByPartnerIdAndIssuerName]
(
	@issuerName NVARCHAR(200),
	@channelPartnerId INT
)
AS
BEGIN

	SELECT 
		 ChannelPartnerCertificateId
		,ChannelPartnerId
		,Issuer
		,ThumbPrint
		,DTServerCreate
		,DTServerLastModified
		 
	FROM tChannelPartnerCertificate 
	WHERE (ChannelPartnerId = @channelPartnerId) AND (Issuer = @issuerName);

END