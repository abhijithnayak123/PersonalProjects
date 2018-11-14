-- ==================================================
-- Author:	Kaushik Sakala
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
	@messageKey NVARCHAR(50)
)
AS
BEGIN
	DECLARE @partnerId BIGINT = 1

	IF EXISTS(SELECT 1 FROM tMessageStore WHERE PartnerPK = @ChannelPartnerId AND MessageKey = @messageKey) 
	BEGIN
		SELECT 
			MessageKey,Content,AddlDetails,Processor,Type
		FROM 
			tMessageStore 
		WHERE 
			PartnerPK = @ChannelPartnerId AND MessageKey = @messageKey 
	END
	ELSE IF EXISTS(SELECT 1 FROM tMessageStore WHERE PartnerPK = @partnerId AND MessageKey = @messageKey) 
	BEGIN
		SELECT 
			MessageKey,Content,AddlDetails,Processor,Type
		FROM 
			tMessageStore 
		WHERE 
			PartnerPK = @partnerId AND MessageKey = @messageKey 
	END
	
END
GO