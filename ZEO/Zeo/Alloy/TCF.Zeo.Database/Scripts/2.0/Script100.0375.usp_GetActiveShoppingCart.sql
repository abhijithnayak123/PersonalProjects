-- ================================================================================
-- Author:		Manikandan Govindraj
-- Create date: 12/07/2016
-- Description: Get Active Shopping Cart
-- Jira ID:		AL-8047
-- ================================================================================

-- exec usp_GetActiveShoppingCart 1000000002


IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'usp_GetActiveShoppingCart')
BEGIN
	DROP PROCEDURE usp_GetActiveShoppingCart
END
GO

CREATE PROCEDURE usp_GetActiveShoppingCart
(
	@customerSessionId BIGINT
)
AS
BEGIN
	
	BEGIN TRY

	    SET NOCOUNT ON
		DECLARE @isReferralSectionEnable BIT
		DECLARE @isCheckFrank BIT
		DECLARE @cartId BIGINT = dbo.ufn_GetShoppingCartId(@customerSessionId, 1)		
		DECLARE @channelPartnerId SMALLINT		

		SELECT 
		  @channelPartnerId = ChannelPartnerId
	    FROM 
		  tCustomers tc WITH (NOLOCK)
		  INNER JOIN tCustomerSessions tcs WITH (NOLOCK)
				ON tc.CustomerID = tcs.CustomerID 
		WHERE tcs.CustomerSessionID = @customerSessionId

		
		SELECT 
		   @isCheckFrank = IsCheckFrank,
		   @isReferralSectionEnable = IsReferralSectionEnable
		FROM 
		   tChannelPartnerConfig WITH (NOLOCK)
		WHERE 
		   ChannelPartnerId = @channelPartnerId -- ChannelPartnerId 


   
	    -- Get Shopping Cart details
		SELECT 
		     CartId,
			 IsReferral,		
		     @isReferralSectionEnable AS IsReferralSectionEnabled,  -- Need to add some logic in this flag
			 @isCheckFrank AS IsCheckFrank

	    FROM
		    tShoppingCarts WITH (NOLOCK)
	    WHERE 
		    CartID = @cartId


     IF(@cartId IS NOT NULL)
     BEGIN

         -- Get Check transaction
        SELECT 
           tc.TransactionId,
           tc.Amount,
	       tc.Fee,
	       tc.Description,
	       tc.[State] AS State,
	       tc.BaseFee,
	       tc.DiscountApplied,
	       tc.DiscountName,
	       ct.Message AS DeclineMessage,	    
		   sct.ProductId as ProductId
	    FROM
	       tShoppingCartTransactions sct WITH (NOLOCK)
	    INNER JOIN
	       tTxn_Check tc WITH (NOLOCK)
	       ON sct.TransactionId = tc.TransactionId 	
	    INNER JOIN
	       tChxr_Trx ct WITH (NOLOCK)
	       ON tc.CXNId = ct.ChxrTrxID 
	    WHERE
			sct.CartId = @cartId 
			AND 
			sct.ProductId = 1 --Check
			AND
			sct.CartItemStatus = 0
	    

	    -- Get Bill Pay transaction

        SELECT 
			 bp.AccountNumber,
			 bp.Amount,
			 bp.Fee,
			 bp.ConfirmationNumber,
			 bp.Description,
			 bp.[State] as State,
			 bp.ProductId as BillerId,
			 bp.TransactionId,
			 mc.BillerName,
			 sct.ProductId as ProductId			 
        FROM tTxn_BillPay bp 
		INNER JOIN tMasterCatalog mc
			 ON bp.ProductId = mc.MasterCatalogID	
		INNER JOIN
		     tShoppingCartTransactions sct 
			 ON sct.TransactionId = bp.TransactionId
		WHERE 
			sct.CartId = @cartId 
			AND 
			sct.ProductId = 2 --BillPay
			AND
			sct.CartItemStatus = 0



		-- Get Money Order Transaction        
	    SELECT 
			mo.TransactionId as TransactionId,
			mo.Amount,
			mo.Fee,
			mo.Description,
			mo.[State] AS State,		
			mo.ConfirmationNumber,
			mo.BaseFee,
			mo.DiscountApplied,			
			mo.DiscountName,
			sct.ProductId as ProductId			
		FROM
		    tTxn_MoneyOrder mo			
		INNER JOIN 
		    tShoppingCartTransactions sct
		    ON mo.TransactionId = sct.TransactionId
		WHERE 
			sct.CartId = @cartId 
			AND 
			sct.ProductId = 5 
			AND
			sct.CartItemStatus = 0

	     -- Get Fund transactions
         SELECT 		 
			f.TransactionId,			
			f.Amount,
			f.[State] as State,
			f.BaseFee,
			f.Fee,
			f.DiscountApplied,
			f.DiscountName,
			f.Description,
			f.FundType,
			va.CardNumber
			--c.FirstName + ' ' + c.LastName AS AddOnCustomerName -- This line only for Add On Card  customer, need to join tcustomer table
		 FROM
		    tTxn_Funds f			
		INNER JOIN 
		    tVisa_Account va
		    ON f.ProviderAccountId = va.VisaAccountID
		INNER JOIN
		    tShoppingCartTransactions sct
		    ON f.TransactionId = sct.TransactionId
		WHERE 
			sct.CartId = @cartId 
			AND 
			sct.ProductId = 6
			AND
			sct.CartItemStatus = 0


	    -- Get Money Transfer transaction

         SELECT 
			 mt.Amount,
			 mt.Fee,
			 mt.ConfirmationNumber,
			 mt.Description,
			 mt.[State] as State,
			 wmt.TranascationType,
			 wmt.TransactionSubType,
			 mt.TransactionId,
			 wmt.DestinationCountryCode,
			 wmt.DestinationCurrencyCode,
			 wmt.DestinationPrincipalAmount,
			 wmt.ExchangeRate,
			 wmt.TaxAmount,
			 wr.Address,
			 wr.City,
			 wmt.RecieverFirstName,
			 wmt.RecieverLastName,
			 wmt.DestinationState,
			 wmt.GrossTotalAmount,
			 wmt.OriginatingCountryCode,
			 wmt.OriginatingCurrencyCode,
			 wmt.TaxAmount,
			 wmt.OriginalTransactionId,
			 c.FirstName,
			 c.LastName,
			 c.MiddleName,
			 c.LastName2,
			 wmt.Charges,
			 sct.ProductId as ProductId			 
        FROM 
		     tTxn_MoneyTransfer mt 	
		INNER JOIN
		     tShoppingCartTransactions sct 
			 ON sct.TransactionId = mt.TransactionId
		INNER JOIN
		     tWUnion_Trx wmt
			 ON mt.CXNId = wmt.WUTrxId 
		LEFT JOIN
		     tWUnion_Receiver wr
			 ON wmt.WUReceiverId = wr.WUReceiverId
		INNER JOIN
		     tCustomerSessions cs
			 ON cs.CustomerSessionID = mt.CustomerSessionId
		INNER JOIN
		     tCustomers c
		     ON c.CustomerId = cs.CustomerId 
		WHERE 
			 sct.CartId = @cartId 
			 AND 
			 sct.ProductId = 3     
			 AND
			 sct.CartItemStatus = 0


	     -- Get Cash In transaction

         SELECT 
		    c.TransactionId,			
			c.Amount,
			c.[State] as State,
			c.CashType,
			sct.ProductId as ProductId
		 FROM
		    tTxn_Cash c			
		inner join 
		    tShoppingCartTransactions sct
		    ON c.TransactionId = sct.TransactionId
		WHERE 
			sct.CartId = @cartId 
			AND 
			sct.ProductId = 7
			AND
			sct.CartItemStatus = 0


      END

	END TRY
	BEGIN CATCH
	      EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
	END CATCH
END
GO




