IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetEnfClientPolicyRuleSetNameVal'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetEnfClientPolicyRuleSetNameVal;

	PRINT 'DROPPED USP_EX_GetEnfClientPolicyRuleSetNameVal';
END
GO

/******************************************************************************                
** New Name:	 USP_EX_GetEnfClientPolicyRuleSetNameVal
** Old Name:     USP_EIS_ENF_CLIENTPOLICY_RULESETNAME_ValSelProc                        
** Short Desc: To validate the set name, check duplicate name.               
**                        
** Full Description: To Validate Rule Set Name               
**                                
** Input Arguments: NONE      
**           
** Sample Call                        
**  EXEC USP_EIS_ENF_CLIENTPOLICY_RULESETNAME_ValSelProc      
		@ClientID = 100010, @RuleSetName = 'Annuity Trusts'      
         
**               
** Return values: NONE      
**                        
**                        
** Standard declarations                        
**       SET LOCK_TIMEOUT         30000   -- 30 seconds                        
**                         
** Created By: Soorya                
** Company   : Kaspick & Company                        
** Project   : Excelsior  - Enfuego 4C                        
** Created DT: 08/20/2010                        
**                                    
*******************************************************************************                  
**       Change History                        
*******************************************************************************                  
** Date:        Author:  Bug #     Description:                           Rvwd                  
** --------  -------- ------    -------------------------------------- --------                  
** 12-Mar-2014  Mallikarjun  EXCREQ3.1 Modified  
** 23-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard    
*******************************************************************************                        
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved                        
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION                        
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetEnfClientPolicyRuleSetNameVal] (
	@ManagerCode VARCHAR(15)
	,@RuleSetName VARCHAR(100)
	)
AS
BEGIN
	--  Variable Declarations  --              
	DECLARE @ProcName VARCHAR(60);
	DECLARE @ErrorMessage VARCHAR(1000);
	DECLARE @ErrorNumber INT;
	-- Variables used for error handling - uncomment if needed              
	DECLARE @val1 VARCHAR(30);
	DECLARE @val2 VARCHAR(30);

	--  Variable Data Assignment  --              
	SET @ProcName = 'USP_EX_GetEnfClientPolicyRuleSetNameVal';

	-- Body of procedure  --              
	BEGIN TRY
		DECLARE @ReturnValue INT

		SET @ReturnValue = 0

		IF EXISTS (
				SELECT *
				FROM TBL_INV_ClientInvestmentPolicyTrustType
				WHERE ManagerCode = @ManagerCode
					AND TrustTypeIPName = @RuleSetName
				)
		BEGIN
			SET @ReturnValue = 1
		END

		SELECT @ReturnValue
	END TRY

	BEGIN CATCH
		SET @ErrorMessage = ERROR_MESSAGE();
		SET @ErrorNumber = ERROR_NUMBER();
		SET @val1 = '';
		SET @val2 = '';

		EXEC USP_EX_SYSErrorHandler @CodeName = @ProcName
			,@ErrorMessage = @ErrorMessage
			,@ErrorNumber = @ErrorNumber
			,@val1 = ''
			,@val1str = 'USP_EX_GetEnfClientPolicyRuleSetNameVal: Cannot Select.'
			,@val2 = ''
			,@val2str = '';
	END CATCH
END
GO

SET NOCOUNT OFF;

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetEnfClientPolicyRuleSetNameVal'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetEnfClientPolicyRuleSetNameVal';
END