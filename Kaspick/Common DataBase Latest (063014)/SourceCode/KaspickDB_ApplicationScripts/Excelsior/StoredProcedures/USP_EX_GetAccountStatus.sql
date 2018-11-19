IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetAccountStatus'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetAccountStatus;

	PRINT 'DROPPED USP_EX_GetAccountStatus';
END
GO

/******************************************************************************        
** New Name:     USP_EX_GetAccountStatus
** Old Name:     USP_EIS_EX_Account_Status_SelProc        
** Short Desc: Procedure to get the status of an account        
**        
** Full Description        
**          This procedure is used to determine the status of an account. If all the mandatory and required later        
   fields are entered in account profile pages then the status is active Else transition.        
   If closed date is entered then the status is Inactive.        
**        
** Sample Call        
        EXEC USP_EIS_EX_Account_Status_SelProc  1223,0        
**        
** Return values: int (status id)        
**        
**        
** Standard declarations        
**       SET LOCK_TIMEOUT         30000   -- 30 seconds        
**         
** Created By: Ganapati        
** Company   : Kaspick & Company        
** Project   : Excelsior        
** Created DT: 04-06-2007        
**                    
*******************************************************************************        
**       Change History        
*******************************************************************************        
** Date:        Author:  Bug #     Description:                           Rvwd        
** --------     -------- ------    -------------------------------------- --------        
** 21-May-07 Ganapati   Removed delete flag      
** 05-July-07 Tanuj Add else part in operation(if no record exits in operation for that account than it is transition)  
** 01-Aug-07 Ganapati Added Tax check  
** 20-May-08 Saravanan PM Commented Performance Start date Ref CR #6830    
** 8-Apr-2014 Yugandhar EXCREQ 7.4
** 23-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard 
*******************************************************************************        
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved        
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION        
*******************************************************************************/
CREATE PROCEDURE USP_EX_GetAccountStatus --'352L418',0            
	(
	@account_id VARCHAR(15)
	,@account_status_id INT OUTPUT
	)
