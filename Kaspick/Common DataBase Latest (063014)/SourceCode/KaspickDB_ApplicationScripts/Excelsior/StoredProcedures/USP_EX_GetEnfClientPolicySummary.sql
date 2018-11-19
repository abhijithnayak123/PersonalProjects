IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetEnfClientPolicySummary'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetEnfClientPolicySummary;

	PRINT 'DROPPED PROCEDURE USP_EX_GetEnfClientPolicySummary';
END
GO

/******************************************************************************                    
** New Name:	 USP_EX_GetEnfClientPolicySummary
** Old Name:     USP_EIS_ENF_CLIENTPOLICYSUMMARY_SelProc                    
** Short Desc: To retrieve the Client Investment Policy - Summary           
**                    
** Full Description: To retrieve the Client Investment Policy - Summary           
**                            
** Input Arguments: NONE  
**       
** Sample Call                    
**  EXEC USP_EIS_ENF_CLIENTPOLICYSUMMARY_SelProc @ClientID = 100015  
     
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
** 04-Mar-2014  Mallikarjun  EXCREQ3.1 Modified 
** 23-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard  
*******************************************************************************                    
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved                    
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION                    
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetEnfClientPolicySummary] --'acl'   
	(@ManagerCode VARCHAR(15) = '')
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
	DECLARE @ClientPolicySummary TABLE (
		IPTrustTypeID BIGINT
		,TrustTypeIPName VARCHAR(255) NULL
		,StrategicAllocationID INT
		,StrategicAllocationCode VARCHAR(30)
		,MinHorizonValue INT NULL
		,MinHorizonCond CHAR(2) NULL
		,MaxHorizonValue INT NULL
		,MaxHorizonCond CHAR(2) NULL
		,HorizonInfo NVARCHAR(30) NULL
		,TrustTypeID INT
		)
	DECLARE @TrustTypeRecord TABLE (
		TrustTypeID INT
		,TrustTypeRecordCount INT
		)

	--  Variable Data Assignment  --          
	SET @ProcName = 'USP_EX_GetEnfClientPolicySummary';

	-- Body of procedure  --          
	BEGIN TRY
		INSERT INTO @TrustTypeRecord
		SELECT TrustTypeID
			,COUNT(*) AS TrustTypeRecordCount
		FROM TBL_INV_ClientInvestmentPolicyTrustType
		WHERE ManagerCode = @ManagerCode
		GROUP BY ManagerCode
			,TrustTypeID

		INSERT INTO @ClientPolicySummary
		SELECT ClntInvstPlcyTrstTyp.IPTrustTypeID
			,ClntInvstPlcyTrstTyp.TrustTypeIPName
			,StgcAlc.StrategicAllocationID
			,StgcAlc.StrategicAllocationCode
			,ClntInvstPlcyTrstTyp.MinHorizonValue
			,ClntInvstPlcyTrstTyp.MinHorizonCond
			,ClntInvstPlcyTrstTyp.MaxHorizonValue
			,ClntInvstPlcyTrstTyp.MaxHorizonCond
			,(
				CASE 
					WHEN ClntInvstPlcyDtls.MinPayoutValue <> 0
						AND ClntInvstPlcyDtls.MaxPayoutValue <> 0
						AND ClntInvstPlcyDtls.MinPayoutValue = ClntInvstPlcyDtls.MaxPayoutValue
						THEN (RTRIM(LTRIM(CAST(ISNULL(ClntInvstPlcyDtls.MinPayoutValue, '') AS VARCHAR(10)))) + '%')
					WHEN ClntInvstPlcyDtls.MinPayoutValue <> 0
						AND ClntInvstPlcyDtls.MaxPayoutValue <> 0
						AND ClntInvstPlcyDtls.MinPayoutValue != ClntInvstPlcyDtls.MaxPayoutValue
						THEN (RTRIM(LTRIM(CAST(ISNULL(ClntInvstPlcyDtls.MinPayoutValue, '') AS VARCHAR(10)))) + '-' + RTRIM(LTRIM(CAST(ISNULL(ClntInvstPlcyDtls.MaxPayoutValue, '') AS VARCHAR(10)))) + '%')
					WHEN ClntInvstPlcyDtls.MaxPayoutValue <> 0
						THEN (ClntInvstPlcyDtls.MaxPayoutCond + RTRIM(LTRIM(CAST(ISNULL(ClntInvstPlcyDtls.MaxPayoutValue, '') AS VARCHAR(10)))) + '%')
					WHEN ClntInvstPlcyDtls.MinPayoutValue <> 0
						THEN (ClntInvstPlcyDtls.MinPayoutCond + RTRIM(LTRIM(CAST(ISNULL(ClntInvstPlcyDtls.MinPayoutValue, '') AS VARCHAR(10)))) + '%')
					END
				) AS HorizonInfo
			,ClntInvstPlcyTrstTyp.TrustTypeID
		FROM TBL_INV_StrategicAllocation StgcAlc
		INNER JOIN TBL_INV_StrategicAllocationSetDetail StgcAlcSetDtls ON StgcAlcSetDtls.StrategicAllocationID = StgcAlc.StrategicAllocationID
			AND StgcAlc.IsActive = 1
		INNER JOIN TBL_INV_StrategicAllocationSet StgcAlcSet ON StgcAlcSet.StrategicAllocationSetID = StgcAlcSetDtls.StrategicAllocationSetID
			AND StgcAlcSet.IsActive = 1
		INNER JOIN TBL_INV_ClientStrategicAllocation ClntStgcAlc ON ClntStgcAlc.StrategicAllocationSetID = StgcAlcSet.StrategicAllocationSetID
		LEFT JOIN TBL_INV_ClientInvestmentPolicyDetail ClntInvstPlcyDtls ON ClntInvstPlcyDtls.StrategicAllocationID = StgcAlc.StrategicAllocationID
		LEFT JOIN TBL_INV_ClientInvestmentPolicyTrustType ClntInvstPlcyTrstTyp ON ClntInvstPlcyTrstTyp.IPTrustTypeID = ClntInvstPlcyDtls.IPTrustTypeID
			AND ClntInvstPlcyTrstTyp.ManagerCode = @ManagerCode
		WHERE ClntStgcAlc.ManagerCode = @ManagerCode
		ORDER BY StgcAlc.StrategicAllocationID ASC

		INSERT INTO @ClientPolicySummary
		SELECT ClntInvstPlcyTrstTyp.IPTrustTypeID
			,ClntInvstPlcyTrstTyp.TrustTypeIPName
			,ClntInvstPlcyDtls.StrategicAllocationID
			,'Review' AS StrategicAllocationCode
			,ClntInvstPlcyTrstTyp.MinHorizonValue
			,ClntInvstPlcyTrstTyp.MinHorizonCond
			,ClntInvstPlcyTrstTyp.MaxHorizonValue
			,ClntInvstPlcyTrstTyp.MaxHorizonCond
			,(
				CASE 
					WHEN ClntInvstPlcyDtls.MinPayoutValue <> 0
						AND ClntInvstPlcyDtls.MaxPayoutValue <> 0
						AND ClntInvstPlcyDtls.MinPayoutValue = ClntInvstPlcyDtls.MaxPayoutValue
						THEN (RTRIM(LTRIM(CAST(ISNULL(ClntInvstPlcyDtls.MinPayoutValue, '') AS VARCHAR(10)))) + '%')
					WHEN ClntInvstPlcyDtls.MinPayoutValue <> 0
						AND ClntInvstPlcyDtls.MaxPayoutValue <> 0
						AND ClntInvstPlcyDtls.MinPayoutValue != ClntInvstPlcyDtls.MaxPayoutValue
						THEN (RTRIM(LTRIM(CAST(ISNULL(ClntInvstPlcyDtls.MinPayoutValue, '') AS VARCHAR(10)))) + '-' + RTRIM(LTRIM(CAST(ISNULL(ClntInvstPlcyDtls.MaxPayoutValue, '') AS VARCHAR(10)))) + '%')
					WHEN ClntInvstPlcyDtls.MaxPayoutValue <> 0
						THEN (ClntInvstPlcyDtls.MaxPayoutCond + RTRIM(LTRIM(CAST(ISNULL(ClntInvstPlcyDtls.MaxPayoutValue, '') AS VARCHAR(10)))) + '%')
					WHEN ClntInvstPlcyDtls.MinPayoutValue <> 0
						THEN (ClntInvstPlcyDtls.MinPayoutCond + RTRIM(LTRIM(CAST(ISNULL(ClntInvstPlcyDtls.MinPayoutValue, '') AS VARCHAR(10)))) + '%')
					END
				) AS HorizonInfo
			,ClntInvstPlcyTrstTyp.TrustTypeID
		FROM TBL_INV_ClientInvestmentPolicyDetail ClntInvstPlcyDtls
		INNER JOIN TBL_INV_ClientInvestmentPolicyTrustType ClntInvstPlcyTrstTyp ON ClntInvstPlcyTrstTyp.IPTrustTypeID = ClntInvstPlcyDtls.IPTrustTypeID
			AND ClntInvstPlcyTrstTyp.ManagerCode = @ManagerCode
		WHERE ClntInvstPlcyDtls.StrategicAllocationID IS NULL

		SELECT IPTrustTypeID
			,TrustTypeIPName
			,StrategicAllocationID
			,StrategicAllocationCode
			,MinHorizonValue
			,MinHorizonCond
			,MaxHorizonValue
			,MaxHorizonCond
			,HorizonInfo
			,TrustTypeRecordCount
		FROM @ClientPolicySummary CPS
		LEFT JOIN @TrustTypeRecord TTR ON TTR.TrustTypeID = CPS.TrustTypeID
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
			,@val1str = 'USP_EX_GetEnfClientPolicySummary: Cannot Select.'
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
			AND NAME = 'USP_EX_GetEnfClientPolicySummary'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetEnfClientPolicySummary';
END