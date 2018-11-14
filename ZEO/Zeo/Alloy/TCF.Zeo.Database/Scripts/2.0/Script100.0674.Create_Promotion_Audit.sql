--- ===============================================================================
-- Author:		<Purna Pushkal>
-- Create date: <01-02-2018>
-- Description:	 Creating the Audit tables for the promotion tables
-- Jira ID:		<B-12321>
-- ================================================================================

IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'tPromotions_Aud')
BEGIN

	CREATE TABLE tPromotions_Aud
	(
		PromotionId bigint,
		Name NVARCHAR(100),
		Description NVARCHAR(1000),
		ProductId INT,
		ProviderId INT,
		StartDate DATE,
		EndDate DATE,
		Priority INT,
		IsSystemApplied BIT,
		IsOverridable BIT,
		IsNextCustomerSession BIT,
		IsPrintable BIT,
		IsActive BIT,
		DTServerCreate DATETIME,
		DTServerLastModified DATETIME,
		DTTerminalCreate DATETIME,
		DTTerminalLastModified DATETIME,
		RevisionNo BIGINT,
		AuditEvent SMALLINT,
		DTAudit DATETIME
	)

END


IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'tPromoQualifiers_Aud')
BEGIN

	CREATE TABLE tPromoQualifiers_Aud
	(
		PromoQualifierId BIGINT,
		PromotionId BIGINT,
		StartDate DATE,
		EndDate DATE,
		Amount MONEY,
		MinTransactionCount INT,
		MaxTransactionCount INT,
		ProductId INT,
		IsPaidFee BIT,
		TransactionStates NVARCHAR(1000),
		IsParked BIT,
		DTServerCreate DATETIME,
		DTServerLastModified DATETIME,
		DTTerminalCreate DATETIME,
		DTTerminalLastModified DATETIME,
		RevisionNo BIGINT,
		AuditEvent SMALLINT,
		DTAudit DATETIME
	)

END

IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'tPromoProvisions_Aud')
BEGIN

	CREATE TABLE tPromoProvisions_Aud
	(
		PromoProvisionId BIGINT,
		PromotionId BIGINT,
		DiscountValue DECIMAL,
		MinAmount MONEY,
		MaxAmount MONEY,
		CheckTypeIds NVARCHAR(500),
		locationIds NVARCHAR(500),
		Groups NVARCHAR(500),
		IsPercentage BIT,
		DTServerCreate DATETIME,
		DTServerLastModified DATETIME,
		DTTerminalCreate DATETIME,
		DTTerminalLastModified DATETIME,
		RevisionNo BIGINT,
		AuditEvent SMALLINT,
		DTAudit DATETIME
	)

END