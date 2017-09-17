--- ===============================================================================
-- Author:		<Purna Pushkal>
-- Create date: <18-11-2016>
-- Description:	 Create bill pay transaction
-- Jira ID:		<AL-8320>
-- ================================================================================

IF OBJECT_ID('usp_CreateBillPayTransaction') IS NOT NULL
BEGIN
	DROP PROCEDURE dbo.usp_CreateBillPayTransaction
END
GO

CREATE PROCEDURE usp_CreateBillPayTransaction
(
      @transactionId      BIGINT OUTPUT,
	  @customerSessionId  BIGINT,
	  @providerAccountId  BIGINT,
	  @providerId         BIGINT,
	  @amount             MONEY,
	  @fee                MONEY,
	  @description        NVARCHAR(255),
	  @state              INT,
	  @confirmationNumber VARCHAR(50),
	  @dtTerminalCreate   DATETIME,
	  @dtServerCreate     DATETIME,
	  @accountNumber      NVARCHAR(50),
	  @billerNameOrCode   VARCHAR(255)
)															
AS
BEGIN
	 BEGIN TRY

		  DECLARE @customerRevisionNo INT
		  DECLARE @productId BIGINT
		  DECLARE @channelPartnerId BIGINT


		  -- Getting the customerRevisionNo from tCustomers_Aud

		SELECT 
			@customerRevisionNo = ISNULL(MAX(ca.RevisionNo), 0),
			@channelPartnerId = tc.ChannelPartnerId
		FROM 
			tCustomers_Aud AS ca WITH (NOLOCK)
			INNER JOIN tCustomerSessions AS cs WITH (NOLOCK) ON ca.CustomerID = cs.CustomerID
			INNER JOIN tCustomers tc WITH (NOLOCK) ON tc.CustomerID = cs.CustomerID
		WHERE 
			cs.CustomerSessionID = @customerSessionId
		GROUP BY 
			tc.ChannelPartnerId

		  --getting the productId from the tMasterCatalog table using the billerCode
		  SELECT @productId = dbo.ufn_GetProductIdByBillerInfo(@billerNameOrCode, @channelPartnerId)

		  --inserting into tTxn_Billpay table

		INSERT INTO dbo.tTxn_BillPay
		( 
			Amount,
			Fee,
			Description,
			State,
			DTTerminalCreate,
			ConfirmationNumber,
			ProductId,
			AccountNumber,
			DTServerCreate,
			CustomerSessionID,
			ProviderAccountID,
			ProviderID,
			CustomerRevisionNo
		)
		VALUES
		( 
			@amount,
			@fee,
			@productId,
			@state,
			@dtTerminalCreate,
			@confirmationNumber,
			@productId,
			@accountNumber,
			@dtServerCreate,
			@customerSessionID,
			@providerAccountID,
			@providerID,
			@customerRevisionNo
		)

		SELECT @transactionId = CAST(SCOPE_IDENTITY() AS BIGINT)

	 END TRY

	 BEGIN CATCH
		  EXECUTE usp_CreateErrorInfo
	 END CATCH

END
GO
