--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-3-2016>
-- Description:	 Get Chexar partner details by transaction id 
-- Jira ID:		<AL-7705>
-- ================================================================================

IF OBJECT_ID(N'usp_GetChxrPartnerByChannelPartnerId', N'P') IS NOT NULL
DROP PROC usp_GetChxrPartnerByChannelPartnerId
GO

-- EXEC usp_GetChxrPartnerByChannelPartnerId '33'

CREATE PROCEDURE usp_GetChxrPartnerByChannelPartnerId
(	
	@channelPartnerId BIGINT
)
AS
BEGIN
	
BEGIN TRY

	   SELECT
	     ChxrPartnerID,
		 Name,
		 URL
	   FROM
		 tChxr_Partner WITH (NOLOCK)
	   WHERE
		 ChxrPartnerId = @channelPartnerId

END TRY
BEGIN CATCH

	EXEC usp_CreateErrorInfo

END CATCH
END
GO


