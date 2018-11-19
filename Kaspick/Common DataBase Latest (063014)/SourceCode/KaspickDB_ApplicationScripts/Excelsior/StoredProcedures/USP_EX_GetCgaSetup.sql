IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetCgaSetup'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetCgaSetup;

	PRINT 'DROPPED PROCEDURE USP_EX_GetCgaSetup';
END
GO

SET QUOTED_IDENTIFIER ON
GO

/**************************************************************************  
* New Procedure Name  : USP_EX_GetCgaSetup  
* Old Procedure Name  : USP_EIX_CgaSetupSelProc  
* Description     : To list out state CGA rule for a selected state in pfa update page  
*            Modification Log                                               
*                              
* Date         Modified By  Description                                              
*-------------------------------------------------------------------------  
* 18-Oct-06    Venugopal B  Created     
  31-jan-07    Venugopal B      isnull fn is added.   
  02-Feb-07    Venugopal B      Dele_flag=0 is added. 
  08-Apr-14    Sanath           Pointing to KaspickDB for req EXCREQ3.1  
  22-May-2014  Sanath           Sp name renamed as per Kaspick naming convention standard 
****************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetCgaSetup] @STATEABBREV_ID INT
AS
BEGIN
	SELECT SETUP_ID
		,CGATYPE_TYPE_ID
		,RESERVE_TYPE_ID
		,SUBMIT_FINANCIAL_STATEMENT_TYPE_ID
		,ANNUAL_FORM_TYPE_ID
		,ANNUAL_REPORTING_FORM_RULE
		,ANNUAL_FINANCIAL_STATEMENT_RULE
		,WEBSITE_ADDRESS
	INTO #TEMP2
	FROM TBL_BR_CGASetup
	WHERE STATE_TYPE_ID = @STATEABBREV_ID

	SELECT SETUP_ID
		,LstItm.LISTITEMNAME AS CGATYPE
		,RESERVE_TYPE_ID
		,SUBMIT_FINANCIAL_STATEMENT_TYPE_ID
		,ANNUAL_FORM_TYPE_ID
		,ANNUAL_REPORTING_FORM_RULE
		,ANNUAL_FINANCIAL_STATEMENT_RULE
		,WEBSITE_ADDRESS
	INTO #TEMP3
	FROM #TEMP2 Tmp
	LEFT JOIN TBL_LISTITEM LstItm ON Tmp.CGATYPE_TYPE_ID = LstItm.LISTITEMID

	SELECT SETUP_ID
		,CGATYPE
		,LstItm.LISTITEMNAME AS RESERVE_TYPE
		,LstItmFS.LISTITEMNAME AS SUBMIT_FINANCIAL_STATEMENT
		,LstItmFrmTyp.LISTITEMNAME AS ANNUAL_FORM_TYPE
		,ANNUAL_REPORTING_FORM_RULE
		,ANNUAL_FINANCIAL_STATEMENT_RULE
		,WEBSITE_ADDRESS
	INTO #TEMP4
	FROM #TEMP3 Tmp
	LEFT JOIN TBL_LISTITEM AS LstItm ON Tmp.RESERVE_TYPE_ID = LstItm.LISTITEMID
	LEFT JOIN TBL_LISTITEM AS LstItmFS ON Tmp.SUBMIT_FINANCIAL_STATEMENT_TYPE_ID = LstItmFS.LISTITEMID
	LEFT JOIN TBL_LISTITEM AS LstItmFrmTyp ON Tmp.ANNUAL_FORM_TYPE_ID = LstItmFrmTyp.LISTITEMID

	SELECT SETUP_ID
		,ISNULL(CGATYPE, '')
		,ISNULL(RESERVE_TYPE, '')
		,ISNULL(SUBMIT_FINANCIAL_STATEMENT, '')
		,ISNULL(ANNUAL_FORM_TYPE, '')
		,ISNULL(ANNUAL_REPORTING_FORM_RULE, '')
		,ISNULL(ANNUAL_FINANCIAL_STATEMENT_RULE, '')
		,ISNULL(WEBSITE_ADDRESS, '')
	FROM #TEMP4
END
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetCgaSetup'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetCgaSetup';
END