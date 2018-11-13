
--===========================================================================================
-- Auther:			Bijo James
-- Date Created:	7/10/2014
-- Updated By  :	RAJKUMAR M
-- Date Updated:	27/04/2015
-- Description:		Alter Procedure USP_PopulateMGramCatalog 
-- JIRA ID    :     <AL-343>
--===========================================================================================

/****** Object:  StoredProcedure [dbo].[USP_PopulateMGramCatalog]    Script Date: 4/27/2015 12:15:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[USP_PopulateMGramCatalog]( 
       @DATABASENAME nvarchar(20), 
	   @PROVIDERID nvarchar(10),
	   @CHANNELPARTNERID nvarchar(10),
	   @RESULT int out) 
	   
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
		SET DtModified = GETDATE() , 
		IsActive = 1 ,
		Keywords = CTL.Keywords  
		FROM tMasterCatalog MCAT 
		INNER JOIN ' + @databasename +'..tMGram_Catalog CTL ON 
		MCAT.BillerCode = CTL.ReceiveCode 
		AND MCAT.ChannelPartnerID= '+@CHANNELPARTNERID+'
		AND ProviderId='+ @PROVIDERID+'
		WHERE CTL.IsActive = 1'
	
	SET @QUERY = @QUERY + CHAR(13)
     
	-- INSERTING THE ACTIVE BILLERS. 	
	SET @QUERY = @QUERY + ' INSERT INTO tMasterCatalog (MasterCatalogPK,PROVIDERCATALOGID, BillerName,ChannelPartnerId,ProviderId,IsActive,DtCreate,BillerCode,Keywords)  
				 SELECT NEWID() AS MasterCatalogPK,CTL.Id AS PROVIDERCATALOGID, CTL.BillerName AS BILLERNAME, '+@CHANNELPARTNERID+' AS CHANNELPARTNERID,
				 ' + @PROVIDERID  + ' AS PROVIDERID, 1 AS ISACTIVE, GETDATE() AS DTCREATE,CTL.ReceiveCode AS BillerCode,CTL.Keywords from ' + @DATABASENAME + '..tMGram_Catalog CTL
				 WHERE NOT EXISTS (SELECT BILLERCODE FROM tMasterCatalog MCTL WHERE MCTL.BillerCode = CTL.ReceiveCode 
				 AND  MCTL.ChannelPartnerID= '+@CHANNELPARTNERID+' and ProviderId='+ @PROVIDERID + ') AND  CTL.ISACTIVE = 1'	 
	print @QUERY	  
	EXEC (@QUERY) 	
	
	-- DELETE ALL THE  BILLERS FROM PARTNERCATALOG	
	DELETE FROM tPartnerCatalog where ChannelPartnerId =@CHANNELPARTNERID
	
	-- INSERTING THE ACTIVE BILLERS TO TPARTNERCATALOG 
	INSERT INTO tPartnerCatalog (tPartnerCatalogPK, BillerName, ChannelPartnerId,ProviderId,DtCreate,BillerCode,Keywords,MasterCatalogPK) 
	SELECT  NEWID(), BillerName , ChannelPartnerId, PROVIDERID,GETDATE(),BillerCode,Keywords,MasterCatalogPK
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

