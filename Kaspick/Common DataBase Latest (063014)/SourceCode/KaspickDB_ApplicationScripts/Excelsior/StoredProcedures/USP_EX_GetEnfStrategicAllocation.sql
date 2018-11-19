IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetEnfStrategicAllocation'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetEnfStrategicAllocation;

	PRINT 'DROPPED USP_EX_GetEnfStrategicAllocation';
END
GO

/******************************************************************************    
** New Name :  USP_EX_GetEnfStrategicAllocation
** Old Name:   USP_EIS_ENF_STRATEGIC_ALLOCATION_SelProc    
** Short Desc:     
**    
** Full Description    
**        This SP is used to fetch the list of Strategics Allocation.    
**    
** Sample Call    
   exec USP_EX_GetEnfStrategicAllocation     
  'Strategic_Code',    
  '%',    
  1,    
  'StrategicDescription',    
  'ASC',    
  1,    
  18,    
  '%'    
**    
** Return values: NONE    
**    
**    
** Standard declarations    
**       SET NOCOUNT             ON    
**       SET LOCK_TIMEOUT         30000   -- 30 seconds    
**     
** Created By: Tanuj    
** Company   : Kaspick & Company    
** Project   : Excelsior - Enfuego4    
** Created DT: 10/12/2009    
**                
*******************************************************************************    
**       Change History    
*******************************************************************************    
** Date:        Author:  Bug #     Description:                           Rvwd    
** --------     -------- ------    -------------------------------------- --------    
** 03/21/2014   Sanath          Investment management module pointing to KaspickDB
** 23-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard 
*******************************************************************************    
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved    
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION    
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetEnfStrategicAllocation] (
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
		StrategicAllocationID INT
		,StrategicCode VARCHAR(30)
		,StrategicName VARCHAR(50)
		,StrategicDescription VARCHAR(1000)
		,IsActive BIT
		,RowNumber BIGINT IDENTITY(1, 1)
		);
	DECLARE @filtered TABLE (
		StrategicAllocationID INT
		,StrategicCode VARCHAR(30)
		,StrategicName VARCHAR(50)
		,StrategicDescription VARCHAR(1000)
		,IsActive BIT
		,RowNumber BIGINT IDENTITY(1, 1)
		);

	IF (@SearchBy = 'Strategic_Name')
	BEGIN
		INSERT INTO @request_filtered
		SELECT StrategicAllocationID
			,ISNULL(StrategicAllocationCode, '') AS StrategicCode
			,ISNULL(StrategicAllocationName, '') AS StrategicName
			,ISNULL(Description, '') AS StrategicDescription
			,ISNULL(IsActive, 0) AS IsActive
		FROM TBL_INV_StrategicAllocation
		WHERE StrategicAllocationName LIKE @LookFor
			AND IsActive = (
				CASE 
					WHEN @IncludeInActive = 0
						THEN 1
					ELSE IsActive
					END
				)
			AND StrategicAllocationCode LIKE @CharFilter
	END
	ELSE IF (@SearchBy = 'Strategic_Code')
	BEGIN
		INSERT INTO @request_filtered
		SELECT StrategicAllocationID
			,ISNULL(StrategicAllocationCode, '') AS StrategicCode
			,ISNULL(StrategicAllocationName, '') AS StrategicName
			,ISNULL(Description, '') AS StrategicDescription
			,ISNULL(IsActive, 0) AS IsActive
		FROM TBL_INV_StrategicAllocation
		WHERE StrategicAllocationCode LIKE @LookFor
			AND IsActive = (
				CASE 
					WHEN @IncludeInActive = 0
						THEN 1
					ELSE IsActive
					END
				)
			AND StrategicAllocationCode LIKE @CharFilter
	END

	IF (@SearchBy = 'AssetClass_Name')
	BEGIN
		INSERT INTO @request_filtered
		SELECT StrgcAllc.StrategicAllocationID
			,ISNULL(StrategicAllocationCode, '') AS StrategicCode
			,ISNULL(StrategicAllocationName, '') AS StrategicName
			,ISNULL(Description, '') AS StrategicDescription
			,ISNULL(IsActive, 0) AS IsActive
		FROM TBL_INV_StrategicAllocation StrgcAllc
		WHERE StrategicAllocationID IN (
				SELECT StrategicAllocationID
				FROM TBL_INV_StrategicAllocationDetail
				WHERE AssetClassName LIKE @LookFor
					AND TargetPercentage > 0
				)
			AND IsActive = (
				CASE 
					WHEN @IncludeInActive = 0
						THEN 1
					ELSE IsActive
					END
				)
			AND StrategicAllocationCode LIKE @CharFilter
	END

	IF (@SortOrder = 'DESC')
	BEGIN
		INSERT INTO @filtered
		SELECT StrategicAllocationID
			,StrategicCode
			,StrategicName
			,StrategicDescription
			,IsActive
		FROM @request_filtered
		ORDER BY CASE 
				WHEN @SortColName = ''
					THEN StrategicCode
				WHEN @SortColName = 'StrategicCode'
					THEN StrategicCode
				WHEN @SortColName = 'StrategicName'
					THEN StrategicName
				WHEN @SortColName = 'StrategicDescription'
					THEN StrategicDescription
				END DESC
	END
	ELSE
	BEGIN
		INSERT INTO @filtered
		SELECT StrategicAllocationID
			,StrategicCode
			,StrategicName
			,StrategicDescription
			,IsActive
		FROM @request_filtered
		ORDER BY CASE 
				WHEN @SortColName = ''
					THEN StrategicCode
				WHEN @SortColName = 'StrategicCode'
					THEN StrategicCode
				WHEN @SortColName = 'StrategicName'
					THEN StrategicName
				WHEN @SortColName = 'StrategicDescription'
					THEN StrategicDescription
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
			AND NAME = 'USP_EX_GetEnfStrategicAllocation'
		)
BEGIN
	PRINT 'CREATED USP_EX_GetEnfStrategicAllocation';
END