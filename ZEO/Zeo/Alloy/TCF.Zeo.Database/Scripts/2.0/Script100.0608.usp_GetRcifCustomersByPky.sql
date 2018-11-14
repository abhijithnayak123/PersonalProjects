--- ===============================================================================
-- Author:		<M.Purna Pushkal>
-- Create date: <08-28-2017>
-- Description: To get the zeo customers fro the PKY numbers.
-- Jira ID:		<B-06198 - Single search screen - card data>
-- ================================================================================

IF OBJECT_ID(N'usp_GetRcifCustomersByPky', N'P') IS NOT NULL
	DROP PROC usp_GetRcifCustomersByPky
GO

CREATE PROCEDURE usp_GetRcifCustomersByPky	
	@pkyNumbers XML
AS
BEGIN

	BEGIN TRY
		DECLARE @xmlTable TABLE
		(
			  PKYNumber NVARCHAR(255)
		)

		INSERT INTO @xmlTable (PKYNumber)
		(
			SELECT 
				T.C.value('PKYNumber[1]', 'NVARCHAR(50)')
		     FROM 
			   @PkyNumbers.nodes('/DocumentElement/PKYTable') AS T(C)
		)
		
		SELECT 
			 C.CustomerID
			,C.FirstName 
			,C.LastName 
			,C.DOB
			,C.Address1
			,C.GovtIdentification AS IdNumber
			,C.SSN
			,(CASE 
				 WHEN ISNULL(C.Phone1,'') != '' THEN C.Phone1
				 WHEN ISNULL(C.Phone2,'') != '' THEN C.Phone2 
				 ELSE NULL
				 END) AS Phone
			,tva.CardNumber
			,tca.PartnerAccountNumber							
		FROM 
			@xmlTable xt
		INNER JOIN tTCIS_Account tca WITH (NOLOCK) 
			ON xt.PKYNumber = tca.PartnerAccountNumber AND tca.ProfileStatus = 1
		INNER JOIN tCustomers C WITH (NOLOCK) 
			ON tca.CustomerId = C.CustomerID AND C.ProfileStatus = 1
      LEFT JOIN tVisa_Account tva WITH (NOLOCK)
		  ON tva.CustomerId = C.CustomerID AND tva.Activated = 1				
			
	END TRY

	BEGIN CATCH
		EXECUTE usp_CreateErrorInfo
	END CATCH
END