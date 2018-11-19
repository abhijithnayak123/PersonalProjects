IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetEnfStrategicallocationDetailsDdl'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetEnfStrategicallocationDetailsDdl;

	PRINT 'DROPPED USP_EX_GetEnfStrategicallocationDetailsDdl';
END
GO

/******************************************************************************                        
** New Name:     USP_EX_GetEnfStrategicallocationDetailsDdl                        
** Old Name:     USP_EIX_EnfStrategicallocationDetailsDDLSelProc                        

** Short Desc: To retrieve the Trad Benchmark dropdown in Investment Allocation Management details               
**                        
** Full Description: To retrieve the Trad Benchmark dropdown in Investment Allocation Management details               
**                                
** Input Arguments: NONE      
**           
** Sample Call                        
**  EXEC USP_EX_GetEnfStrategicallocationDetailsDdl      
         
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
** Created DT: 02/Feb/2010                        
**                                    
*******************************************************************************                  
**       Change History                        
*******************************************************************************                  
** Date:        Author:  Bug #     Description:                           Rvwd                  
** --------  -------- ------    -------------------------------------- --------                  
** 03/21/2014   Sanath          Requirement INVREQ 3.1
** 23-May-2014  Sanath          Sp name renamed as per Kaspick naming convention standard 
*******************************************************************************                        
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved                        
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION                        
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetEnfStrategicallocationDetailsDdl]
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
	SET @ProcName = 'USP_EX_GetEnfStrategicallocationDetailsDdl';

	-- Body of procedure  --            
	BEGIN TRY
		SELECT BnchMrk.BenchMarkID
			,BnchMrk.BenchMarkName
		FROM TBL_INV_BenchMark BnchMrk
		INNER JOIN TBL_INV_BenchMarkType BnchMrkTyp ON BnchMrk.BenchMarkTypeID = BnchMrkTyp.BenchMarkTypeID
		WHERE BnchMrkTyp.BenchMarkTypeName = 'MultiAsset'

		SELECT BnchMrk.BenchMarkID
			,BnchMrk.BenchMarkName
		FROM TBL_INV_BenchMark BnchMrk
		INNER JOIN TBL_INV_BenchMarkType BnchMrkTyp ON BnchMrk.BenchMarkTypeID = BnchMrkTyp.BenchMarkTypeID
		WHERE BnchMrkTyp.BenchMarkTypeName = 'Traditional'

		SELECT BRCommentID
			,PortfolioCode
		FROM TBL_INV_BeneReportComment
		ORDER BY PortfolioCode ASC

		SELECT MasterObjectiveID
			,MasterObjectiveCode
		FROM TBL_INV_MasterObjective

		SELECT AssetClassName + 'us' AS AssetClass
			,AssetClassName + 'us' AS AssetClasses
		FROM TBL_TR_AssetClassList
		ORDER BY AssetClass ASC

		SELECT BRCommentID
			,PortfolioDescription
		FROM TBL_INV_BeneReportComment
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
			,@val1str = 'USP_EX_GetEnfStrategicallocationDetailsDdl: Cannot Select.'
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
			AND NAME = 'USP_EX_GetEnfStrategicallocationDetailsDdl'
		)
BEGIN
	PRINT 'CREATED USP_EX_GetEnfStrategicallocationDetailsDdl';
END