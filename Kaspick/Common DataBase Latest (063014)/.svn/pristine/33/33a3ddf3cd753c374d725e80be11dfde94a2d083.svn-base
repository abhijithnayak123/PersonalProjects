 IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_CalculateExpectedHorizon'
		)
BEGIN
	DROP PROCEDURE USP_EX_CalculateExpectedHorizon;

	PRINT 'DROPPED USP_EX_CalculateExpectedHorizon';
END
GO  
/******************************************************************************        
** New Name:  USP_EX_CalculateExpectedHorizon  
** Old Name:     USP_CRUT_CRAT_CALCULATE_HORIZON         
** Short Desc: Calculate the horizon based Annuity 2000 table for a given account        
**        
** Full Description        
**                
**        
** Sample Call        
	declare @horizon float      
	exec USP_EX_CalculateExpectedHorizon 'gaaal','05/31/2014',@horizon
	select @horizon
**        
** Return values: NONE        
**        
**        
** Standard declarations        
**       SET LOCK_TIMEOUT         30000   -- 30 seconds        
**         
** Created By: Abhsihek Chadha        
** Company   : Kaspick & Company        
** Project   : CRUT/CRAT Audit Load    
** Created DT: 06/019/2009        
**                    
*******************************************************************************        
**       Change History        
*******************************************************************************        
** Date:        Author:  Bug #     Description:                           Rvwd        
** --------     -------- ------    -------------------------------------- --------        
**  4/21/2014   Mallikarjun  INVREQ2.3  Modified   
*******************************************************************************        
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved        
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION        
*******************************************************************************/  
CREATE PROCEDURE [dbo].[USP_EX_CalculateExpectedHorizon]  
 @CustomerAccountNumber CHAR(14)  
 ,@REPORT_DATE DATETIME  
 ,@HORIZON FLOAT OUTPUT  
