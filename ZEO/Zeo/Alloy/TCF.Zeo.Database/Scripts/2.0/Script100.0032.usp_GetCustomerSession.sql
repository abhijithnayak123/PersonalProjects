-- ================================================================================
-- Author:		Kaushik Sakala
-- Create date: 07/27/2015
-- Description:	As an engineer, I want to implement ADO.Net for Customer module
-- Jira ID:		AL-7630
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetCustomerSession'
)
BEGIN
	DROP PROCEDURE usp_GetCustomerSession
END
GO

CREATE PROCEDURE usp_GetCustomerSession
	@customerSessionId UNIQUEIDENTIFIER
AS
BEGIN
	BEGIN TRY
		SELECT C.CustomerSessionPK,
			   C.CustomerSessionID,
			   C.CustomerPK,
			   AgentSessionPK,
			   CustomerPK,
			   DTStart,
			   CardPresent,
			   TimezoneID,
			   CSC.CounterId,
			   CSS.CartPK			   
		 FROM
			tCustomerSessions C  
			INNER JOIN tCustomerSessionCounterIdDetails CSC ON CSC.CustomerSessionPK = C.CustomerSessionPK
			INNER JOIN tCustomerSessionShoppingCarts CSS ON CSS.CustomerSessionPK = C.CustomerSessionPK
		 WHERE
			C.CustomerSessionPK = @customerSessionId
			
END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END