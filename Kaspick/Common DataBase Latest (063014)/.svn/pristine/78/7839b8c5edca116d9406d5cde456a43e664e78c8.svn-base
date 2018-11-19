IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetDeliverableClientReport'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetDeliverableClientReport;

	PRINT 'DROPPED USP_EX_GetDeliverableClientReport';
END
GO

SET QUOTED_IDENTIFIER ON
GO

/******************************************************************************                        
** New Name:     USP_EX_GetDeliverableClientReport
** Old Name:     USP_EIS_DLV_ClientReport_DTLSelProc                        
** Short Desc: Put in Short Description                        
**                        
** Full Description                        
**        More detailed description if necessary                        
**                        
** Sample Call                        
+**                        
** Return values: NONE                        
           USP_EIS_DLV_ClientReport_DTLSelProc     100023,0    
           USP_EIS_DLV_ClientReport_DTLSelProc    4,0     
                    
**                        
** Standard declarations                        
**       SET NOCOUNT             ON                        
**       SET LOCK_TIMEOUT         30000   -- 30 seconds                        
**                         
** Created By: Tanuj Gupta                        
** Company   : Kaspick & Company                        
** Project   : Excelsior -- IM PROFILE                        
** Created DT: 04/11/2011                        
**                                    
*******************************************************************************                        
**       Change History                        
*******************************************************************************                        
** Date:        Author:  Bug #     Description:                           Rvwd                        
** --------     -------- ------    -------------------------------------- --------                        
**26-June-2012   Anand Kumar      Added condition for TrackInReportoire in DLV_Deliverable table for Website 2 Sprint 4  
**6-6-2013		Tanuj Gupta			Improved performance by removing inline query.
** 03-Mar-2014  Mallikarjun        EXCREQ 5.4
** 22-May-2014  Sanath            Sp name renamed as per Kaspick naming convention standard
*******************************************************************************                        
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved                        
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION                        
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetDeliverableClientReport] --'AM' ,0                        
	(
	@ManagerCode VARCHAR(15)
	,@CategoryID INT
	)
AS
BEGIN
	--
	DECLARE @ClientServiceOfferingID INT
   print 'p1'
	--SELECT @ClientServiceOfferingID = SERVICE_OFFERING_ID FROM TBL_BR_CompanyLogo
	SELECT @ClientServiceOfferingID = DlvrbleSrvcOfrng.ServiceOfferingID
	FROM tbl_DLV_DeliverableServiceOffering DlvrbleSrvcOfrng
	INNER JOIN TBL_DLV_ManagerCodeDeliverable MgrCodDlvrble ON MgrCodDlvrble.DeliverableID = DlvrbleSrvcOfrng.DeliverableID
	WHERE MgrCodDlvrble.ManagerCode = @ManagerCode

	---
	DECLARE @CharReplace VARCHAR(10);

	SET @CharReplace = '&#146;';-- &#146; is the HTML Escape character sequence for Single Quote (')    

	DECLARE @Imagepath VARCHAR(200)

	SET @Imagepath = '<img height=''13'' src=''../../images/icon-png.png'' style=''cursor: hand;border:0'' width=''13'' />'
  print '2'
	SELECT Dlvrble.DeliverableID
		,Dlvrble.DeliverableName + (
			CASE 
				WHEN isnull(Dlvrble.DeliverableInternalDescription, '') <> ''
					THEN ' <img height=''13'' Title=''' + REPLACE(ISNULL(Dlvrble.DeliverableInternalDescription, ''), CHAR(39), @CharReplace) + ''' src=''../../images/icon-png.png'' style=''cursor: hand;border:0'' width=''13'' />'
				ELSE @Imagepath
				END
			) AS DeliverableName
		,ClientDeliverableID = (MgrCodDlvrble.ManagerCodeDeliverableID)
		,(
			CASE 
				WHEN isnull((
							SELECT ServiceOfferingID
							FROM TBL_DLV_DeliverableServiceOffering
							WHERE DeliverableID = Dlvrble.DeliverableID
								AND ServiceOfferingID = @ClientServiceOfferingID
							), '') = ''
					THEN '(Non-Standard)'
				ELSE ''
				END
			) AS ServiceOffering
		,dbo.FN_GetClientDeliverableValue('Fiscal', 'Year Type', Dlvrble.DeliverableID, @ManagerCode) AS Fiscal
		,dbo.FN_GetClientDeliverableValue('Calendar', 'Year Type', Dlvrble.DeliverableID, @ManagerCode) AS Calender
		,dbo.FN_GetClientDeliverableValue('Other', 'Year Type', Dlvrble.DeliverableID, @ManagerCode) AS OtherYT
		,dbo.FN_GetClientDeliverableValue('Annually', 'Annual Frequency', Dlvrble.DeliverableID, @ManagerCode) AS Annually
		,dbo.FN_GetClientDeliverableValue('Semi-Annually', 'Annual Frequency', Dlvrble.DeliverableID, @ManagerCode) AS SemiAnnually
		,dbo.FN_GetClientDeliverableValue('Quarterly', 'Annual Frequency', Dlvrble.DeliverableID, @ManagerCode) AS Quaterly
		,dbo.FN_GetClientDeliverableValue('Monthly', 'Annual Frequency', Dlvrble.DeliverableID, @ManagerCode) AS Monthly
		,dbo.FN_GetClientDeliverableValue('other', 'Annual Frequency', Dlvrble.DeliverableID, @ManagerCode) AS otherF
		,Dlvrble.SettingsLinkCode AS SettingsLinkCode
		,dbo.FN_GetClientDeliverableValue('Fiscal', 'ClientDeliverableYearType', Dlvrble.DeliverableID, @ManagerCode) AS ClientFiscal
		,dbo.FN_GetClientDeliverableValue('Calendar', 'ClientDeliverableYearType', Dlvrble.DeliverableID, @ManagerCode) AS ClientCalender
		,dbo.FN_GetClientDeliverableValue('Other', 'ClientDeliverableYearType', Dlvrble.DeliverableID, @ManagerCode) AS ClientOtherYT
		,ClientOtherTypeDate = (
			SELECT Cast(OtherMonth AS VARCHAR) + '/' + Cast(OtherDayofMonth AS VARCHAR)
			FROM TBL_DLV_ManagerCodeDeliverableYearType
			WHERE ManagerCodeDeliverableID IN (MgrCodDlvrble.ManagerCodeDeliverableID)
				AND DeliverableYearTypeID = dbo.FN_GetClientDeliverableValue('Other', 'ClientDeliverableYearType', Dlvrble.DeliverableID, @ManagerCode)
			)
		,dbo.FN_GetClientDeliverableValue('Annually', 'ClientDeliverableFrequency', Dlvrble.DeliverableID, @ManagerCode) AS ClientAnnually
		,dbo.FN_GetClientDeliverableValue('Semi-Annually', 'ClientDeliverableFrequency', Dlvrble.DeliverableID, @ManagerCode) AS ClientSemiAnnually
		,dbo.FN_GetClientDeliverableValue('Quarterly', 'ClientDeliverableFrequency', Dlvrble.DeliverableID, @ManagerCode) AS ClientQuaterly
		,dbo.FN_GetClientDeliverableValue('Monthly', 'ClientDeliverableFrequency', Dlvrble.DeliverableID, @ManagerCode) AS ClientMonthly
		,dbo.FN_GetClientDeliverableValue('other', 'ClientDeliverableFrequency', Dlvrble.DeliverableID, @ManagerCode) AS ClientOtherF
		,ViewRecipientsCount = (
			SELECT count(*)
			FROM TBL_DLV_ManagerCodeDeliverableEmployee
			WHERE ManagerCodeDeliverableID IN (MgrCodDlvrble.ManagerCodeDeliverableID)
			)
		,OtherImage = (
			CASE 
				WHEN isnull((
							SELECT OtherYearTypeDescription
							FROM TBL_DLV_DeliverableYearType
							WHERE DeliverableID = Dlvrble.DeliverableID
								AND isnull(OtherYearTypeDescription, '') <> ''
							), '') != ''
					THEN '<img height=''13'' src=''../../images/icon-png.png'' Title=''' + (
							SELECT REPLACE(ISNULL(OtherYearTypeDescription, ''), CHAR(39), @CharReplace)
							FROM TBL_DLV_DeliverableYearType
							WHERE DeliverableID = Dlvrble.DeliverableID
								AND isnull(OtherYearTypeDescription, '') <> ''
							) + ''' style=''cursor: hand;border:0'' width=''13'' />'
				ELSE @Imagepath
				END
			)
		,FreqOtherImage = (
			CASE 
				WHEN isnull((
							SELECT OtherFrequencyDescription
							FROM TBL_DLV_DeliverableFrequency
							WHERE DeliverableID = Dlvrble.DeliverableID
								AND isnull(OtherFrequencyDescription, '') <> ''
							), '') != ''
					THEN '<img height=''13'' src=''../../images/icon-png.png'' Title=''' + (
							SELECT REPLACE(ISNULL(OtherFrequencyDescription, ''), CHAR(39), @CharReplace)
							FROM TBL_DLV_DeliverableFrequency
							WHERE DeliverableID = Dlvrble.DeliverableID
								AND isnull(OtherFrequencyDescription, '') <> ''
							) + ''' style=''cursor: hand;border:0'' width=''13'' />'
				ELSE @Imagepath
				END
			)
		,DeliverbaleTypeName = (
			SELECT DeliverableTypeName
			FROM TBL_DLV_DeliverableType
			WHERE DeliverableTypeID = Dlvrble.DeliverableTypeID
			)
	FROM TBL_DLV_Deliverable Dlvrble
	LEFT JOIN TBL_DLV_ManagerCodeDeliverable MgrCodDlvrble ON Dlvrble.DeliverableID = MgrCodDlvrble.DeliverableID
		AND MgrCodDlvrble.ManagerCode = @ManagerCode
	WHERE isnull(Dlvrble.Parent_DeliverableID, 0) = 0
		AND IsActive = 1
		AND Dlvrble.DeliverableTypeID IN (
			SELECT DeliverableTypeID
			FROM tbl_Dlv_DeliverableType
			WHERE DeliverableTypeName IN (
					'Report'
					,'Report Package'
					,'Report in a Package'
					)
			)
		AND @CategoryID = (
			CASE 
				WHEN isnull(@CategoryID, 0) = 0
					THEN 0
				ELSE Dlvrble.DeliverableCategoryListID
				END
			)
	ORDER BY Dlvrble.DeliverableName ASC
END

SELECT Dlvrble.DeliverableID
	,Dlvrble.DeliverableName + (
		CASE 
			WHEN isnull(Dlvrble.DeliverableInternalDescription, '') <> ''
				THEN ' <img height=''13'' Title=''' + REPLACE(ISNULL(Dlvrble.DeliverableInternalDescription, ''), CHAR(39), @CharReplace) + ''' src=''../../images/icon-png.png'' style=''cursor: hand;border:0;border:0'' width=''13'' />'
			ELSE @Imagepath
			END
		) AS DeliverableName
	,Parent_DeliverableID
	,FreqViewRecipientsCount = (
		SELECT count(*)
		FROM TBL_DLV_ManagerCodeDeliverableEmployee
		WHERE ManagerCodeDeliverableID IN (
				SELECT ManagerCodeDeliverableID
				FROM TBL_DLV_ManagerCodeDeliverable
				WHERE MgrCodDlvrble.ManagerCode = @ManagerCode
					AND DeliverableID = Dlvrble.DeliverableID
				)
		)
	,ClientDeliverableID = MgrCodDlvrble.ManagerCodeDeliverableID
	,ClientDeliverablesLevel2 = (
		SELECT ManagerCodeDeliverableLevel2
		FROM TBL_DLV_ManagerCodeDeliverableReportInPackage MgrCodDlvrbleRptPkg
		INNER JOIN TBL_DLV_ManagerCodeDeliverable MgrCodDlvrble ON MgrCodDlvrble.ManagerCodeDeliverableID = MgrCodDlvrbleRptPkg.ManagerCodeDeliverableID
			AND MgrCodDlvrble.ManagerCode = @ManagerCode
		WHERE MgrCodDlvrbleRptPkg.DeliverableID = Dlvrble.DeliverableID
		)
FROM TBL_DLV_Deliverable Dlvrble
LEFT JOIN TBL_DLV_ManagerCodeDeliverable MgrCodDlvrble ON Dlvrble.Parent_DeliverableID = MgrCodDlvrble.DeliverableID
	AND MgrCodDlvrble.ManagerCode = @ManagerCode
WHERE isnull(Dlvrble.Parent_DeliverableID, 0) <> 0
	AND Dlvrble.Parent_DeliverableID = (
		SELECT DeliverableID
		FROM TBL_DLV_Deliverable
		WHERE DeliverableID = Dlvrble.Parent_DeliverableID
			AND IsActive = 1
		)
	AND @CategoryID = (
		CASE 
			WHEN isnull(@CategoryID, 0) = 0
				THEN 0
			ELSE Dlvrble.DeliverableCategoryListID
			END
		)
GO

SET NOCOUNT OFF;

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetDeliverableClientReport'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetDeliverableClientReport';
END