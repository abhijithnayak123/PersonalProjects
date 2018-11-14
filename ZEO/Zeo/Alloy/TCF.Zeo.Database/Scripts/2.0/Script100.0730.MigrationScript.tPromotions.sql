--- ===============================================================================
-- Author:		<NITISH BIRADAR>
-- Create date: <04-09-2018>
-- Description:	 Migration script for existing promotions
-- Jira ID:		<B-14128>
-- ================================================================================


DECLARE @promotionId BIGINT

IF NOT EXISTS(SELECT 1 FROM tPromotions WHERE Name = 'FREECORPORATE')
BEGIN
	INSERT 
		INTO 
	tPromotions
	    (Name, Description, ProductId, ProviderId, StartDate, EndDate, Priority, IsSystemApplied, IsOverridable, IsNextCustomerSession, IsPrintable, DTServerCreate, 
		DTServerLastModified, DTTerminalCreate, DTTerminalLastModified, Status, FreeTransactionCount, Stackable)
	VALUES
	    (N'FREECORPORATE', N'Free check cashing for TCF Commercial accounts', 1, 200, '07/01/2015', '07/01/2030', NULL, 0, 1, 0, 0, GETDATE(), NULL, GETDATE(), 
		NULL, 1, NULL, 0)
	
	SET @promotionId = (SELECT PromotionId FROM tPromotions WHERE Name = 'FREECORPORATE')
	
	INSERT 
		INTO 
	tPromoProvisions
		(PromotionId ,Value ,MinAmount ,MaxAmount ,CheckTypeIds, locationIds ,Groups, DTServerCreate ,DTServerLastModified ,DTTerminalCreate ,
		DTTerminalLastModified ,DiscountType)
	VALUES
		(@promotionId, 100, NULL, NULL, '6,7,10', NULL, NULL, GETDATE(), NULL, GETDATE(), NULL, 1)

END

IF NOT EXISTS(SELECT 1 FROM tPromotions WHERE Name = 'TCFDIV')
BEGIN
	INSERT 
		INTO 
	tPromotions
		(Name, Description, ProductId, ProviderId, StartDate, EndDate, Priority, IsSystemApplied, IsOverridable, IsNextCustomerSession, IsPrintable, DTServerCreate, 
		DTServerLastModified, DTTerminalCreate, DTTerminalLastModified, Status, FreeTransactionCount, Stackable)
	VALUES
		(N'TCFDIV', N'Free TCF dividend check cashing', 1, 200, '07/01/2015', '07/01/2030', NULL, 0, 1, 0, 0, GETDATE(), NULL, GETDATE(), NULL, 1, NULL, 0)
	
	SET @promotionId = (SELECT PromotionId FROM tPromotions WHERE Name = 'TCFDIV')
	
	INSERT 
		INTO 
	tPromoProvisions
	   (PromotionId ,Value ,MinAmount ,MaxAmount ,CheckTypeIds, locationIds ,Groups, DTServerCreate ,DTServerLastModified ,DTTerminalCreate ,DTTerminalLastModified, 
	   DiscountType)
	VALUES
		(@promotionId, 100, NULL, NULL, '10', NULL, NULL, GETDATE(), NULL, GETDATE(), NULL, 1)

END

IF NOT EXISTS(SELECT 1 FROM tPromotions WHERE Name = 'INSTOREPAYROLL')
BEGIN
	INSERT 
		INTO 
	tPromotions
		(Name, Description, ProductId, ProviderId, StartDate, EndDate, Priority, IsSystemApplied, IsOverridable, IsNextCustomerSession, IsPrintable, DTServerCreate, 
		DTServerLastModified, DTTerminalCreate, DTTerminalLastModified, Status, FreeTransactionCount, Stackable)
	VALUES
		(N'INSTOREPAYROLL', N'Free payroll check cashing for Jewel/Osco and Cub Foods employees', 1, 200, '07/01/2015', '07/01/2030', NULL, 0, 1, 0, 0, GETDATE(), 
		NULL, GETDATE(), NULL, 1, NULL, 0)
	
	SET @promotionId = (SELECT PromotionId FROM tPromotions WHERE Name = 'INSTOREPAYROLL')
	
	INSERT 
		INTO 
	tPromoProvisions
		(PromotionId ,Value ,MinAmount ,MaxAmount ,CheckTypeIds, locationIds ,Groups, DTServerCreate, DTServerLastModified, DTTerminalCreate, DTTerminalLastModified, 
		DiscountType)
	VALUES
		(@promotionId, 100, NULL, NULL, '6,7', NULL, '2', GETDATE(), NULL, GETDATE(), NULL, 1)

