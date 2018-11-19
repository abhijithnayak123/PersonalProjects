IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetEnfInvestmentAllocation'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetEnfInvestmentAllocation;

	PRINT 'DROPPED USP_EX_GetEnfInvestmentAllocation';
END
GO

/******************************************************************************                
** New Name : USP_EX_GetEnfInvestmentAllocation
** Old Name : USP_EIS_ENF_INVESTMENT_ALLOCATION_SelProc              
** Short Desc: Put in Short Description                
**                
** Full Description                
**        More detailed description if necessary                
**                
** Sample Call                
EXEC <USP_EX_GetEnfInvestmentAllocation>  -- parameters                
**                
** Return values: NONE                
**                
**                
** Standard declarations                
**       SET NOCOUNT             ON                
**       SET LOCK_TIMEOUT         30000   -- 30 seconds                
**                 
** Created By: Saravanan               
** Company   : Kaspick & Company                
** Project   : Excelsior                
** Created DT:                 
**                            
*******************************************************************************                
**       Change History                
*******************************************************************************                
** Date:        Author:  Bug #     Description:                           Rvwd                
** --------     -------- ------    -------------------------------------- --------                
  18-Feb-2010 Tanuj Enhancement Adding @StrategicAllocationID,@BRCommentID  
  22-Mar-2014  Sanath Requirement INVREQ 3.1
  23-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard
*******************************************************************************                    
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved                    
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION                    
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetEnfInvestmentAllocation] (
	@SearchBy VARCHAR(30)
	,@LookFor VARCHAR(30)
	,@IncludeInActive BIT
	,@SortColName VARCHAR(30)
	,@SortOrder VARCHAR(4)
	,@startrow BIGINT
	,@endrow BIGINT
	,@CharFilter VARCHAR(5)
	)
AS
BEGIN
	DECLARE @iCount BIGINT;
	DECLARE @request_filtered TABLE (
		ObjectiveCode VARCHAR(30)
		,ObjectiveName VARCHAR(40)
		,ObjectiveDescription VARCHAR(60)
		,ExpectedYield FLOAT
		,AccountCount BIGINT
		,IsActive BIT
		,StrategicAllocationCode VARCHAR(30)
		,RowNumber BIGINT IDENTITY(1, 1)
		);
	DECLARE @filtered TABLE (
		ObjectiveCode VARCHAR(30)
		,ObjectiveName VARCHAR(40)
		,ObjectiveDescription VARCHAR(60)
		,ExpectedYield FLOAT
		,AccountCount BIGINT
		,IsActive BIT
		,StrategicAllocationCode VARCHAR(30)
		,RowNumber BIGINT IDENTITY(1, 1)
		);

	IF (@SearchBy = 'Security_Symbol')
	BEGIN
		INSERT INTO @request_filtered
		SELECT ISNULL(InvObj.ObjectiveCode, '') AS ObjectiveCode
			,ISNULL(ObjectiveName, '') AS ObjectiveName
			,ISNULL(ObjectiveDescription, '') AS ObjectiveDescription
			,ISNULL(ExpectedYield, 0) AS ExpectedYield
			,COUNT(AccPrfl.CustomerAccountNumber) AS AccountCount
			,ISNULL(InvObj.IsActive, 0) AS IsActive
			,ISNULL(StrgcAllc.StrategicAllocationCode, '') AS StrategicAllocationCode
		FROM TBL_INV_InvestmentObjective InvObj
		LEFT JOIN TBL_INV_StrategicAllocation StrgcAllc ON StrgcAllc.StrategicAllocationID = InvObj.StrategicAllocationID
		LEFT JOIN TBL_INV_AccountProfile AccPrfl ON InvObj.objectivecode = AccPrfl.objectivecode
		INNER JOIN TBL_INV_TargetFundAllocation TgtFndAllc ON InvObj.ObjectiveCode = TgtFndAllc.ObjectiveCode
			AND TgtFndAllc.SecuritySymbol LIKE @LookFor
			AND InvObj.IsActive = (
				CASE 
					WHEN @IncludeInActive = 0
						THEN 1
					ELSE InvObj.IsActive
					END
				)
			AND InvObj.ObjectiveCode LIKE @CharFilter
		GROUP BY InvObj.ObjectiveCode
			,ObjectiveName
			,ObjectiveDescription
			,ExpectedYield
			,InvObj.IsActive
			,StrategicAllocationCode
	END
	ELSE IF (@SearchBy = 'Objective_Code')
	BEGIN
		INSERT INTO @request_filtered
		SELECT ISNULL(InvObj.ObjectiveCode, '') AS ObjectiveCode
			,ISNULL(ObjectiveName, '') AS ObjectiveName
			,ISNULL(ObjectiveDescription, '') AS ObjectiveDescription
			,ISNULL(ExpectedYield, 0) AS ExpectedYield
			,COUNT(AccPrfl.CustomerAccountNumber) AS AccountCount
			,ISNULL(InvObj.IsActive, 0) AS IsActive
			,ISNULL(StrgcAllc.StrategicAllocationCode, '') AS StrategicAllocationCode
		FROM TBL_INV_InvestmentObjective InvObj
		LEFT JOIN TBL_INV_StrategicAllocation StrgcAllc ON StrgcAllc.StrategicAllocationID = InvObj.StrategicAllocationID
		LEFT JOIN TBL_INV_AccountProfile AccPrfl ON InvObj.objectivecode = AccPrfl.objectivecode
		WHERE InvObj.ObjectiveCode LIKE @LookFor
			AND InvObj.ObjectiveCode LIKE @CharFilter
			AND InvObj.IsActive = (
				CASE 
					WHEN @IncludeInActive = 0
						THEN 1
					ELSE InvObj.IsActive
					END
				)
		GROUP BY InvObj.ObjectiveCode
			,ObjectiveName
			,ObjectiveDescription
			,ExpectedYield
			,InvObj.IsActive
			,StrategicAllocationCode
	END
	ELSE IF (@SearchBy = 'Objective_Name')
	BEGIN
		INSERT INTO @request_filtered
		SELECT ISNULL(InvObj.ObjectiveCode, '') AS ObjectiveCode
			,ISNULL(ObjectiveName, '') AS ObjectiveName
			,ISNULL(ObjectiveDescription, '') AS ObjectiveDescription
			,ISNULL(ExpectedYield, 0) AS ExpectedYield
			,COUNT(AccPrfl.CustomerAccountNumber) AS AccountCount
			,ISNULL(InvObj.IsActive, 0) AS IsActive
			,ISNULL(StrgcAllc.StrategicAllocationCode, '') AS StrategicAllocationCode
		FROM TBL_INV_InvestmentObjective InvObj
		LEFT JOIN TBL_INV_StrategicAllocation StrgcAllc ON StrgcAllc.StrategicAllocationID = InvObj.StrategicAllocationID
		LEFT JOIN TBL_INV_AccountProfile AccPrfl ON InvObj.objectivecode = AccPrfl.objectivecode
		WHERE ObjectiveName LIKE @LookFor
			AND InvObj.ObjectiveCode LIKE @CharFilter
			AND InvObj.IsActive = (
				CASE 
					WHEN @IncludeInActive = 0
						THEN 1
					ELSE InvObj.IsActive
					END
				)
		GROUP BY InvObj.ObjectiveCode
			,ObjectiveName
			,ObjectiveDescription
			,ExpectedYield
			,InvObj.IsActive
			,StrategicAllocationCode
	END

	IF (@SortOrder = 'DESC')
	BEGIN
		INSERT INTO @filtered
		SELECT ObjectiveCode
			,ObjectiveName
			,ObjectiveDescription
			,ExpectedYield
			,AccountCount
			,IsActive
			,StrategicAllocationCode
		FROM @request_filtered
		ORDER BY CASE 
				WHEN @SortColName = 'ObjectiveCode'
					THEN ObjectiveCode
				WHEN @SortColName = 'ObjectiveName'
					THEN ObjectiveName
				WHEN @SortColName = 'ObjectiveDescription'
					THEN ObjectiveDescription
				WHEN @SortColName = 'ExpectedYield'
					THEN CONVERT(VARCHAR, ExpectedYield)
				END DESC
	END
	ELSE
	BEGIN
		INSERT INTO @filtered
		SELECT ObjectiveCode
			,ObjectiveName
			,ObjectiveDescription
			,ExpectedYield
			,AccountCount
			,IsActive
			,StrategicAllocationCode
		FROM @request_filtered
		ORDER BY CASE 
				WHEN @SortColName = 'ObjectiveCode'
					THEN ObjectiveCode
				WHEN @SortColName = 'ObjectiveName'
					THEN ObjectiveName
				WHEN @SortColName = 'ObjectiveDescription'
					THEN ObjectiveDescription
				WHEN @SortColName = 'ExpectedYield'
					THEN CONVERT(VARCHAR, ExpectedYield)
				END ASC
	END

	SELECT @iCount = COUNT(*)
	FROM @request_filtered

	SELECT *
		,@iCount AS TotalCount
	FROM @filtered
	WHERE RowNumber >= @startrow
		AND RowNumber <= @endrow
END
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetEnfInvestmentAllocation'
		)
BEGIN
	PRINT 'CREATED USP_EX_GetEnfInvestmentAllocation';
END