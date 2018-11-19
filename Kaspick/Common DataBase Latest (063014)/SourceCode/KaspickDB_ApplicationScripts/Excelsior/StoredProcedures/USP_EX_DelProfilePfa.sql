

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_DelProfilePfa'
		)
BEGIN
	DROP PROCEDURE USP_EX_DelProfilePfa;

	PRINT 'DROPPED USP_EX_DelProfilePfa';
END
GO

/******************************************************************************    
** Name : USP_EX_DelProfilePfa  
** Old Name:     USP_EIS_EX_PROFILE_PFA_DelProc     
** Short Desc: Put in Short Description    
**    
** Full Description    
**            
**    
** Sample Call    
        EXEC USP_EIS_EX_PROFILE_PFA_DelProc  '<root><Item ID="1001"></Item></root>','CLIENT',30    
**    
** Return values: NONE    
**    
**    
** Standard declarations    
**       SET LOCK_TIMEOUT         30000   -- 30 seconds    
**     
** Created By: Venugopal. B    
** Company   : Kaspick & Company    
** Project   : Excelsior    
** Created DT: 05/14/2007    
**                
*******************************************************************************    
**       Change History    
*******************************************************************************    
** Date:        Author:  Bug #     Description:                           Rvwd    
** --------     -------- ------    -------------------------------------- --------    
   05/14/2007 Venugopal. B           
   05/31/2007 Venugopal    updated for deleted user id    
   04/18/2014  Sanath     Req EXCREQ 6.4 CGA registration tab        
   23/05/2013 Mallikarjun SP Name Renamed and Formatted  
*******************************************************************************    
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved    
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION    
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_DelProfilePfa] @xmldoc NVARCHAR(4000),
	@ENTITY_TYPE VARCHAR(20),
	@USER_ID INT
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

		--  Variable Declarations  --    
		DECLARE @procname VARCHAR(60);
		DECLARE @ErrorMessage VARCHAR(1000);
		DECLARE @ErrorNumber INT;
		-- Variables used for error handling - uncomment if needed    
		DECLARE @val1 VARCHAR(30);
		DECLARE @val2 VARCHAR(30);

		--  Variable Data Assignment  --    
		SET @procname = 'USP_EX_DelProfilePfa';

		-- Body of procedure  --    
		--  Transactions    
		DECLARE @idoc INT,
			@PFA_ID INT,
			@REQUIRED_LATER_ID INT,
			@ENTITY_TYPE_ID INT,
			@ENTITY_ID VARCHAR(20),
			@PAGE_NAME VARCHAR(100),
			@FIELD_NAME VARCHAR(100)

		---LOAD AND PARSE THE XML DOCUMENT---    
		EXEC sp_xml_preparedocument @idoc OUTPUT,
			@xmldoc

		UPDATE TBL_EX_ProfilePFA
		SET MODIFIED_DATE = GETDATE(),
			MODIFIED_USER_ID = @USER_ID,
			DELETED_USER_ID = @USER_ID
		WHERE PFA_ID IN (
				SELECT *
				FROM OPENXML(@idoc, '/root/Item', 1) WITH (ID INT)
				)

		DELETE
		FROM TBL_EX_ProfilePFA
		WHERE PFA_ID IN (
				SELECT *
				FROM OPENXML(@idoc, '/root/Item', 1) WITH (ID INT)
				)

		-- Start    
		SELECT @ENTITY_TYPE_ID = LstItm.LISTITEMID
		FROM TBL_LISTITEM LstItm
		INNER JOIN TBL_LISTTYPE LstTyp ON LstItm.LISTTYPEID = LstTyp.LISTTYPEID
		WHERE LstTyp.LISTTYPENAME = 'ENTITY'
			AND LstItm.LISTITEMNAME = @ENTITY_TYPE

		COMMIT TRANSACTION
	END TRY

	BEGIN CATCH
		ROLLBACK TRANSACTION

		SET @ErrorMessage = ERROR_MESSAGE();
		SET @ErrorNumber = ERROR_NUMBER();
		SET @val1 = '';
		SET @val2 = '';

		EXEC USP_EX_SYSErrorHandler @codename = @procname,
			@ErrorMessage = @ErrorMessage,
			@ErrorNumber = @ErrorNumber,
			@val1 = '',
			@val1str = 'Can not delete PFAs',
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
			AND NAME = 'USP_EX_DelProfilePfa'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_DelProfilePfa';
END

