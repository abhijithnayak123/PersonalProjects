IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetEnfStategicallocationsetDetails'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetEnfStategicallocationsetDetails;

	PRINT 'DROPPED USP_EX_GetEnfStategicallocationsetDetails';
END
GO

SET QUOTED_IDENTIFIER ON
GO

/******************************************************************************                      
** New Name:     USP_EX_GetEnfStategicallocationsetDetails                      
** Old Name:     USP_EIX_EnfStategicallocationsetDetailsSelProc                      

** Short Desc: To retrieve the Strategict Allocation Set details             
**                      
** Full Description: To retrieve the Strategict Allocation Set details             
**                              
** Input Arguments: NONE    
**         
** Sample Call                      
   EXEC USP_EX_GetEnfStategicallocationsetDetails  
  @XMLDATA = '<ListItemCollection>  
     <ListItem ItemID="5" />   
     <ListItem ItemID="7" />   
     </ListItemCollection>'         
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
** <MM/DD/YYYY>  
   03/21/2014   Sanath          Investment management module pointing to KaspickDB
    23-May-2014  Sanath         Sp name renamed as per Kaspick naming convention standard 
*******************************************************************************                      
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved                      
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION                      
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetEnfStategicallocationsetDetails] --'<ListItemCollection><ListItem ItemID="51"  /><ListItem ItemID="9"  /><ListItem ItemID="10"  /><ListItem ItemID="16"  /><ListItem ItemID="11"  /><ListItem ItemID="12"  /><ListItem ItemID="15"  /><ListItem ItemID="14"  /><ListItem ItemID="13"  /></ListItemCollection>'  
	(@XMLDATA XML)
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
	SET @ProcName = 'USP_EX_GetEnfStategicallocationsetDetails';

	-- Body of procedure  --          
	BEGIN TRY
		SELECT StgcAlc.StrategicAllocationCode
			,AstClsLst.AssetClassType
			,StgcAlcDtls.AssetClassName
			,AstClsLst.AssetClassDescription
			,StgcAlcDtls.TargetPercentage
		FROM TBL_INV_StrategicAllocationDetail StgcAlcDtls
		INNER JOIN TBL_TR_AssetClassList AstClsLst ON StgcAlcDtls.AssetClassName = AstClsLst.AssetClassName + 'us'
		INNER JOIN TBL_INV_StrategicAllocation StgcAlc ON StgcAlc.StrategicAllocationID = StgcAlcDtls.StrategicAllocationID
		WHERE StgcAlcDtls.StrategicAllocationID IN (
				SELECT XMLDATA.item.value('@ItemID[1]', 'INT') AS AllocationID
				FROM @XMLDATA.nodes('//ListItemCollection/ListItem') AS XMLDATA(item)
				)
		ORDER BY AstClsLst.AssetClassType
			,StgcAlcDtls.AssetClassName
			,AstClsLst.AssetClassDescription
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
			,@val1str = 'USP_EX_GetEnfStategicallocationsetDetails: Cannot Select.'
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
			AND NAME = 'USP_EX_GetEnfStategicallocationsetDetails'
		)
BEGIN
	PRINT 'CREATED USP_EX_GetEnfStategicallocationsetDetails';
END