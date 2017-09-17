--- ===============================================================================
-- Author:		<Rizwana Shaik>
-- Create date: <23-03-2017>
-- Description:	Get Get Summary Receipt Data. 
-- Jira ID:		<8613>

-- ================================================================================

IF OBJECT_ID(N'usp_GetSummaryReceiptData', N'P') IS NOT NULL
BEGIN
     DROP PROCEDURE  usp_GetSummaryReceiptData
END
GO

CREATE PROCEDURE usp_GetSummaryReceiptData
(
      @customerSessionId BIGINT
)
AS
BEGIN
    BEGIN TRY
			DECLARE @CheckCount				INT
			DECLARE @MOCount				INT
			DECLARE @BPCount				INT
			DECLARE @SMCount				INT
			DECLARE @RMCount				INT
			DECLARE @CheckTotal				money
			DECLARE @MoneyOrderTotal		money
			DECLARE @BillpayTotal			money
			DECLARE @CashCollected			money
			DECLARE @CashToCustomer			money
			DECLARE @FundsGeneratingTotal	money
			DECLARE @FundsDepletingTotal	money
			DECLARE @GPRWithDraw			money
			DECLARE @GPRLoad				money
			DECLARE @GPRActivate			money
			DECLARE @MoneyTransferSend		money
			DECLARE @MoneyTransferReceive	money
			DECLARE @MoneyTransferModified	money
			DECLARE @MoneyTransferCancelled money
			DECLARE @MoneyTransferRefund	money
			DECLARE @NetAmount				money
			DECLARE @CardNumber				VARCHAR(50)
			DECLARE @TotalMsg				VARCHAR(50)
			DECLARE @IsAddOn				INT
			DECLARE @Gpr                    VARCHAR(50)
			DECLARE @GprCount               INT
			DECLARE @cartId                 BIGINT   

					SELECT @cartId = MAX(dbo.tShoppingCarts.CartID) 
					FROM  tShoppingCarts 
					WHERE dbo.tShoppingCarts.CustomerSessionId = @customerSessionId 

					SELECT @GprCount = COUNT(1) 
					FROM   tCustomerSessions tCS 
							INNER JOIN tCustomers tC					ON tC.CustomerID = tCS.CustomerID
							INNER JOIN tVisa_Account tVA                ON tVA.CustomerId = tC.CustomerID
					WHERE  tCS.CustomerSessionID = @customerSessionId

					SELECT @CheckTotal = ISNULL(SUM(Amount),0.00) -ISNULL(SUM(Fee),0.00), 
							@CheckCount = Count(1) 
					FROM   tTxn_Check tTC
							JOIN tShoppingCartTransactions tSCT ON tSCT.TransactionId = tTC.TransactionID AND tSCT.ProductId = 1
							JOIN tShoppingCarts tSC ON tSC.CartID = tSCT.CartId
					WHERE tSC.CartID=@cartId

					SELECT @MoneyOrderTotal = ISNULL(SUM(Amount),0.00) + ISNULL(SUM(Fee),0.00), 
							@MOCount = Count(1) 
					FROM   tTxn_MoneyOrder tTC
							JOIN tShoppingCartTransactions tSCT ON tSCT.TransactionId = tTC.TransactionID AND tSCT.ProductId = 5 
							JOIN tShoppingCarts tSC ON tSC.CartID = tSCT.CartId
					WHERE tSC.CartID = @cartId

					SELECT @BillpayTotal = ISNULL(SUM(Amount),0.00) +ISNULL(SUM(Fee),0.00), 
							@BPCount = Count(1) 
					FROM   tTxn_BillPay tTC
							JOIN tShoppingCartTransactions tSCT ON tSCT.TransactionId = tTC.TransactionID AND tSCT.ProductId = 2
							JOIN tShoppingCarts tSC ON tSC.CartID = tSCT.CartId
					WHERE tSC.CartID = @cartId

					SELECT @MoneyTransferSend = ISNULL(SUM(Amount),0.00) + ISNULL(SUM(Fee),0.00), 
							@SMCount = COUNT(1) 
					FROM   tTxn_MoneyTransfer tTC
							JOIN tShoppingCartTransactions tSCT ON tSCT.TransactionId = tTC.TransactionID AND tSCT.ProductId = 3
							JOIN tShoppingCarts tSC ON tSC.CartID = tSCT.CartId
					WHERE tSC.CartID = @cartId AND TransferType = 1 AND TransactionSubType is NULL OR TransactionSubType = ''

					SELECT @MoneyTransferReceive = ISNULL(SUM(Amount),0.00),
							@RMCount = COUNT(1) FROM tTxn_MoneyTransfer tTC
							JOIN tShoppingCartTransactions tSCT ON tSCT.TransactionId = tTC.TransactionID AND tSCT.ProductId = 3
							JOIN tShoppingCarts tSC ON tSC.CartID = tSCT.CartId
					WHERE tSC.CartID = @cartId AND TransferType = 2 

					SELECT @MoneyTransferCancelled = ISNULL(SUM(Amount),0.00) + ISNULL(SUM(Fee),0.00) 
					FROM   tTxn_MoneyTransfer tTC
							JOIN tShoppingCartTransactions tSCT ON tSCT.TransactionId = tTC.TransactionID AND tSCT.ProductId = 3
							JOIN tShoppingCarts tSC ON tSC.CartID = tSCT.CartId
					WHERE tSC.CartID = @cartId AND TransferType = 1 AND TransactionSubType = 1

					SELECT @MoneyTransferModified = ISNULL(SUM(Amount),0.00) + ISNULL(SUM(Fee),0.00) 
					FROM   tTxn_MoneyTransfer tTC
							JOIN tShoppingCartTransactions tSCT ON tSCT.TransactionId = tTC.TransactionID AND tSCT.ProductId = 3
							JOIN tShoppingCarts tSC ON tSC.CartID = tSCT.CartId
					WHERE tSC.CartID = @cartId AND TransferType = 1 AND TransactionSubType=2

					SELECT @MoneyTransferRefund = ISNULL(SUM(Amount),0.00) + ISNULL(SUM(Fee),0.00) 
					FROM   tTxn_MoneyTransfer tTC
							JOIN tShoppingCartTransactions tSCT ON tSCT.TransactionId = tTC.TransactionID AND tSCT.ProductId = 3
							JOIN tShoppingCarts tSC ON tSC.CartID = tSCT.CartId
					WHERE tSC.CartID = @cartId AND TransferType = 1 AND TransactionSubType=3

					SELECT @GPRLoad = ISNULL(SUM(Amount),0.00) 
					FROM tTxn_Funds tTC
						JOIN tShoppingCartTransactions tSCT ON tSCT.TransactionId = tTC.TransactionID AND tSCT.ProductId = 6
						JOIN tShoppingCarts tSC ON tSC.CartID = tSCT.CartId
					WHERE tSC.CartID = @cartId AND FundType = 1

					SELECT @GPRWithDraw = ISNULL(SUM(Amount),0.00)
					FROM tTxn_Funds tTC
						JOIN tShoppingCartTransactions tSCT ON tSCT.TransactionId = tTC.TransactionID AND tSCT.ProductId = 6
						JOIN tShoppingCarts tSC ON tSC.CartID = tSCT.CartId
					WHERE tSC.CartID = @cartId AND FundType = 0

					SELECT @GPRActivate = ISNULL(SUM(Amount),0.00) + ISNULL(SUM(Fee),0.00) 
					FROM tTxn_Funds tTC
						JOIN tShoppingCartTransactions tSCT ON tSCT.TransactionId = tTC.TransactionID AND tSCT.ProductId = 6
						JOIN tShoppingCarts tSC ON tSC.CartID = tSCT.CartId
					WHERE tSC.CartID = @cartId AND FundType = 2

					SELECT @IsAddOn = COUNT(*) 
					FROM tTxn_Funds tTC
						JOIN tShoppingCartTransactions tSCT ON tSCT.TransactionId = tTC.TransactionID AND tSCT.ProductId = 6
						JOIN tShoppingCarts tSC ON tSC.CartID = tSCT.CartId
					WHERE tSC.CartID = @cartId AND FundType = 3

					SELECT @CashCollected = ISNULL(SUM(Amount),0.00) 
					FROM tTxn_Cash tTC
						JOIN tShoppingCartTransactions tSCT ON tSCT.TransactionId = tTC.TransactionID AND tSCT.ProductId=7
						JOIN tShoppingCarts tSC ON tSC.CartID = tSCT.CartId
					WHERE tSC.CartID = @cartId AND CashType = 1

				
					SELECT @CardNumber = tVA.CardNumber 
					FROM    tCustomerSessions tCS 
							INNER JOIN tCustomers tC					ON tC.CustomerID = tCS.CustomerID
							INNER JOIN tVisa_Account tVA                ON tVA.CustomerId = tC.CustomerID
					WHERE  tCS.CustomerSessionID = @customerSessionId and tVA.Activated=1

						SET @FundsGeneratingTotal = @CheckTotal+ @GPRWithDraw + @MoneyTransferReceive + @MoneyTransferCancelled + @MoneyTransferRefund
						SET @FundsDepletingTotal = @BillpayTotal + @MoneyTransferSend + @MoneyOrderTotal + @GPRLoad + @GPRActivate + @MoneyTransferModified
						SET @NetAmount = @FundsGeneratingTotal - @FundsDepletingTotal
						SET @CashToCustomer = @CashCollected + @NetAmount 
						SET @TotalMsg = CASE	WHEN @NetAmount > 0 THEN 'TOTAL DUE TO CUSTOMER'
												ELSE 'TOTAL AMOUNT DUE'
												END
						SET @Gpr      = CASE	WHEN @GprCount > 0 THEN 'Gpr'
												ELSE 'NonGpr'
												END
                    SELECT 
						@CheckCount									AS CheckCount,			
						@MOCount									AS MOCount,
						@BPCount									AS BPCount,				
						@SMCount									AS SMCount,				
						@RMCount									AS RMCount,				
						@CheckTotal									AS CheckTotal,				
						@MoneyOrderTotal							AS MoneyOrderTotal,		
						@BillpayTotal								AS BillpayTotal,			
						@CashCollected								AS CashCollected,		
						@CashToCustomer								AS CashToCustomer,			
						@FundsGeneratingTotal						AS FundsGeneratingTotal,	
						@FundsDepletingTotal						AS FundsDepletingTotal,	
						@GPRWithDraw								AS GPRWithDraw,			
						@GPRLoad									AS GPRLoad,				
						@GPRActivate								AS GPRActivate,			
						@MoneyTransferSend							AS MoneyTransferSend,		
						@MoneyTransferReceive						AS MoneyTransferReceive,	
						@MoneyTransferModified						AS MoneyTransferModified,	
						@MoneyTransferCancelled						AS MoneyTransferCancelled, 
						@MoneyTransferRefund						AS MoneyTransferRefund,	
						@NetAmount									AS NetAmount,				
						@CardNumber									AS CardNumber,				
						@TotalMsg									AS TotalMsg,				
						@IsAddOn									AS IsAddOn,
						@Gpr										AS  Gpr,
						ISNULL(tSC.DTTerminalLastModified, tSC.DTTerminalCreate) 
																	AS ReceiptDate,				
						tCS.CustomerSessionID						AS SessionlID,
						tCS.DTStart									AS CustomerSessionDate,
						tC.FirstName + ' ' + ISNULL(tC.LastName, '') 
																	AS CustomerName,
						tC.FirstName + ' ' + ISNULL(tC.LastName, '') 
																	AS SenderName,
						tC.Address1 + ' ' + ISNULL(tC.Address2,'')
																	AS SenderAddress,
						tC.City										AS SenderCity,
						tC.State									AS SenderState,
						tC.ZipCode									AS SenderZip,
						tC.Phone1									AS SenderPhoneNumber,
						CASE	WHEN tC.Phone1Type = 'Cell' THEN tC.Phone1
								WHEN ISNULL(tC.Phone2Provider, '') = 'Cell' THEN tC.Phone2
								ELSE ''
								END									AS SenderMobileNumber,

						-- Agent Details
						tAD.ClientAgentIdentifier					AS TellerNumber,
						SUBSTRING(tAD.UserName, 0, 6)				AS TellerName,

						-- Terminal Details
						tT.TerminalID								AS TerminalID,

						-- Location Details
						tL.Address1 + ' ' + ISNULL(tL.Address2,'') 
																	AS LocationAddress,
						tL.City										AS LocationCity,
						tL.State									AS LocationState,
						tL.ZipCode									AS LocationZip,
						tL.BranchID									AS BranchId,
						tL.BankID									AS BankId,
						tL.PhoneNumber								AS LocationPhoneNumber,
						tL.LocationName								AS LocationName,
						tL.TimezoneID								AS Timezone,
				CASE	WHEN tL.TimezoneID = 'Pacific Standard Time' THEN 'PST'
						WHEN tL.TimezoneID = 'Mountain Standard Time' THEN 'MST'
						WHEN tL.TimezoneID = 'Eastern Standard Time' THEN 'EST'
						WHEN tL.TimezoneID = 'Central Standard Time' THEN 'CST'
						ELSE ''
						END									        AS TimezoneId,
				-- Channel Partner Details
				       tCP.Name									    AS ClientName,
				       tCP.LogoFileName							    AS LogoUrl
                 FROM  tCustomerSessions tCS WITH (NOLOCK)
						INNER JOIN tShoppingCarts tSC WITH (NOLOCK)	            ON tSC.CustomerSessionId=tCS.CustomerSessionId     	
						INNER JOIN tCustomers tC WITH (NOLOCK)					ON tC.CustomerID = tCS.CustomerID
						INNER JOIN tAgentSessions tAS WITH (NOLOCK)				ON tAS.AgentSessionID = tCS.AgentSessionId
						INNER JOIN tAgentDetails tAD WITH (NOLOCK)				ON tAD.AgentID = tAS.AgentId
						INNER JOIN tTerminals tT WITH (NOLOCK)					ON tT.TerminalID = tAS.TerminalId
						INNER JOIN tLocations tL WITH (NOLOCK)					ON tL.LocationID = tT.LocationId
						INNER JOIN tChannelPartners tCP WITH (NOLOCK)			ON tL.ChannelPartnerId = tCP.ChannelPartnerId
                 WHERE tCS.CustomerSessionId=@customerSessionId
     END TRY
	 BEGIN CATCH
	           EXECUTE usp_CreateErrorInfo
	 END CATCH
END
