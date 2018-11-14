-- ================================================================================
-- Author:		Manikandan Govindraj
-- Create date: 14/03/2017
-- Description: Create the function to get the decline message
-- ================================================================================


IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'FN' AND NAME = 'ufn_GetDeclineMessageByKey')
BEGIN
    DROP FUNCTION dbo.ufn_GetDeclineMessageByKey
END
GO

CREATE FUNCTION ufn_GetDeclineMessageByKey
(
    @declineMessagekey  NVARCHAR(20),
	@channelPartnerId   INT,
	@lang               INT    
)
RETURNS NVARCHAR(1000)
BEGIN
	

	DECLARE @declineMessage NVARCHAR(1000)  


	SELECT 
		@declineMessage = Content
	FROM  
		tMessageStore WITH (NOLOCK)
	WHERE  
		MessageKey = @declineMessagekey 
		AND 
		Language = @lang AND ChannelPartnerId = @channelPartnerId


	IF(@declineMessage IS NULL)
	BEGIN
		SELECT 
			@declineMessage = Content
		FROM  
			tMessageStore WITH (NOLOCK)
		WHERE  
			MessageKey = @declineMessagekey 
			AND 
			Language = @lang AND ChannelPartnerId = 1 -- Default channel Partner Id
	END


	IF( @declineMessage IS NULL AND @lang != 0 )  -- get message with default language
	BEGIN
		SELECT 
			@declineMessage = Content
		FROM  
			tMessageStore WITH (NOLOCK)
		WHERE  
			MessageKey = @declineMessagekey 
			AND 
			Language = 0 AND ChannelPartnerId = @channelPartnerId 

        IF(@declineMessage IS NULL) -- Default language with default channel partner
	    BEGIN
			SELECT 
				@declineMessage = Content
			FROM  
				tMessageStore WITH (NOLOCK)
			WHERE  
				MessageKey = @declineMessagekey 
				AND 
				Language = 0 AND ChannelPartnerId = 1 -- Default channel Partner Id and language
	    END


	END
	
	RETURN @declineMessage

END
GO

