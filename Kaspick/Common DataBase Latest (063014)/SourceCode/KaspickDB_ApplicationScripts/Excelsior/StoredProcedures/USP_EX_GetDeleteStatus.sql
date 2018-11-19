IF EXISTS (SELECT *
           FROM   sysobjects 
           WHERE  type = 'P'
                  AND name = 'USP_EX_GetDeleteStatus')
    BEGIN
        DROP PROCEDURE USP_EX_GetDeleteStatus;
        PRINT 'DROPPED USP_EX_GetDeleteStatus';
    END
GO               
     
/*********************************************************************************************************************                                                         
* New Procedure Name : USP_EX_GetDeleteStatus
* Old Procedure name  : [USP_EIS_EX_GET_DELETE_STATUS]        
* Description     : Procedure to check whether a record is deleted or not    
     Return 1 if deleted, 0 if active    
* Input Parameter :    @TABLE_NAME VARCHAR(100),    
      @FIELD_NAME VARCHAR(100),    
      @FIELD_VALUE INT    
* Input Parameter :     @STATUS bit OUTPUT                                      
* Modification log                                                             
*                                                                   
* Date          Modified By   Description                                                            
*--------------------------------------------------------------------------------------------------------------------                                                         
* 06-Mar-07     Ganapati   Created     
* 12-Mar-14     Mallikarjun  EXCREQ 5.1 Modified
*********************************************************************************************************************/   

 
    
CREATE PROCEDURE [dbo].[USP_EX_GetDeleteStatus] --'TBL_EIS_EX_TRUSTADVISOR_SUPPLEMENT','TRUSTADVISORID',100004,0    
(             
 @TABLE_NAME VARCHAR(100),    
 @FIELD_NAME VARCHAR(100),    
 @FIELD_VALUE INT,    
 @STATUS BIT OUTPUT    
)            
AS    
BEGIN    
 DECLARE @TBL TABLE(STATUS BIT)       
 DECLARE @QUERY NVARCHAR(4000)    
 SET @STATUS = 1      
 SET @QUERY = 'SELECT QUERY=0 FROM ' +  @TABLE_NAME + ' WHERE ' + @FIELD_NAME + ' = ' + CAST(@FIELD_VALUE AS VARCHAR(10))    
     
 INSERT INTO @TBL(STATUS) EXECUTE(@QUERY)    
 SELECT @STATUS = STATUS FROM @TBL    
     
 RETURN @STATUS    
    
 IF (@@ERROR != 0)    
  RETURN 1    
END     
    
    
  GO
  
  IF EXISTS (SELECT *
           FROM   sysobjects 
           WHERE  type = 'P'
                  AND name = 'USP_EX_GetDeleteStatus')
    BEGIN
       
        PRINT 'CREATED USP_EX_GetDeleteStatus';
    END
      
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    