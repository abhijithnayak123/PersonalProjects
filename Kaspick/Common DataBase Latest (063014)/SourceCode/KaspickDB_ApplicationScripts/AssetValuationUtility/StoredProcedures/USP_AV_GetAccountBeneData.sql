--WARNING! ERRORS ENCOUNTERED DURING SQL PARSING!
/****** Object:  StoredProcedure [dbo].[USP_AV_GetAccountBeneData]    Script Date: 06/12/2014 17:43:14 ******/
IF EXISTS (
		SELECT *
		FROM sys.objects
		WHERE object_id = OBJECT_ID(N'[dbo].[USP_AV_GetAccountBeneData]')
			AND type IN (
				N'P'
				,N'PC'
				)
		)
	DROP PROCEDURE [dbo].[USP_AV_GetAccountBeneData]
GO

/****** Object:  StoredProcedure [dbo].[USP_AV_GetAccountBeneData]    Script Date: 06/12/2014 17:43:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/******************************************************************************          
** Name   :   USP_AV_GetAccountBeneData          
** Short Desc : Put in Short Description          
**          
** Full Description          
**                  
**          
** Sample Call          
        EXEC USP_AV_GetAccountBeneData 
   -- parameters          
**          
** Return values: NONE          
**          
**          
** Standard declarations          
**       SET LOCK_TIMEOUT         30000   -- 30 seconds          
**           
** Created By : Tanuj     
** Company  :  Kaspick & Company          
** Project  :  BOI - Asset Valuation Utility          
** Created DT :  05/15/2014          
**                      
*******************************************************************************          
**       Change History          
*******************************************************************************          
** Date:        Author:  Bug #     Description:                           Rvwd          
** --------     -------- ------    -------------------------------------- --------          
** 05/15/2014   Tanuj             Created 
******************************************************************************          
** Copyright (C) 2007 Kaspick & Company, All Rights Reserved          
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION          
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_AV_GetAccountBeneData]
AS
BEGIN
	SELECT am.ManagerCode
		,am.CustomerAccountNumber AS Adventid
		,cm.ContactName AS CLIENT_FULLNAME
		,UAM.UDFAMColumn045 Account_fullname
	FROM SYN_IT_ContactMaster cm
	INNER JOIN SYN_IT_AllianceNumbers an ON cm.ContactID = an.ContactID
	INNER JOIN SYN_IT_AccountMaster am ON an.AllianceNumber = am.ManagerCode
	INNER JOIN SYN_IT_UDF_AccountMaster UAM ON am.CustomerAccountNumber = uam.CustomerAccountNumber_Key
	WHERE am.ActiveFlag = - 1
		AND am.ClosedFlag != - 1
	ORDER BY am.customerAccountNumber

	SELECT cmDonor.ContactName AS DonorName
		,carDonor.CustomerAccountNumber AS Adventid
	FROM SYN_IT_ContactMaster cmDonor
	INNER JOIN SYN_IT_ContactAccountRoles carDonor ON cmDonor.ContactID = carDonor.ContactID
		AND carDonor.ContactRoleCode = 24

	SELECT CMBene.ContactName AS BeneficiaryName
		,carBene.CustomerAccountNumber AS Adventid
	FROM SYN_IT_ContactMaster CMBene
	INNER JOIN SYN_IT_ContactAccountRoles carBene ON CMBene.ContactID = carBene.ContactID
		AND carBene.ContactRoleCode IN (
			21
			,37
			)
		AND CMBene.DateOfDeath IS NULL
END
