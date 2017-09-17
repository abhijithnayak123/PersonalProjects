--- ===============================================================================
-- Author:		<Rizwana Shaik>
-- Create date: <1-12-2017>
-- Description:	Create procedure for getting tips by channel partnerName
-- Jira ID:		<AL-7580>
-- ================================================================================


IF OBJECT_ID(N'usp_GetTipsAndOffersByChannelPartnerAndViewName', N'P') IS NOT NULL
DROP PROCEDURE usp_GetTipsAndOffersByChannelPartnerAndViewName   -- Drop the existing procedure.
GO

CREATE PROCEDURE [dbo].[usp_GetTipsAndOffersByChannelPartnerAndViewName]
(
 @channelPartnerId BIGINT
	,@viewName NVARCHAR(100)
	,@lang  NVARCHAR(10)
)
AS
BEGIN

BEGIN TRY
SELECT 
	CASE 
		WHEN LOWER(@lang)='es-us' 
		      THEN TipsAndOffersEs
		ELSE TipsAndOffersEn
	    END AS TipsAndOffersValue, OptionalFilter 
	           FROM tTipsAndOffers tt 
		       INNER JOIN tChannelPartners tc ON tc.Name=tt.ChannelPartnerName
	           WHERE ViewName=@viewName AND ChannelPartnerId=@channelPartnerId;
END TRY
	BEGIN CATCH	        
	-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END