--- ===============================================================================
-- Author:		<Rizwana Shaik>
-- Create date: <11-15-2016>
-- Description:	Create procedure for Getting Biller Details 
-- Jira ID:		<AL-8421>
-- ================================================================================


IF OBJECT_ID(N'usp_GetBillers', N'P') IS NOT NULL
	DROP PROCEDURE usp_GetBillers   -- Drop the existing trigger.
GO

CREATE PROCEDURE usp_GetBillers
(
	@term NVARCHAR(50),
	@channelPartnerID INT
)
AS
BEGIN
	BEGIN TRY
		SELECT 
			BillerName,
			BillerCode
		FROM 
			tPartnerCatalog
		WHERE 		   
			ChannelPartnerId = @ChannelPartnerId			
			AND 
			(
				BillerName LIKE @term + '%' 
				OR 
				BillerCode LIKE @term + '%' 
				OR 
				Keywords LIKE '%' + @term + '%'
			)
	END TRY 

	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END