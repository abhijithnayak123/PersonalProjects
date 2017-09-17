IF OBJECT_ID(N'usp_CreateReceiveMoneyWUTransaction', N'P') IS NOT NULL
DROP PROC usp_CreateReceiveMoneyWUTransaction
GO

CREATE PROCEDURE usp_CreateReceiveMoneyWUTransaction
(
	@customerSessionId BIGINT,
	@mtcn NVARCHAR(50),
	@channelPartnerId BIGINT,
	@providerId INT,
	@tranascationType VARCHAR(20),
	@dtTerminalCreate DATETIME,
	@dtServerCreate DATETIME,
	@referenceNo VARCHAR(50),
	@wuTrxId BIGINT OUTPUT
)

AS
BEGIN
	
BEGIN TRY

	--DECLARE @IsExist INT = 
	--(
	--	SELECT COUNT(1) FROM tWUnion_Trx 
	--	WHERE WUTrxId = @WUTrxId
	--)

	--IF @IsExist = 0
	--BEGIN
			
			DECLARE @providerAccount BIGINT
			DECLARE @gcNumber NVARCHAR(50)

			SELECT 
				@providerAccount = wa.WUAccountID, @gcNumber = wa.PreferredCustomerAccountNumber 
			FROM 
				tCustomers c
			INNER JOIN 
				tCustomerSessions cs ON cs.CustomerID = c.CustomerID
			INNER JOIN 
				tWUnion_Account wa ON wa.CustomerId = c.CustomerID
			WHERE 
				cs.CustomerSessionID = @customerSessionId

			INSERT INTO [tWUnion_Trx](
			       [WUAccountID],
				   [ReferenceNo],
				   [GCNumber],
				   [Mtcn],
				   [TranascationType],
				   [ChannelPartnerId],
			       [ProviderId],
				   [DTTerminalCreate],
				   [DTServerCreate])
			VALUES(
				   @providerAccount,
				   @referenceNo,
				   @gcNumber,
				   @mtcn,
				   @tranascationType,
				   @channelPartnerId,
				   @providerId,
				   @dtTerminalCreate,
				   @dtServerCreate) 


			SET @WUTrxId = CAST (SCOPE_IDENTITY() AS BIGINT)
	--END

END TRY

BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH
END
GO
