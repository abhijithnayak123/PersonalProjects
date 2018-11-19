

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_DelProfileTaxCondition'
		)
BEGIN
	DROP PROCEDURE USP_EX_DelProfileTaxCondition;

	PRINT 'DROPPED USP_EX_DelProfileTaxCondition';
END
GO

/**************************************************************************                          
* New Procedure Name : USP_EX_DelProfileTaxCondition  
* Old PROCEDURE NAME  : USP_EIS_EX_PROFILE_TAX_CONDITION_DELPROC                          
* DESCRIPTION     : DELETES ACCOUNT TAX CONDITIONS SUPPLIED IN XML AS PARAMETER    
*     CONDITION IDS ARE SENT IN XML DOCUMENT    
* INPUT           :               
* MODIFICATION LOG:               
*                                                      
* DATE   CREATED BY  DESCRIPTION                                                                      
*-------------------------------------------------------------------------                  
* 21-Sept-07 Chirag Parekh CREATED  
* 15-Apr-2014 Yugandhar EXCREQ 6.1     
* 23/05/2014 Mallikarjun  SP Name Renamed and Formatted  
****************************************************************************/
CREATE PROCEDURE USP_EX_DelProfileTaxCondition (
	@XMLDOC NVARCHAR(4000),
	@USER_ID INT
	)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

		--SET TRANSACTION ISOLATION LEVEL SNAPSHOT;    
		DECLARE @procname VARCHAR(60);
		DECLARE @ErrorMessage VARCHAR(1000);
		DECLARE @ErrorNumber INT;
		DECLARE @val1 VARCHAR(30);
		DECLARE @val2 VARCHAR(30);

		SET @procname = 'USP_EX_DelProfileTaxCondition';

		DECLARE @CONDITION_ID INT
		DECLARE @IDOC INT

		---load and parse the xml document---              
		EXEC sp_xml_preparedocument @IDOC OUTPUT,
			@XMLDOC

		DECLARE CUR_TAX_CONDITION CURSOR
		FOR
		(
				SELECT *
				FROM OPENXML(@IDOC, '/root/Item', 1) WITH (ID INT)
				)

		OPEN CUR_TAX_CONDITION

		FETCH
		FROM CUR_TAX_CONDITION
		INTO @CONDITION_ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
			UPDATE TBL_IRS_ProfileTaxCondition
			SET DELETED_USER_ID = @USER_ID
			WHERE TAX_CONDITION_ID = @CONDITION_ID

			DELETE
			FROM TBL_IRS_ProfileTaxCondition
			WHERE TAX_CONDITION_ID = @CONDITION_ID

			FETCH NEXT
			FROM CUR_TAX_CONDITION
			INTO @CONDITION_ID
		END

		CLOSE CUR_TAX_CONDITION

		DEALLOCATE CUR_TAX_CONDITION

		---remove xml document from memory---              
		EXEC sp_xml_removedocument @IDOC

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
			@val1str = 'Can not delete Conditions',
			@val2 = '',
			@val2str = '';

		ROLLBACK TRANSACTION
	END CATCH
		-- End of procedure  --        
END
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_DelProfileTaxCondition'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_DelProfileTaxCondition';
END

