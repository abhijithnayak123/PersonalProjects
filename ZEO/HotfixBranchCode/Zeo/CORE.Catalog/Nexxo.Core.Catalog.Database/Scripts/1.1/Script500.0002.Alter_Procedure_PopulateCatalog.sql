-- ===================================================================
-- Author:		Bijo James
-- Create date: 2/feb/2014
-- Updated By: Mohammed Firoz
-- Modified Date: 27/Apr/2015
-- Description:	Populating Master and Partner Catalog
-- ===================================================================

/****** Object:  StoredProcedure [dbo].[PopulateCatalog]    Script Date: 4/27/2015 2:04:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[PopulateCatalog]
	   @DATABASENAME nvarchar(20), 
	   @PROVIDERID nvarchar(10),
	   @CHANNELPARTNERID nvarchar(10),
	   @RESULT int out 	  
AS
BEGIN
	
	SET NOCOUNT ON;		
	BEGIN TRANSACTION;  	
	DECLARE @QUERY NVARCHAR(MAX)
	
	-- Updating all the records to false.	
	SET @QUERY = 'UPDATE tMasterCatalog SET ISACTIVE = 0, DtModified = GETDATE() 
	where ChannelPartnerID= ' + @CHANNELPARTNERID +' and ProviderId='+ @PROVIDERID
	
	SET @QUERY = @QUERY + CHAR(13)
	
	-- UPDATE ONLY ACTIVE RECORDS. 
	SET @QUERY = @QUERY + 'UPDATE tMasterCatalog
		SET DtModified = GETDATE() , IsActive = 1 
		FROM tMasterCatalog MCAT 
		INNER JOIN ' + @databasename +'..tWUnion_Catalog CTL ON 
		MCAT.BillerName = CTL.CompanyName 
		AND MCAT.ChannelPartnerID= '+@CHANNELPARTNERID+'
		AND ProviderId='+ @PROVIDERID+'
		WHERE CTL.IsActive = 1'
	
	SET @QUERY = @QUERY + CHAR(13)
     
	-- INSERTING THE ACTIVE BILLERS. 	
	SET @QUERY = @QUERY + ' INSERT INTO tMasterCatalog (MasterCatalogPK,PROVIDERCATALOGID, BillerName,ChannelPartnerId,ProviderId,IsActive,DtCreate)  
				 SELECT NEWID() AS MasterCatalogPK,CTL.WUCatalogID AS PROVIDERCATALOGID, CTL.CompanyName AS BILLERNAME, '+@CHANNELPARTNERID+' AS CHANNELPARTNERID,
				 ' + @PROVIDERID  + ' AS PROVIDERID, 1 AS ISACTIVE, GETDATE() AS DTCREATE  from ' + @DATABASENAME + '..tWUnion_Catalog CTL
				 WHERE NOT EXISTS (SELECT BILLERNAME FROM tMasterCatalog MCTL WHERE MCTL.BillerName = CTL.CompanyName 
				 AND  MCTL.ChannelPartnerID= '+@CHANNELPARTNERID+' and ProviderId='+ @PROVIDERID + ') AND  CTL.ISACTIVE = 1'	 
	print @QUERY	  
	EXEC (@QUERY) 	
	
	-- DELETE ALL THE  BILLERS FROM PARTNERCATALOG	
	DELETE FROM tPartnerCatalog where ChannelPartnerID=@CHANNELPARTNERID
	
	-- INSERTING THE ACTIVE BILLERS TO TPARTNERCATALOG 
	INSERT INTO tPartnerCatalog (tPartnerCatalogPK, BillerName, ChannelPartnerId,ProviderId,DtCreate,MasterCatalogPK) 
	SELECT  NEWID(), BillerName , ChannelPartnerId, PROVIDERID,GETDATE(), MasterCatalogPK
	FROM tMasterCatalog  WHERE IsActive = 1 and ChannelPartnerID=@CHANNELPARTNERID	
	
	
			
	IF @@ERROR > 0  
    Begin  
        ROLLBACK TRANSACTION;   
        Set @RESULT = 0           
    End   
	ELSE  
	BEGIN   
	 IF @@TRANCOUNT > 0   
	 BEGIN
	    SET @RESULT = 1 
		COMMIT TRANSACTION;    
	 END  
       
END

END
GO


