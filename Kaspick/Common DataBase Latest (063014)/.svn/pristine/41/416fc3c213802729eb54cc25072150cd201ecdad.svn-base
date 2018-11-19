IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetAccountInvestmentObjective'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetAccountInvestmentObjective;

	PRINT 'DROPPED USP_EX_GetAccountInvestmentObjective';
END
GO

/******************************************************************************      
** New Name:     USP_EX_GetAccountInvestmentObjective
** Old Name:     USP_EIX_AccountInvestmentObjectiveSelproc      
** Short Desc: Put in Short Description      
**      
** Full Description      
**        More detailed description if necessary      
**      
** Sample Call      
        Exec USP_EIS_EX_ACCOUNT_INVESTMENTOBJECTIVE_SELPROC 103938    
      
**      
** Return values: NONE      
**      
**      
** Standard declarations      
**       SET NOCOUNT             ON      
**       SET LOCK_TIMEOUT         30000   -- 30 seconds      
**       
** Created By: Rahul Sharma      
** Company   : Kaspick & Company      
** Project   : Excelsior -- IM PROFILE      
** Created DT: 03/15/2007      
**                  
*******************************************************************************      
**       Change History      
*******************************************************************************      
** Date:        Author:  Bug #     Description:                           Rvwd      
** --------     -------- ------    -------------------------------------- --------      
** 05-NOV-2009  Soorya      Added where condition IsActive = 1    
** 19-OCT-2010  Soorya      Modified the SP to get the ClientID based on the AccountID    
** 07-Apr-2014 Yugandhar  Modified   EXCREQ 7.4
** 22-May-2014  Sanath    Sp name renamed as per Kaspick naming convention standard 
*******************************************************************************      
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved      
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION      
*******************************************************************************/
CREATE PROCEDURE USP_EX_GetAccountInvestmentObjective --'ACSTO3'
	(@AccountID VARCHAR(15))
AS
--  Variable Declarations  --      
DECLARE @ProcName VARCHAR(60);
DECLARE @ErrorMessage VARCHAR(1000);
DECLARE @ErrorNumber INT;
DECLARE @ClientID VARCHAR(15);
DECLARE @KCoSetID INT;

BEGIN
	SET @KCoSetID = 0;

	SELECT @KCoSetID = StrategicAllocationSetID
	FROM TBL_INV_StrategicAllocationSet
	WHERE StrategicAllocationSetName = 'K&Co'

	SELECT @ClientID = ClientID
	FROM VW_EX_Account
	WHERE AccountID = @AccountID

	SELECT ISNULL(ClntStgcAlc.ManagerCode, 0) AS ClientID
		,-- Non-Zero ClientID to depict whether the Account is associated with K&Co Set.  
		InvstObjct.ObjectiveCode
		,InvstObjct.ObjectiveName
	FROM TBL_INV_InvestmentObjective InvstObjct
	LEFT JOIN TBL_INV_StrategicAllocation StgcAlcn ON StgcAlcn.StrategicAllocationID = InvstObjct.StrategicAllocationID
	LEFT JOIN TBL_INV_StrategicAllocationSetDetail StgcAlcSetDtls ON StgcAlcSetDtls.StrategicAllocationID = StgcAlcn.StrategicAllocationID
	LEFT JOIN TBL_INV_StrategicAllocationSet StgcAlcSet ON StgcAlcSet.StrategicAllocationSetID = StgcAlcSetDtls.StrategicAllocationSetID
	LEFT JOIN TBL_INV_ClientStrategicAllocation ClntStgcAlc ON ClntStgcAlc.StrategicAllocationSetID = StgcAlcSet.StrategicAllocationSetID
		AND ClntStgcAlc.ManagerCode = @ClientID
		AND StgcAlcSet.StrategicAllocationSetID = @KCoSetID
	WHERE InvstObjct.IsActive = 1
	ORDER BY InvstObjct.ObjectiveCode ASC
END
	-- End of procedure  --      
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetAccountInvestmentObjective'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetAccountInvestmentObjective';
END