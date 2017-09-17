-- ================================================================================
-- Author:		<Kaushik Sakal>
-- Create date: <07/27/2015>
-- Description:	<As an engineer, I want to implement ADO.Net for Customer module>
-- Jira ID:		<AL-7630>
-- EXEC usp_GetAlloyContextByCustomerSessionId 1000000013
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetAlloyContextByCustomerSessionId'
)
BEGIN
	DROP PROCEDURE usp_GetAlloyContextByCustomerSessionId
END
GO

CREATE PROCEDURE usp_GetAlloyContextByCustomerSessionId
	@customerSessionId BIGINT
AS
BEGIN
	BEGIN TRY
		SELECT 
			cs.CustomerSessionID AS CustomerSessionID
			,cp.ChannelPartnerId AS ChannelPartnerID
			,cp.Name AS ChannelPartnerName
			,l.LocationID AS LocationID
			,l.LocationName AS LocationName
			,l.BranchID AS BranchID
			,l.BankID AS BankID
			,l.TimezoneID AS TimezoneID
			,a.AgentSessionId AS AgentSessionID
			,t.Name AS TerminalName
			--lp.UserName AS LocationUserName,
			--lp.Password AS LocationPassword,
			--lp.ProviderId AS ProviderID, 
			,ad.AgentID AS AgentID
			,ad.FirstName AS AgentFirstName
			,ad.LastName AS AgentLastName
			,ad.FullName AS AgentName
			,c.CustomerID AS CustomerID
			,ingochecklp.UserName AS CheckUserName
			,ingochecklp.Password AS CheckPassword 
			,visalp.Identifier AS VisaLocationNodeId
		FROM 
			tCustomerSessions cs WITH (NOLOCK) 
			--INNER JOIN  tPartnerCustomers pc WITH (NOLOCK) ON pc.CustomerPK = cs.CustomerPK
			INNER JOIN  tCustomers c WITH (NOLOCK) ON cs.CustomerID = c.CustomerID
			INNER JOIN  tAgentSessions a WITH (NOLOCK) ON A.AgentSessionPK = CS.AgentSessionPK
			INNER JOIN tTerminals t WITH (NOLOCK) ON t.TerminalPK = a.TerminalPK
			INNER JOIN tLocations l WITH (NOLOCK) ON l.LocationPK = t.LocationPK
			INNER JOIN tAgentDetails ad WITH (NOLOCK) ON ad.AgentID = a.AgentId
			--INNER JOIN tLocationProcessorCredentials lp WITH (NOLOCK) ON lp.LocationId = l.LocationPK
			INNER JOIN tChannelPartners cp WITH (NOLOCK) ON cp.ChannelPartnerId = ad.ChannelPartnerId
			LEFT JOIN tLocationProcessorCredentials ingochecklp WITH (NOLOCK) ON ingochecklp.LocationId = l.LocationPK AND ingochecklp.ProviderId = 200
			LEFT JOIN tLocationProcessorCredentials visalp WITH (NOLOCK) ON visalp.LocationId = l.LocationPK AND visalp.ProviderId = 103
		WHERE 
			cs.CustomerSessionID = @customerSessionId
			
	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END