END

IF NOT EXISTS(SELECT 1 FROM tPromotions WHERE Name = 'COMMERCIALPAYROLL')
BEGIN
	INSERT 
		INTO 
	tPromotions
		(Name, Description, ProductId, ProviderId, StartDate, EndDate, Priority, IsSystemApplied, IsOverridable, IsNextCustomerSession, IsPrintable, DTServerCreate, 
		DTServerLastModified, DTTerminalCreate, DTTerminalLastModified, Status, FreeTransactionCount, Stackable)
	VALUES
		(N'COMMERCIALPAYROLL', N'Free TCF payroll check cashing for commercial accounts', 1, 200, '07/01/2015', '07/01/2030', NULL, 0, 1, 0, 0, GETDATE(), NULL, 
		GETDATE(), NULL, 1, NULL, 0)
	
	SET @promotionId = (SELECT PromotionId FROM tPromotions WHERE Name = 'COMMERCIALPAYROLL')
	
	INSERT 
		INTO 
	tPromoProvisions
		(PromotionId ,Value ,MinAmount ,MaxAmount ,CheckTypeIds, locationIds ,Groups, DTServerCreate ,DTServerLastModified ,DTTerminalCreate ,DTTerminalLastModified, 
		DiscountType)
	VALUES
		(@promotionId, 100, NULL, NULL, '6,7', NULL, '1', GETDATE(), NULL, GETDATE(), NULL, 1)

END

IF NOT EXISTS(SELECT 1 FROM tPromotions WHERE Name = 'TCFOCMO')
BEGIN
	INSERT 
		INTO 
	tPromotions
		(Name, Description, ProductId, ProviderId, StartDate, EndDate, Priority, IsSystemApplied, IsOverridable, IsNextCustomerSession, IsPrintable, DTServerCreate, 
		DTServerLastModified, DTTerminalCreate, DTTerminalLastModified, Status, FreeTransactionCount, Stackable)
	VALUES
		(N'TCFOCMO', N'Free TCF official check & money order cashing', 1, 200, '07/01/2015', '07/01/2030', NULL, 0, 1, 0, 0, GETDATE(), NULL, GETDATE(), NULL, 1, 
		NULL, 0)
	
	SET @promotionId = (SELECT PromotionId FROM tPromotions WHERE Name = 'TCFOCMO')
	
	INSERT 
		INTO 
	tPromoProvisions
		(PromotionId ,Value ,MinAmount ,MaxAmount ,CheckTypeIds, locationIds ,Groups ,DTServerCreate ,DTServerLastModified ,DTTerminalCreate, DTTerminalLastModified,
		DiscountType)
	VALUES
		(@promotionId, 100, NULL, NULL, '1,5', NULL, NULL, GETDATE(), NULL, GETDATE(), NULL, 1)

END

IF NOT EXISTS(SELECT 1 FROM tPromotions WHERE Name = 'FREECOMMAND')
BEGIN
	INSERT 
		INTO 
	tPromotions
		(Name, Description, ProductId, ProviderId, StartDate, EndDate, Priority, IsSystemApplied, IsOverridable, IsNextCustomerSession, IsPrintable, DTServerCreate, 
		DTServerLastModified, DTTerminalCreate, DTTerminalLastModified, Status, FreeTransactionCount, Stackable)
	VALUES
		(N'FREECOMMAND', N'Free command credit line check cashing', 1, 200, '07/01/2015', '07/01/2030', NULL, 0, 1, 0, 0, GETDATE(), NULL, GETDATE(), NULL, 1, 
		NULL, 0)
	
	SET @promotionId = (SELECT PromotionId FROM tPromotions WHERE Name = 'FREECOMMAND')
	
	INSERT 
		INTO 
	tPromoProvisions
		(PromotionId ,Value ,MinAmount ,MaxAmount ,CheckTypeIds, locationIds ,Groups ,DTServerCreate ,DTServerLastModified ,DTTerminalCreate, DTTerminalLastModified,
		DiscountType)
	VALUES
		(@promotionId, 100, NULL, NULL, '10', NULL, NULL, GETDATE(), NULL, GETDATE(), NULL, 1)

END

