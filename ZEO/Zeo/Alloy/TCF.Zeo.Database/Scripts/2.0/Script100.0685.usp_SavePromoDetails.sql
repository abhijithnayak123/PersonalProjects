--- ===============================================================================
-- Author:		<M.Purna Pushkal>
-- Create date: <02-22-2018>
-- Description:	 SP to save the promotion details
-- Jira ID:		<B-13218>
-- ================================================================================

IF EXISTS(SELECT 1 FROM sys.objects WHERE name = 'usp_SavePromoDetails')
BEGIN
	DROP PROCEDURE usp_SavePromoDetails
END
GO

CREATE PROCEDURE usp_SavePromoDetails
(
	 @promotionId BIGINT
	,@name NVARCHAR(100)
	,@description NVARCHAR(1000)
	,@productId INT
	,@startDate DATE
	,@endDate DATE
	,@priority INT
	,@providerId INT
	,@isSystemApplied BIT
	,@isOverridable BIT
	,@isNextCustomerSession BIT
	,@isPrintable BIT
	,@isActive BIT
	,@dTServerDate DATETIME
	,@dTTerminalDate DATETIME
	,@dbPromoId BIGINT OUTPUT
)
AS 
BEGIN
BEGIN TRY 

	IF @promotionId = 0
	BEGIN
		INSERT INTO tPromotions
		(
			 Name
			,[Description]
			,ProductId
			,StartDate
			,EndDate
			,[Priority]
			,ProviderId
			,IsSystemApplied
			,IsOverridable
			,IsNextCustomerSession
			,IsPrintable
			,IsActive
			,DTServerCreate
			,DTTerminalCreate
		)
		VALUES
		(
			 @name
			,@description
			,@productId
			,@startDate
			,@endDate
			,@priority
			,@providerId
			,@isSystemApplied
			,@isOverridable
			,@isNextCustomerSession
			,@isPrintable
			,@isActive
			,@dTServerDate
			,@dTTerminalDate
		)

		SELECT @dbPromoId = CAST(SCOPE_IDENTITY() AS BIGINT)

	END 
	ELSE 
	BEGIN
		UPDATE tPromotions
		SET
		     Name = @name
		    ,Description = @description
		    ,ProductId = @productId
		    ,StartDate = @startDate
		    ,EndDate = @endDate
		    ,Priority = @priority
			,ProviderId = @providerId
		    ,IsSystemApplied = @isSystemApplied
		    ,IsOverridable = @isOverridable
		    ,IsNextCustomerSession = @isNextCustomerSession
		    ,IsPrintable = @isPrintable
		    ,IsActive = @isActive
		    ,DTServerLastModified = @dTServerDate
		    ,DTTerminalLastModified = @dTTerminalDate
		WHERE 
			PromotionId = @promotionId

		SELECT @dbPromoId = @promotionId

		END
		END TRY

	BEGIN CATCH
		ROLLBACK
		EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
	END CATCH

END 
