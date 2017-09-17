-- ==================================================
-- Author:	Kaushik Sakala
-- Date: 06/02/2017
-- Description:	Sp to get message from tmessagestore
-- ===================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetErrorMessage'
)
BEGIN
	DROP PROCEDURE usp_GetErrorMessage
END
GO
CREATE PROCEDURE usp_GetErrorMessage
(
	@ChannelPartnerId BIGINT,
	@errorCode NVARCHAR(50),
	@language INT,
	@providerCode NVARCHAR(50),
	@productCode NVARCHAR(50)
)
AS
BEGIN
	DECLARE @lang NVARCHAR(08) =  CAST(@language as NVARCHAR(08))
	DECLARE	@errorKey NVARCHAR(100) = (@productCode +'.'+ @providerCode +'.'+ @errorCode)
	DECLARE	@errorKey1 NVARCHAR(100)  = (@providerCode +'.'+ @errorCode)	
	DECLARE	@errorKey2 NVARCHAR(100) = (@productCode +'.'+ @providerCode)	
	DECLARE @partnerId BIGINT = 1

	IF EXISTS(SELECT 1 FROM tMessageStore WHERE PartnerPK = @ChannelPartnerId AND MessageKey = @errorKey AND Language = @lang) 
	BEGIN
		SELECT 
			MessageKey,Content,AddlDetails,Processor,Type
		FROM 
			tMessageStore 
		WHERE 
			PartnerPK = @ChannelPartnerId AND MessageKey = @errorKey AND Language = @lang
	END
	ELSE IF EXISTS(SELECT 1 FROM tMessageStore WHERE PartnerPK = @partnerId AND MessageKey = @errorKey AND Language = @lang) 
	BEGIN
		SELECT 
			MessageKey,Content,AddlDetails,Processor,Type
		FROM 
			tMessageStore 
		WHERE 
			PartnerPK = @partnerId AND MessageKey = @errorKey AND Language = @lang
	END
	ELSE IF EXISTS(SELECT 1 FROM tMessageStore WHERE PartnerPK = @partnerId AND MessageKey = @errorKey1 AND Language = @lang)
	BEGIN
		SELECT 
			MessageKey,Content,AddlDetails,Processor,Type
		FROM 
			tMessageStore 
		WHERE 
			PartnerPK = @partnerId AND MessageKey = @errorKey1 AND Language = @lang
	END

	ELSE IF EXISTS(SELECT 1 FROM tMessageStore WHERE PartnerPK = @partnerId AND MessageKey = @errorKey2 AND Language = @lang)
	BEGIN
		SELECT 
			MessageKey,Content,AddlDetails,Processor,Type
		FROM 
			tMessageStore 
		WHERE 
			PartnerPK = @partnerId AND MessageKey = @errorKey2 AND Language = @lang
	END
	ELSE
	BEGIN 
		SELECT 
		@errorKey as  MessageKey,
        'This operation could not be completed' as Content,
		'Please contact your technical support team for more information' as AddlDetails,
        '2' as Type,
		'Zeo' as Processor
	END
END
GO
