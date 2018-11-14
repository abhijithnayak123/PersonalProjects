--- ===============================================================================
-- Author:		<Purna Pushkal>
-- Create date: <18-01-2017>
-- Description: Get the Prepaid card number
-- Jira ID:		<AL-8320>
-- ================================================================================

IF OBJECT_ID('usp_GetVisaCardNumber') IS NOT NULL
BEGIN
	DROP PROCEDURE dbo.usp_GetVisaCardNumber
END
GO
CREATE PROCEDURE usp_GetVisaCardNumber
(
	 @customerId BIGINT
)
AS
BEGIN
	 BEGIN TRY
		  SELECT 
				CardNumber 
		   FROM 
				tVisa_Account WITH(NOLOCK)
			WHERE 
				CustomerId = @customerId
				AND
				Activated = 1
	 END TRY

	 BEGIN CATCH
		  Execute usp_CreateErrorInfo
	 END CATCH
END