AS  
BEGIN  
 SET NOCOUNT ON;  
  
 DECLARE @ACCOUNT_TYPE VARCHAR(20)  
 DECLARE @EXPECTED_MATURE_DATE DATETIME  
 DECLARE @TERM_YEARS_BEYOND_LIFE TINYINT  
 DECLARE @TERMINATION_TYPE VARCHAR(1)  
 DECLARE @ALL_DOB_NULL INT  
 DECLARE @MIN_DOB DATETIME  
 DECLARE @maxDOD DATETIME  
 DECLARE @HORIZON_DAYS FLOAT  
 DECLARE @HORIZON_CASE_L FLOAT  
 DECLARE @HORIZON_CASE_T FLOAT  
 DECLARE @ANNUITY2000_CALCULATED_HORIZON FLOAT  
 
  CREATE TABLE [#AccountTypes] (  
	AccountTypeCode char(4)
  )  
  
 INSERT INTO #AccountTypes(AccountTypeCode)
 SELECT 'GAP'
 UNION
 SELECT 'GAPR'
 UNION
 SELECT 'GAPP'
 UNION
 SELECT 'PIF'
 UNION
 SELECT 'CORP'
 UNION
 SELECT 'END'
 UNION
 SELECT 'EST'
 UNION
 SELECT 'ENDQ'
 UNION
 SELECT 'ENDT'
 UNION
 SELECT 'DAF'
 UNION
 SELECT 'DDF'
 UNION
 SELECT 'QPE'
 UNION
 SELECT 'QPNE'
 UNION
 SELECT 'NQP'
 UNION
 SELECT 'EMP'
 UNION
 SELECT 'OPS'
 UNION
 SELECT 'POOL'

 SET @HORIZON_CASE_L = 0.0  
 SET @HORIZON_CASE_T = 0.0  
 SET @HORIZON = 0.0  
 SET @ANNUITY2000_CALCULATED_HORIZON = 0.0  
  
 /*Get the account details*/  
 SELECT @ACCOUNT_TYPE = AccountTypeCode  
  ,@EXPECTED_MATURE_DATE = ProjectedAccountCloseDate  
  ,@TERM_YEARS_BEYOND_LIFE = UDFAMColumn018  
  ,@TERMINATION_TYPE = UDFAMColumn020  
 FROM SYN_IT_AccountMaster AccMstr  
 INNER JOIN SYN_IT_UDF_AccountMaster UdfAccMstr ON AccMstr.CustomerAccountNumber = UdfAccMstr.CustomerAccountNumber_Key  
 WHERE AccMstr.CustomerAccountNumber = @CustomerAccountNumber   
   
 /***START- CALCULATE THE @HORIZON_CASE_T FOR THE CASE WHERE TERMINATIONTYPE = T*/  
 SELECT @HORIZON_CASE_T = CAST(DATEDIFF(DAY, @REPORT_DATE, @EXPECTED_MATURE_DATE) AS FLOAT) / 365  
 /***ENDS- CALCULATE THE @HORIZON_CASE_T FOR THE CASE WHERE TERMINATIONTYPE = T*/  
  
 /***START CALCULATE THE @HORIZON_CASE_L FOR THE CASE WHERE TERMINATIONTYPE = L*/  
 IF EXISTS (  
   SELECT *  
   FROM TEMPDB..sysobjects  
   WHERE ID = OBJECT_ID(N'TEMPDB..[#TMP_TBL_BENEFICIARY_DOB]')  
   )  
  DROP TABLE [#TMP_TBL_BENEFICIARY_DOB]  
  
 CREATE TABLE [#TMP_TBL_BENEFICIARY_DOB] (  
  [ACCOUNTID] CHAR(14) NOT NULL  
  ,[PARTICIPANTID] INT NOT NULL  
  ,[BENEFICIARYID] INT NOT NULL  
  ,[BIRTHDATE] DATETIME NULL  
  ,[EXPIREDATE] DATETIME NULL  
  ,[AGE] INT NULL  
  )  
  
 /*    
 **Condition 1- For Given account get BirthDate for all beneficiaries where ExcludeFromLifeOfTrust = 0     
 **and (ExpireDate > ReportDate or ExpireDate is Null)    
 */  
 INSERT INTO #TMP_TBL_BENEFICIARY_DOB (  
  ACCOUNTID  
  ,PARTICIPANTID  
  ,BENEFICIARYID  
  ,BIRTHDATE  
  ,EXPIREDATE  
  ,AGE  
  )  
 SELECT AccMstr.CustomerAccountNumber AS ACCOUNTID  
  ,ConMstr.ContactID AS PARTICIPANTID  
  ,ConMstr.ContactID AS BENEFICIARYID  
  ,ConMstr.DateOfBirth AS BIRTHDATE  
  ,ConMstr.DateOfDeath AS EXPIREDATE  
  ,NULL AS AGE  
 FROM dbo.SYN_IT_ContactMaster AS ConMstr  
 INNER JOIN dbo.SYN_IT_ContactAccountRoles AS ConAccRol ON ConAccRol.ContactId = ConMstr.ContactId  
 INNER JOIN dbo.SYN_IT_ContactRoleCodes AS ConRolCds ON ConRolCds.Id = ConAccRol.ContactRoleCode  
  AND ConRolCds.ID IN (  
   21  
   --,10 No proxies
   --,24 --Donors should not be considered 
   --,37 --Contingent beneficiaries should not be considered  
   )  
 INNER JOIN dbo.SYN_IT_AccountMaster AS AccMstr ON AccMstr.CustomerAccountNumber = ConAccRol.CustomerAccountNumber  
 WHERE (  
   ConMstr.DateOfDeath > @REPORT_DATE  
   OR ConMstr.DateOfDeath IS NULL  
   OR ConMstr.DateOfDeath = '01/01/1900'  
   )  
   AND AccMstr.CustomerAccountNumber = @CustomerAccountNumber   
   AND AccMstr.AccountTypeCode NOT IN (SELECT AccountTypeCode from  #AccountTypes) 
    
  
 /***check if any record exists as per condition one and if so then check if     
  birth dates for all the beneficiar are null or not*/  
 IF EXISTS (  
   SELECT *  
   FROM #TMP_TBL_BENEFICIARY_DOB  
   )  
 BEGIN  
  IF EXISTS (  
    SELECT *  
    FROM #TMP_TBL_BENEFICIARY_DOB  
    WHERE BIRTHDATE IS NOT NULL  
    )  
   SET @ALL_DOB_NULL = 0 --DOB for SOME the beneficiaries is NULL    
  ELSE  
   SET @ALL_DOB_NULL = 1 --DOB for ALL the beneficiaries is NULL    
 END  
 ELSE  
 BEGIN  
  SET @ALL_DOB_NULL = 2 --There were NO beneficiaries found as per the Condition 1    
 END  
  
 /*If BirthDate is Null for one beneficiary but not all in a given account,     
 **assume age is equal to minimum age for other beneficiaries in this account.    
 **Mark Bday field with an "(M)" to signify missing date.    
 */  
 IF @ALL_DOB_NULL = 0  
 BEGIN  
  SELECT @MIN_DOB = MIN(BIRTHDATE)  
  FROM #TMP_TBL_BENEFICIARY_DOB  
  WHERE BIRTHDATE IS NOT NULL  
  
  UPDATE #TMP_TBL_BENEFICIARY_DOB  
  SET BIRTHDATE = @MIN_DOB  
  WHERE BIRTHDATE IS NULL  
 END  
  
 /*    
 **IF beneficiaries were found as per condition1 then convert BirthDate for each beneficiary to age     
 **in integer format (round down to nearest year) as of REPORTDATE    
 */  
 IF @ALL_DOB_NULL <> 2  
 BEGIN  
  UPDATE #TMP_TBL_BENEFICIARY_DOB  
  SET AGE = CASE   
    WHEN MONTH(BIRTHDATE) > MONTH(@REPORT_DATE)  
     THEN (YEAR(@REPORT_DATE) - YEAR(BIRTHDATE) - 1)  
    WHEN MONTH(BIRTHDATE) < MONTH(@REPORT_DATE)  
     THEN (YEAR(@REPORT_DATE) - YEAR(BIRTHDATE))  
    WHEN MONTH(BIRTHDATE) = MONTH(@REPORT_DATE)  
     AND DAY(BIRTHDATE) > DAY(@REPORT_DATE)  
     THEN YEAR(@REPORT_DATE) - YEAR(BIRTHDATE) - 1  
    WHEN MONTH(BIRTHDATE) = MONTH(@REPORT_DATE)  
     AND DAY(BIRTHDATE) < DAY(@REPORT_DATE)  
     THEN YEAR(@REPORT_DATE) - YEAR(BIRTHDATE)  
    WHEN MONTH(BIRTHDATE) = MONTH(@REPORT_DATE)  
     AND DAY(BIRTHDATE) = DAY(@REPORT_DATE)  
     THEN YEAR(@REPORT_DATE) - YEAR(BIRTHDATE)  
    ELSE NULL  
    END  
 END  
  
 /*** added the delete on 6/30 to exclude the beneficiaries less than 5 years ***/  
 DELETE  
 FROM #TMP_TBL_BENEFICIARY_DOB  
 WHERE AGE < 5  
  
 /***STARTS - Calculate the horizon for the beneficiaries in the #TMP_TBL_BENEFICIARY_DOB table as per the TBL_ANNUITY2000 table*/  
 IF EXISTS (  
   SELECT *  
   FROM TEMPDB..sysobjects  
   WHERE ID = OBJECT_ID(N'TEMPDB..[#TMP_TBL_AGE_PROBABILITY]')  
   )  
  DROP TABLE [#TMP_TBL_AGE_PROBABILITY]  
  
 CREATE TABLE [#TMP_TBL_AGE_PROBABILITY] (  
  [BENEFICIARYID] INT NOT NULL  
  ,[AGE] INT NULL  
  ,[PROBABILITYALIVE] FLOAT NULL  
  ,[ROW_NO] BIGINT NULL  
  )  
  
 INSERT INTO #TMP_TBL_AGE_PROBABILITY  
 SELECT [BENEFICIARYID]  
  ,A.AGE  
  ,1 - (  
   A.ALIVE / (  
    SELECT ALIVE  
    FROM TBL_IE_Annuity2000 A2  
    WHERE A2.AGE = B.AGE  
    )  
   )  
  ,ROW_NUMBER() OVER (  
   PARTITION BY BENEFICIARYID ORDER BY [BENEFICIARYID]  
    ,A.AGE  
   )  
 FROM #TMP_TBL_BENEFICIARY_DOB B  
 INNER JOIN TBL_IE_Annuity2000 A ON A.Age > B.AGE  
   
 --INSERT INTO #TMP_TBL_AGE_PROBABILITY    
 -- SELECT [BENEFICIARYID],A.AGE, 1-(A.ALIVE/(SELECT ALIVE FROM TBL_IE_Annuity2000 A2 WHERE A2.AGE = B.AGE)),    
 -- ROW_NUMBER() OVER(PARTITION BY BENEFICIARYID ORDER BY  [BENEFICIARYID],A.AGE)    
 -- FROM #TMP_TBL_BENEFICIARY_DOB B    
 --   JOIN TBL_IE_Annuity2000 A ON A.Age > B.AGE    
 SELECT @ANNUITY2000_CALCULATED_HORIZON = @ANNUITY2000_CALCULATED_HORIZON + (  
   1 - (  
    ISNULL(CASE   
      WHEN MAX(CASE   
         WHEN PROBABILITYALIVE = 0  
          THEN 1  
         END) = 1  
       THEN 0  
      ELSE CASE   
        WHEN COUNT(CASE   
           WHEN PROBABILITYALIVE < 0  
            THEN 1  
           END) % 2 = 0  
         THEN 1  
        ELSE - 1  
        END * EXP(SUM(LOG(NULLIF(ABS(PROBABILITYALIVE), 0))))  
      END, 1)  
    )  
   )  
 FROM #TMP_TBL_AGE_PROBABILITY AP  
 GROUP BY ROW_NO  
  
 /***ENDS - Calculate the horizon for the beneficiaries in the #TMP_TBL_BENEFICIARY_DOB table as per the TBL_ANNUITY2000 table*/  
 IF @ALL_DOB_NULL = 1 --DOB for ALL the beneficiaries is NULL=== mark horizon as n/a    
 BEGIN  
  SET @HORIZON_CASE_L = 99999.0  
 END  
 ELSE IF @ALL_DOB_NULL = 2 --There were NO beneficiaries found as per the Condition 1    
 BEGIN  
  /*    
   **If no beneficiaries meet condition 1 then add TermYearsBeyondLife to ExpireDate of most     
   **recently deceased beneficiary where ExcludeFromLifeOfTrust = 0    
   */  
  SELECT @maxDOD = max(ISNULL(ConMstr.DateOfDeath, '01/01/1900'))  
  FROM dbo.SYN_IT_ContactMaster AS ConMstr  
  INNER JOIN dbo.SYN_IT_ContactAccountRoles AS ConAccRol ON ConAccRol.ContactId = ConMstr.ContactId  
   AND ConAccRol.ContactRoleCode IN (  
    21  
    --,10 No proxies
    --,24 --Donors should not be considered  
    --,37 --Contingent Beneficiaries should not be considered  
    )  
  INNER JOIN dbo.SYN_IT_AccountMaster AS AccMstr ON AccMstr.CustomerAccountNumber = ConAccRol.CustomerAccountNumber  
  WHERE (  
    ConMstr.DateOfDeath > @REPORT_DATE  
    OR ConMstr.DateOfDeath IS NULL  
    OR ConMstr.DateOfDeath = '01/01/1900'  
    )  
   AND AccMstr.AccountTypeCode NOT IN (SELECT AccountTypeCode from  #AccountTypes)
  
  /*Add Term of Years Beyond Life to the Most recent  date of death for bene to get the termination date.       
   **And Then subtract this from the Report date to get the horizon      
   */  
  SET @HORIZON_DAYS = datediff(day, @REPORT_DATE, DATEADD(YEAR, @TERM_YEARS_BEYOND_LIFE, @maxDOD))  
  SET @HORIZON_CASE_L = round(@HORIZON_DAYS / 365, 1)  
 END  
 ELSE  
 BEGIN  
  SET @HORIZON_CASE_L = ROUND(@ANNUITY2000_CALCULATED_HORIZON, 1) + @TERM_YEARS_BEYOND_LIFE  
 END  
  
 /***ENDS- CALCULATE THE @HORIZON_CASE_L FOR THE CASE WHERE TERMINATIONTYPE = L*/
  --adding 0.5 from the calculated horizon to make up of a bug in the Annuity 200 formula  
 SELECT @HORIZON_CASE_L =@HORIZON_CASE_L+0.5    

 /***Assign the correct horizon to @HORIZON variable based upon the Terminaton Type code (@TERMINATION_TYPE)*/  
 IF @TERMINATION_TYPE = 'L'  
 BEGIN  
  SET @HORIZON = @HORIZON_CASE_L  
 END  
  
 IF @TERMINATION_TYPE = 'T'  
 BEGIN  
  SET @HORIZON = ROUND(@HORIZON_CASE_T, 1)  
 END  
  
 IF @TERMINATION_TYPE = 'F'  
 BEGIN  
  IF (@HORIZON_CASE_T < @HORIZON_CASE_L)  
   SET @HORIZON = ROUND(@HORIZON_CASE_T, 1)  
  ELSE  
   SET @HORIZON = ROUND(@HORIZON_CASE_L, 1)  
 END  
  
 IF @TERMINATION_TYPE = 'S'  
 BEGIN  
  IF (@HORIZON_CASE_T > @HORIZON_CASE_L)  
   SET @HORIZON = ROUND(@HORIZON_CASE_T, 1)  
  ELSE  
   SET @HORIZON = ROUND(@HORIZON_CASE_L, 1)  
 END  
  
 SET @HORIZON = CASE   
   WHEN ISNULL(@HORIZON, 0.0) < 0  
    THEN 0.0  
   ELSE ISNULL(@HORIZON, 0.0)  
   END  
    
END  
GO
SET NOCOUNT OFF;

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_CalculateExpectedHorizon'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_CalculateExpectedHorizon';
END