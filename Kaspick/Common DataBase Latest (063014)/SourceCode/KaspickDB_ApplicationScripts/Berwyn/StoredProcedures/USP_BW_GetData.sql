/****** Object:  StoredProcedure [dbo].[USP_BW_GetData]    Script Date: 05/02/2014 15:41:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_BW_GetData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_BW_GetData]
GO

/****** Object:  StoredProcedure [dbo].[USP_BW_GetData]    Script Date: 05/02/2014 15:41:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
  
/********************************************************************************************************************              
* Procedure Name  : USP_BW_GetData          
* Purpose         :     
* Input Parameter :        
*                 : 
*                 : 
*            
* Modification Log               
* Date       Modified By         Description              
*--------------------------------------------------------------------------------------------------------------------              
* 
* 
*--------------------------------------------------------------------------------------------------------------------              
*             Exec USP_BW_GetData  
*--------------------------------------------------------------------------------------------------------------------              
*********************************************************************************************************************/

CREATE PROCEDURE [dbo].[USP_BW_GetData]    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
SET NOCOUNT ON;    
    
SELECT  a.CustomerShortName 'ORGANIZATION_SHORT_NAME' ,
        am.ManagerName 'ORGANIZATION_FULL_NAME' ,
        rtrim(A.CustomerAccountNumber) +  ' ('
        + CASE WHEN crc.ContactRoleCodeDesc = 'Donor' THEN 'Donor'
               WHEN crc.ContactRoleCodeDesc = 'Beneficiary' THEN 'Bene'
               WHEN crc.ContactRoleCodeDesc = 'Contingent Beneficiary' THEN 'Cont-Bene'
               WHEN crc.ContactRoleCodeDesc = 'Proxy Recipient' THEN 'Proxy'
               ELSE crc.ContactRoleCodeDesc
          END + ')' AS 'ACCOUNT_NAME' ,
          CM.SSN AS 'TAX_ID'  , 
          ISNULL(cm.PrimaryLastName,'')  AS 'LAST_NAME' ,
          ISNULL(cm.PrimaryFirstName,'')  AS 'FIRST_NAME' ,
          ISNULL(cm.PrimaryMIDDLEINITIAL,'') AS 'MIDDLE_INITIAL' ,
          ISNULL(CONVERT(VARCHAR, cm.DateOfBirth, 101), '') AS DATE_OF_BIRTH
FROM    dbo.SYN_IT_ContactAccountRoles CAR
	   INNER JOIN dbo.SYN_IT_AccountMaster A ON A.CustomerAccountNumber=CAR.CustomerAccountNumber
        INNER JOIN dbo.SYN_IT_ContactRoleCodes CRC ON CAR.ContactRoleCode = CRC.ID
        INNER JOIN dbo.SYN_IT_AccountManagerCodes AM ON am.ManagerCode = a.ManagerCode
        INNER JOIN dbo.SYN_IT_CONTACTMASTER CM ON CAR.CONTACTID  = CM.CONTACTID
        INNER JOIN dbo.syn_it_UDF_AccountMaster UAM on A.CustomerAccountNumber=UAM.CustomerAccountNumber_key
WHERE   
	   crc.ContactRoleCodeDesc IN ('Donor', 'Beneficiary' ,'Contingent Beneficiary','Proxy Recipient')
	   and CM.dateofdeath IS NULL
        AND UAM.UDFAMColumn030 IS NULL
        AND CM.SSNFlag <> 1
        AND CM.SSNFlag <> 2 -- added this condition to fix ET ISSUE # 12253
        AND ( (
			crc.ContactRoleCodeDesc = 'Donor' AND 
              (CM.SSN <> '000000000' or CM.SSN <> '999999999' )
               )
            )
        ORDER BY A.customerAccountNumber
END 