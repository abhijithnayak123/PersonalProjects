/****** Object:  StoredProcedure [dbo].[PopulateCatalog]    Script Date: 12/24/2013 16:29:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PopulateCatalog]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PopulateCatalog]
GO

/****** Object:  StoredProcedure [dbo].[PopulateCatalog]    Script Date: 12/24/2013 16:29:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PopulateCatalog]
	   @DATABASENAME nvarchar(20), 
	   @PROVIDERID nvarchar(10), 
	   @RESULT int out 	  
AS
BEGIN
	
	SET NOCOUNT ON;		
	BEGIN TRANSACTION;  	
	DECLARE @QUERY NVARCHAR(MAX)
	
	-- Updating all the records to false.	
	SET @QUERY = 'UPDATE tMasterCatalog SET ISACTIVE = 0, DtModified = GETDATE()' 
	
	SET @QUERY = @QUERY + CHAR(13)
	
	-- UPDATE ONLY ACTIVE RECORDS. 
	SET @QUERY = @QUERY + 'UPDATE tMasterCatalog
			  SET DtModified = GETDATE() , IsActive = 1 
			  FROM tMasterCatalog MCAT 
			  INNER JOIN ' + @databasename +'..tWUnion_Catalog CTL ON 
			  MCAT.BillerName = CTL.CompanyName WHERE CTL.IsActive = 1'
	
	SET @QUERY = @QUERY + CHAR(13)
     
	-- INSERTING THE ACTIVE BILLERS. 	
	SET @QUERY = @QUERY + ' INSERT INTO tMasterCatalog (ROWGUID,PROVIDERCATALOGID, BillerName,ChannelPartnerId,ProviderId,IsActive,DtCreate)  
				 SELECT NEWID() AS ROWGUID,CTL.Id AS PROVIDERCATALOGID, CTL.CompanyName AS BILLERNAME, CTL.ChannelPartnerId AS CHANNELPARTNERID,
				 ' + @PROVIDERID  + ' AS PROVIDERID, 1 AS ISACTIVE, GETDATE() AS DTCREATE  from ' + @DATABASENAME + '..tWUnion_Catalog CTL
				 WHERE NOT EXISTS (SELECT BILLERNAME FROM tMasterCatalog MCTL WHERE MCTL.BillerName = CTL.CompanyName)
				 AND  CTL.ISACTIVE = 1'	 
		  
	EXEC (@QUERY) 	
	
	-- DELETE ALL THE  BILLERS FROM PARTNERCATALOG	
	DELETE FROM tPartnerCatalog 
	
	-- INSERTING THE ACTIVE BILLERS TO TPARTNERCATALOG 
	INSERT INTO tPartnerCatalog (rowguid, id, BillerName, ChannelPartnerId,ProviderId,DtCreate) 
	SELECT  NEWID(), id, BillerName , ChannelPartnerId, PROVIDERID,GETDATE()
	FROM tMasterCatalog  WHERE IsActive = 1	
	
	
			
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

