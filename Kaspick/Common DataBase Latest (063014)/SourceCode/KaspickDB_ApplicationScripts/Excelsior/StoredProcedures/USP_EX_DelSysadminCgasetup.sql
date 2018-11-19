

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_DelSysadminCgasetup'
		)
BEGIN
	DROP PROCEDURE USP_EX_DelSysadminCgasetup;

	PRINT 'DROPPED PROCEDURE USP_EX_DelSysadminCgasetup';
END
GO

SET QUOTED_IDENTIFIER ON
GO

/*********************************************************************************************************************                                                   
* Procedure Name  : USP_EX_DelSysadminCgasetup
* Old Procedure Name  : USP_EIS_EX_SYSADMIN_CGASETUP_DelProc        
* Description     :           
*                
* Input Parameter :    
*                                      
* Modification Log                                                       
*                                      
* Date       Modified By  Description                                                      
*--------------------------------------------------------------------------------------------------------------------                                                   
* 29-Jan-06  Venugopal.B   Created  
* 09-Apr-14  Sanath         Changed table name for Excelsior Prime project for req EXCREQ3.1     
* 23-MAY-2014  Mallikarjun     SP Name Renamed and Formatted        
*********************************************************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_DelSysadminCgasetup] (
	@XMLDOC NVARCHAR(4000),
	@USER_ID INT,
	@CSV_UNDELETED VARCHAR(4000) OUTPUT
	)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

		DECLARE @SETUP_ID INT

		SET @CSV_UNDELETED = '' --INITIALIZING THE VALUE      

		DECLARE @IDOC INT

		---load and parse the xml document---      
		EXEC sp_xml_preparedocument @IDOC OUTPUT,
			@XMLDOC

		DECLARE CUR_CGA_SETUP CURSOR
		FOR
		(
				SELECT *
				FROM OPENXML(@IDOC, '/ROOT/ITEM', 1) WITH (ID INT)
				)

		OPEN CUR_CGA_SETUP

		FETCH
		FROM CUR_CGA_SETUP
		INTO @SETUP_ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
			IF NOT EXISTS (
					SELECT CGA_SETUP_ID
					FROM TBL_EX_ProfilePFA
					WHERE CGA_SETUP_ID = @SETUP_ID
					)
			BEGIN
				UPDATE TBL_BR_CGASetup
				SET MODIFIED_USER_ID = @USER_ID,
					MODIFIED_DATE = GETDATE(),
					DELETED_USER_ID = @USER_ID
				WHERE SETUP_ID = @SETUP_ID

				DELETE
				FROM TBL_BR_CGASetup
				WHERE SETUP_ID = @SETUP_ID
			END
			ELSE
			BEGIN
				IF @CSV_UNDELETED = ''
					SET @CSV_UNDELETED = ISNULL((
								SELECT LstItm.LISTITEMNAME
								FROM TBL_BR_CGASetup BrCgaStp
								INNER JOIN TBL_LISTITEM LstItm ON LstItm.LISTITEMID = BrCgaStp.STATE_TYPE_ID
								WHERE SETUP_ID = @SETUP_ID
								), '')
				ELSE
					SET @CSV_UNDELETED = @CSV_UNDELETED + ',' + ISNULL((
								SELECT LstItm.LISTITEMNAME
								FROM TBL_BR_CGASetup BrCgaStp
								INNER JOIN TBL_ListItem LstItm ON LstItm.LISTITEMID = BrCgaStp.STATE_TYPE_ID
								WHERE SETUP_ID = @SETUP_ID
								), '')
			END

			FETCH NEXT
			FROM CUR_CGA_SETUP
			INTO @SETUP_ID
		END

		CLOSE CUR_CGA_SETUP

		DEALLOCATE CUR_CGA_SETUP

		---remove xml document from memory---      
		EXEC sp_xml_removedocument @IDOC

		COMMIT TRANSACTION
	END TRY

	BEGIN CATCH
		ROLLBACK TRANSACTION

		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT @ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE();

		RAISERROR (
				@ErrorMessage,
				-- Message text.
				@ErrorSeverity,
				-- Severity.
				@ErrorState -- State.
				);
	END CATCH
END
	
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_DelSysadminCgasetup'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_DelSysadminCgasetup';
END

