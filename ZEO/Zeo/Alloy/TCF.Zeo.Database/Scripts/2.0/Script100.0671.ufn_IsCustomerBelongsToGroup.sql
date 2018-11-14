-- ==========================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <01/30/2018>
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

                IF (ISNULL(@groupIds,'') = '' OR (SELECT COUNT(1) FROM tCustomers WHERE CustomerID = @customerId AND (Group1 IN (SELECT CAST(item AS INT) FROM dbo.SplitString(@groupIds,',')) OR Group2 IN (SELECT CAST(item AS INT) FROM dbo.SplitString(@groupIds,',')))) > 0)
				BEGIN
				     SET @IsCustomerBelongsToGroup = 1
		        END
				
				RETURN @IsCustomerBelongsToGroup

END
GO
