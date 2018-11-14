-- ==================================================
-- Author:	Kaushik Sakala
-- Modify By: Nitish Biradar
-- Date: 06/02/2017
-- Description:	Sp to get message from tmessagestore
-- ===================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetErrorMessageByMessageKey'
)
BEGIN
	DROP PROCEDURE usp_GetErrorMessageByMessageKey
END
GO
CREATE PROCEDURE usp_GetErrorMessageByMessageKey
(
	@ChannelPartnerId BIGINT,
	@messageKey NVARCHAR(50),
	@locationId BIGINT
)
AS
BEGIN
	DECLARE @partnerId BIGINT = 1
	DECLARE @stateCode NVARCHAR(2) = dbo.ufn_GetStateCodeBasedOnLacationId(@locationId)
	DECLARE @rbsType NVARCHAR(10) = 'RBS'

	IF EXISTS(SELECT 1 FROM tMessageStore WHERE ChannelPartnerId = @ChannelPartnerId AND MessageKey = @messageKey) 
	BEGIN
		SELECT 
			MessageKey,Content,Processor,Type,
			Replace(ms.AddlDetails, '{0}', ISNULL(si.Phone1, '')) AS AddlDetails
		FROM 
			tMessageStore ms
			LEFT JOIN tSupportInformation si WITH (NOLOCK) ON 
				ms.ContactType = si.ContactType AND ((ms.ContactType != @rbsType) OR (ms.ContactType = @rbsType AND si.StateCode = @stateCode))
		WHERE 
			ms.ChannelPartnerId = @ChannelPartnerId AND MessageKey = @messageKey 
	END
	ELSE IF EXISTS(SELECT 1 FROM tMessageStore WHERE ChannelPartnerId = @partnerId AND MessageKey = @messageKey) 
	BEGIN
		SELECT 
			MessageKey,Content,Processor,Type,
			Replace(ms.AddlDetails, '{0}', ISNULL(si.Phone1, '')) AS AddlDetails
		FROM 
			tMessageStore ms
			LEFT JOIN tSupportInformation si WITH (NOLOCK) ON 
				ms.ContactType = si.ContactType AND ((ms.ContactType != @rbsType) OR (ms.ContactType = @rbsType AND si.StateCode = @stateCode))
		WHERE 
			ms.ChannelPartnerId = @partnerId AND MessageKey = @messageKey 
	END
	
END
GO
