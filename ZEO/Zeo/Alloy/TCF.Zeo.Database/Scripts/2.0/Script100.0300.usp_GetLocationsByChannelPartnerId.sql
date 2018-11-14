--- ===============================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <12/12/2016>
-- Description:	 Create stored procedure to get the locations by channel partner Id
-- Jira ID:		<AL-7582>
-- ================================================================================


IF EXISTS (SELECT 1 FROM SYS.PROCEDURES WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetLocationsByChannelPartnerId]') AND TYPE IN (N'P'))
DROP PROCEDURE [dbo].[usp_GetLocationsByChannelPartnerId]
GO

CREATE PROCEDURE usp_GetLocationsByChannelPartnerId
@channelPartnerId SMALLINT
AS

BEGIN

    SET NOCOUNT ON;

	BEGIN TRY    

		SELECT 
		  LocationID,
		  LocationName, 
		  IsActive 
		FROM 
			tLocations WITH (NOLOCK)
		WHERE 
			ChannelPartnerId = @channelPartnerId

	END TRY
	BEGIN CATCH	        

		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
			
	END CATCH
END
GO

