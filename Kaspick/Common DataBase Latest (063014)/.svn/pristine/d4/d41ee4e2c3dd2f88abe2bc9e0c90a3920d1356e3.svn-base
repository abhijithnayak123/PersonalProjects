IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetEnfAllocationObjectiveCodeVal'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetEnfAllocationObjectiveCodeVal;

	PRINT 'DROPPED USP_EX_GetEnfAllocationObjectiveCodeVal';
END
GO

/******************************************************************************                  
** New Name:	 USP_EX_GetEnfAllocationObjectiveCodeVal
** OlD Name:     USP_EIS_ENF_ALLOCATION_OBJECTIVE_CODE_ValSelProc                  
** Short Desc: To retrieve the Investment Allocation Management details         
**                  
** Full Description: To Validate Objective Code         
**                          
** Input Arguments: NONE
**					
** Sample Call                  
**		EXEC USP_EIS_ENF_ALLOCATION_OBJECTIVE_CODE_ValSelProc
			@ObjectiveCode = 'Gro_50'
			
**									
** Return values:	NONE
**                  
**                  
** Standard declarations                  
**       SET LOCK_TIMEOUT         30000   -- 30 seconds                  
**                   
** Created By: Saravanan P Muthu		         
** Company   : Kaspick & Company                  
** Project   : Excelsior  - Enfuego 4                  
** Created DT: 10/21/2009                  
**                              
*******************************************************************************            
**       Change History                  
*******************************************************************************            
** Date:        Author:  Bug #     Description:                           Rvwd            
** --------		-------- ------    -------------------------------------- --------            
** 22-Mar-2014 Yugandhar  Modified   EXCREQ 3.1
** 22-May-2014  Sanath            Sp name renamed as per Kaspick naming convention standard
*******************************************************************************                  
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved                  
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION                  
*******************************************************************************/
CREATE PROCEDURE USP_EX_GetEnfAllocationObjectiveCodeVal (@ObjectiveCode VARCHAR(100))
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
	SET @ProcName = 'USP_EX_GetEnfAllocationObjectiveCodeVal';

	-- Body of procedure  --        
	BEGIN TRY
		DECLARE @ReturnValue INT

		SET @ReturnValue = 0

		IF EXISTS (
				SELECT ObjectiveCode
				FROM TBL_INV_InvestmentObjective
				WHERE ObjectiveCode = @ObjectiveCode
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
			,@val1str = 'USP_EX_GetEnfAllocationObjectiveCodeVal: Cannot Select.'
			,@val2 = ''
			,@val2str = '';
	END CATCH
END
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetEnfAllocationObjectiveCodeVal'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetEnfAllocationObjectiveCodeVal';
END