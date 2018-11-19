IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetAccountAdventID'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetAccountAdventID;

	PRINT 'DROPPED USP_EX_GetAccountAdventID';
END
GO

/*********************************************************************************************************************                                                 
* New Procedure Name : USP_EX_GetAccountAdventID
* Old Procedure Name  : USP_EIS_EX_ADVENTID_SelProc        
* Description     : Retrieving all AdventIds      
* Input Parameter :      
* Modification Log                                                     
*                                    
* Date       Modified By    Description                                                    
*--------------------------------------------------------------------------------------------------------------------                                                 
* 01-02-07  Geetha Priya.V Created
* 13-Mar-2014 Mallikarjun EXCREQ7.4 Modified   
* 22-May-2014  Sanath Sp name renamed as per Kaspick naming convention standard      
*********************************************************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetAccountAdventID]
AS
BEGIN
	SELECT CustomerAccountNumber
	FROM dbo.SYN_IT_AccountMaster
END
GO

SET NOCOUNT OFF;

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetAccountAdventID'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetAccountAdventID';
END