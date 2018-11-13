-- ================================================================================
-- Author:		<Ashok Kumar>
-- Create date: <07/01/2015>
-- Description:	<Alter views date columns to DTTerminalCreate, DTTerminalLastModified, DTServerCreate, DTServerLastModified. >
-- Jira ID:		<AL-602>
-- ================================================================================

-- USP_AddFiledetails
DROP PROCEDURE [dbo].[USP_AddFiledetails]
GO

--create [USP_AddFiledetails] to add file details 
CREATE PROCEDURE [dbo].[USP_AddFiledetails]  
 -- Add the parameters for the stored procedure here  
    @NychaFilePath nvarchar(max),  
    @NYCHAFileName nvarchar(500),   
    @HCreateDate datetime,   
    @HCreateTime nvarchar(100),      
    @Id bigint  out  ,
    @FileType char(1) = null,
	@LCKBATCHNO	nvarchar(20) = null,
	@LCKJOBNO	nvarchar(20) = null,
	@LCKOPERNO	nvarchar(20) = null,
	@LCKSEQNO	nvarchar(20) = null
AS  
BEGIN  
 BEGIN TRANSACTION;  
     
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
   
  Insert into tNYCHAFiles(NYCHAFilePath,NYCHAFileName, HCreateDate,HCreateTime,DTServerCreate,FileType,LCKBATCHNO,LCKJOBNO,LCKOPERNO,LCKSEQNO)  
  values (@NychaFilePath,@NYCHAFileName,@HCreateDate,@HCreateTime,getdate(),@FileType,@LCKBATCHNO,@LCKJOBNO,@LCKOPERNO,@LCKSEQNO)  
  --SET @Id = (Select ID from tNYCHAFiles where NYCHAFileName = @NYCHAFileName and  )  
  set @Id = @@IDENTITY  
    
   
    IF @@ERROR > 0  
    Begin  
        ROLLBACK TRANSACTION;   
        Set @Id = 0           
    End   
 ELSE  
 BEGIN   
  IF @@TRANCOUNT > 0   
  BEGIN  
  COMMIT TRANSACTION;    
  END  
 END  
  
END
GO