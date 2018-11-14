-- ====================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <01/17/2018>
-- Description:	<Verify, the promotion is already applied or not>
-- =====================================================================

-- select  dbo.ufn_IsPromotionApplied(10000001, 2540247527168537, 1, 1)

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'ufn_IsPromotionApplied') AND xtype IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION ufn_IsPromotionApplied
GO

CREATE FUNCTION [dbo].[ufn_IsPromotionApplied]
(    
     @promotionId BIGINT
	,@transactionId BIGINT
	,@customerId  BIGINT
	,@minTrxCount INT
	,@maxTrxCount INT
)
RETURNS BIT
AS
BEGIN
	
		 DECLARE @trxCount INT = 0, @IsPromotionApplied BIT = 0
		

		        SELECT @trxCount = COUNT(1) FROM tTxn_FeeAdjustments WHERE CustomerId = @customerId AND PromotionId = @promotionId AND TransactionId != @transactionId

				-- eg. Consider a promotion - ThreeThenTwofree, In this case for 4th and 5th transaction we need to give the Promotion and these values (i.e, 4,5) are configured
				-- in Min and Max trx count. So here '@IsPromotionApplied' is used to check whether the Promo need to be applied for the current trx or not.  
				
				IF((@maxTrxCount - @minTrxCount) + 1 = @trxCount)  
				BEGIN
				    SET @IsPromotionApplied = 1
				END
		         
		
		-- Return the result of the function
		RETURN @IsPromotionApplied

END
GO

-- select  dbo.ufn_IsItemInList('4', '1,2,2,3,4,5')

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'ufn_IsItemInList') AND xtype IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION ufn_IsItemInList
GO

CREATE FUNCTION [dbo].[ufn_IsItemInList]
(
	 @ItemId VARCHAR(500)
	,@List  NVARCHAR(MAX)
)
RETURNS BIT
AS
BEGIN
		DECLARE @isExists BIT = 0

		SET @isExists =
		(
			SELECT 
				CASE 
					WHEN CHARINDEX(',' + @ItemId + ',', ',' + @List + ',') > 0
					THEN 1
					ELSE 0
				END
		)

		-- Return the result of the function
		RETURN @isExists

END
GO

-- ==========================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <01/17/2018>
-- Description:	<Verify, whether the customer belongs to this group or not.>
-- ==========================================================================

-- select  dbo.ufn_IsCustomerBelongsToGroup(2540247527168537, '1,2')

IF EXISTS (SELECT 1 FROM SYSOBJECTS WHERE id = OBJECT_ID(N'ufn_IsCustomerBelongsToGroup') AND xtype IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION ufn_IsCustomerBelongsToGroup
GO

CREATE FUNCTION ufn_IsCustomerBelongsToGroup
(    
     @customerId  BIGINT
	,@groupIds    NVARCHAR(100)
)
RETURNS BIT
AS
BEGIN
	
		        DECLARE @IsCustomerBelongsToGroup BIT = 0

                IF (ISNULL(@groupIds,'') = '' OR (SELECT count(1) FROM tCustomers WHERE CustomerID = @customerId AND (Group1 IN (@groupIds) OR Group2 IN (@groupIds))) > 0)
				BEGIN
				     SET @IsCustomerBelongsToGroup = 1
		        END
				
				RETURN @IsCustomerBelongsToGroup

END
GO

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'SplitString') AND xtype IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION SplitString
GO

CREATE FUNCTION SplitString
(    
      @Input NVARCHAR(MAX),
      @Character CHAR(1)
)
RETURNS @Output TABLE (
      Item NVARCHAR(1000)
)
AS
BEGIN
      DECLARE @StartIndex INT, @EndIndex INT
 
      SET @StartIndex = 1
      IF SUBSTRING(@Input, LEN(@Input) - 1, LEN(@Input)) <> @Character
      BEGIN
            SET @Input = @Input + @Character
      END
 
      WHILE CHARINDEX(@Character, @Input) > 0
      BEGIN
            SET @EndIndex = CHARINDEX(@Character, @Input)
           
            INSERT INTO @Output(Item)
            SELECT SUBSTRING(@Input, @StartIndex, @EndIndex - 1)
           
            SET @Input = SUBSTRING(@Input, @EndIndex + 1, LEN(@Input))
      END
 
      RETURN
END
GO
