-- ================================================================================
-- Author:		<Kaushik Sakal>
-- Create date: <07/27/2015>
-- Description:	<As an engineer, I want to implement ADO.Net for Customer module>
-- Jira ID:		<AL-7630>
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_TCF_GetTCISAccountById'
)
BEGIN
	DROP PROCEDURE usp_TCF_GetTCISAccountById
END
GO

CREATE PROCEDURE usp_TCF_GetTCISAccountById
	@Id BIGINT
AS
BEGIN
	BEGIN TRY
		SELECT 
			ta.CustomerID,ta.BankId,ta.BranchId,ta.CustomerSessionID,
			ta.PartnerAccountNumber,ta.RelationshipAccountNumber,ta.TCISAccountID,
			ta.TcfCustInd,ta.ProfileStatus
		FROM 
			tTCIS_Account ta 
		WHERE 
			ta.TCISAccountID = @Id
		 
	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END