AS
BEGIN
	----  Initial Set statements  --        
	SET NOCOUNT ON;

	--        
	---- Variable Declarations        
	DECLARE @INACTIVE_ID INT -- STORING INACTIVE ID        
	DECLARE @TRANSITION_ID INT -- STORING TRANSITION ID        
	DECLARE @ACTIVE_ID INT -- STORING ACTIVE ID        
	DECLARE @CTR INT -- MIN COUNT        
	DECLARE @MAXCTR INT -- MAX COUNT        
	DECLARE @STATEMENT VARCHAR(400) -- SQL STATEMENT        
	DECLARE @RECORD_COUNT INT -- USED TO COUNT     
	DECLARE @COUNT INT
	--            
	---- Account General tab            
	DECLARE @MATURE_DATE DATETIME -- ACCOUNT MATURE DATE                
	DECLARE @PROGRAMID CHAR(15)
	DECLARE @PROGRAM_STATUS_ID INT

	--            
	---- Store transition id and active id                
	EXEC USP_EX_GetListItemID 'STATUS'
		,'ACTIVE'
		,@ACTIVE_ID OUTPUT -- 58      

	EXEC USP_EX_GetListItemID 'STATUS'
		,'INACTIVE'
		,@INACTIVE_ID OUTPUT -- 59       

	EXEC USP_EX_GetListItemID 'STATUS'
		,'TRANSITION'
		,@TRANSITION_ID OUTPUT -- 60      

	--            
	---- Checking account existance                
	SELECT @RECORD_COUNT = COUNT(*)
	FROM SYN_IT_AccountMaster
	WHERE CustomerAccountNumber = @ACCOUNT_ID

	IF @RECORD_COUNT = 0 -- ACCOUNT DOES NOT EXIST         
	BEGIN
		SELECT @ACCOUNT_STATUS_ID = - 1

		RETURN
	END

	---- Check If account is matured                
	IF @MATURE_DATE IS NOT NULL
	BEGIN
		SELECT @ACCOUNT_STATUS_ID = @INACTIVE_ID

		RETURN
	END

	--            
	---- Check the related program status, If program is inactive or transition then account is also inactive or transition           
	SELECT @PROGRAMID = AllianceNumber
	FROM SYN_IT_AccountMaster
	WHERE CustomerAccountNumber = @ACCOUNT_ID -- STORE PROGRAMID               

	EXEC USP_EX_GetProgramStatus @PROGRAMID
		,@PROGRAM_STATUS_ID OUTPUT -- GET PROGRAM STATUS  

	IF @PROGRAM_STATUS_ID = @INACTIVE_ID
		OR @PROGRAM_STATUS_ID = @TRANSITION_ID
	BEGIN
		SELECT @ACCOUNT_STATUS_ID = @PROGRAM_STATUS_ID

		RETURN
	END

	---- Checking general tab      
	IF @RECORD_COUNT > 0
	BEGIN
		SELECT @ACCOUNT_STATUS_ID = @TRANSITION_ID

		RETURN
	END

	--2. 'KCO VOTE PROXY'   
	EXEC Usp_EX_GetPolicyItemExits @ENTITYID = @ACCOUNT_ID
		,@ENTITYTYPE = 'ACCOUNT'
		,@POLICYNAME = 'KCO VOTE PROXY'
		,@COUNT = @COUNT OUTPUT

	IF (@COUNT = 0)
	BEGIN
		SELECT @ACCOUNT_STATUS_ID = @TRANSITION_ID

		RETURN
	END

	---- Checking Operations        
	---- If any records are there then check each record for required later field            
	IF @RECORD_COUNT > 0
	BEGIN
		IF @RECORD_COUNT > 0
		BEGIN
			SELECT @ACCOUNT_STATUS_ID = @TRANSITION_ID

			RETURN
		END
	END
	ELSE
	BEGIN
		SELECT @ACCOUNT_STATUS_ID = @TRANSITION_ID

		RETURN
	END

	--            
	---- Checking for Payments        
	EXEC Usp_EX_GetPolicyItemExits @ENTITYID = @ACCOUNT_ID
		,@ENTITYTYPE = 'ACCOUNT'
		,@POLICYNAME = 'Net Inc Pmt Rule'
		,@COUNT = @COUNT OUTPUT

	IF (@COUNT = 0)
	BEGIN
		SELECT @ACCOUNT_STATUS_ID = @TRANSITION_ID

		RETURN
	END

	-- --Checking for Tax                
	SELECT @RECORD_COUNT = COUNT(*)
	FROM TBL_IRS_ProfileTax
	WHERE CustomerAccountNumber = @ACCOUNT_ID

	IF @RECORD_COUNT = 0
	BEGIN
		SELECT @ACCOUNT_STATUS_ID = @TRANSITION_ID

		RETURN
	END

	SELECT @record_count = Count(*)
	FROM TBL_INV_AccountProfile AccPrf
	LEFT JOIN TBL_BR_CommentLinkage CmntLnkg ON AccPrf.CustomerAccountNumber = CmntLnkg.CustomerAccountNumber
	LEFT JOIN TBL_INV_BeneReportComment BeneRptCmnt ON CmntLnkg.commentid = BeneRptCmnt.brcommentid
	WHERE (
			AccPrf.fsitypecode = 0
			OR AccPrf.objectivecode IS NULL
			OR AccPrf.investmenttypecode = 0
			OR AccPrf.tradestatuscode = 0
			OR AccPrf.lotaccountingcode = 0
			OR AccPrf.tranchestatuscode = 0
			OR BeneRptCmnt.portfoliocode IS NULL
			)
		AND AccPrf.CustomerAccountNumber = @account_id

	IF @record_count > 0
	BEGIN
		SELECT @account_status_id = @TRANSITION_ID

		RETURN
	END

	--END        
	---- if not transition or closed then status is active        
	SELECT @account_status_id = @ACTIVE_ID
END
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetAccountStatus'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetAccountStatus';
END