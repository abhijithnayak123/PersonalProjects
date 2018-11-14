-- ================================================================================
-- Author:		  <M.Purna Pushkal>
-- Create date:   <10/10/2017>
-- Description:   <Auto search the customers from the customer registration>
-- Jira ID:		  <B-07648>
-- ================================================================================

IF EXISTS(SELECT 1 FROM SYS.objects WHERE NAME = 'usp_GetCustomersForAutoSearch')
BEGIN
	DROP PROCEDURE usp_GetCustomersForAutoSearch
END
GO

CREATE PROCEDURE usp_GetCustomersForAutoSearch
(
	 @pkyNumbers XML,
	 @ssn NVARCHAR(50),
	 @dob DATETIME,
	 @lastName NVARCHAR(255)
)
AS

BEGIN 
	 BEGIN TRY

	 DECLARE @resultTable TABLE
		(
			 CustomerID BIGINT
			,FirstName NVARCHAR(255)
			,LastName NVARCHAR(255)
			,DOB DATETIME
			,Address1 NVARCHAR(255)
			,IdNumber NVARCHAR(255)			
			,GovtIDExpirationDate DATE
			,SSN NVARCHAR(50)
			,Phone NVARCHAR(50)
			,CardNumber NVARCHAR(50)
			,PartnerAccountNumber NVARCHAR(100)
		)

	INSERT INTO @resultTable 
	EXECUTE usp_GetRcifCustomersByPky @pkyNumbers, @ssn

	 IF NOT EXISTS(SELECT 1 FROM @resultTable) AND CAST(@pkyNumbers AS NVARCHAR(500)) = ''
	 BEGIN
			SELECT 
				  C.CustomerID
				 ,C.FirstName 
				 ,C.LastName 
				 ,C.DOB
				 ,C.Address1
				 ,C.GovtIdentification AS IdNumber
				 ,C.GovtIDExpirationDate
				 ,C.SSN
				 ,(CASE 
					  WHEN ISNULL(C.Phone1,'') != '' THEN C.Phone1
					  WHEN ISNULL(C.Phone2,'') != '' THEN C.Phone2 
					  ELSE NULL
					  END) AS Phone
				 ,tva.CardNumber
				 ,tca.PartnerAccountNumber							
			 FROM 
				 tCustomers C WITH (NOLOCK)
			 INNER JOIN tTCIS_Account tca WITH (NOLOCK) ON tca.CustomerID = C.CustomerID AND C.DOB = @dob AND C.LastName = @lastName
			 LEFT JOIN tVisa_Account tva WITH (NOLOCK) ON tva.CustomerId = c.CustomerID AND tva.Activated = 1
			 WHERE
					 tca.ProfileStatus = 1 AND C.ProfileStatus = 1
	 END
	 ELSE
	 BEGIN
		  SELECT * FROM @resultTable
	 END

	 END TRY

	 BEGIN CATCH
		  EXECUTE usp_CreateErrorInfo
	 END CATCH

END