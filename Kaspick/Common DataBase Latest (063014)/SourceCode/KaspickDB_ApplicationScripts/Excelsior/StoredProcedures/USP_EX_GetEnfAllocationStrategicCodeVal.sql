IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetEnfAllocationStrategicCodeVal'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetEnfAllocationStrategicCodeVal;

	PRINT 'DROPPED USP_EX_GetEnfAllocationStrategicCodeVal';
END
GO

/******************************************************************************                      
** New Name:	 USP_EX_GetEnfAllocationStrategicCodeVal
** Old Name:     USP_EIX_EnfAllocationStrategicCodeValSelProc                      
** Short Desc: To retrieve the Investment Allocation Management details             
**                      
** Full Description: To Validate Strategic Code             
**                              
** Input Arguments: NONE    
**         
** Sample Call                      
**  EXEC USP_EX_GetEnfAllocationStrategicCodeVal    
   @StrategicCode = 'Gro_50'    
       
**             
** Return values: NONE    
**                      
**                      
** Standard declarations                      
**       SET LOCK_TIMEOUT         30000   -- 30 seconds                      
**                       
** Created By: Tanuj Gupta              
** Company   : Kaspick & Company                      
** Project   : Excelsior  - Enfuego 4                      
** Created DT: 10/21/2009                      
**                                  
*******************************************************************************                
**       Change History                      
*******************************************************************************                
** Date:        Author:  Bug #     Description:                           Rvwd                
** --------  -------- ------    -------------------------------------- --------                
** <MM/DD/YYYY>  
**03/21/2014   Sanath   Requirement INVREQ3.1 
** 22-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard 
*******************************************************************************                      
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved                      
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION                      
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetEnfAllocationStrategicCodeVal] (@StrategicCode VARCHAR(100))
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
	SET @ProcName = 'USP_EX_GetEnfAllocationStrategicCodeVal';

	-- Body of procedure  --          
	BEGIN TRY
		DECLARE @ReturnValue INT

		SET @ReturnValue = 0

		IF EXISTS (
				SELECT StrategicAllocationCode
				FROM TBL_INV_StrategicAllocation
				WHERE StrategicAllocationCode = @StrategicCode
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
			,@val1str = 'USP_EX_GetEnfAllocationStrategicCodeVal: Cannot Select.'
			,@val2 = ''
			,@val2str = '';
	END CATCH
END
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetEnfAllocationStrategicCodeVal'
		)
BEGIN
	PRINT 'CREATED USP_EX_GetEnfAllocationStrategicCodeVal';
END