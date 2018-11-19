IF EXISTS (SELECT *
           FROM   sysobjects 
           WHERE  type = 'P'
                  AND name = 'USP_EX_GetAccountProfileDetails')
    BEGIN
        DROP PROCEDURE USP_EX_GetAccountProfileDetails;
        PRINT 'DROPPED USP_EX_GetAccountProfileDetails';
    END
GO

/***************************************************************                    
** Name : USP_EX_GetAccountProfileDetails
** Old Name:     Did'nt Exist                
** Short Desc: To get account profile in account-investment-account profile details           
**                    
** Full Description: To get account profile in account-investment-account profile details          
**                            
** Input Arguments: CustomerAccountNo  
**       
** Sample Call                    
**  EXEC USP_EX_GetAccountProfileDetails  'DUNEG'
   @ObjectiveCode = 'DUNEG'  
     
**           
** Return values: NONE  
**                    
**                    
** Standard declarations                    
**       SET LOCK_TIMEOUT         30000   -- 30 seconds                    
**                     
** Created By: Mallikarjun
** Company   : Opteamix                   
** Project   : Excelsior Prime
** Created DT: 19/Apr/2014                 
**                                
*******************************************************************************              
**       Change History                    
*******************************************************************************              
** Date:        Author:  Bug #     Description:                           Rvwd              
** --------  -------- ------    -------------------------------------- --------              
** <MM/DD/YYYY>  
**04/22/2014  Mallikarjun  INVREQ2.2 Created for Account-Investment-AccountProfile tab
** 23/05/2014	Mallikarjun		SP Name Renamed and Formatted
*******************************************************************************                    
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved                    
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION                    
*******************************************************************************/     
             
CREATE PROCEDURE [dbo].[USP_EX_GetAccountProfileDetails] --'AZBIN'--'352L418'
(  
 @CustomerAccountNumber char(14)
)  
AS              
BEGIN 

Declare @HORIZON FLOAT
Declare @HORIZONSTR VARCHAR(20)
SET @HORIZON=0

DECLARE @AsOnDate DATE
SELECT @AsOnDate=GETDATE()

EXEC USP_EX_CalculateExpectedHorizon 
@CustomerAccountNumber,
@AsOnDate,
@HORIZON  output


IF(@HORIZON=0 OR @HORIZON IS NULL)
	BEGIN
		SET @HORIZONSTR='Could not Calc'
	END
ELSE
	BEGIN
		SET @HORIZONSTR=@HORIZON
	END


DECLARE @UpiaDesc varchar(40)

SELECT @UpiaDesc=ISNULL(CAST(UTablDef.TableDesc AS VARCHAR(40)), '')
FROM SYN_IT_AccountMaster AcntMstr
INNER JOIN SYN_IT_UDF_AccountMaster UAcntMstr ON UAcntMstr.CustomerAccountNumber_Key = AcntMstr.CustomerAccountNumber
LEFT OUTER JOIN SYN_IT_UDFTableDefinitions UTablDef ON UTablDef.TableCode = UAcntMstr.UDFAMColumn035 
LEFT OUTER JOIN SYN_IT_UDFColumnDefinitions UColDef ON UColDef.TableID = UTablDef.TableID 
WHERE UColDef.ColumnName= 'UDFAMColumn035' and CustomerAccountNumber=@CustomerAccountNumber

DECLARE @RealizedSTGLTG varchar(40)

SELECT @RealizedSTGLTG=ISNULL(CAST(UTablDef.TableDesc AS VARCHAR(40)), '')
FROM SYN_IT_AccountMaster AcntMstr
INNER JOIN SYN_IT_UDF_AccountMaster UAcntMstr ON UAcntMstr.CustomerAccountNumber_Key = AcntMstr.CustomerAccountNumber
LEFT OUTER JOIN SYN_IT_UDFTableDefinitions UTablDef ON UTablDef.TableCode = UAcntMstr.UDFAMColumn038 
LEFT OUTER JOIN SYN_IT_UDFColumnDefinitions UColDef ON UColDef.TableID = UTablDef.TableID 
WHERE UColDef.ColumnName= 'UDFAMColumn038' and CustomerAccountNumber=@CustomerAccountNumber     

DECLARE @PostGiftGain varchar(40)

SELECT @PostGiftGain=ISNULL(CAST(UTablDef.TableDesc AS VARCHAR(40)), '')
FROM SYN_IT_AccountMaster AcntMstr
INNER JOIN SYN_IT_UDF_AccountMaster UAcntMstr ON UAcntMstr.CustomerAccountNumber_Key = AcntMstr.CustomerAccountNumber
LEFT OUTER JOIN SYN_IT_UDFTableDefinitions UTablDef ON UTablDef.TableCode = UAcntMstr.UDFAMColumn016
LEFT OUTER JOIN SYN_IT_UDFColumnDefinitions UColDef ON UColDef.TableID = UTablDef.TableID 
WHERE UColDef.ColumnName= 'UDFAMColumn016' and CustomerAccountNumber=@CustomerAccountNumber 


SELECT 
AccMstr.CustomerDescriptionLine1 AS AccountName,

CASE AccMstr.AccountTypeCode
		WHEN 'CRUT' THEN CtAccDtls.PAYOUTPERCENTAGE
		WHEN 'NIMU' THEN CtAccDtls.PAYOUTPERCENTAGE
		WHEN 'NICT' THEN CtAccDtls.PAYOUTPERCENTAGE
		ELSE NULL
	  
	END AS UniTrustPerc,
--CtAccDtls.PAYOUTPERCENTAGE AS UniTrustPerc,
CtAccDtls.CYDistAmount AS AnnuityAmt,
@HORIZONSTR AS ExpectedHorizon,
	CASE UdfAccMstr.UDFAMColumn006 
		WHEN 'Y' THEN 'Yes' 
		WHEN 'N' THEN 'No' 
	END AS FlipProvision,
PreCloseDate As AccountNotificationDate, 
AccMstr.ClosedDate As AccountClosureDate,
UdfAccMstr.UDFAMColumn030 AS MatureDate, 
@UpiaDesc AS UpiaDesc,
UdfAccMstr.UDFAMColumn014 AS IncludeSGD,
UdfAccMstr.UDFAMColumn012 AS IncludeLGD,
UdfAccMstr.UDFAMColumn015 AS IncludeSTG,
UdfAccMstr.UDFAMColumn013 AS IncludeLTG,
@RealizedSTGLTG AS RealizedSTGLTG,
@PostGiftGain AS PostGiftGain
FROM SYN_IT_AccountMaster AccMstr
INNER JOIN SYN_IT_ContactMaster ConMstr ON AccMstr.OwnerContactID=ConMstr.CONTACTID
INNER JOIN SYN_IT_CTAccountDetails CtAccDtls ON AccMstr.CustomerAccountNumber=CtAccDtls.CustomerAccountNumber
INNER JOIN SYN_IT_UDF_AccountMaster UdfAccMstr ON AccMstr.CustomerAccountNumber=UdfAccMstr.CustomerAccountNumber_Key
LEFT JOIN SYN_IT_AccountCloseSettings AccCloseStngs ON AccMstr.CustomerAccountNumber=AccCloseStngs.CustomerAccountNumber
WHERE AccMstr.CustomerAccountNumber=@CustomerAccountNumber
	
END
GO
IF EXISTS (SELECT *
           FROM   sysobjects 
           WHERE  type = 'P'
                  AND name = 'USP_EX_GetAccountProfileDetails')
    BEGIN
       
        PRINT 'CREATED USP_EX_GetAccountProfileDetails';
    END 