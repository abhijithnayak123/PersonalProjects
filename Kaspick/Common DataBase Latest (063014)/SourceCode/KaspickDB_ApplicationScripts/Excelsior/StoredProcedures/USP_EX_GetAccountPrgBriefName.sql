IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetAccountPrgBriefName'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetAccountPrgBriefName;

	PRINT 'DROPPED PROCEDURE USP_EX_GetAccountPrgBriefName';
END
GO

--sp_helptext USP_EIS_EX_PROGRAM_CL_SelProc    
/*********************************************************************************************************************                                                       
* New Procedure Name : USP_EX_GetAccountPrgBriefName               
* Old Procedure Name : USP_EIS_EX_CLIENT_PROGRAMBRIEFNAME_SelProc               
* Description    : To retrieve PROGRAM Header Details.              
* Input          : @CLIENTID: Program Id, of which the details should be fetched.              
* Modification Log                                                           
*                                          
* Date  Modified By  Description                                                          
*--------------------------------------------------------------------------------------------------------------------                                                       
* 3-Jan-07  Pranav  Created              
* 21-May-07  Ganapati Removed the delete flag  
* 25-Apr-14 Mallikarjun Modified
* 22-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard 
*********************************************************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetAccountPrgBriefName] (@CLIENTID VARCHAR(15))
AS
BEGIN
	SELECT AlnsNmbr.AllianceNumber
		,ISNULL(AlnsNmbr.AllianceNumber, '') AS PROGRAM_BRIEF_NAME
	FROM SYN_IT_AllianceNumbers AlnsNmbr
	INNER JOIN SYN_IT_ContactMaster ConMstr ON AlnsNmbr.ContactID = ConMstr.ContactID
	INNER JOIN SYN_IT_ContactMaster ClConMstr ON ConMstr.ManagerCode = ClConMstr.ManagerCode
	WHERE ClConMstr.ManagerCode = @CLIENTID
END
GO

SET NOCOUNT OFF;

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetAccountPrgBriefName'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetAccountPrgBriefName';
END