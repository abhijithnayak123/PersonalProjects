IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetEnfStrategicallocationDetails'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetEnfStrategicallocationDetails;

	PRINT 'DROPPED USP_EX_GetEnfStrategicallocationDetails';
END
GO

/******************************************************************************                      
** New Name:     USP_EX_GetEnfStrategicallocationDetails                      
** Old Name:     USP_EIX_EnfStrategicallocationDetailsSelProc                      
** Short Desc: To retrieve the Investment Allocation Management details             
**                      
** Full Description: To retrieve the Investment Allocation Management details             
**                              
** Input Arguments: NONE    
**         
** Sample Call                      
**  EXEC USP_EX_GetEnfStrategicallocationDetails    
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
** Created DT: 05/Feb/2009                      
**                                  
*******************************************************************************                
**       Change History                      
*******************************************************************************                
** Date:        Author:  Bug #     Description:                           Rvwd                
** --------  -------- ------    -------------------------------------- --------                
** <MM/DD/YYYY>  
** 03/21/2014  Sanath Requirement INVREQ 3.1
** 23-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard 
  
*******************************************************************************                      
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved                      
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION                      
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetEnfStrategicallocationDetails] --18 
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
	SET @ProcName = 'USP_EX_GetEnfStrategicallocationDetails';

	-- Body of procedure  --          
	BEGIN TRY
		DECLARE @DecisionComments VARCHAR(max)
			,@DecisionDate DATETIME
			,@CommentUser INT
			,@StrategicCode VARCHAR(30)
			,@MABenchMarkID INT
			,@TradeBenchMarkID INT
			,@HistoricMABenchMarkID INT
			,@HistoricTradeBenchMarkID INT

		SET @StrategicCode = (
				SELECT StrategicAllocationCode
				FROM TBL_INV_StrategicAllocation
				WHERE StrategicAllocationID = @StrategicAllocationID
				)
		--Get Decision Comments for the selected Objective Code    
		SET @DecisionComments = ''

		SELECT @DecisionComments = @DecisionComments + '(' + left(DATENAME(dw, DecsnCmnt.DecisionDate), 3) + ' ' + left(DATENAME(mm, DecsnCmnt.DecisionDate), 3) + ' ' + CAST(DATEPART(dd, DecsnCmnt.DecisionDate) AS VARCHAR(2)) + ' ' + CAST(DATEPART(hh, DecsnCmnt.DecisionDate) AS VARCHAR(2)) + ':' + CAST(DATEPART(mi, DecsnCmnt.DecisionDate) AS VARCHAR(2)) + ' ' + CAST(DATEPART(year, DecsnCmnt.DecisionDate) AS VARCHAR(4)) + ', ' + KsUsr.LoginName + ') ' + DecsnCmnt.Comment + CHAR(13) + CHAR(10)
		FROM TBL_INV_DecisionComment DecsnCmnt
		INNER JOIN TBL_INV_DecisionCommentTypeLink DecsnCmntTypLnk ON DecsnCmnt.DecisionCommentID = DecsnCmntTypLnk.DecisionCommentID
		INNER JOIN TBL_INV_DecisionCommentType DecsnCmntTyp ON DecsnCmntTyp.TypeID = DecsnCmntTypLnk.TypeID
			AND DecsnCmntTyp.TypeName = 'Strategic'
		INNER JOIN TBL_KS_User KsUsr ON KsUsr.UserID = DecsnCmnt.CommentUser
		WHERE DecsnCmntTypLnk.decisionTypeValue = @StrategicCode
		ORDER BY DecsnCmnt.DecisionCommentID DESC

		--Get MABenchMarkID for the selected StrategicAllocation    
		SET @MABenchMarkID = (
				SELECT StrgcAllcBnchMrk.BenchMarkID
				FROM TBL_INV_StrategicAllocationBenchMark StrgcAllcBnchMrk
				INNER JOIN TBL_INV_BenchMark BnchMrk ON BnchMrk.BenchMarkID = StrgcAllcBnchMrk.BenchMarkID
				INNER JOIN TBL_INV_BenchMarkType BnchMrkTyp ON BnchMrk.BenchMarkTypeID = BnchMrkTyp.BenchMarkTypeID
				WHERE BnchMrkTyp.BenchMarkTypeName = 'MultiAsset'
					AND StrgcAllcBnchMrk.StrategicAllocationID = @StrategicAllocationID
				)
		--Get HistoricMABenchMarkID for the selected StrategicAllocation    
		SET @HistoricMABenchMarkID = (
				SELECT HistoricBenchMarkID
				FROM TBL_INV_BenchMark
				WHERE BenchMarkID = @MABenchMarkID
					AND BenchMarkTypeID = 1
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
		SET @HistoricTradeBenchMarkID = (
				SELECT HistoricBenchMarkID
				FROM TBL_INV_BenchMark
				WHERE BenchMarkID = @TradeBenchMarkID
					AND BenchMarkTypeID = 2
				)

		--Get Master Objective details for the selected objective_code    
		SELECT StgcAlc.StrategicAllocationCode
			,StrategicAllocationName
			,Description
			,BRCommentID
			,MasterObjectiveID
			,@MABenchMarkID AS MABenchMarkID
			,@TradeBenchMarkID AS TradeBenchMarkID
			,@DecisionComments AS DecisionComments
		FROM TBL_INV_StrategicAllocation StgcAlc
		WHERE StrategicAllocationID = @StrategicAllocationID

		--Strategic Asset Allocation    
		SELECT AssetClassName
			,TargetPercentage
		FROM TBL_INV_StrategicAllocationDetail
		WHERE StrategicAllocationID = @StrategicAllocationID
		ORDER BY AssetClassName

		--Get Cascading Detail    
		SELECT StgcAlc.StrategicAllocationID
			,StgcAlc.StrategicAllocationCode
			,InvstObjct.ObjectiveCode
			,Is_MABenchMark = (
				CASE 
					WHEN @HistoricMABenchMarkID = InvstObjct.MABenchmarkID
						THEN 'True'
					ELSE 'False'
					END
				)
			,Is_TradeBenchMark = (
				CASE 
					WHEN @HistoricTradeBenchMarkID = InvstObjct.TradBenchmarkID
						THEN 'True'
					ELSE 'False'
					END
				)
			,Is_ObjectivePortfoiloCode = (
				CASE 
					WHEN StgcAlc.BRCommentID = InvstObjct.BRCommentID
						THEN 'True'
					ELSE 'False'
					END
				)
			,Is_PortfolioCode = (
				CASE 
					WHEN StgcAlc.BRCommentID = (
							SELECT CommentID
							FROM TBL_BR_CommentLinkage
							WHERE CustomerAccountNumber = AcntPrfl.CustomerAccountNumber
								AND ComLinkTypeID = (
									SELECT ComLinkTypeID
									FROM TBL_BR_CommentLinkType
									WHERE comdescription = 'BeneComment'
									)
							)
						THEN 'True'
					ELSE 'False'
					END
				)
			,AcntPrfl.CustomerAccountNumber
			,InvstObjct.IsActive AS Is_AssociatedToTactical
		FROM TBL_INV_StrategicAllocation StgcAlc
		INNER JOIN TBL_INV_InvestmentObjective InvstObjct ON StgcAlc.StrategicAllocationID = InvstObjct.StrategicAllocationID
		LEFT JOIN TBL_INV_AccountProfile AcntPrfl ON InvstObjct.ObjectiveCode = AcntPrfl.ObjectiveCode
		WHERE StgcAlc.StrategicAllocationID = @StrategicAllocationID

		-- checking whether StrategicAllocation is Associated to StrategicAllocationSet. Validating while deleting Allocation   
		IF EXISTS (
				SELECT *
				FROM TBL_INV_StrategicAllocationSetDetail StgcAlcSetDtls
				INNER JOIN TBL_INV_StrategicAllocationSet StgcAlcSet ON StgcAlcSet.StrategicAllocationSetID = StgcAlcSetDtls.StrategicAllocationSetID
					AND StgcAlcSet.IsActive = 'True'
				WHERE StgcAlcSetDtls.StrategicAllocationID = @StrategicAllocationID
				)
		BEGIN
			SELECT 'True' AS Is_AssociatedToSet
		END
		ELSE
		BEGIN
			SELECT 'False' AS Is_AssociatedToSet
		END
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
			,@val1str = 'USP_EX_GetEnfStrategicallocationDetails: Cannot Select.'
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
			AND NAME = 'USP_EX_GetEnfStrategicallocationDetails'
		)
BEGIN
	PRINT 'CREATED USP_EX_GetEnfStrategicallocationDetails';
END