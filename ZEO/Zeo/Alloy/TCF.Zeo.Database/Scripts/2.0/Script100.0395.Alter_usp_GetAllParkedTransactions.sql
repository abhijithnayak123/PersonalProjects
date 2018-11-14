-- ================================================================================
-- Author:		Kaushik Sakala
-- Create date: 07/27/2015
-- Description:	To Get all parked transactions for Shopping Cart.
-- Jira ID:		AL-9009
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetAllParkedTransactions'
)
BEGIN
	DROP PROCEDURE usp_GetAllParkedTransactions
END
GO

CREATE PROCEDURE usp_GetAllParkedTransactions
AS
BEGIN
	BEGIN TRY
		SELECT 
			 sct.TransactionId
			,sct.ProductId
			,sc.CustomerSessionId
			,c.CustomerID
			,cp.ChannelPartnerId
			,l.LocationID AS LocationID
			,l.LocationName AS LocationName
			,l.BranchID AS BranchID
			,l.BankID AS BankID
			,cp.Name AS ChannelPartnerName
			,ingolp.UserName  AS CheckUserName
			,ingolp.Password AS CheckPassword
			,l.TimeZoneId
			,ad.FirstName AS AgentFirstName
			,ad.LastName AS AgentLastName
			,ad.FullName AS AgentName
			,ad.AgentID AS AgentID
			,csc.CounterId AS WUCounterId
		FROM tShoppingCarts sc
			INNER JOIN tShoppingCartTransactions sct WITH (NOLOCK) ON sct.CartId = sc.CartID
			INNER JOIN tCustomersessions cs WITH (NOLOCK) ON cs.CustomerSessionID = sc.CustomerSessionId
			INNER JOIN tCustomers c WITH (NOLOCK) ON c.CustomerID = cs.CustomerID
			INNER JOIN tChannelPartners cp WITH (NOLOCK) ON cp.ChannelPartnerId = c.ChannelPartnerId
			INNER JOIN tAgentSessions a WITH (NOLOCK) ON a.AgentSessionID = cs.AgentSessionId
			INNER JOIN tAgentDetails ad WITH (NOLOCK) ON ad.AgentID = a.AgentID
			INNER JOIN tTerminals t WITH (NOLOCK) ON t.TerminalID = a.TerminalId
			INNER JOIN tLocations l WITH (NOLOCK) ON l.LocationID = t.LocationId
			LEFT JOIN tCustomerSessionCounterIdDetails csc WITH (NOLOCK) ON csc.CustomerSessionID = cs.CustomerSessionID
			LEFT JOIN tLocationProcessorCredentials ingolp WITH (NOLOCK) ON ingolp.LocationId = l.LocationID AND ingolp.ProviderId = 200
		WHERE SC.State = 2 AND sct.CartItemStatus = 0 AND sct.ProductId in (1,2,3)	
	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END


