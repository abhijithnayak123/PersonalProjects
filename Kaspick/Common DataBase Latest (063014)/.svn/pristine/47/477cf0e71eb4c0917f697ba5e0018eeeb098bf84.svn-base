IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetAccountInvestmentportfolio'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetAccountInvestmentportfolio;

	PRINT 'DROPPED USP_EX_GetAccountInvestmentportfolio';
END
GO

/******************************************************************************        
** New Name:     USP_EX_GetAccountInvestmentportfolio        
** Old Name:     USP_EIS_EX_ACCOUNT_INVESTMENTPORTFOLIO_SelProc        

** Short Desc: Put in Short Description        
**        
** Full Description        
**        More detailed description if necessary        
**        
** Sample Call        
        USP_EIS_EX_ACCOUNT_INVESTMENTPORTFOLIO_SelProc  '100% Mutual Funds'        
*         
*        
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
** <mm/dd/yyyy>   
   06/19/2007   Rahul    5502    To Display records sorted by portfolio code       
** 2-Apr-2014 Yugandhar           EXCREQ 7.4
** 22-May-2014  Sanath           Sp name renamed as per Kaspick naming convention standard 
*******************************************************************************        
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved        
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION        
*******************************************************************************/
CREATE PROCEDURE USP_EX_GetAccountInvestmentportfolio --'CCRAF'        
	(@portfoliocode VARCHAR(50))
AS
--  Variable Declarations  --        
DECLARE @ProcName VARCHAR(60);
DECLARE @ErrorMessage VARCHAR(1000);
DECLARE @ErrorNumber INT;

BEGIN
	IF (
			(@PORTFOLIOCODE <> '')
			OR (@PORTFOLIOCODE <> NULL)
			)
		SELECT PortfolioCode [PORTFOLIO CODE]
			,PortfolioDescription [PORTFOLIO DESCRIPTION]
		FROM TBL_INV_BeneReportComment
		WHERE PortfolioCode = @portfoliocode
	ELSE
		SELECT BeneRptCmnt.PortfolioCode [PORTFOLIO CODE]
			,BeneRptCmnt.PortfolioDescription [PORTFOLIO DESCRIPTION]
		FROM TBL_INV_BeneReportComment BeneRptCmnt
		INNER JOIN TBL_INV_BeneReportComment BeneRptCmnts ON BeneRptCmnt.BrCommentID = BeneRptCmnts.BRCommentID
		ORDER BY BeneRptCmnt.PortfolioCode
END
	-- End of procedure  --        
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetAccountInvestmentportfolio'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetAccountInvestmentportfolio';
END