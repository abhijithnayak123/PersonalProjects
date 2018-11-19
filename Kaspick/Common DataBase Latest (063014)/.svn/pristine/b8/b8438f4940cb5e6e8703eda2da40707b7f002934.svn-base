IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_DelEnfStrategicAllocationDetails'
		)
BEGIN
	DROP PROCEDURE USP_EX_DelEnfStrategicAllocationDetails;

	PRINT 'DROPPED USP_EX_DelEnfStrategicAllocationDetails';
END
GO

/******************************************************************************                    
** New Name:     USP_EX_DelEnfStrategicAllocationDetails                    
** Old Name:     USP_EIX_EnfStrategicAllocationDetailsDelProc                    

** Short Desc: To Ddelete the Investment Allocation Management details           
**                    
** Full Description: To Validate Objective Code           
**                            
** Input Arguments: NONE  
**       
** Sample Call                    
**  EXEC USP_EX_DelEnfStrategicAllocationDetails  
   @ObjectiveCode = 'Gro_50'  
     
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
** Created DT: 11/Feb/2010                    
**                                
*******************************************************************************              
**       Change History                    
*******************************************************************************              
** Date:        Author:  Bug #     Description:                           Rvwd              
** --------  -------- ------    -------------------------------------- --------              
** <MM/DD/YYYY>  
**03/21/2014    Sanath               Requirement INVREQ3.1
** 23-May-2014  Sanath               Sp name renamed as per Kaspick naming convention standard 
*******************************************************************************                    
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved                    
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION                    
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_DelEnfStrategicAllocationDetails] --18,''                   
	(
	@StrategicAllocationID INT,
	@IS_Deleted VARCHAR(20) OUTPUT
	)
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

		--  Temp tables, Cursors, Table Variables  --        
		SET @IS_Deleted = 'False'
		--  Variable Data Assignment  --        
		SET @ProcName = 'USP_EX_DelEnfStrategicAllocationDetails';

		-- Body of procedure  --        
		IF NOT EXISTS (
				SELECT TOP 1 *
				FROM TBL_INV_InvestmentObjective
				WHERE StrategicAllocationID = @StrategicAllocationID
					AND IsActive = 1
				)
		BEGIN
			IF NOT EXISTS (
					SELECT TOP 1 *
					FROM TBL_INV_StrategicAllocationSetDetail StrgAllcStDtls
					INNER JOIN TBL_INV_StrategicAllocationSet StrgAllcSet ON StrgAllcSet.StrategicAllocationSetID = StrgAllcStDtls.StrategicAllocationSetID
						AND StrgAllcSet.IsActive = 1
					WHERE StrgAllcStDtls.StrategicAllocationID = @StrategicAllocationID
					)
			BEGIN
				UPDATE TBL_INV_StrategicAllocation
				SET IsActive = 0
				WHERE StrategicAllocationID = @StrategicAllocationID

				SET @IS_Deleted = 'True'
			END
		END

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
			@val1str = 'USP_EX_DelEnfStrategicAllocationDetails: Cannot Select.',
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
			AND NAME = 'USP_EX_DelEnfStrategicAllocationDetails'
		)
BEGIN
	PRINT 'CREATED USP_EX_DelEnfStrategicAllocationDetails';
END