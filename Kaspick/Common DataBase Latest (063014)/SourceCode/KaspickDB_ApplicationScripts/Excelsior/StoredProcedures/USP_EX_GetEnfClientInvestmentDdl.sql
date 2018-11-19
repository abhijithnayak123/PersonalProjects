IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetEnfClientInvestmentDdl'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetEnfClientInvestmentDdl;

	PRINT 'DROPPED PROCEDURE USP_EX_GetEnfClientInvestmentDdl';
END
GO

/******************************************************************************                    
** New Name: USP_EX_GetEnfClientInvestmentDdl
** Old Name: USP_EIS_ENF_CLIENT_INVESTMENT_DDLSelProc                    
** Short Desc: To retrieve the list of Strategic Allocations Set in Client - Investment Tab.           
**                    
** Full Description: To retrieve the list of Strategic Allocations Set in Client - Investment Tab.           
**                            
** Input Arguments: NONE  
**       
** Sample Call                    
**  EXEC USP_EIS_ENF_CLIENT_INVESTMENT_DDLSelProc  
     
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
** 04-Mar-2014 Mallikarjun EXCREQ3.1 Modified
** 22-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard 
*******************************************************************************                    
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved                    
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION                    
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetEnfClientInvestmentDdl]
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
	SET @ProcName = 'USP_EX_GetEnfClientInvestmentDdl';

	-- Body of procedure  --          
	BEGIN TRY
		SELECT StrategicAllocationSetID
			,StrategicAllocationSetName
		FROM TBL_INV_StrategicAllocationSet
		WHERE IsActive = 1
		ORDER BY StrategicAllocationSetName ASC
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
			,@val1str = 'USP_EX_GetEnfClientInvestmentDdl: Cannot Select.'
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
			AND NAME = 'USP_EX_GetEnfClientInvestmentDdl'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetEnfClientInvestmentDdl';
END