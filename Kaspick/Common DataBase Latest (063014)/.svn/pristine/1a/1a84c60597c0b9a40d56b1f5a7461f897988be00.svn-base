

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_DelTrading'
		)
BEGIN
	DROP PROCEDURE USP_EX_DelTrading;

	PRINT 'DROPPED USP_EX_DelTrading';
END
GO

SET QUOTED_IDENTIFIER ON
GO

/******************************************************************************  
** Name : USP_EX_DelTrading
** Old Name:     USP_EIS_EX_TRADING_DelProc  
** Short Desc: SP WRITTEN TO DELETES THE ALERT EVENT FOR THE RESPECTIVE ALERT ID  
**  
** Full Description  
**        THIS SP HELPS WILL LOG A DELETE FLAG IN TBL_EIS_EX_ALERTEVENT_SUPPLEMENT TABLE FOR THE RESPECTIVE ALERTID'S WHICH ARE  
     PASSED AS A PARAMETER.  
**  
** Sample Call  
        USP_EIS_EX_TRADING_DelProc @XMLDATA = '<NewDataSet><TBL_DETAILS><AlertID>10001</AlertID><Deleteflag>1</Deleteflag><MODIDATE>2007-03-14 16:36:59.887</MODIDATE><MODIUSER>36</MODIUSER></TBL_DETAILS><TBL_DETAILS><ALERTID>10003</ALERTID><DELETEFLAG>1</
DELETEFLAG><MODIDATE>2007-03-14 16:36:59.887</MODIDATE><MODIUSER>36</MODIUSER></TBL_DETAILS></NewDataSet>', @XPATH = '/NewDataSet/TBL_DETAILS', @retval = 10  
**  
** Return values: NONE  
**  
**  
** Standard declarations  
**       SET LOCK_TIMEOUT         30000   -- 30 seconds  
**   
** Created By: N.R.AMARNATH  
** Company   : Kaspick & Company  
** Project   : Excelsior -- IM PROFILE  
** Created DT: 04/04/2007  
**              
*******************************************************************************  
**       Change History  
*******************************************************************************  
** Date:        Author:  Bug #     Description:                           Rvwd  
** --------     -------- ------    -------------------------------------- --------  
** 04/03/2014  Sanath   Modified   EXCREQ 3.1
** 23-MAY-2014  Mallikarjun     SP Name Renamed and Formatted
*******************************************************************************  
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved  
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION  
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_DelTrading] (
	@xmldata VARCHAR(max),
	@xpath VARCHAR(50),
	@retval INT OUTPUT
	)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

		--  Variable Declarations  --  
		DECLARE @procname VARCHAR(60);
		DECLARE @ErrorMessage VARCHAR(1000);
		DECLARE @ErrorNumber INT;
		-- Variables used for error handling -   
		DECLARE @val1 VARCHAR(Max);
		DECLARE @val2 VARCHAR(30);

		--  Variable Data Assignment  --  
		SET @procname = 'USP_EX_DelTrading';

		DECLARE @hdoc INT
		DECLARE @tab TABLE (
			alertid INT,
			deleteflag BIT,
			modidate DATETIME,
			modiuser VARCHAR(50)
			)

		EXEC sp_xml_preparedocument @hdoc OUTPUT,
			@xmldata

		INSERT INTO @Tab (
			alertid,
			deleteflag,
			modidate,
			modiuser
			)
		SELECT *
		FROM Openxml(@hdoc, @xpath, 2) WITH (
				alertid INT,
				deleteflag BIT,
				modidate DATETIME,
				modiuser VARCHAR(50)
				)

		IF EXISTS (
				SELECT alertid
				FROM TBL_BR_AlertEvent
				WHERE alertid IN (
						SELECT alertid
						FROM openxml(@hdoc, @xpath, 2) WITH (alertid INT)
						)
				)
		BEGIN
			--   --Deleted_User_Id column is updated in supplement table. This is for Audit TRail  
			UPDATE TBL_BR_AlertEvent
			SET modified_user_id = T.modiuser,
				modified_date = T.modidate,
				deleted_user_id = T.modiuser
			FROM TBL_BR_AlertEvent BrAlrtEvnt
			INNER JOIN @tab t ON BrAlrtEvnt.alertid = T.alertid

			-- Deleting data from TBL_EIS_EX_Alertevent_Supplement as per alert id  
			DELETE
			FROM TBL_BR_AlertEvent
			WHERE AlertId IN (
					SELECT alertid
					FROM @Tab
					)

			-- Deleting data from Alertevent as per alert id  
			DELETE
			FROM TBL_BR_AlertEvent
			WHERE AlertId IN (
					SELECT alertid
					FROM @Tab
					)

			EXEC SP_XML_REMOVEDOCUMENT @hdoc

			SET @retval = 1;

			SELECT @retval [Success];
		END
		ELSE
		BEGIN
			SET @retval = - 1;

			SELECT @retval [Failure];
		END

		COMMIT TRANSACTION
	END TRY

	BEGIN CATCH
		ROLLBACK TRANSACTION

		SET @ErrorMessage = ERROR_MESSAGE();
		SET @ErrorNumber = ERROR_NUMBER();
		SET @val1 = Cast(@xmldata AS VARCHAR(MAX));
		SET @val2 = Cast(@xpath AS VARCHAR(30));

		EXEC USP_EX_SYSErrorHandler @codename = @procname,
			@ErrorMessage = @ErrorMessage,
			@ErrorNumber = @ErrorNumber,
			@val1 = @val1,
			@val1str = 'xmldata',
			@val2 = @val2,
			@val2str = 'xpath';

		SET @retval = - 1;

		SELECT @retval [Error];
	END CATCH;
END
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_DelTrading'
		)
BEGIN
	PRINT 'CREATED USP_EX_DelTrading';
END

