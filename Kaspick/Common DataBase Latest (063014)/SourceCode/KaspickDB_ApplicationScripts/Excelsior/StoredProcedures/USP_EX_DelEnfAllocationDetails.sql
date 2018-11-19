

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_DelEnfAllocationDetails'
		)
BEGIN
	DROP PROCEDURE USP_EX_DelEnfAllocationDetails;

	PRINT 'DROPPED USP_EX_DelEnfAllocationDetails';
END
GO

/******************************************************************************                  
** New Name:     USP_EX_DelEnfAllocationDetails
** Old Name:     USP_EIS_ENF_ALLOCATION_DETAILS_DelProc                  
** Short Desc: To Delete the Investment Allocation Management details         
**                  
** Full Description: To Validate Objective Code         
**                          
** Input Arguments: NONE
**					
** Sample Call                  
**		EXEC USP_EIS_ENF_ALLOCATION_DETAILS_DelProc
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
** 23-May-2014  Sanath               Sp name renamed as per Kaspick naming convention standard 
*******************************************************************************                  
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved                  
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION                  
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_DelEnfAllocationDetails] (@ObjectiveCode VARCHAR(100))
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

		--  Variable Declarations  --        
		DECLARE @ProcName VARCHAR(60);
		DECLARE @ErrorMessage VARCHAR(1000);
		DECLARE @ErrorNumber INT;
		-- Variables used for error handling - uncomment if needed        
		DECLARE @val1 VARCHAR(30);
		DECLARE @val2 VARCHAR(30);

		--  Variable Data Assignment  --        
		SET @ProcName = 'USP_EX_DelEnfAllocationDetails';

		-- Body of procedure  --        
		UPDATE TBL_INV_InvestmentObjective
		SET IsActive = 0
		WHERE ObjectiveCode = @ObjectiveCode

		COMMIT TRANSACTION
	END TRY

	BEGIN CATCH
		ROLLBACK TRANSACTION

		SET @ErrorMessage = ERROR_MESSAGE();
		SET @ErrorNumber = ERROR_NUMBER();
		SET @val1 = '';
		SET @val2 = '';

		EXEC USP_EX_SYSErrorHandler @CodeName = @ProcName,
			@ErrorMessage = @ErrorMessage,
			@ErrorNumber = @ErrorNumber,
			@val1 = '',
			@val1str = 'USP_EX_DelEnfAllocationDetails: Cannot Select.',
			@val2 = '',
			@val2str = '';
	END CATCH
		-- End of procedure  --        
END
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_DelEnfAllocationDetails'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_DelEnfAllocationDetails';
END

