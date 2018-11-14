--- ===============================================================================
-- Author:		<M.Purna Pushkal>
-- Modified By: Abhijith
-- Create date: <01-19-2018>
-- Modified Date: 04-10-2018
-- Modified Date: 04-18-2018
-- Description:	 SP to Insert or Update the promotoions
-- Modified Reason: locationIds column changed from NVARCHAR(500) to NVARCHAR(MAX) so changing the same in SP.
-- Modified Reason: Banker should have the option to hide the Promotions by selecting the option.
-- Jira ID:		<B-12321>
-- ================================================================================

IF EXISTS(SELECT 1 FROM sys.objects WHERE name = 'usp_InsertOrUpdatePromotion')
BEGIN
	DROP PROCEDURE usp_InsertOrUpdatePromotion
END 
GO

CREATE PROCEDURE usp_InsertOrUpdatePromotion
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
	,@promotionStatus INT
	,@freeTxnCount INT
	,@stackable BIT
	,@isPromotionHidden BIT
	,@dTServerDate DATETIME
	,@dTTerminalDate DATETIME
	,@qualifiers XML
	,@provisions XML
)
AS
BEGIN
	BEGIN TRY

	BEGIN TRANSACTION

	BEGIN /* Variables */

	DECLARE @promoId BIGINT

	DECLARE @qualifiersTable TABLE
	(	
		 PromoQualifierId BIGINT
		,PromotionId BIGINT
		,StartDate DATE
		,EndDate DATE
		,Amount MONEY
		,MinTransactionCount INT
		,ProductId INT
		,TransactionStates NVARCHAR(50)
		,IsPaidFee BIT
		,IsParked BIT
		,DTServerDate DATETIME
		,DTTerminalDate DATETIME
	)

	DECLARE @provisionsTable TABLE
	(
		  PromoProvisionId BIGINT
		 ,PromotionId BIGINT
		 ,[Value] DECIMAL(18,2)
		 ,MinAmount MONEY
		 ,MaxAmount MONEY
		 ,CheckTypeIds NVARCHAR(500)
		 ,Groups NVARCHAR(500)
		 ,DiscountType INT
		 ,locationIds NVARCHAR(MAX)
		 ,DTServerDate DATETIME
		 ,DTTerminalDate DATETIME
	)

	END 

	BEGIN /* Inserting or Updating in tPromotions Table */

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
			,Status
			,FreeTransactionCount
			,Stackable
			,IsPromotionHidden
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
			,@promotionStatus
			,@freeTxnCount
			,@stackable
			,@isPromotionHidden
			,@dTServerDate
			,@dTTerminalDate
		)

		SELECT @promoId = CAST(SCOPE_IDENTITY() AS BIGINT)
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
		    ,Status = @promotionStatus
			,FreeTransactionCount = @freeTxnCount
			,Stackable = @stackable
			,IsPromotionHidden = @isPromotionHidden
		    ,DTServerLastModified = @dTServerDate
		    ,DTTerminalLastModified = @dTTerminalDate
		WHERE 
			PromotionId = @promotionId

			SET @promoId = @promotionId
	END 

	END

	BEGIN /* Inserting or Updating the tPromoQualifiers table*/

	INSERT INTO @qualifiersTable 
	(			
		PromotionId,
	    PromoQualifierId,
		StartDate, 
		EndDate, 
		Amount, 
		MinTransactionCount, 
		ProductId,
		TransactionStates,
		IsPaidFee,
		IsParked,
		DTServerDate, 
		DTTerminalDate
	)	
	(
		SELECT 
			@promoId,         
			Qualifier.value('PromoQualifierId[1]', 'BIGINT') AS PromoQualifierId,
			Qualifier.value('StartDate[1]', 'NVARCHAR(100)') AS StartDate,
			Qualifier.value('EndDate[1]', 'NVARCHAR(100)')AS EndDate,
			Qualifier.value('Amount[1]', 'MONEY') AS Amount,
			Qualifier.value('MinTransactionCount[1]', 'INT') AS MinTransactionCount,
			Qualifier.value('ProductId[1]', 'INT') AS ProductId,
			Qualifier.value('TransactionStates[1]', 'NVARCHAR(500)') AS TransactionStates,
			Qualifier.value('IsPaidFee[1]', 'BIT') AS IsPaidFee,
			Qualifier.value('ConsiderParkedTxns[1]', 'BIT') AS IsParked,
			Qualifier.value('DTServerDate[1]', 'DATETIME') AS DTServerDate,
			Qualifier.value('DTTerminalDate[1]', 'DATETIME') AS DTTerminalDate
		 FROM 
			@qualifiers.nodes('/Qualifiers/Qualifier') AS Promotion(Qualifier)
	)

	IF @promotionId != 0
	BEGIN
		UPDATE tp
		SET
		    StartDate = qt.StartDate,
		    EndDate = qt.EndDate,
		    Amount = qt.Amount,
		    MinTransactionCount = qt.MinTransactionCount,
		    ProductId = qt.ProductId,
			TransactionStates = qt.TransactionStates,
			IsPaidFee = qt.IsPaidFee,
			IsParked = qt.IsParked,
		    DTServerLastModified = qt.DTServerDate,
		    DTTerminalLastModified = qt.DTTerminalDate
		FROM 
		    tPromoQualifiers tp WITH (NOLOCK)
		    INNER JOIN @qualifiersTable qt ON qt.PromoQualifierId = tp.PromoQualifierId

		DELETE tpq
		FROM 
		tPromoQualifiers tpq
		LEFT JOIN @qualifiersTable qt ON qt.PromoQualifierId = tpq.PromoQualifierId AND tpq.PromotionId = qt.PromotionId
		WHERE 
		qt.PromoQualifierId IS NULL AND tpq.PromotionId = @promotionId
   END 


	INSERT INTO tPromoQualifiers
	(
	    PromotionId,
	    StartDate,
	    EndDate,
	    Amount,
	    MinTransactionCount,
	    ProductId,
		TransactionStates,
		IsPaidFee,
		IsParked,
	    DTServerCreate,
	    DTTerminalCreate
	)
	( 
	   SELECT 
		 PromotionId,
		 StartDate, 
		 EndDate, 
		 Amount, 
		 MinTransactionCount, 
		 ProductId,
		 TransactionStates,
		 IsPaidFee, 
		 IsParked,
		 DTServerDate, 
		 DTTerminalDate
	   FROM 
		 @qualifiersTable
       WHERE PromoQualifierId = 0
	)

	
   END

	BEGIN /* Inserting or Updating the tPromoProvisions table*/

	INSERT INTO @provisionsTable
	(		
		PromotionId,
		PromoProvisionId,
		[Value],
		MinAmount,
		MaxAmount, 
		CheckTypeIds, 
		DiscountType,
		locationIds,
		Groups,
		DTServerDate, 
		DTTerminalDate
	)
	(
		SELECT
			@promoId
		   ,Provision.value('PromoProvisionId[1]', 'DECIMAL(18,2)') AS PromoProvisionId
		   ,Provision.value('Value[1]', 'DECIMAL(18,2)') AS DiscountValue
		   ,Provision.value('MinAmount[1]', 'MONEY') AS MinAmount
		   ,Provision.value('MaxAmount[1]', 'MONEY') AS MaxAmount
		   ,Provision.value('CheckTypeIds[1]', 'NVARCHAR(500)') AS CheckTypeIds
		   ,Provision.value('DiscountType[1]', 'INT') AS DiscountType
		   ,Provision.value('locationIds[1]', 'NVARCHAR(MAX)') AS locationIds
		   ,Provision.value('Groups[1]', 'NVARCHAR(500)') AS Groups
		   ,Provision.value('DTServerDate[1]', 'DATETIME') AS DTServerDate
		   ,Provision.value('DTTerminalDate[1]', 'DATETIME') AS DTTerminalDate	
		 FROM 
			@provisions.nodes('/Provisions/Provision') AS Promotion(Provision)		
	)

	IF @promotionId != 0
	BEGIN
		UPDATE tpp 
		SET
		    PromotionId = tp.PromotionId,
		    [Value] = tp.[Value],
		    MinAmount = tp.MinAmount,
		    MaxAmount = tp.MaxAmount,
		    CheckTypeIds = tp.CheckTypeIds,
			DiscountType = tp.DiscountType,
		    locationIds = tp.locationIds,
			Groups = tp.Groups,
		    DTServerLastModified = tp.DTServerDate,
		    DTTerminalLastModified = tp.DTTerminalDate
		FROM  
			@provisionsTable tp 
			INNER JOIN tPromoProvisions tpp WITH (NOLOCK) ON tpp.PromoProvisionId = tp.PromoProvisionId

		DELETE tpp
		FROM 
		tPromoProvisions tpp
		LEFT JOIN @provisionsTable pt ON pt.PromoProvisionId = tpp.PromoProvisionId AND tpp.PromotionId = pt.PromotionId
		WHERE 
		pt.PromoProvisionId IS NULL AND tpp.PromotionId = @promotionId
    END 

	INSERT INTO tPromoProvisions
	(
	    PromotionId,
	    [Value],
	    MinAmount,
	    MaxAmount,
	    CheckTypeIds,
		DiscountType,
	    locationIds,
		Groups,
	    DTServerCreate,
	    DTTerminalCreate
	)
	(
	   SELECT 
		 PromotionId,
		 [Value],
		 MinAmount,
		 MaxAmount, 
		 CheckTypeIds, 
		 DiscountType,
		 LocationIds,
		 Groups,
		 DTServerDate, 
		 DTTerminalDate
	   FROM 
		 @provisionsTable
	   WHERE PromoProvisionId = 0
	)

	

	END

	COMMIT 

	END TRY

	BEGIN CATCH
		ROLLBACK
		EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
	END CATCH

END
