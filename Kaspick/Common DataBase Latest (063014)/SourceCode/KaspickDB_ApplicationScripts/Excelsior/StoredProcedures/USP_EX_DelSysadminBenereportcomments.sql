IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_DelSysadminBenereportcomments'
		)
BEGIN
	DROP PROCEDURE USP_EX_DelSysadminBenereportcomments;

	PRINT 'DROPPED USP_EX_DelSysadminBenereportcomments';
END
GO

/******************************************************************************  
** Name:     USP_EX_DelSysadminBenereportcomments  
   Old Name: USP_EIS_EX_SYSADMIN_BENEREPORTCOMMENTS_DelProc
** Short Desc: SP WRITTEN TO DELETES THE BENE REPORT COMMENTS FOR THE RESPECTIVE COMMENT ID  
**  
** Full Description  
**        THIS SP HELPS WILL LOG A DELETE FLAG IN TBL_EIS_EX_BENEREPORTCOMMENTS_SUPPLEMENT TABLE FOR THE RESPECTIVE COMMENTID'S WHICH ARE  
     PASSED AS A PARAMETER.  
**  
** Sample Call  
        USP_EIS_EX_SYSADMIN_BENEREPORTCOMMENTS_DelProc @XMLDATA = '<NewDataSet><TBL_DETAILS><COMMENTID>261</COMMENTID><DELETEFLAG>1</DELETEFLAG><MODIDATE>2007-03-14 16:36:59.887</MODIDATE><MODIUSER>36</MODIUSER></TBL_DETAILS><TBL_DETAILS><COMMENTID>260</C
OMMENTID><DELETEFLAG>1</DELETEFLAG><MODIDATE>2007-03-14 16:36:59.887</MODIDATE><MODIUSER>36</MODIUSER></TBL_DETAILS></NewDataSet>', @XPATH = '/NewDataSet/TBL_DETAILS'  
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
** Created DT: 03/12/2007  
**              
*******************************************************************************  
**       Change History  
*******************************************************************************  
** Date:        Author:  Bug #     Description:                           Rvwd  
** --------     -------- ------    -------------------------------------- --------  
** 23-MAY-2014  Mallikarjun     SP Name Renamed and Formatted 
*******************************************************************************  
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved  
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION  
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_DelSysadminBenereportcomments] (
	@xmldata VARCHAR(max),
	@xpath VARCHAR(50)
	)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

		--  Initial Set statements  --  
		--SET NOCOUNT ON;  
		--SET LOCK_TIMEOUT                30000;   -- 30 seconds  
		--SET TRANSACTION ISOLATION LEVEL SNAPSHOT;  
		--  Variable Declarations  --  
		DECLARE @procname VARCHAR(60);
		DECLARE @ErrorMessage VARCHAR(1000);
		DECLARE @ErrorNumber INT;
		-- Variables used for error handling -   
		DECLARE @val1 VARCHAR(Max);
		DECLARE @val2 VARCHAR(30);

		--  Temp tables, Cursors, Table Variables  --  
		--  Variable Data Assignment  --  
		SET @procname = 'USP_EX_DelSysadminBenereportcomments';

		DECLARE @hdoc INT
		DECLARE @tab TABLE (
			commentid INT,
			deleteflag BIT,
			modidate DATETIME,
			modiuser VARCHAR(50)
			)

		EXEC SP_XML_PREPAREDOCUMENT @hdoc OUTPUT,
			@xmldata

		INSERT INTO @tab (
			commentid,
			deleteflag,
			modidate,
			modiuser
			)
		SELECT *
		FROM OPENXML(@hdoc, @xpath, 2) WITH (
				commentid INT,
				deleteflag BIT,
				modidate DATETIME,
				modiuser VARCHAR(50)
				)

		DECLARE @ModUserID VARCHAR(50),
			@ModDate DATETIME

		SELECT @ModUserID = modiuser,
			@ModDate = modidate
		FROM @tab

		IF EXISTS (
				SELECT brcommentid
				FROM TBL_INV_BeneReportComment
				WHERE brcommentid IN (
						SELECT commentid
						FROM openxml(@hdoc, @xpath, 2) WITH (commentid INT)
						)
				)
		BEGIN
			--Deleted_User_Id column is updated in supplement table. This is for Audit TRail  
			UPDATE TBL_INV_BeneReportComment
			SET ModifiedBy = @ModUserID,
				ModifiedDate = @ModDate,
				DeletedBy = @ModUserID
			WHERE brcommentid IN (
					SELECT commentid
					FROM openxml(@hdoc, @xpath, 2) WITH (commentid INT)
					)

			--DELETES THE VALUE FROM THE COMMENT LINKAGE TABLE FOR THE RESPECTIVE COMMENTID  
			DELETE
			FROM TBL_BR_CommentLinkage
			WHERE commentid IN (
					SELECT commentid
					FROM openxml(@hdoc, @xpath, 2) WITH (commentid INT)
					)

			--DELETES THE VALUE FROM THE BENEREPORT SUPPLEMENT TABLE FOR THE RESPECTIVE COMMENTID  
			DELETE
			FROM TBL_INV_BeneReportComment
			WHERE brcommentid IN (
					SELECT commentid
					FROM openxml(@hdoc, @xpath, 2) WITH (commentid INT)
					)

			--DELETES THE VALUE FROM THE BENEREPORT TABLE FOR THE RESPECTIVE COMMENTID  
			-- Delete from TBL_INV_BeneReportComment   
			-- where  brcommentid in (Select commentid from openxml(@hdoc, @xpath, 2) With (commentid int ))  
			EXEC SP_XML_REMOVEDOCUMENT @hdoc
		END
		ELSE
			PRINT 'CommentID Does not Exists'

		COMMIT TRANSACTION;
	END TRY

	BEGIN CATCH
		ROLLBACK TRANSACTION;

		SET @ErrorMessage = ERROR_MESSAGE();
		SET @ErrorNumber = ERROR_NUMBER();
		SET @val1 = Cast(@XMLDATA AS VARCHAR(MAX));
		SET @val2 = Cast(@XPATH AS VARCHAR(30));

		EXEC USP_EX_SYSErrorHandler @codename = @procname,
			@ErrorMessage = @ErrorMessage,
			@ErrorNumber = @ErrorNumber,
			@val1 = @val1,
			@val1str = 'xmldata ',
			@val2 = @val2,
			@val2str = 'xpath';
	END CATCH
END
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_DelSysadminBenereportcomments'
		)
BEGIN
	PRINT 'CREATED USP_EX_DelSysadminBenereportcomments';
END

