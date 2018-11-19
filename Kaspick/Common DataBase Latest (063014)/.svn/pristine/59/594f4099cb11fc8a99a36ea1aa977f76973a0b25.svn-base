IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetEnfStategicallocationsetDetailsDdl'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetEnfStategicallocationsetDetailsDdl;

	PRINT 'DROPPED USP_EX_GetEnfStategicallocationsetDetailsDdl';
END
GO

/******************************************************************************                        
** New Name:	 USP_EX_GetEnfStategicallocationsetDetailsDdl
** Old Name:     USP_EIX_EnfStategicallocationsetDetailsDDLSelProc                       
** Short Desc: To retrieve the Strategic Allocations List in Strategic Allocation Set Management details.               
**                        
** Full Description: To retrieve the Strategic Allocations List in Strategic Allocation Set Management details.               
**                                
** Input Arguments: NONE      
**           
** Sample Call                        
**  EXEC USP_EX_GetEnfStategicallocationsetDetailsDdl 'K&Co'     
         
**               
** Return values: NONE      
**                        
**                        
** Standard declarations                        
**       SET LOCK_TIMEOUT         30000   -- 30 seconds                        
**                         
** Created By: Soorya                 
** Company   : Kaspick & Company                        
** Project   : Excelsior  - Enfuego 4                        
** Created DT: 08/Feb/2010                        
**                                    
*******************************************************************************                  
**       Change History                        
*******************************************************************************                  
** Date:        Author:  Bug #     Description:                           Rvwd                  
** --------  -------- ------    -------------------------------------- --------                  
**03/22/2014  Sanath Requirement INVERQ 3.1
** 23-May-2014  Sanath             Sp name renamed as per Kaspick naming convention standard 
*******************************************************************************                        
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved                        
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION                        
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetEnfStategicallocationsetDetailsDdl] (@StrategicAllocationSetName VARCHAR(30))
AS
BEGIN
	--  Variable Declarations  --            
	DECLARE @ProcName VARCHAR(60);
	DECLARE @ErrorMessage VARCHAR(1000);
	DECLARE @ErrorNumber INT;
	-- Variables used for error handling - uncomment if needed            
	DECLARE @val1 VARCHAR(30);
	DECLARE @val2 VARCHAR(30);
	--  Temp tables, Cursors, Table Variables  --            
	DECLARE @AssociatedSet TABLE (
		IsAssociated INT
		,StrategicAllocationID INT
		,StrategicAllocationSetID INT
		);

	--  Variable Data Assignment  --            
	SET @ProcName = 'USP_EX_GetEnfStategicallocationsetDetailsDdl';

	-- Body of procedure  --            
	BEGIN TRY
		INSERT INTO @AssociatedSet
		SELECT DISTINCT 1 AS IsAssociated
			,ClntInvstPlcDtls.StrategicAllocationID
			,StgcAlcSetDtls.StrategicAllocationSetID
		FROM TBL_INV_ClientInvestmentPolicyTrustType ClntInvstPlcTrstTyp
		INNER JOIN TBL_INV_ClientInvestmentPolicyDetail ClntInvstPlcDtls ON ClntInvstPlcDtls.IPTrustTypeID = ClntInvstPlcTrstTyp.IPTrustTypeID
		INNER JOIN TBL_INV_StrategicAllocationSetDetail StgcAlcSetDtls ON StgcAlcSetDtls.StrategicAllocationID = ClntInvstPlcDtls.StrategicAllocationID
		INNER JOIN TBL_INV_StrategicAllocationSet StgcAlcSet ON StgcAlcSet.StrategicAllocationSetID = StgcAlcSetDtls.StrategicAllocationSetID
		WHERE StgcAlcSet.StrategicAllocationSetName = @StrategicAllocationSetName

		SELECT Description
		FROM TBL_INV_StrategicAllocationSet
		WHERE StrategicAllocationSetName = @StrategicAllocationSetName;

		SELECT StgcAlc.StrategicAllocationID
			,StgcAlc.StrategicAllocationCode
			,StrategicAllocationName
			,isnull(eq.TargetPercentage, 0) EquityTargetPercentage
			,isnull(fx.TargetPercentage, 0) FixedTargetPercentage
			,isnull(ca.TargetPercentage, 0) CashTargetPercentage
			,isnull(eq.TargetPercentage, 0) + isnull(fx.TargetPercentage, 0) + isnull(ca.TargetPercentage, 0) TotalTargetPercentage
		FROM TBL_INV_StrategicAllocation StgcAlc
		LEFT JOIN (
			SELECT StrategicAllocationID
				,sum(TargetPercentage) TargetPercentage
			FROM TBL_INV_StrategicAllocationDetail
			WHERE AssetClassType = 'Equity'
			GROUP BY StrategicAllocationID
			) eq ON eq.StrategicAllocationID = StgcAlc.StrategicAllocationID
		LEFT JOIN (
			SELECT StrategicAllocationID
				,sum(TargetPercentage) TargetPercentage
			FROM TBL_INV_StrategicAllocationDetail
			WHERE AssetClassType = 'Cash'
			GROUP BY StrategicAllocationID
			) ca ON ca.StrategicAllocationID = StgcAlc.StrategicAllocationID
		LEFT JOIN (
			SELECT StrategicAllocationID
				,sum(TargetPercentage) TargetPercentage
			FROM TBL_INV_StrategicAllocationDetail
			WHERE AssetClassType = 'Fixed'
			GROUP BY StrategicAllocationID
			) fx ON fx.StrategicAllocationID = StgcAlc.StrategicAllocationID
		WHERE IsActive = 1
		ORDER BY StrategicAllocationCode ASC

		SELECT DISTINCT StgcAlc.StrategicAllocationID
			,StrategicAllocationCode
			,StrategicAllocationName
			,ISNULL(IsAssociated, 0) AS IsAssociated
		FROM TBL_INV_StrategicAllocation StgcAlc
		INNER JOIN TBL_INV_StrategicAllocationSetDetail StgcAlcSetDtls ON StgcAlc.StrategicAllocationID = StgcAlcSetDtls.StrategicAllocationID
		INNER JOIN TBL_INV_StrategicAllocationSet StgcAlcSet ON StgcAlcSetDtls.StrategicAllocationSetID = StgcAlcSet.StrategicAllocationSetID
		LEFT JOIN @AssociatedSet AST ON AST.StrategicAllocationID = StgcAlcSetDtls.StrategicAllocationID
		WHERE StgcAlcSet.StrategicAllocationSetName = @StrategicAllocationSetName

		SELECT StgcAlc.StrategicAllocationCode
			,ACL.AssetClassType
			,StgcAlcDtls.AssetClassName
			,ACL.AssetClassDescription
			,StgcAlcDtls.TargetPercentage
		FROM TBL_INV_StrategicAllocationDetail StgcAlcDtls
		INNER JOIN TBL_TR_AssetClassList ACL ON StgcAlcDtls.AssetClassName = ACL.AssetClassName + 'us'
		INNER JOIN TBL_INV_StrategicAllocation StgcAlc ON StgcAlc.StrategicAllocationID = StgcAlcDtls.StrategicAllocationID
		INNER JOIN TBL_INV_StrategicAllocationSetDetail StgcAlcSetDtls ON StgcAlcSetDtls.StrategicAllocationID = StgcAlcDtls.StrategicAllocationID
		INNER JOIN TBL_INV_StrategicAllocationSet StgcAlcSet ON StgcAlcSet.StrategicAllocationSetID = StgcAlcSetDtls.StrategicAllocationSetID
		WHERE StgcAlcSet.StrategicAllocationSetName = @StrategicAllocationSetName
		ORDER BY StgcAlc.StrategicAllocationCode
			,ACL.AssetClassType
			,StgcAlcDtls.AssetClassName
			,ACL.AssetClassDescription
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
			,@val1str = 'USP_EX_GetEnfStategicallocationsetDetailsDdl: Cannot Select.'
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
			AND NAME = 'USP_EX_GetEnfStategicallocationsetDetailsDdl'
		)
BEGIN
	PRINT 'CREATED USP_EX_GetEnfStategicallocationsetDetailsDdl';
END
GO

