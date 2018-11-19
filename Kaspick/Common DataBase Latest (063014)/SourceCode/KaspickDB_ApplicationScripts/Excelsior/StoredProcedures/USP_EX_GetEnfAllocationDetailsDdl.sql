IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetEnfAllocationDetailsDdl'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetEnfAllocationDetailsDdl;

	PRINT 'DROPPED USP_EX_GetEnfAllocationDetailsDdl';
END
GO

/******************************************************************************                                
** New Name:     USP_EX_GetEnfAllocationDetailsDdl                                
** Old Name:     USP_EIX_EnfAllocationDetailsDDLSelProc                                
** Short Desc: To retrieve the Trad Benchmark dropdown in Investment Allocation Management details                       
**                                
** Full Description: To retrieve the Trad Benchmark dropdown in Investment Allocation Management details                       
**                                        
** Input Arguments: NONE              
**                   
** Sample Call                                
**  EXEC USP_EX_GetEnfAllocationDetailsDdl              
                 
**                       
** Return values: NONE              
**                                
**                                
** Standard declarations                                
**       SET LOCK_TIMEOUT         30000   -- 30 seconds                                
**                                 
** Created By: Saravanan                         
** Company   : Kaspick & Company                                
** Project   : Excelsior  - Enfuego 4                                
** Created DT: 10/09/2009                                
**                                            
*******************************************************************************                          
**       Change History                                
*******************************************************************************                          
** Date:        Author:  Bug #     Description:                           Rvwd                          
** --------  -------- ------    -------------------------------------- --------                          
** 06/11/2009 Saravanan P Muthu Added Log Information and Indentation 
**22/03/2014   Sanath  Requirement INVREQ 3.1
** 22-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard  
*******************************************************************************                                
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved                                
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION                                
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetEnfAllocationDetailsDdl]
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
	SET @ProcName = 'USP_EX_GetEnfAllocationDetailsDdl';

	-- Body of procedure  --                    
	BEGIN TRY
		SELECT BnchMrk.BenchMarkID AS MABenchmarkID
			,BnchMrk.BenchMarkName AS MABenchmark
		FROM TBL_INV_BenchMark BnchMrk
		INNER JOIN TBL_INV_BenchMarkType BnchMrkTyp ON BnchMrk.BenchMarkTypeID = BnchMrkTyp.BenchMarkTypeID
		WHERE BnchMrkTyp.BenchMarkTypeName = 'MultiAsset'

		SELECT BnchMrk.BenchMarkID AS TradBenchmarkID
			,BnchMrk.BenchMarkName AS TradBenchmark
		FROM TBL_INV_BenchMark BnchMrk
		INNER JOIN TBL_INV_BenchMarkType BnchMrkTyp ON BnchMrk.BenchMarkTypeID = BnchMrkTyp.BenchMarkTypeID
		WHERE BnchMrkTyp.BenchMarkTypeName = 'Traditional'

		SELECT DISTINCT AssetClassName AS AssetClass
			,SecuritySymbol
		FROM TBL_TR_Asset
		WHERE (
				SecuritySymbol LIKE '[a-z]%'
				OR SecuritySymbol = '407288XE4'
				OR SecuritySymbol = '290886100'
				)
		ORDER BY SecuritySymbol ASC

		SELECT DISTINCT AssetClassName AS AssetClass
			,AssetClassName AS AssetClasses
		FROM TBL_TR_Asset
		ORDER BY AssetClass ASC

		SELECT StrategicAllocationID AS StrategicID
			,StrategicAllocationCode AS StrategicCode
		FROM TBL_INV_StrategicAllocation
		WHERE IsActive = 1
		ORDER BY StrategicAllocationCode

		SELECT BRCommentID
			,PortfolioCode
		FROM TBL_INV_BeneReportComment
		ORDER BY PortfolioCode ASC

		SELECT BRCommentID
			,PortfolioDescription
		FROM TBL_INV_BeneReportComment

		SELECT StgcAlcn.StrategicAllocationID
			,StrategicAllocationCode
			,1 AS IsAssociated
			,ObjectiveCode
		FROM TBL_INV_ClientInvestmentPolicyDetail ClntInvstPlcyDtls
		INNER JOIN TBL_INV_InvestmentObjectivePolicy InvstObjctPlcy ON InvstObjctPlcy.ClientInvestmentPolicyDetailID = ClntInvstPlcyDtls.ClientInvestmentPolicyDetailID
		INNER JOIN TBL_INV_StrategicAllocation StgcAlcn ON StgcAlcn.StrategicAllocationID = ClntInvstPlcyDtls.StrategicAllocationID
		ORDER BY StgcAlcn.StrategicAllocationID
			,ObjectiveCode
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
			,@val1str = 'USP_EX_GetEnfAllocationDetailsDdl: Cannot Select.'
			,@val2 = ''
			,@val2str = '';
	END CATCH
END
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetEnfAllocationDetailsDdl'
		)
BEGIN
	PRINT 'CREATED USP_EX_GetEnfAllocationDetailsDdl';
END