--===========================================================================================
-- Auther:			Ratheesh
-- Date Created:	26/19/2014
-- Description:		Script for Creating PopulateMGramCatalog Stored Procedure
--===========================================================================================
--CREATE mastercatalog

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_PopulateMGramCatalog]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_PopulateMGramCatalog]
GO

CREATE PROCEDURE [dbo].[USP_PopulateMGramCatalog]( 
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
		SET DtModified = GETDATE() , IsActive = 1 
		FROM tMasterCatalog MCAT 
		INNER JOIN ' + @databasename +'..tMGram_Catalog CTL ON 
		MCAT.BillerCode = CTL.ReceiveCode 
		AND MCAT.ChannelPartnerID= '+@CHANNELPARTNERID+'
		AND ProviderId='+ @PROVIDERID+'
		WHERE CTL.IsActive = 1'
	
	SET @QUERY = @QUERY + CHAR(13)
     
	-- INSERTING THE ACTIVE BILLERS. 	
	SET @QUERY = @QUERY + ' INSERT INTO tMasterCatalog (ROWGUID,PROVIDERCATALOGID, BillerName,ChannelPartnerId,ProviderId,IsActive,DtCreate,BillerCode)  
				 SELECT NEWID() AS ROWGUID,CTL.Id AS PROVIDERCATALOGID, CTL.BillerName AS BILLERNAME, '+@CHANNELPARTNERID+' AS CHANNELPARTNERID,
				 ' + @PROVIDERID  + ' AS PROVIDERID, 1 AS ISACTIVE, GETDATE() AS DTCREATE,CTL.ReceiveCode AS BillerCode from ' + @DATABASENAME + '..tMGram_Catalog CTL
				 WHERE NOT EXISTS (SELECT BILLERCODE FROM tMasterCatalog MCTL WHERE MCTL.BillerCode = CTL.ReceiveCode 
				 AND  MCTL.ChannelPartnerID= '+@CHANNELPARTNERID+' and ProviderId='+ @PROVIDERID + ') AND  CTL.ISACTIVE = 1'	 
	print @QUERY	  
	EXEC (@QUERY) 	
	
	-- DELETE ALL THE  BILLERS FROM PARTNERCATALOG	
	DELETE FROM tPartnerCatalog where ChannelPartnerID=@CHANNELPARTNERID
	
	-- INSERTING THE ACTIVE BILLERS TO TPARTNERCATALOG 
	INSERT INTO tPartnerCatalog (rowguid, id, BillerName, ChannelPartnerId,ProviderId,DtCreate) 
	SELECT  NEWID(), id, BillerName , ChannelPartnerId, PROVIDERID,GETDATE()
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


