IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetEnfTacticalStrategicDetails'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetEnfTacticalStrategicDetails;

	PRINT 'DROPPED USP_EX_GetEnfTacticalStrategicDetails';
END
GO

/******************************************************************************                        
        DROP PROCEDURE USP_EX_GetEnfTacticalStrategicDetails;
** New Name:	 
** Old Name:     USP_EX_GetEnfTacticalStrategicDetails                        
** Short Desc: To retrieve the Investment Allocation Management details               
**                        
** Full Description: To retrieve the Investment Allocation Management details               
**                                
** Input Arguments: NONE      
**           
** Sample Call                        
**  EXEC USP_EX_GetEnfTacticalStrategicDetails   
   @StrategicAllocationID = '3'      
         
**               
** Return values: NONE      
**                        
**                        
** Standard declarations                        
**       SET LOCK_TIMEOUT         30000   -- 30 seconds                        
**                         
** Created By: Tanuj                
** Company   : Kaspick & Company                        
** Project   : Excelsior  - Enfuego 4                        
** Created DT: 19/Feb/2009                        
**                                    
*******************************************************************************                  
**       Change History                        
*******************************************************************************                  
** Date:        Author:  Bug #     Description:                           Rvwd                  
** --------  -------- ------    -------------------------------------- --------                  
** <MM/DD/YYYY> 
**03/22/2014  Sanath             Requirement INVREQ3.1
** 23-May-2014  Sanath           Sp name renamed as per Kaspick naming convention standard 
     
*******************************************************************************                        
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved                        
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION                        
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetEnfTacticalStrategicDetails] --472    
	(@StrategicAllocationID INT = 0)
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
	SET @ProcName = 'USP_EX_GetEnfTacticalStrategicDetails';

	-- Body of procedure  --            
	BEGIN TRY
		DECLARE @MABenchMarkID INT
			,@TradeBenchMarkID INT

		--Get MABenchMarkID for the selected StrategicAllocation      
		SET @MABenchMarkID = (
				SELECT StrgcAllcBnchMrk.BenchMarkID
				FROM TBL_INV_StrategicAllocationBenchMark StrgcAllcBnchMrk
				INNER JOIN TBL_INV_BenchMark BnchMrk ON BnchMrk.BenchMarkID = StrgcAllcBnchMrk.BenchMarkID
				INNER JOIN TBL_INV_BenchMarkType BnchMrkTyp ON BnchMrk.BenchMarkTypeID = BnchMrkTyp.BenchMarkTypeID
				WHERE BnchMrkTyp.BenchMarkTypeName = 'MultiAsset'
					AND StrgcAllcBnchMrk.StrategicAllocationID = @StrategicAllocationID
				)
		--Get @TradeBenchMarkID for the selected StrategicAllocation      
		SET @TradeBenchMarkID = (
				SELECT StrgcAllcBnchMrk.BenchMarkID
				FROM TBL_INV_StrategicAllocationBenchMark StrgcAllcBnchMrk
				INNER JOIN TBL_INV_BenchMark BnchMrk ON BnchMrk.BenchMarkID = StrgcAllcBnchMrk.BenchMarkID
				INNER JOIN TBL_INV_BenchMarkType BnchMrkTyp ON BnchMrk.BenchMarkTypeID = BnchMrkTyp.BenchMarkTypeID
				WHERE BnchMrkTyp.BenchMarkTypeName = 'Traditional'
					AND StrgcAllcBnchMrk.StrategicAllocationID = @StrategicAllocationID
				)

		--Get Master Objective details for the selected objective_code      
		SELECT sa.StrategicAllocationCode
			,BRCommentID
			,@MABenchMarkID AS MABenchMarkID
			,@TradeBenchMarkID AS TradeBenchMarkID
		FROM TBL_INV_StrategicAllocation sa
		WHERE StrategicAllocationID = @StrategicAllocationID
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
			,@val1str = 'USP_EX_GetEnfTacticalStrategicDetails: Cannot Select.'
			,@val2 = ''
			,@val2str = '';
	END CATCH
		-- End of procedure  --            
END
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetEnfTacticalStrategicDetails'
		)
BEGIN
	PRINT 'CREATED USP_EX_GetEnfTacticalStrategicDetails';
END