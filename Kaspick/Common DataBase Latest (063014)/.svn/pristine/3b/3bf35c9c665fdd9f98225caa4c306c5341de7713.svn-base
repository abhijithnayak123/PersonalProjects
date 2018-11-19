IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetEnfAllocationCommentHistory'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetEnfAllocationCommentHistory;

	PRINT 'DROPPED USP_EX_GetEnfAllocationCommentHistory';
END
GO

/******************************************************************************                          
** New Name:     USP_EX_GetEnfAllocationCommentHistory                          
** Old Name:     USP_EIX_EnfAllocationCommentHistorySelproc                          
** Short Desc: To retrieve the Allocation Comment History Details                 
**                          
** Full Description: To retrieve the Allocation Comment History Details                 
**      
** Sample Call 
	USP_EX_GetEnfAllocationCommentHistory 'ACADW'
                            
** Input Arguments: NONE        
**             
** Sample Call                          
   EXEC USP_EIS_ENF_ALLOCATION_COMMENT_HISTORY_SELPROC 103938                
**                 
** Return values: NONE        
**                          
**                          
** Standard declarations                          
**       SET LOCK_TIMEOUT         30000   -- 30 seconds                          
**                           
** Created By: Soorya                   
** Company   : Kaspick & Company                          
** Project   : Excelsior  - Enfuego 3B                          
** Created DT: 11/10/2010                          
**                                      
*******************************************************************************                    
**       Change History                          
*******************************************************************************                    
** Date:        Author:  Bug #     Description:                           Rvwd                    
** --------  -------- ------    -------------------------------------- --------                    

** 3-Apr-2014  Yugandhar          EXCREQ 7.4  
** 22-May-2014  Sanath           Sp name renamed as per Kaspick naming convention standard 
*******************************************************************************                          
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved                          
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION                          
*******************************************************************************/
CREATE PROCEDURE USP_EX_GetEnfAllocationCommentHistory (@AccountID VARCHAR(14))
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
	SET @ProcName = 'USP_EX_GetEnfAllocationCommentHistory';

	-- Body of procedure  --              
	BEGIN TRY
		SELECT (
				CASE 
					WHEN DecsnCmntTyp2.TypeName = 'Objective'
						THEN 'Tactical'
					ELSE DecsnCmntTyp2.TypeName
					END
				) AS CommentLevel
			,DecsnCmnt.DecisionDate AS CreateDate
			,KsUsr.LoginName AS CreatedUser
			,DecsnCmnt.Comment AS Comments
		FROM TBL_INV_DecisionComment DecsnCmnt
		INNER JOIN TBL_INV_DecisionCommentTypeLink DecsnCmntTypLnk ON DecsnCmnt.DecisionCommentID = DecsnCmntTypLnk.DecisionCommentID
		INNER JOIN TBL_INV_DecisionCommentType DecsnCmntTyp ON DecsnCmntTyp.TypeID = DecsnCmntTypLnk.TypeID
		INNER JOIN TBL_KS_User KsUsr ON KsUsr.userid = DecsnCmnt.CommentUser
		INNER JOIN TBL_INV_AccountProfile AcntPrfl ON AcntPrfl.CustomerAccountNumber = @AccountID
		LEFT JOIN TBL_INV_DecisionCommentType DecsnCmntTyp2 ON DecsnCmntTyp2.TypeID = DecsnCmnt.OriginType
		WHERE DecsnCmntTypLnk.decisionTypeValue = AcntPrfl.CustomerAccountNumber
			AND DecsnCmntTyp.TypeName = 'Account'
		ORDER BY DecsnCmnt.DecisionDate DESC
	END TRY

	BEGIN CATCH
		SET @ErrorMessage = ERROR_MESSAGE();
		SET @ErrorNumber = ERROR_NUMBER();
		SET @val1 = '';
		SET @val2 = '';

		EXEC dbo.USP_EX_SYSErrorHandler @CodeName = @ProcName
			,@ErrorMessage = @ErrorMessage
			,@ErrorNumber = @ErrorNumber
			,@val1 = ''
			,@val1str = 'USP_EX_GetEnfAllocationCommentHistory: Cannot Select.'
			,@val2 = ''
			,@val2str = '';
	END CATCH
END
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetEnfAllocationCommentHistory'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetEnfAllocationCommentHistory';
END