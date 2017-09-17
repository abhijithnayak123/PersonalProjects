--- ===============================================================================
-- Author:		 Kaushik Sakala
-- Description:	 Script to populate the WU Country Translations
-- Jira ID:		<AL-8998>
-- ================================================================================

IF OBJECT_ID(N'usp_PopulateBillerCatalog', N'P') IS NOT NULL
DROP PROC usp_PopulateBillerCatalog
GO

CREATE PROCEDURE usp_PopulateBillerCatalog	
	@providerId nvarchar(10),
	@channelPartnerId nvarchar(10),
	@dtServerDate DATETIME
AS
BEGIN

 BEGIN TRY
        BEGIN TRANSACTION
        SET NOCOUNT ON;
	-- Updating all the records to false.	
	UPDATE 
		tMasterCatalog 
	SET 
		ISACTIVE = 0, 
		DTServerLastModified = @dtServerDate
	WHERE 
		ChannelPartnerID = @CHANNELPARTNERID 
		AND ProviderId = @PROVIDERID
	
	-- UPDATE ONLY ACTIVE RECORDS. 
	UPDATE tMasterCatalog
		SET 
			DTServerLastModified = @dtServerDate,
			IsActive = 1 
		FROM tMasterCatalog MCAT 
			INNER JOIN tWUnion_Catalog CTL ON MCAT.BillerName = CTL.CompanyName 
			AND MCAT.ChannelPartnerID= @CHANNELPARTNERID AND ProviderId= @PROVIDERID
		WHERE 
			CTL.IsActive = 1
	
	-- INSERTING THE ACTIVE BILLERS. 	
	INSERT INTO 
		tMasterCatalog (PROVIDERCATALOGID, BillerName,ChannelPartnerId,ProviderId,IsActive,DTServerCreate)  
				SELECT CTL.WUCatalogID, CTL.CompanyName, @CHANNELPARTNERID,@PROVIDERID, 1 ,@dtServerDate
				FROM tWUnion_Catalog CTL
				WHERE NOT EXISTS (
					SELECT BILLERNAME 
					FROM tMasterCatalog MCTL 
					WHERE MCTL.BillerName = CTL.CompanyName 
					AND  MCTL.ChannelPartnerID= @CHANNELPARTNERID AND ProviderId= @PROVIDERID
				) AND  CTL.ISACTIVE = 1	 
	
	-- DELETE ALL THE  BILLERS FROM PARTNERCATALOG	
	DELETE FROM tPartnerCatalog WHERE ChannelPartnerID = @CHANNELPARTNERID
	
	-- INSERTING THE ACTIVE BILLERS TO TPARTNERCATALOG 
	INSERT INTO 
		tPartnerCatalog (BillerName, ChannelPartnerId,ProviderId,DTServerCreate,MasterCatalogId) 
		SELECT BillerName , ChannelPartnerId, PROVIDERID,@dtServerDate, MasterCatalogID
		FROM tMasterCatalog  
		WHERE IsActive = 1 AND ChannelPartnerID=@CHANNELPARTNERID	

      COMMIT TRANSACTION;	          
 END TRY
BEGIN CATCH

    IF @@TRANCOUNT > 0 
    BEGIN   	  
        ROLLBACK TRANSACTION 		
    END

    EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError

END CATCH
END
GO


