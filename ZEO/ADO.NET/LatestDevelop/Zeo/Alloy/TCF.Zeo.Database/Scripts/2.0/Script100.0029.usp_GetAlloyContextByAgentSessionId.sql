-- ================================================================================
-- Author:		<Kaushik Sakal>
-- Create date: <07/27/2015>
-- Description:	<As an engineer, I want to implement ADO.Net for Customer module>
-- Jira ID:		<AL-7630>
-- EXEC usp_GetAlloyContextByAgentSessionId 1000000001
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetAlloyContextByAgentSessionId'
)
BEGIN
	DROP PROCEDURE usp_GetAlloyContextByAgentSessionId
END
GO

CREATE PROCEDURE usp_GetAlloyContextByAgentSessionId
	@agentSessionId BIGINT
AS
BEGIN
	BEGIN TRY
		SELECT 
			cp.ChannelPartnerId AS ChannelPartnerID
			,cp.Name AS ChannelPartnerName
			,l.LocationID AS LocationID
			,l.LocationName AS LocationName
			,l.BranchID AS BranchID
			,l.BankID AS BankID
			,l.TimezoneID AS TimezoneID
			,a.AgentSessionId AS AgentSessionID
			,t.Name AS TerminalName
			,ad.AgentID AS AgentID
			,ad.FirstName AS AgentFirstName
			,ad.LastName AS AgentLastName
			,ad.FullName AS AgentName
			,ingochecklp.UserName AS CheckUserName
			,ingochecklp.Password AS CheckPassword 
			,visalp.Identifier AS VisaLocationNodeId
			--,mtlp.Identifier AS WUCounterId
			--,lp.UserName AS LocationUserName
			--,lp.Password AS LocationPassword
			--,lp.ProviderId AS ProviderID
		FROM 
			tAgentSessions a WITH (NOLOCK) 
			INNER JOIN tAgentDetails ad WITH (NOLOCK) ON ad.AgentID = a.AgentId
			INNER JOIN tChannelPartners cp WITH (NOLOCK) ON cp.ChannelPartnerId = ad.ChannelPartnerId 
			LEFT JOIN tTerminals t WITH (NOLOCK) ON t.TerminalPK = a.TerminalPK  --Left Join because before NULL will be before setting the terminal
			LEFT JOIN tLocations l WITH (NOLOCK) ON l.LocationPK = t.LocationPK --Left Join because before NULL will be before setting the terminal
			LEFT JOIN tLocationProcessorCredentials ingochecklp WITH (NOLOCK) ON ingochecklp.LocationId = l.LocationPK AND ingochecklp.ProviderId = 200
			LEFT JOIN tLocationProcessorCredentials visalp WITH (NOLOCK) ON visalp.LocationId = l.LocationPK AND visalp.ProviderId = 103
		WHERE
			a.AgentSessionId = @agentSessionId 
	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END