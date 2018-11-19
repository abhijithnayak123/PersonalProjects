

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_DelRptPifProjection'
		)
BEGIN
	DROP PROCEDURE USP_EX_DelRptPifProjection;

	PRINT 'DROPPED USP_EX_DelRptPifProjection';
END
GO

SET QUOTED_IDENTIFIER ON
GO

/**********************************************************************************************                      
** Name : USP_EX_DelRptPifProjection  
** Old Name:     USP_EIS_RPT_PIF_PROJECTION_DelProc                      
** Short Desc: To retrieve PIF Projection of an Account for a specific year    
**                      
** Full Description: To retrieve PIF Projection of an Account  for a specific year    
**                              
** Input Arguments: @Account_ID, @Year, @userid    
**         
** Sample Call                      
**  EXEC USP_EIS_RPT_PIF_PROJECTION_DelProc       
   '<root><Item ID="1996" AccountID="301551"></Item>    
   <Item ID="1995" AccountID="301551"></Item>    
   <Item ID="1997" AccountID="301551"></Item>    
   <Item ID="2005" AccountID="301544"></Item>    
   </root>',1    
       
**             
** Return values: NONE    
**                      
**                      
** Standard declarations                      
**       SET LOCK_TIMEOUT         30000   -- 30 seconds                      
**                       
** Created By: Madhuri    
** Company   : Kaspick & Company                      
** Project   : Excelsior  - BeneReport                      
** Created DT: 06/17/2009                      
**                                  
**********************************************************************************************                
**       Change History                      
**********************************************************************************************                
** Date:    Author:   Bug #   Description:                           Rvwd                
** --------  -------- ------    -----------  ---------------------------   --------                
** 06/15/2009    Madhuri V         Created     
** 06/19/2009    Venugopal B                          Updated for xml Input    
** 07/13/2009    Venugopal B        ET #9948          Updated      
** 04/22/2014    Sanath             Modified  
  
** 22-MAY-2014 Mallikarjun EXCREQ    SP Name Renamed and Formatted  
*********************************************************************************************                      
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved                      
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION                      
*********************************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_DelRptPifProjection] (
	@XMLProjection XML,
	@UserID INT
	)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

		--  Variable Declarations  --          
		DECLARE @procname VARCHAR(60);
		DECLARE @ErrorMessage VARCHAR(1000);
		DECLARE @ErrorNumber INT;
		--DECLARE @idoc INT;  Delete         
		-- Variables used for error handling - uncomment if needed          
		DECLARE @val1 VARCHAR(30);
		DECLARE @val2 VARCHAR(30);

		--  Temp tables, Cursors, Table Variables  --          
		--  Variable Data Assignment  --          
		SET @procname = 'USP_EX_DelRptPifProjection';

		-- Body of procedure  --          
		DECLARE @Projection_Delete TABLE (
			ProjectionYear INT,
			CustomerAccountNumber CHAR(15)
			)

		INSERT INTO @Projection_Delete
		SELECT XMLDATA.item.value('@ID', 'int') AS ProjectionYear,
			XMLDATA.item.value('@AccountID', 'char(15)') AS AccountID
		FROM @XMLProjection.nodes('//root/Item') AS XMLDATA(item)

		-- Updating modified_user_id, deleted_user_id and modified_date to track the deleted information in audit trail table    
		UPDATE TBL_BR_PIFProjection
		SET Modified_User_ID = @UserID,
			Modified_Date = getdate(),
			Deleted_User_ID = @UserID
		WHERE CustomerAccountNumber IN (
				SELECT CustomerAccountNumber
				FROM @Projection_Delete
				)
			AND Year IN (
				SELECT ProjectionYear
				FROM @Projection_Delete
				)

		-- Deleting ExpectedIncome for the selected account for a particular year    
		DELETE
		FROM TBL_BR_PIFProjection
		WHERE CustomerAccountNumber IN (
				SELECT CustomerAccountNumber
				FROM @Projection_Delete
				)
			AND Year IN (
				SELECT ProjectionYear
				FROM @Projection_Delete
				)

		COMMIT TRANSACTION
	END TRY

	BEGIN CATCH
		SET @ErrorMessage = ERROR_MESSAGE();
		SET @ErrorNumber = ERROR_NUMBER();
		SET @val1 = '';
		SET @val2 = '';

		EXEC USP_EX_SYSErrorHandler @codename = @procname,
			@ErrorMessage = @ErrorMessage,
			@ErrorNumber = @ErrorNumber,
			@val1 = '',
			@val1str = 'USP_EX_DelRptPifProjection: Cannot Delete.',
			@val2 = '',
			@val2str = '';
	END CATCH
		-- End of procedure  --          
END
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_DelRptPifProjection'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_DelRptPifProjection';
END

