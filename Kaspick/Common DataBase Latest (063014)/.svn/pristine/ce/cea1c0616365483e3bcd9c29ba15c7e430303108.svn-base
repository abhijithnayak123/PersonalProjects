IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetEnfClientInvestment'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetEnfClientInvestment;

	PRINT 'DROPPED PROCEDURE USP_EX_GetEnfClientInvestment';
END
GO

/******************************************************************************                      
** New Name: USP_EX_GetEnfClientInvestment
** Old Name: USP_EIS_ENF_CLIENT_INVESTMENT_SelProc                      
** Short Desc: To retrieve the Strategic Allocations Set associated to a Client - Investment Tab.             
**                      
** Full Description: To retrieve the Strategic Allocations Set associated to a Client - Investment Tab.             
**                              
** Input Arguments: NONE    
**         
** Sample Call                      
**  EXEC USP_EIS_ENF_CLIENT_INVESTMENT_SelProc 100008   
       
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
** Created DT: 19/Feb/2010                      
**                                  
*******************************************************************************                
**       Change History                      
*******************************************************************************                
** Date:        Author:  Bug #     Description:                           Rvwd                
** --------  -------- ------    -------------------------------------- --------                
  30/Aug/2010	Soorya			ClientPolicy Changes
** 04-Mar-2014  Mallikarjun EXCREQ3.1 Modified
** 22-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard 
*************************************************************                      
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved                      
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION                      
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetEnfClientInvestment] --'ACL' 
	(@ManagerCode VARCHAR(15))
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
	DECLARE @SelectedSet TABLE (
		StrategicAllocationSetID INT
		,StrategicAllocationSetName VARCHAR(30)
		,LastModifiedUser VARCHAR(100)
		,LastModifiedDate DATETIME
		);

	--  Variable Data Assignment  --            
	SET @ProcName = 'USP_EX_GetEnfClientInvestment';

	-- Body of procedure  --            
	BEGIN TRY
		INSERT INTO @AssociatedSet
		SELECT DISTINCT 1 AS IsAssociated
			,ClntInvstPlcyDtls.StrategicAllocationID
			,StgcAlcSetDtls.StrategicAllocationSetID
		FROM TBL_INV_ClientInvestmentPolicyTrustType ClntInvstPlcyTrstTyp
		INNER JOIN TBL_INV_ClientInvestmentPolicyDetail ClntInvstPlcyDtls ON ClntInvstPlcyDtls.IPTrustTypeID = ClntInvstPlcyTrstTyp.IPTrustTypeID
		INNER JOIN TBL_INV_StrategicAllocationSetDetail StgcAlcSetDtls ON StgcAlcSetDtls.StrategicAllocationID = ClntInvstPlcyDtls.StrategicAllocationID
		WHERE ClntInvstPlcyTrstTyp.ManagerCode = @ManagerCode

		INSERT INTO @SelectedSet
		SELECT DISTINCT StgcAlcSet.StrategicAllocationSetID
			,StgcAlcSet.StrategicAllocationSetName
			,(ISNULL(KsUsr.FirstName, '') + ' ' + ISNULL(KsUsr.LastName, '')) AS LastModifiedUser
			,ISNULL(ClntStgcAlc.ModifiedDate, GETDATE()) AS LastModifiedDate
		FROM TBL_INV_ClientStrategicAllocation ClntStgcAlc
		INNER JOIN TBL_INV_StrategicAllocationSet StgcAlcSet ON StgcAlcSet.StrategicAllocationSetID = ClntStgcAlc.StrategicAllocationSetID
		INNER JOIN tbl_ks_user KsUsr ON KsUsr.USERID = ClntStgcAlc.ModifiedBy
		WHERE ClntStgcAlc.ManagerCode = @ManagerCode
		ORDER BY LastModifiedDate ASC

		SELECT StrategicAllocationSetName
			,LastModifiedUser
			,LastModifiedDate
			,IsAssociated
		FROM @SelectedSet SS
		LEFT JOIN @AssociatedSet AST ON AST.StrategicAllocationSetID = SS.StrategicAllocationSetID
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
			,@val1str = 'USP_EX_GetEnfClientInvestment: Cannot Select.'
			,@val2 = ''
			,@val2str = '';
	END CATCH
END
GO

SET NOCOUNT OFF;

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetEnfClientInvestment'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetEnfClientInvestment';
END