
IF EXISTS (SELECT *
           FROM   sysobjects 
           WHERE  type = 'P'
                  AND name = 'USP_EX_GetDeleteStatusFlag')
    BEGIN
        DROP PROCEDURE USP_EX_GetDeleteStatusFlag;
        PRINT 'DROPPED USP_EX_GetDeleteStatusFlag';
    END
GO               
          
/*********************************************************************************************************************                                                         
* New Procedure Name : USP_EX_GetDeleteStatusFlag
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
* 12-Apr-14     Yugandhar  EXCREQ 7.4    
*********************************************************************************************************************/   
--EXEC [USP_EX_GetDeleteStatusFlag] 'TBL_INV_AccountProfile','CustomerAccountNumber','100004',0    
    
CREATE PROCEDURE [dbo].[USP_EX_GetDeleteStatusFlag] --'TBL_INV_AccountProfile','CustomerAccountNumber','100004',0    
(             
 @TABLE_NAME VARCHAR(100),    
 @FIELD_NAME VARCHAR(100),    
 @FIELD_VALUE VARCHAR(15),    
 @STATUS BIT OUTPUT    
)            
AS    
BEGIN    
 DECLARE @TBL TABLE(STATUS BIT)       
 DECLARE @QUERY NVARCHAR(4000)    
 SET @STATUS = 1      
 SET @QUERY = 'SELECT QUERY=0 FROM ' +  @TABLE_NAME + ' WHERE ' + @FIELD_NAME + ' = ' + '''' + @FIELD_VALUE + ''''    
   
  --PRINT @QUERY 
     
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
                  AND name = 'USP_EX_GetDeleteStatusFlag')
    BEGIN
       
        PRINT 'CREATED USP_EX_GetDeleteStatusFlag';
    END
      
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    