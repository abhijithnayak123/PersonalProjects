--- ===============================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <12/12/2016>
-- Description:	Alter
-- Jira ID:		<AL-7582>
-- ================================================================================

IF EXISTS (SELECT 1 FROM SYS.PROCEDURES WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetAlloyContextByAgentSessionId]') AND TYPE IN (N'P'))
DROP PROCEDURE [dbo].[usp_GetAlloyContextByAgentSessionId]
GO


CREATE PROCEDURE [dbo].[usp_GetAlloyContextByAgentSessionId]
(
	@agentSessionId BIGINT
)
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
			LEFT JOIN tTerminals t WITH (NOLOCK) ON t.TerminalID = a.TerminalId --Left Join because before NULL will be before setting the terminal
			LEFT JOIN tLocations l WITH (NOLOCK) ON l.LocationID = t.LocationId --Left Join because before NULL will be before setting the terminal
			LEFT JOIN tLocationProcessorCredentials ingochecklp WITH (NOLOCK) ON ingochecklp.LocationId = l.Locationid AND ingochecklp.ProviderId = 200
			LEFT JOIN tLocationProcessorCredentials visalp WITH (NOLOCK) ON visalp.LocationId = l.Locationid AND visalp.ProviderId = 103
		WHERE
			a.AgentSessionId = @agentSessionId 
	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END

