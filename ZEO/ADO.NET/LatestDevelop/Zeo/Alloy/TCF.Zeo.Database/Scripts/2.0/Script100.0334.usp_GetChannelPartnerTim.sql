-- =============================================
-- Author:		<Shwetha Mohan>
-- Create date: <01/21/2017>
-- Description: <Store Proc to GET All Location using ID>
-- Jira ID:		<AL-7582>
-- =============================================
--
IF EXISTS (SELECT 1 FROM SYS.PROCEDURES WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetChannelPartnerTim]') AND TYPE IN (N'P'))
DROP PROCEDURE [dbo].usp_GetChannelPartnerTim
GO

CREATE PROCEDURE usp_GetChannelPartnerTim	
(
	@locationId BIGINT
)
AS
BEGIN
    
	SET NOCOUNT ON;
	BEGIN TRY

		Select tc.Tim AS TIM
		from tChannelPartners AS tc
		INNER JOIN 
		 tTerminals AS tt
		 ON
		tc.ChannelPartnerId =tt.ChannelPartnerId
        WHERE LocationID =  @locationId
	END TRY	
	BEGIN CATCH      
		EXECUTE usp_CreateErrorInfo; 
		 
	END CATCH
END
GO
