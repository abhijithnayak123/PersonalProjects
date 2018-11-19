IF EXISTS (
		SELECT 1
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_FASB_GetCustAccNumByMgrCd'
		)
BEGIN
	DROP PROCEDURE USP_EX_FASB_GetCustAccNumByMgrCd;

	PRINT 'DROPPED USP_EX_FASB_GetCustAccNumByMgrCd';
END
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/******************************************************************************            
** Name	      : [USP_EX_FASB_GetCustAccNumByMgrCd]
** Short Desc : Gets the list AccountNumbers which can be overriden for calculationMethod
**            
** Full Description            
**                
**            
** Return values: NONE            
**     
** Sample Call  
**      
** EXEC [USP_EX_FASB_GetCustAccNumByMgrCd] 'ACL'
**             
** Created By : Srikanth R
** Company  :  Kaspick & Company            
** Project  :  Excelsior - FASB            
** Created DT :  08/12/2014            
**                        
*******************************************************************************            
**       Change History            
*******************************************************************************            
** Date:			Author:  Bug #		Description:                           Rvwd            
** ------------		-------- ------		-------------------------------------- --------            
** 08/13/2014		Srikanth R			Changes for FASB BOI
******************************************************************************            
** Copyright (C) 2007 Kaspick & Company, All Rights Reserved            
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION            
*******************************************************************************/

CREATE PROCEDURE [dbo].[USP_EX_FASB_GetCustAccNumByMgrCd]
	@ClientID VARCHAR(4)
AS
BEGIN
	SELECT DISTINCT
		--CONVERT(INT, Rank() OVER (ORDER BY AM.CustomerAccountNumber ASC)) AS 'Value'
		LTRIM(RTRIM(AM.CustomerAccountNumber)) AS 'AdventID'
		,AM.ManagerCode
		,LTRIM(RTRIM(AM.AccountTypeCode)) AS 'AccountType'
	FROM
		SYN_IT_AccountMaster AM
	INNER JOIN
		TBL_BR_FASBTrustTypeTranslation TRANS
	ON
		AM.AccountTypeCode = TRANS.InnotrustAccountType
	WHERE
		AM.ManagerCode = @ClientID
		AND AM.AccountTypeCode NOT IN ('GAP', 'PIF')
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
