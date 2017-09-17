--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <08-04-2016>
-- Description:	 Create or update the Chexar session
-- Jira ID:		<AL-7582>
-- ================================================================================

IF OBJECT_ID(N'usp_CreateOrUpdateChexarSession', N'P') IS NOT NULL
DROP PROC usp_CreateOrUpdateChexarSession
GO

CREATE PROCEDURE usp_CreateOrUpdateChexarSession
(		
		@location NVARCHAR(50),
		@companyToken NVARCHAR(50),
		@branchId INT,
		@employeeId INT,
		@chexarPartnerId BIGINT,
		@dTTerminalCreate DATETIME,
		@dTServerCreate DATETIME
)
AS
BEGIN
	BEGIN TRY

		IF EXISTS(SELECT 1 FROM dbo.tChxr_Session WHERE Location = @location)
		BEGIN
			UPDATE dbo.tChxr_Session
			SET
				Location = @location, 
				CompanyToken = @companyToken,
				BranchId = @branchId,
				EmployeeId = @employeeId,
				DTTerminalLastModified = @dTTerminalCreate,
				ChxrPartnerId = @chexarPartnerId 
			WHERE 
			    Location = @location
		END

		ELSE
		BEGIN
			INSERT INTO dbo.tChxr_Session
			(
				Location,
				CompanyToken,
				BranchId,
				EmployeeId,
				DTTerminalCreate,
				DTTerminalLastModified,
				ChxrPartnerId
			)
			VALUES
			(
				@location, 
				@companyToken,
				@branchId,
				@employeeId,
				@dTTerminalCreate,
				NULL,
				@chexarPartnerId
			)
		END

	END TRY

	BEGIN CATCH
		EXECUTE usp_CreateErrorInfo
	END CATCH
END