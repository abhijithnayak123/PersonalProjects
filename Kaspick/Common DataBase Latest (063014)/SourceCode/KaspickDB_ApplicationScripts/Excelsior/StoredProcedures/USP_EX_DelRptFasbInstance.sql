
IF EXISTS (SELECT *
           FROM   sysobjects 
           WHERE  type = 'P'
                  AND name = 'USP_EX_DelRptFasbInstance')
    BEGIN
        DROP PROCEDURE USP_EX_DelRptFasbInstance;
        PRINT 'DROPPED USP_EX_DelRptFasbInstance';
    END
GO

     
/*********************************************************************************************************************                                                       
* New Procedure Name : USP_EX_DelRptFasbInstance  
* Old Procedure Name  : USP_EIS_RPT_FASB_Instance_DelProc               
* Description   : To delete a specific FASB instance            
* Input           : @FASBInstanceID           
* Sample call : [USP_EIS_RPT_FASB_Instance_DelProc] 100008                                         
* Modification log:                                              
*                                          
* Date   Modified BY  Description                                                          
*--------------------------------------------------------------------------------------------------------------------                                                       
* 20-Sep-12  Ganapati  Created              
* 14-Apr-2014 Mallikarjun EXCREQ5.4    
* 23/05/2014  Mallikarjun SP Name Renamed and Formatted  
*********************************************************************************************************************/              
CREATE PROCEDURE [dbo].[USP_EX_DelRptFasbInstance]      
(                
 @FASBInstanceID int            
)                
AS                
BEGIN                    
BEGIN TRY         
 BEGIN TRANSACTION             
    
 delete from TBL_BR_FASBAccountExclusionConditionList where FASB_InstanceID =  @FASBInstanceID       
 delete from TBL_BR_FASBOutputParam where FASB_InstanceID =  @FASBInstanceID       
 delete from TBL_BR_FASBInputParam where FASB_InstanceID =  @FASBInstanceID       
 delete from TBL_BR_FASBProfileInformation where FASB_InstanceID =  @FASBInstanceID     
           
         COMMIT TRANSACTION  
END TRY              
BEGIN CATCH   
  ROLLBACK TRANSACTION  
  
  DECLARE @ErrorMessage NVARCHAR(4000);  
  DECLARE @ErrorSeverity INT;  
  DECLARE @ErrorState INT;  
  
  
  SELECT @ErrorMessage = ERROR_MESSAGE()  
   ,@ErrorSeverity = ERROR_SEVERITY()  
   ,@ErrorState = ERROR_STATE();  
   
  RAISERROR (
            @ErrorMessage
            ,-- Message text.
            @ErrorSeverity
            ,-- Severity.
            @ErrorState -- State.
            );              

 
                   
END CATCH                     
END     


GO
  IF EXISTS (	SELECT *
			FROM sysobjects
			WHERE type = 'P'
			AND name = 'USP_EX_DelRptFasbInstance') 
	BEGIN
			PRINT 'CREATED PROCEDURE USP_EX_DelRptFasbInstance';
	END