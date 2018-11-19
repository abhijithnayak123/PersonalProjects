IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetAccountPrgVal'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetAccountPrgVal;

	PRINT 'DROPPED USP_EX_GetAccountPrgVal';
END
GO

/****** Object:  StoredProcedure [dbo].[USP_EX_GetAccountPrgVal]    Script Date: 06/28/2014 10:26:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/*********************************************************************************************************************                      
* Procedure Name : USP_EX_GetAccountPrgVal
* Old Procedure Name  : USP_EIS_EX_ACCOUNT_PRG_VALSelProc
* Description     : To CHECK client validity                       
* Input           : @PROGRAM_BN,  @USER_ID                      
*                                                  
* Modification Log: Changed the parameter Program_id. New Parameter is  @PROGRAM_BN VARCHAR(25)                     
*                                                  
* Date       Created By  Description                                                                  
*--------------------------------------------------------------------------------------------------------------------                      
* 14-DEC-06  TANUJ		Created.                      
* 21-May-07  Ganapati	Removed delete flag 
* 31-Oct-07	 Chirag		Added block of code under the section where 'Return Value' is being returned as 2
*********************************************************************************************************************/                      
CREATE PROCEDURE [dbo].[USP_EX_GetAccountPrgVal] --'ACS',68          
(            
 @PROGRAM_BN VARCHAR(25),             
 @USER_ID INT,  
 @ADVENTID VARCHAR(15) =NULL          
)                      
AS                      
BEGIN            
          
   DECLARE @STATUS_LI_ID INT ,            
   @STATUS_ID INT ,            
   @DELETE_FLAG INT            
             
 -- GET STATUS LIST ITEM ID             
-- EXEC USP_EIS_EX_LIST_ITEM_ID_SelProc                
--  @LIST_TYPE_NAME = 'STATUS',              
--  @LIST_ITEM_NAME = 'ACTIVE',              
--  @LIST_ITEM_ID   = @STATUS_ID OUTPUT             
          
	-- CHECK FOR RECORD EXITS IN PROGRAM SUPPLEMENT         
	-- if nit return 1
	IF NOT EXISTS(SELECT AlliNum.AllianceNumber
		FROM SYN_IT_AllianceNumbers AlliNum 
		WHERE AlliNum.AllianceNumber=@PROGRAM_BN)          
	BEGIN           
		SELECT 1 AS RET_VAL , 0 AS PROGRAMID, '' AS BRIEFNAME                  
		RETURN(-1)            
	END          
          
	-- CHECK FOR RECORD EXITS IN PROGRAM SUPPLEMENT WITH  STATUS NOT 'INACTIVE'            
	-- IF NOT RETURN 3          
	IF NOT EXISTS(SELECT AlliNum.AllianceNumber           
				  FROM SYN_IT_AllianceNumbers AlliNum
				  WHERE AlliNum.AllianceNumber=@PROGRAM_BN 
				 )          
	BEGIN           
		SELECT 3 AS RET_VAL , 0 AS PROGRAMID, '' AS BRIEFNAME                  
		RETURN(-1)
	END
          
	-- CHECK FOR RECORD EXITS IN PROGRAM SUPPLEMENT WITH  STAFFROLE TABLE HAS RECORD FOR             
	-- THIS PROGRAMID AS CLIENTID AND LOGGED IN USER ID            
	-- IF NOT RETURN 2
	IF NOT EXISTS(SELECT AlliNum.AllianceNumber           
		FROM SYN_IT_AllianceNumbers AlliNum          
		 INNER JOIN SYN_IT_ContactMaster KCoStfConMstr ON KCoStfConMstr.ContactID =AlliNum.ContactID          
		 INNER JOIN SYN_IT_SubContactRoles subConRol ON subConRol.ContactID = KCoStfConMstr.ContactID        
		 INNER JOIN TBL_KS_User KsUsr ON KsUsr.InnotrustContactID = KCoStfConMstr.contactID     
		WHERE AlliNum.AllianceNumber =@PROGRAM_BN AND KsUsr.USERID=@USER_ID)          
	BEGIN           
		--SELECT 2 AS RET_VAL , 0 AS PROGRAMID, '' AS BRIEFNAME
		DECLARE @IS_ADVENTID_UNIQUE INT
		SET @IS_ADVENTID_UNIQUE = 1
		-->FIRST CHECK (BEFORE SENDING 2 AS RET_VAL) WHETHER ADVENTID IS UNIQUE OR NOT?
		IF EXISTS(SELECT CustomerAccountNumber FROM SYN_IT_AccountMaster WHERE CustomerAccountNumber=@ADVENTID)
		BEGIN
			SET @IS_ADVENTID_UNIQUE = 0
		END
		
		IF(@IS_ADVENTID_UNIQUE = 1)--IF ADVENTID IS UNIQUE THEN GO AHEAD AND SEND RET_VAL = 2 WITH PROGRAMID AND BRIEFNAME.
		BEGIN
			SELECT 2 AS RET_VAL,AlliNum.AllianceNumber ,ISNULL(AlliNum.AllianceNumber,'') PROGRAM_BRIEFNAME
			FROM SYN_IT_AllianceNumbers AlliNum
			WHERE AlliNum.AllianceNumber=@PROGRAM_BN
		END
		ELSE--ADVENTID IS NOT UNIQUE SO RET_VAL=5
		BEGIN
			SELECT 5 AS RET_VAL , 0 AS PROGRAMID, '' AS BRIEFNAME
		END
		RETURN(-1)
	END

	IF EXISTS(SELECT CustomerAccountNumber FROM SYN_IT_AccountMaster WHERE CustomerAccountNumber=@ADVENTID)  
	BEGIN  
		SELECT 5 AS RET_VAL , 0 AS PROGRAMID, '' AS BRIEFNAME
		RETURN(-1)
	END

	SELECT 0 AS RET_VAL,
	  AlliNum.AllianceNumber,              
	  ISNULL(AlliNum.AllianceNumber,'') PROGRAM_BRIEFNAME            
	FROM SYN_IT_AllianceNumbers AlliNum 
	WHERE AlliNum.AllianceNumber=@PROGRAM_BN          
	RETURN(0)
END
GO
IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetAccountPrgVal'
		)
BEGIN
	PRINT 'CREATED USP_EX_GetAccountPrgVal';
END
