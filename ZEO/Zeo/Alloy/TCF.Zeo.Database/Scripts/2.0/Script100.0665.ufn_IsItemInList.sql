-- ====================================================================
-- Author:		<Abhijith>
-- Create date: <01/19/2018>
-- Description:	<Verify, the promotion is already applied or not>
-- =====================================================================


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
