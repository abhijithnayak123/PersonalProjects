IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetEnfStrategicAllocationSet  '
		)
BEGIN
	DROP PROCEDURE USP_EX_GetEnfStrategicAllocationSet;

	PRINT 'DROPPED PROCEDURE USP_EX_GetEnfStrategicAllocationSet  ';
END
GO

/******************************************************************************  
** New Name :     USP_EX_GetEnfStrategicAllocationSet
** Old Name :     USP_EIS_ENF_STRATEGIC_ALLOCATION_SET_SelProc  
** Short Desc:   
**  
** Full Description  
**        This SP is used to fetch the list of strategic allocation sets.  
**  
** Sample Call  
   exec USP_EX_GetEnfStrategicAllocationSet     
  @SearchBy = 'StrategicSetName',  
  @LookFor = '%',  
  @IncludeInActive = 1,  
  @SortColName = 'StrategicSetName',  
  @SortOrder = 'DESC',  
  @startrow = 1,  
  @endrow = 18,  
  @CharFilter = '%'  
**  
** Return values: NONE  
**  
**  
** Standard declarations  
**       SET NOCOUNT             ON  
**       SET LOCK_TIMEOUT         30000   -- 30 seconds  
**   
** Created By: Soorya  
** Company   : Kaspick & Company  
** Project   : Excelsior - Enfuego4B  
** Created DT: 02/05/2010  
**              
*******************************************************************************  
**       Change History  
*******************************************************************************  
** Date:        Author:  Bug #     Description:                           Rvwd  
** --------     -------- ------    -------------------------------------- --------  
**03/22/2014  Sanath Requirement INVERQ 3.1
** 23-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard 
*******************************************************************************  
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved  
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION  
*******************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetEnfStrategicAllocationSet] (
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
		StrategicSetID BIGINT
		,StrategicSetName VARCHAR(30)
		,Description VARCHAR(1000)
		,ClientCount BIGINT
		,IsActive BIT
		,RowNumber BIGINT IDENTITY(1, 1)
		);
	DECLARE @filtered TABLE (
		StrategicSetID BIGINT
		,StrategicSetName VARCHAR(30)
		,Description VARCHAR(1000)
		,ClientCount BIGINT
		,IsActive BIT
		,RowNumber BIGINT IDENTITY(1, 1)
		);

	IF (@SearchBy = 'StrategicSetName')
	BEGIN
		INSERT INTO @request_filtered
		SELECT StrategicAllocationSetID AS StrategicSetID
			,ISNULL(StrategicAllocationSetName, '') AS SetName
			,ISNULL(Description, '') AS Description
			,0 AS ClientCount
			,ISNULL(IsActive, 0) AS IsActive
		FROM TBL_INV_StrategicAllocationSet
		WHERE StrategicAllocationSetName LIKE @LookFor
			AND StrategicAllocationSetName LIKE @CharFilter
			AND IsActive = (
				CASE 
					WHEN @IncludeInActive = 0
						THEN 1
					ELSE IsActive
					END
				)
		ORDER BY StrategicAllocationSetName
	END

	IF (@SortOrder = 'DESC')
	BEGIN
		INSERT INTO @filtered
		SELECT StrategicSetID
			,StrategicSetName
			,Description
			,ClientCount
			,IsActive
		FROM @request_filtered
		ORDER BY CASE 
				WHEN @SortColName = 'StrategicSetName'
					THEN StrategicSetName
				WHEN @SortColName = 'Description'
					THEN Description
				END DESC
	END
	ELSE
	BEGIN
		INSERT INTO @filtered
		SELECT StrategicSetID
			,StrategicSetName
			,Description
			,ClientCount
			,IsActive
		FROM @request_filtered
		ORDER BY CASE 
				WHEN @SortColName = 'StrategicSetName'
					THEN StrategicSetName
				WHEN @SortColName = 'Description'
					THEN Description
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
			AND NAME = 'USP_EX_GetEnfStrategicAllocationSet  '
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetEnfStrategicAllocationSet  ';
END
GO