IF NOT EXISTS(SELECT 1 FROM tPromotions WHERE Name = 'COURTESYFREE')
BEGIN
	INSERT 
		INTO 
	tPromotions
		(Name, Description, ProductId, ProviderId, StartDate, EndDate, Priority, IsSystemApplied, IsOverridable, IsNextCustomerSession, IsPrintable, DTServerCreate, 
		DTServerLastModified, DTTerminalCreate, DTTerminalLastModified, Status, FreeTransactionCount, Stackable)
     VALUES
		(N'COURTESYFREE', N'Free check cashing for TCF Check Cash Customers', 1, 200, '07/01/2015', '07/01/2030', NULL, 0, 1, 0, 0, GETDATE(), NULL, GETDATE(), NULL, 
		1, NULL, 0)

	SET @promotionId = (SELECT PromotionId FROM tPromotions WHERE Name = 'COURTESYFREE')

	INSERT 
		INTO 
	tPromoProvisions
		(PromotionId ,Value ,MinAmount ,MaxAmount ,CheckTypeIds, locationIds ,Groups ,DTServerCreate ,DTServerLastModified ,DTTerminalCreate, DTTerminalLastModified,
		DiscountType)
	VALUES
		(@promotionId, 100, NULL, NULL, NULL, NULL, NULL, GETDATE(), NULL, GETDATE(), NULL, 1)

END

IF NOT EXISTS(SELECT 1 FROM tPromotions WHERE Name = 'LOANCUST')
BEGIN
	INSERT 
		INTO 
	tPromotions
		(Name, Description, ProductId, ProviderId, StartDate, EndDate, Priority, IsSystemApplied, IsOverridable, IsNextCustomerSession, IsPrintable, DTServerCreate, 
		DTServerLastModified, DTTerminalCreate, DTTerminalLastModified, Status, FreeTransactionCount, Stackable)
	VALUES
		 (N'LOANCUST', N'Free check cashing when making a TCF loan payment', 1, 200, '07/01/2015', '07/01/2030', NULL, 0, 1, 0, 0, GETDATE(), NULL, GETDATE(), NULL, 
		 1, NULL, 0)
	
	SET @promotionId = (SELECT PromotionId FROM tPromotions WHERE Name = 'LOANCUST')
	
	INSERT 
		INTO 
	tPromoProvisions
		(PromotionId ,Value ,MinAmount ,MaxAmount ,CheckTypeIds, locationIds ,Groups ,DTServerCreate ,DTServerLastModified ,DTTerminalCreate, DTTerminalLastModified,
		DiscountType)
	VALUES
		(@promotionId, 100, NULL, NULL, NULL, NULL, NULL, GETDATE(), NULL, GETDATE(), NULL, 1)

END

IF NOT EXISTS(SELECT 1 FROM tPromotions WHERE Name = 'GETONE')
BEGIN
	INSERT 
		INTO 
	tPromotions
	    (Name, Description, ProductId, ProviderId, StartDate, EndDate, Priority, IsSystemApplied, IsOverridable, IsNextCustomerSession, IsPrintable, DTServerCreate, 
		DTServerLastModified, DTTerminalCreate, DTTerminalLastModified, Status, FreeTransactionCount, Stackable)
	VALUES
	    (N'GETONE', N'Cash One, Get One: When you cash one check, your next check will be free (up to a check amount of $1,500).', 1, 200, '01/01/2018', 
		'05/15/2018', NULL, 0, 1, 0, 0, GETDATE(), NULL, GETDATE(),  NULL, 1, NULL, 0)
	
	SET @promotionId = (SELECT PromotionId FROM tPromotions WHERE Name = 'GETONE')
	
	INSERT 
		INTO 
	tPromoQualifiers
           (PromotionId, StartDate, EndDate, Amount, MinTransactionCount, ProductId, IsPaidFee, TransactionStates, IsParked, DTServerCreate, DTServerLastModified, 
		   DTTerminalCreate, DTTerminalLastModified)
	VALUES
           (@promotionId, '01/01/2018', '04/15/2018', NULL, 1, 1, 1, '4', 0, GETDATE(), NULL, GETDATE(), NULL)

	INSERT 
		INTO 
	tPromoProvisions
		(PromotionId ,Value ,MinAmount ,MaxAmount ,CheckTypeIds, locationIds ,Groups, DTServerCreate ,DTServerLastModified ,DTTerminalCreate ,
		DTTerminalLastModified ,DiscountType)
	VALUES
		(@promotionId, 100, 1, 1500, NULL, NULL, NULL, GETDATE(), NULL, GETDATE(), NULL, 1)

END
GO


