IF EXISTS(SELECT 1 FROM sys.objects WHERE name = 'usp_InsertOrUpdateQualifiers')
BEGIN
	DROP PROCEDURE usp_InsertOrUpdateQualifiers
END 
GO

CREATE PROCEDURE usp_InsertOrUpdateQualifiers
(
	 @promotionId BIGINT
	,@qualifiers XML
)
AS
BEGIN

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
			Qualifier.value('PromotionId[1]', 'BIGINT') AS PromotionId,     
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


	SELECT
		pq.PromotionId AS PromotionId, pq.PromoQualifierId AS QualifierId, tp.ProductsID AS ProductId, tp.Name as ProductName, pq.EndDate, pq.Amount, pq.MinTransactionCount 
		,pq.TransactionStates, pq.IsPaidFee
	FROM
		tPromoQualifiers pq WITH(NOLOCK)
		INNER JOIN tProducts tp WITH(NOLOCK) ON pq.ProductId = tp.ProductsID
	WHERE 
		pq.PromotionId = @promotionId

END