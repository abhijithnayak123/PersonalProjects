-- ================================================================================
-- Author:		<Abhijith>
-- Create date: <03/24/2017>
-- Description:	<As an engineer, I want to implement ADO.Net for Customer module>
-- Jira ID:		<AL-7630>
-- ================================================================================


IF EXISTS(
	SELECT 1
	FROM SYS.objects
	WHERE NAME = 'usp_TCF_AddTCISAccount'
)

BEGIN
	DROP PROCEDURE usp_TCF_AddTCISAccount
END
GO


CREATE PROCEDURE usp_TCF_AddTCISAccount
   @PartnerAccountNumber NVARCHAR(100)
   ,@RelationshipAccountNumber NVARCHAR(100)
   ,@ProfileStatus SMALLINT
   ,@DTTerminalCreate DATETIME
   ,@BankId NVARCHAR(40)
   ,@BranchId NVARCHAR(40)
   ,@DTServerCreate DATETIME
   ,@TcfCustInd BIT
   ,@CustomerID BIGINT
   ,@CustomerSessionID BIGINT
AS
BEGIN
	BEGIN TRY
		
		   
       DECLARE @customerRevisionNo BIGINT =
       (
              SELECT 
                 ISNULL(MAX(RevisionNo),1) 
               FROM
                 tCustomers_Aud 
               WHERE 
                 CustomerId = @customerId
       )

		INSERT INTO [tTCIS_Account]
           ([PartnerAccountNumber]
           ,[RelationshipAccountNumber]
           ,[ProfileStatus]
           ,[DTTerminalCreate]
           ,[BankId]
           ,[BranchId]
           ,[TcfCustInd]
           ,[DTServerCreate]
           ,[CustomerID]
           ,[CustomerSessionID]
           ,[CustomerRevisionNo])
           VALUES
			   (@PartnerAccountNumber,
				@RelationshipAccountNumber,
				@ProfileStatus,
				@DTTerminalCreate,
				@BankId,
				@BranchId,
				@TcfCustInd,
				@DTServerCreate,
				@CustomerID,
				@CustomerSessionID, 
				@customerRevisionNo)
		
		
		SELECT CAST(SCOPE_IDENTITY() AS BIGINT) AS TCISAccountID

	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END
