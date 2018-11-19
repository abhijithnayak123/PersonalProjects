IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetEnfClientPolicyRulesetDetails'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetEnfClientPolicyRulesetDetails;

	PRINT 'DROPPED USP_EX_GetEnfClientPolicyRulesetDetails';
END
GO

/******************************************************************************                    
** New Name:	 USP_EX_GetEnfClientPolicyRulesetDetails
** Old Name:     USP_EIS_ENF_CLIENTPOLICY_RULESETDETAILS_SelProc                    
** Short Desc: To retrieve the Client Investment Policy - Rule Set details           
**                    
** Full Description: To retrieve the Client Investment Policy - Rule Set details           
**                            
** Input Arguments: NONE  
**       
** Sample Call                    
**  EXEC USP_EIS_ENF_CLIENTPOLICY_RULESETDETAILS_SelProc  
		 @Mode = 'EDIT', @ClientID = 100008, @IPTrustTypeID = 168  
     
**           
** Return values: NONE  
**                    
**                    
** Standard declarations                    
**       SET LOCK_TIMEOUT         30000   -- 30 seconds                    
**                     
** Created By: Soorya            
** Company   : Kaspick & Company                    
** Project   : Excelsior  - Enfuego4C                    
** Created DT: 09/Aug/2010                    
**                                
*******************************************************************************              
**       Change History                    
*******************************************************************************              
** Date:        Author:  Bug #     Description:                           Rvwd              
** --------  -------- ------    -------------------------------------- --------              
** 12-Mar-2014 Mallikarjun EXCREQ3.1 Modified  
** 23-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard  
*******************************************************************************                    
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved                    
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION                    
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetEnfClientPolicyRulesetDetails] --18  
	(
	@Mode VARCHAR(10)
	,@ManagerCode VARCHAR(15)
	,@IPTrustTypeID INT = 0
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
	SET @ProcName = 'USP_EX_GetEnfClientPolicyRulesetDetails';

	-- Body of procedure  --          
	BEGIN TRY
		DECLARE @RuleSetDetail TABLE (
			IPTrustTypeID BIGINT
			,ClientInvestmentPolicyDetailsID BIGINT
			,StrategicAllocationID BIGINT
			,StrategicAllocationName VARCHAR(50)
			,MinPayoutValue FLOAT NULL
			,MinPayoutCond CHAR(2) NULL
			,MaxPayoutValue FLOAT NULL
			,MaxPayoutCond CHAR(2) NULL
			,TacticalAllocation VARCHAR(50)
			)
		DECLARE @StrategicAlloc TABLE (
			StrategicAllocationID BIGINT
			,StrategicAllocationName VARCHAR(50)
			)

		IF (@Mode = 'NEW')
		BEGIN
			SELECT '' AS TrustTypeIPName
				,'' AS IPTrustTypeID
				,'' AS TrustTypeID
				,'' AS MinHorizonValue
				,'' AS MinHorizonCond
				,'' AS MaxHorizonValue
				,'' AS MaxHorizonCond
				,'' AS Review
				,'' AS MinReviewValue
				,'' AS MinReviewCond
				,'' AS MaxReviewValue
				,'' AS MaxReviewCond
			FROM TBL_INV_ClientInvestmentPolicyTrustType
			WHERE IPTrustTypeID = @IPTrustTypeID

			INSERT INTO @RuleSetDetail
			SELECT '' AS IPTrustTypeID
				,'' AS ClientInvestmentPolicyDetailsID
				,StgcAlc.StrategicAllocationID
				,StgcAlc.StrategicAllocationName
				,'' AS MinPayoutValue
				,'' AS MinPayoutCond
				,'' AS MaxPayoutValue
				,'' AS MaxPayoutCond
				,'' AS TacticalAllocation
			FROM TBL_INV_StrategicAllocation StgcAlc
			INNER JOIN TBL_INV_StrategicAllocationSetDetail StgcAlcSetDtls ON StgcAlcSetDtls.StrategicAllocationID = StgcAlc.StrategicAllocationID
				AND StgcAlc.IsActive = 1
			INNER JOIN TBL_INV_StrategicAllocationSet StgcAlcSet ON StgcAlcSet.StrategicAllocationSetID = StgcAlcSetDtls.StrategicAllocationSetID
				AND StgcAlcSet.IsActive = 1
			INNER JOIN TBL_INV_ClientStrategicAllocation ClntStgcAlc ON ClntStgcAlc.StrategicAllocationSetID = StgcAlcSet.StrategicAllocationSetID
			WHERE ClntStgcAlc.ManagerCode = @ManagerCode
			
			UNION
			
			SELECT '' AS IPTrustTypeID
				,'' AS ClientInvestmentPolicyDetailsID
				,NULL AS StrategicAllocationID
				,'Review' AS StrategicAllocationName
				,'' AS MinPayoutValue
				,'' AS MinPayoutCond
				,'' AS MaxPayoutValue
				,'' AS MaxPayoutCond
				,'' AS TacticalAllocation
			ORDER BY StgcAlc.StrategicAllocationID ASC

			SELECT *
			FROM @RuleSetDetail

			SELECT StrategicAllocationID
				,LTRIM(RTRIM(ObjectiveCode)) AS ObjectiveCode
				,LTRIM(RTRIM(ObjectiveName)) AS ObjectiveName
			FROM TBL_INV_InvestmentObjective InvstObjtv
			WHERE StrategicAllocationID IN (
					SELECT StrategicAllocationID
					FROM @RuleSetDetail
					)
				AND InvstObjtv.IsActive = 1
			
			UNION
			
			SELECT NULL AS StrategicAllocationID
				,'Review' AS ObjectiveCode
				,'Review' AS ObjectiveName
			ORDER BY StrategicAllocationID ASC
		END
		ELSE
		BEGIN
			SELECT TrustTypeIPName
				,IPTrustTypeID
				,TrustTypeID
				,MinHorizonValue
				,MinHorizonCond
				,MaxHorizonValue
				,MaxHorizonCond
				,Review
				,MinReviewValue
				,MinReviewCond
				,MaxReviewValue
				,MaxReviewCond
			FROM TBL_INV_ClientInvestmentPolicyTrustType
			WHERE IPTrustTypeID = @IPTrustTypeID

			INSERT INTO @StrategicAlloc
			SELECT StgcAlc.StrategicAllocationID
				,StgcAlc.StrategicAllocationName
			FROM TBL_INV_StrategicAllocation StgcAlc
			INNER JOIN TBL_INV_StrategicAllocationSetDetail StgcAlcSetDtls ON StgcAlcSetDtls.StrategicAllocationID = StgcAlc.StrategicAllocationID
				AND StgcAlc.IsActive = 1
			INNER JOIN TBL_INV_StrategicAllocationSet StgcAlcSet ON StgcAlcSet.StrategicAllocationSetID = StgcAlcSetDtls.StrategicAllocationSetID
				AND StgcAlcSet.IsActive = 1
			INNER JOIN TBL_INV_ClientStrategicAllocation ClntStgcAlc ON ClntStgcAlc.StrategicAllocationSetID = StgcAlcSet.StrategicAllocationSetID
			WHERE ClntStgcAlc.ManagerCode = @ManagerCode
			
			UNION
			
			SELECT NULL AS StrategicAllocationID
				,'Review' AS StrategicAllocationName
			ORDER BY StgcAlc.StrategicAllocationID ASC

			INSERT INTO @RuleSetDetail
			SELECT ClntInvstPlcyTrstTyp.IPTrustTypeID
				,ClntInvstPlcyDtls.ClientInvestmentPolicyDetailID
				,ClntInvstPlcyDtls.StrategicAllocationID
				,StgcAlc.StrategicAllocationName
				,ClntInvstPlcyDtls.MinPayoutValue
				,ClntInvstPlcyDtls.MinPayoutCond
				,ClntInvstPlcyDtls.MaxPayoutValue
				,ClntInvstPlcyDtls.MaxPayoutCond
				,LTRIM(RTRIM(InvstObjctPlcy.ObjectiveCode)) AS TacticalAllocation
			FROM TBL_INV_ClientInvestmentPolicyDetail ClntInvstPlcyDtls
			INNER JOIN TBL_INV_ClientInvestmentPolicyTrustType ClntInvstPlcyTrstTyp ON ClntInvstPlcyTrstTyp.IPTrustTypeID = ClntInvstPlcyDtls.IPTrustTypeID
			INNER JOIN TBL_INV_StrategicAllocation StgcAlc ON StgcAlc.StrategicAllocationID = ClntInvstPlcyDtls.StrategicAllocationID
				AND StgcAlc.IsActive = 1
			INNER JOIN TBL_INV_InvestmentObjectivePolicy InvstObjctPlcy ON InvstObjctPlcy.ClientInvestmentPolicyDetailID = ClntInvstPlcyDtls.ClientInvestmentPolicyDetailID
			WHERE ClntInvstPlcyDtls.IPTrustTypeID = @IPTrustTypeID

			INSERT INTO @RuleSetDetail
			SELECT ClntInvstPlcyTrstTyp.IPTrustTypeID
				,ClntInvstPlcyDtls.ClientInvestmentPolicyDetailID
				,NULL AS StrategicAllocationID
				,'Review' AS StrategicAllocationName
				,ClntInvstPlcyDtls.MinPayoutValue
				,ClntInvstPlcyDtls.MinPayoutCond
				,ClntInvstPlcyDtls.MaxPayoutValue
				,ClntInvstPlcyDtls.MaxPayoutCond
				,'Review' AS TacticalAllocation
			FROM TBL_INV_ClientInvestmentPolicyDetail ClntInvstPlcyDtls
			INNER JOIN TBL_INV_ClientInvestmentPolicyTrustType ClntInvstPlcyTrstTyp ON ClntInvstPlcyTrstTyp.IPTrustTypeID = ClntInvstPlcyDtls.IPTrustTypeID
				AND ClntInvstPlcyDtls.StrategicAllocationID IS NULL
			WHERE ClntInvstPlcyDtls.IPTrustTypeID = @IPTrustTypeID

			SELECT RSD.IPTrustTypeID
				,RSD.ClientInvestmentPolicyDetailsID
				,StgcAlc.StrategicAllocationID
				,StgcAlc.StrategicAllocationName
				,RSD.MinPayoutValue
				,RSD.MinPayoutCond
				,RSD.MaxPayoutValue
				,RSD.MaxPayoutCond
				,RSD.TacticalAllocation
			FROM @StrategicAlloc StgcAlc
			LEFT JOIN @RuleSetDetail RSD ON ISNULL(StgcAlc.StrategicAllocationID, 0) = ISNULL(RSD.StrategicAllocationID, 0)

			SELECT StgcAlc.StrategicAllocationID
				,LTRIM(RTRIM(InvstObjtv.ObjectiveCode)) AS ObjectiveCode
				,LTRIM(RTRIM(InvstObjtv.ObjectiveName)) AS ObjectiveName
			FROM TBL_INV_StrategicAllocation StgcAlc
			INNER JOIN TBL_INV_InvestmentObjective InvstObjtv ON InvstObjtv.StrategicAllocationID = StgcAlc.StrategicAllocationID
				AND InvstObjtv.IsActive = 1
				AND StgcAlc.IsActive = 1
			INNER JOIN TBL_INV_StrategicAllocationSetDetail StgcAlcSetDtls ON StgcAlcSetDtls.StrategicAllocationID = StgcAlc.StrategicAllocationID
			INNER JOIN TBL_INV_StrategicAllocationSet StgcAlcSet ON StgcAlcSet.StrategicAllocationSetID = StgcAlcSetDtls.StrategicAllocationSetID
				AND StgcAlcSet.IsActive = 1
			INNER JOIN TBL_INV_ClientStrategicAllocation ClntStgcAlc ON ClntStgcAlc.StrategicAllocationSetID = StgcAlcSet.StrategicAllocationSetID
			WHERE ClntStgcAlc.ManagerCode = @ManagerCode
			
			UNION
			
			SELECT NULL AS StrategicAllocationID
				,'Review' AS ObjectiveCode
				,'Review' AS ObjectiveName
			ORDER BY StgcAlc.StrategicAllocationID ASC
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
			,@val1str = 'USP_EX_GetEnfClientPolicyRulesetDetails: Cannot Select.'
			,@val2 = ''
			,@val2str = '';
	END CATCH
		-- End of procedure  --          
END
GO

SET NOCOUNT OFF;

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetEnfClientPolicyRulesetDetails'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetEnfClientPolicyRulesetDetails';
END