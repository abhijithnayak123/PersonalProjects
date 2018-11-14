--- ===============================================================================
-- Author     :	 Manikandan Govindraj
-- Description:  Creating the new tables for Promo Engine.
-- Creatd Date:  11-01-1987
-- Story Id   :  B-12321
-- ================================================================================


-- Create tPromotionTypes table

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tPromotionTypes')
BEGIN

    CREATE TABLE tPromotionTypes 
                ( 
				    PromotionTypeId BIGINT IDENTITY(10000001,1),
				    Name NVARCHAR(100),
				    DTServerCreate DATETIME NOT NULL,
				    DTServerLastModified DATETIME NULL,
				    DTTerminalCreate DATETIME NOT NULL,
				    DTTerminalLastModified DATETIME NULL,				 
				    CONSTRAINT PK_tPromotionTypes PRIMARY KEY CLUSTERED 
				    (
				    	PromotionTypeId ASC
				    )
                 )
END
GO


-- Create tPromotions table 

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tPromotions')
BEGIN

     CREATE TABLE tPromotions
				(
				    PromotionId BIGINT IDENTITY(100000001,1) NOT NULL,
				    --PromotionTypeId BIGINT NOT NULL,
				    Name NVARCHAR(100) NOT NULL,
				    Description NVARCHAR(1000) NULL,				 
				    ProductId INT NOT NULL,
					ProviderId INT NOT NULL,
				    StartDate DATE NOT NULL,
				    EndDate DATE NULL,
				    Priority INT,
				    IsSystemApplied BIT NOT NULL,
				    IsOverridable BIT NOT NULL,
				    IsNextCustomerSession BIT NOT NULL,				 
				    IsPrintable BIT NOT NULL,
				    IsActive BIT NOT NULL,
				    DTServerCreate DATETIME NOT NULL,
				    DTServerLastModified DATETIME NULL,
				    DTTerminalCreate DATETIME NOT NULL,
				    DTTerminalLastModified DATETIME NULL,				 
				    CONSTRAINT PK_tPromotions PRIMARY KEY CLUSTERED 
				    (
				    	PromotionId ASC
				    )
                )

END
GO


-- Create a foreign key reference from tPromotions to tPromotype table

--IF NOT EXISTS 
--(
--		SELECT 1
--		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
--		WHERE TABLE_NAME = 'tPromotions'
--		AND CONSTRAINT_NAME = 'FK_tPromotions_tPromotionTypes'
--)
--BEGIN

--	ALTER TABLE tPromotions  WITH CHECK ADD  CONSTRAINT FK_tPromotions_tPromotionTypes FOREIGN KEY(PromotionTypeId)
--	REFERENCES tPromotionTypes (PromotionTypeId)

--END
--GO


-- Create tPromoQualifiers table

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tPromoQualifiers')
BEGIN

		     
       CREATE TABLE tPromoQualifiers
               (
			        PromoQualifierId BIGINT IDENTITY(100000001,1) NOT NULL,
					PromotionId BIGINT NOT NULL,
					StartDate DATE NOT NULL,
				    EndDate DATE NULL,   
					Amount  MONEY NULL,
					MinTransactionCount INT NULL,
					MaxTransactionCount INT NULL,
					ProductId INT NOT NULL,
					IsPaidFee BIT NULL,
					TransactionStates NVARCHAR(50) NULL,
					IsParked BIT NULL,
					DTServerCreate DATETIME NOT NULL,
				    DTServerLastModified DATETIME NULL,
				    DTTerminalCreate DATETIME NOT NULL,
				    DTTerminalLastModified DATETIME NULL,				 
				    CONSTRAINT PK_tPromoQualifiers PRIMARY KEY CLUSTERED 
				    (
				    	PromoQualifierId ASC
				    )
                )

END
GO


-- create a foreign key reference from tPromoQualifiers to tPromotions table

IF NOT EXISTS 
(
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tPromoQualifiers'
		AND CONSTRAINT_NAME = 'FK_tPromoQualifiers_tPromotions'
)
BEGIN

	ALTER TABLE tPromoQualifiers  WITH CHECK ADD  CONSTRAINT FK_tPromoQualifiers_tPromotions FOREIGN KEY(PromotionId)
	REFERENCES tPromotions (PromotionId)

END
GO


-- Create tPromoProvisions table

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tPromoProvisions')
BEGIN

     CREATE TABLE tPromoProvisions
                (
				    PromoProvisionId BIGINT IDENTITY(1000000001,1) NOT NULL,
					PromotionId BIGINT NOT NULL,
					DiscountValue DECIMAL(18,2),
					MinAmount MONEY NULL,
					MaxAmount MONEY NULL,
					CheckTypeIds NVARCHAR(500),
					locationIds NVARCHAR(500),
					Groups NVARCHAR(500),
					IsPercentage BIT NOT NULL,
					DTServerCreate DATETIME NOT NULL,
				    DTServerLastModified DATETIME NULL,
				    DTTerminalCreate DATETIME NOT NULL,
				    DTTerminalLastModified DATETIME NULL,				 
				    CONSTRAINT PK_tPromoProvisions PRIMARY KEY CLUSTERED 
				    (
				    	PromoProvisionId ASC
				    )
				)

END
GO



-- create a foreign key reference from tPromoProvisions to tPromotions table

IF NOT EXISTS 
(
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME = 'tPromoProvisions'
		AND CONSTRAINT_NAME = 'FK_tPromoProvisions_tPromotions'
)
BEGIN

	ALTER TABLE tPromoProvisions  WITH CHECK ADD  CONSTRAINT FK_tPromoProvisions_tPromotions FOREIGN KEY(PromotionId)
	REFERENCES tPromotions (PromotionId)

END
GO