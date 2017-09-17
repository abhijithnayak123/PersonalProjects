--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-07-2016>
-- Description:	This SP is used to create a account fro money transfer.
-- Jira ID:		<AL-8324>
--exec usp_CreateMoneyTransferWUAccount 'D',1000000015,1000000000000020,'','','12/8/2016 4:27:08 PM ','12/8/2016 4:27:08 PM '
-- ================================================================================

IF OBJECT_ID(N'usp_CreateMoneyTransferWUAccount', N'P') IS NOT NULL
DROP PROC usp_CreateMoneyTransferWUAccount
GO

CREATE PROCEDURE [dbo].[usp_CreateMoneyTransferWUAccount]
(
       @nameType VARCHAR(200) = 'D'
       ,@customerSessionId BIGINT
       ,@customerId BIGINT
       ,@preferredCustomerAccountNumber VARCHAR(250)
       ,@preferredCustomerLevelCode VARCHAR(250)
       ,@dTServerCreate DATETIME
       ,@dTTerminalCreate DATETIME
)
AS
BEGIN
       
BEGIN TRY

       --DECLARE @accountId BIGINT = 
       -- (
       --       SELECT ISNULL(MAX(WUAccountID),1000000000) + 1 
       --       FROM tWUnion_Account
       --)

       DECLARE @rowCount BIGINT=
       (
              SELECT count(1) FROM tWUnion_Account WHERE CustomerID=@customerId
       )
       
        DECLARE @customerRevisionNo BIGINT =
       (
              SELECT 
                 MAX(RevisionNo) 
               FROM
                 tCustomers_Aud 
               WHERE 
                 CustomerId = @customerId
       )
       
        IF(@rowCount>0)
              BEGIN
                     SELECT CAST(WUAccountID AS bigint) AS accountId FROM tWUnion_Account  WHERE CustomerID = @customerId
              END
       ELSE   
              BEGIN
       
                     INSERT INTO [dbo].[tWUnion_Account]
                            ([DTTerminalCreate]
                            ,[NameType]
                            ,[PreferredCustomerAccountNumber]
                            ,[PreferredCustomerLevelCode]
                            ,[DTServerCreate]
                            ,[CustomerId]
                            ,[CustomerRevisionNo]
                            ,[CustomerSessionId])
                     VALUES
                            (@dTTerminalCreate
                            ,@nameType
                            ,@preferredCustomerAccountNumber
                            ,@preferredCustomerLevelCode
                            ,@dTServerCreate
                            ,@customerId
                            ,@customerRevisionNo
                            ,@customerSessionId)

                              SELECT CAST (SCOPE_IDENTITY() AS bigint) AS accountId
              END

END TRY
BEGIN CATCH

       EXECUTE usp_CreateErrorInfo
              
END CATCH
END



GO


