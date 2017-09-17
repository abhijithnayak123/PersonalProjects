-- =============================================
-- Author:		Shwetha		
-- Create date: 10/18/2016	
-- Description:	To get card class by state code
-- =============================================

IF OBJECT_ID(N'usp_GetCardClassbyStateCode', N'P') IS NOT NULL
DROP PROC usp_GetCardClassbyStateCode
GO

CREATE PROCEDURE usp_GetCardClassbyStateCode 
	(
		@stateCode VARCHAR(5),
		@channelPartnerId INT
	)
	
AS
BEGIN
BEGIN TRY
	SELECT 
		CardClass
	FROM
		tVisa_CardClass
	WHERE 
		StateCode = @stateCode AND ChannelPartnerId = @channelPartnerId
END TRY

BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH
END
GO
