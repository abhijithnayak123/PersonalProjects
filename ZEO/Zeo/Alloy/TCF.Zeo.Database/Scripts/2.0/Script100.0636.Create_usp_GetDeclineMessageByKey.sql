-- ====================================================================================
-- Author:		<Author,,Manikandan Govindraj>
-- Create date: <10/23/2017>
-- Description:	<Create the SP to get check decline message and decline display message>
-- =====================================================================================


IF OBJECT_ID(N'usp_GetDeclineMessageByKey', N'P') IS NOT NULL
DROP PROC usp_GetDeclineMessageByKey
GO

CREATE PROCEDURE usp_GetDeclineMessageByKey
(
	@declineMessagekey     NVARCHAR(20),
	@channelPartnerId      INT,
	@lang                  INT, 
	@declineMessage        NVARCHAR(4000) OUTPUT, 
	@declineDisplayMessage NVARCHAR(4000) OUTPUT
)
AS
BEGIN	



	SELECT 
		@declineMessage = Content,
		@declineDisplayMessage = DisplayMessage
	FROM  
		tMessageStore WITH (NOLOCK)
	WHERE  
		MessageKey = @declineMessagekey 
		AND 
		Language = @lang AND ChannelPartnerId = @channelPartnerId


	IF(@declineMessage IS NULL)
	BEGIN
		SELECT 
			@declineMessage = Content,
		    @declineDisplayMessage = DisplayMessage
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
			@declineMessage = Content,
		    @declineDisplayMessage = DisplayMessage
		FROM  
			tMessageStore WITH (NOLOCK)
		WHERE  
			MessageKey = @declineMessagekey 
			AND 
			Language = 0 AND ChannelPartnerId = @channelPartnerId 

        IF(@declineMessage IS NULL) -- Default language with default channel partner
	    BEGIN
			SELECT 
				@declineMessage = Content,
		        @declineDisplayMessage = DisplayMessage
			FROM  
				tMessageStore WITH (NOLOCK)
			WHERE  
				MessageKey = @declineMessagekey 
				AND 
				Language = 0 AND ChannelPartnerId = 1 -- Default channel Partner Id and language
	    END


	END
	
	

END