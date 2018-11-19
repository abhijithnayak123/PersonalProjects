

IF EXISTS (SELECT *
           FROM   sysobjects 
           WHERE  type = 'P'
                  AND name = 'USP_EX_DelPolicyitemReportingCascade ')
    BEGIN
        DROP PROCEDURE USP_EX_DelPolicyitemReportingCascade ;
        PRINT 'DROPPED USP_EX_DelPolicyitemReportingCascade ';
    END
GO
   
  
/******************************************************************************                    
** Name : USP_EX_DelPolicyitemReportingCascade
** Old Name:     USP_EIS_EX_POLICYITEM_REPORTING_CASCADE_DelProc                     
** Short Desc: To Insert/Update the Reporting POLICYITEM table for a particular dimension.              
**                    
** Full Description: To Insert/Update the Reporting POLICYITEM table for a particular dimension.              
**                            
 @POLICYDIMENSIONID INT ,  
 @ENTITY_TYPE VARCHAR(100),  
 @ENTITY_ID INT,  
 @USERID INT  
** Sample Call                    
        EXEC USP_EIS_EX_POLICYITEM_REPORTING_CASCADE_DelProc   
    @NEW_VALUE   =  ,  
    @POLICYDIMENSIONID =  ,  
    @ENTITY_TYPE  = 'Client' ,  
    @ENTITY_ID  =  ,  
    @USERID   =   
**                    
** Return values: NONE                    
**                    
**                    
** Standard declarations                    
**       SET LOCK_TIMEOUT         30000   -- 30 seconds                    
**                     
** Created By: Saravanan P Muthu                   
** Company   : Kaspick & Company                    
** Project   : Excelsior                    
** Created DT: Aug 11 2008  
**                                
*******************************************************************************              
**       Change History                    
*******************************************************************************              
** Date:        Author:  Bug #     Description:                           Rvwd              
** --------     -------- ------    -------------------------------------- --------    
** 12 Apr 2014 Yugandhar			EXCREQ 6.1 Modified      
  
*******************************************************************************                    
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved                    
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION                    
*******************************************************************************/                    
CREATE PROCEDURE USP_EX_DelPolicyitemReportingCascade    
(  
 @NEW_VALUE VARCHAR(100),  
 @POLICYDIMENSIONID INT ,  
 @ENTITY_TYPE VARCHAR(100),  
 @ENTITY_ID VARCHAR(14),  
 @USERID INT  
)  
AS  
BEGIN   
 BEGIN TRY  
  DECLARE @POLICYDIMENSIONNAME VARCHAR(100),  
    @POLICYITEM_ID INT,  
    @MAX_POLICY_LEVEL INT,  
    @MIN_RECORDID INT,  
    @MAX_RECORDID INT,  
    @DATA_TYPE VARCHAR(20),  
    @POLICY_LEVEL INT,  
    @VAL_DATE DATETIME,  
    @VAL_LIST INT ,  
    @VAL_NUMERIC FLOAT,  
    @VAL_TEXT VARCHAR(100),   
    @VAL_LOGICAL BIT,  
    @TMP_CL_ID VARCHAR(14),  
    @PRE_DATEVAL DATETIME,  
    @PRE_NUMERICVAL FLOAT,  
    @PRE_LISTVAL INT,  
    @PRE_TEXTVAL VARCHAR(100),  
    @PRE_LOGICALVAL BIT  
     
  --Declare Temp Table  
  DECLARE @TMP_C_ACT TABLE(ACCOUNTID VARCHAR(14)) --Temp Table for Client Account  
  DECLARE @TMP_ACT_EQUAL TABLE(ACCOUNTID VARCHAR(14)) --Temp Table for Client ACCOUNT with equal value  
    
  --get policy dimension details for selected policydimension  
  SELECT @MAX_POLICY_LEVEL=MAXPOLICYLEVEL, @DATA_TYPE=DATATYPE, @POLICYDIMENSIONNAME =FULLNAME  FROM TBL_PolicyDimension 
  WHERE POLICYDIMENSIONID =@POLICYDIMENSIONID  
  
  --If the policy is being changed from Client page/level  
  IF(@ENTITY_TYPE='MANAGER')  
  BEGIN  
   IF @POLICYDIMENSIONID IN (206,264,265)  
   BEGIN  
    INSERT INTO @TMP_C_ACT SELECT ACCOUNTID FROM VW_EX_Account WHERE CLIENTID=@ENTITY_ID AND ACCOUNTTYPE = 'PIF'    
   END  
   ELSE  
   BEGIN  
    INSERT INTO @TMP_C_ACT SELECT ACCOUNTID FROM VW_EX_Account WHERE CLIENTID=@ENTITY_ID  
   END  
   --END HERE  
     
   IF NOT EXISTS(SELECT ACCOUNTID FROM @TMP_C_ACT)    
   BEGIN    
    --Since no account found under this client, return.    
    RETURN(1)    
   END   
  
   --Set all the previous client policy values  
   SELECT @PRE_DATEVAL=DATEVALUE ,  
     @PRE_NUMERICVAL=NUMERICVALUE ,  
     @PRE_LISTVAL=POLICYDROPDOWNID ,  
     @PRE_LOGICALVAL=LOGICALVALUE,  
     @PRE_TEXTVAL=TEXTVALUE  
   FROM TBL_PolicyItem   
   WHERE OWNERID=@ENTITY_ID AND POLICYLEVEL=100 AND POLICYDIMENSIONID=@POLICYDIMENSIONID  
  
   --fetch all the account under matching programs for this client  
   --------------------------------------  
   IF(@DATA_TYPE='DATE')  
   BEGIN  
    INSERT INTO @TMP_ACT_EQUAL (ACCOUNTID)  
     SELECT OWNERID FROM TBL_PolicyItem   
     WHERE  POLICYLEVEL=300   
     AND POLICYDIMENSIONID=@POLICYDIMENSIONID  
     AND DATEDIFF(DAY,DATEVALUE,@PRE_DATEVAL)=0  
     AND OWNERID IN (SELECT ACCOUNTID FROM @TMP_C_ACT)  
   END  
   ELSE IF(@DATA_TYPE='LIST')  
   BEGIN  
    INSERT INTO @TMP_ACT_EQUAL (ACCOUNTID)  
     SELECT OWNERID FROM TBL_PolicyItem   
     WHERE  POLICYLEVEL=300   
     AND POLICYDIMENSIONID=@POLICYDIMENSIONID  
     AND  POLICYDROPDOWNID = @PRE_LISTVAL  
     AND OWNERID IN (SELECT ACCOUNTID FROM @TMP_C_ACT)  
   END  
   ELSE IF(@DATA_TYPE='LOGICAL')  
   BEGIN  
    INSERT INTO @TMP_ACT_EQUAL (ACCOUNTID)  
     SELECT OWNERID FROM TBL_PolicyItem   
     WHERE  POLICYLEVEL=300   
     AND POLICYDIMENSIONID=@POLICYDIMENSIONID  
     AND LOGICALVALUE = @PRE_LOGICALVAL  
     AND OWNERID IN (SELECT ACCOUNTID FROM @TMP_C_ACT)  
     END  
     ELSE IF(@DATA_TYPE='NUMERIC')  
     BEGIN  
    INSERT INTO @TMP_ACT_EQUAL (ACCOUNTID)  
     SELECT OWNERID FROM TBL_PolicyItem   
     WHERE  POLICYLEVEL=300   
     AND POLICYDIMENSIONID=@POLICYDIMENSIONID  
     AND NUMERICVALUE = @PRE_NUMERICVAL  
     AND OWNERID IN (SELECT ACCOUNTID FROM @TMP_C_ACT)  
   END  
   ELSE IF(@DATA_TYPE='TEXT')  
   BEGIN  
    INSERT INTO @TMP_ACT_EQUAL (ACCOUNTID)  
     SELECT OWNERID FROM TBL_PolicyItem   
     WHERE  POLICYLEVEL=300   
     AND POLICYDIMENSIONID=@POLICYDIMENSIONID  
     AND TEXTVALUE = @PRE_TEXTVAL  
     AND OWNERID IN (SELECT ACCOUNTID FROM @TMP_C_ACT)  
   END  
  
   --Update for Client  
   UPDATE TBL_PolicyItem   
   SET DELETEDUSERID=@USERID,  
    MODIFIEDDATE=GETDATE()  
   WHERE POLICYITEMID IN(SELECT POLICYITEMID FROM TBL_PolicyItem   
        WHERE OWNERID=@ENTITY_ID AND POLICYLEVEL=100   
        AND POLICYDIMENSIONID=@POLICYDIMENSIONID)  
  
   --Update for all Client associated to Account   
   UPDATE TBL_PolicyItem  
   SET DELETEDUSERID=@USERID,  
    MODIFIEDDATE=GETDATE()  
   WHERE POLICYITEMID IN (SELECT POLICYITEMID FROM TBL_PolicyItem   
        WHERE OWNERID IN (SELECT ACCOUNTID FROM @TMP_ACT_EQUAL)  
        AND POLICYLEVEL=300 AND POLICYDIMENSIONID=@POLICYDIMENSIONID)  
  
  
 
   DELETE FROM TBL_PolicyItem  
    WHERE POLICYITEMID IN(SELECT POLICYITEMID FROM TBL_PolicyItem   
         WHERE OWNERID=@ENTITY_ID AND POLICYLEVEL=100   
         AND POLICYDIMENSIONID=@POLICYDIMENSIONID)  
      
    DELETE FROM TBL_PolicyItem  
    WHERE POLICYITEMID IN(SELECT POLICYITEMID FROM TBL_PolicyItem   
         WHERE OWNERID IN((SELECT ACCOUNTID FROM @TMP_ACT_EQUAL))  
         AND POLICYLEVEL=300 AND POLICYDIMENSIONID=@POLICYDIMENSIONID)  
  
    DELETE FROM TBL_PolicyItem  
    WHERE OWNERID=@ENTITY_ID AND POLICYLEVEL=100 AND POLICYDIMENSIONID=@POLICYDIMENSIONID   
  
    DELETE FROM TBL_PolicyItem  
    WHERE OWNERID IN((SELECT ACCOUNTID FROM @TMP_ACT_EQUAL))  
    AND POLICYLEVEL=300 AND POLICYDIMENSIONID=@POLICYDIMENSIONID  
     
  END  
  ELSE IF(@ENTITY_TYPE='ACCOUNT')  
  BEGIN  
   SELECT @TMP_CL_ID =CLIENTID FROM VW_EX_Account WHERE ACCOUNTID=@ENTITY_ID  
   --Check for Client value  
   IF EXISTS(SELECT POLICYITEMID FROM TBL_PolicyItem WHERE OWNERID=@TMP_CL_ID AND POLICYLEVEL=100 AND POLICYDIMENSIONID=@POLICYDIMENSIONID)  
   BEGIN  
    SELECT   
     @VAL_DATE=DATEVALUE ,  
     @VAL_NUMERIC=NUMERICVALUE ,  
     @VAL_LIST=POLICYDROPDOWNID ,  
     @VAL_LOGICAL=LOGICALVALUE ,  
     @VAL_TEXT=TEXTVALUE   
    FROM TBL_PolicyItem   
    WHERE OWNERID=@TMP_CL_ID AND POLICYLEVEL=100 AND POLICYDIMENSIONID=@POLICYDIMENSIONID      
   END  
    
   ELSE  
   BEGIN  
    EXEC USP_EX_UpdPolicyItemReportingCascade  
      @NEW_VALUE   = @NEW_VALUE,   
      @POLICYDIMENTISON = @POLICYDIMENSIONNAME,   
      @ENTITY_TYPE  = @ENTITY_TYPE,   
      @ENTITY_ID   = @ENTITY_ID,   
      @USERID    = @USERID     
   END  
   --get policyitem id for selected account  
   SELECT  @POLICYITEM_ID=POLICYITEMID FROM TBL_PolicyItem   
   WHERE OWNERID = @ENTITY_ID AND POLICYLEVEL=300 AND POLICYDIMENSIONID=@POLICYDIMENSIONID  
  
   --update policy item supplement  
   UPDATE TBL_PolicyItem  
   SET MODIFIEDUSERID=@USERID,  
    MODIFIEDDATE=GETDATE()  
   WHERE POLICYITEMID=@POLICYITEM_ID   
  
  
   IF(@DATA_TYPE='DATE')  
   BEGIN  
    UPDATE TBL_PolicyItem SET DATEVALUE=@VAL_DATE WHERE POLICYITEMID=@POLICYITEM_ID  
   END  
   ELSE IF(@DATA_TYPE='LIST')  
   BEGIN  
    UPDATE TBL_PolicyItem SET POLICYDROPDOWNID=@VAL_LIST WHERE POLICYITEMID=@POLICYITEM_ID  
   END  
   ELSE IF(@DATA_TYPE='LOGICAL')  
   BEGIN  
    UPDATE TBL_PolicyItem SET LOGICALVALUE=@VAL_LOGICAL WHERE POLICYITEMID=@POLICYITEM_ID  
   END  
   ELSE IF(@DATA_TYPE='NUMERIC')  
   BEGIN  
    UPDATE TBL_PolicyItem SET NUMERICVALUE=@VAL_NUMERIC WHERE POLICYITEMID=@POLICYITEM_ID  
   END  
   ELSE IF(@DATA_TYPE='TEXT')  
   BEGIN  
    UPDATE TBL_PolicyItem SET TEXTVALUE=@VAL_TEXT WHERE POLICYITEMID=@POLICYITEM_ID  
   END   
     
  END  
 END TRY  
 BEGIN CATCH  
  RAISERROR 1000092 'USP_EX_DelPolicyitemReportingCascade: Cannot update  in TBL_POLICYITEM table'              
 END CATCH  
END  




  GO
  IF EXISTS (	SELECT *
			FROM sysobjects
			WHERE type = 'P'
			AND name = 'USP_EX_DelPolicyitemReportingCascade') 
	BEGIN
			PRINT 'CREATED PROCEDURE USP_EX_DelPolicyitemReportingCascade';
	END  