IF EXISTS (
		SELECT 1
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_FASB_GetCalculationMethodOverrideList'
		)
BEGIN
	DROP PROCEDURE USP_EX_FASB_GetCalculationMethodOverrideList;

	PRINT 'DROPPED USP_EX_FASB_GetCalculationMethodOverrideList';
END
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/******************************************************************************            
** Name	      : [USP_EX_FASB_GetCalculationMethodOverrideList]
** Short Desc : Gets the list CalculationMethod which can be used to override.
**            
** Full Description            
**                
**            
** Return values: NONE            
**     
** Sample Call  
**      
** EXEC [USP_EX_FASB_GetCalculationMethodOverrideList]
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

CREATE PROCEDURE [dbo].[USP_EX_FASB_GetCalculationMethodOverrideList]
AS
BEGIN
	SELECT
		CONVERT(INT, Rank() OVER (ORDER BY ListItemName ASC)) AS 'Value'
		,LTRIM(RTRIM(ListItemName)) AS 'Text'
	FROM
		VW_ListItem
	WHERE
		ListTypeName = 'Calculation Method' -- List Of ValidAccountTypes that can be overriden. To be confirmed by Donald.			
END
GO