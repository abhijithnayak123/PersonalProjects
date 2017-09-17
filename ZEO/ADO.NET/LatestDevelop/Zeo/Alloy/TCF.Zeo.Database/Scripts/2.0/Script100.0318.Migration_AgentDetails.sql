--- ===============================================================================
-- Author:		<Rizwana Shaik>
-- Create date: <12-13-2016>
-- Description:	 Migration scripts for Agent related tables.
-- Jira ID:		<AL-7581>
-- ================================================================================


--------------------------- tAgentSessions -----------------------------------

BEGIN TRY

	 BEGIN TRAN;
	 ------------------------------------------tAgentSessions--------------------------
	 UPDATE tas
	 SET
		  tas.TerminalId = tt.TerminalID
	 FROM tAgentSessions tas
	 INNER JOIN tTerminals tt ON tt.TerminalPK=tas.TerminalPK


	UPDATE cs
	SET cs.AgentSessionId = tas.agentsessionid
	FROM tCustomerSessions cs 
	INNER JOIN tAgentSessions tas ON tas.agentsessionpk = cs.agentsessionpk

	--ALTER TABLE tCustomerSessions
	--ADD AgentSessionId BIGINT NOT NULL

	 COMMIT TRAN
END TRY

BEGIN CATCH
	 IF(@@TRANCOUNT > 0)
		SELECT
		ERROR_NUMBER() AS ErrorNumber
		,ERROR_SEVERITY() AS ErrorSeverity
		,ERROR_STATE() AS ErrorState
		,ERROR_PROCEDURE() AS ErrorProcedure
		,ERROR_LINE() AS ErrorLine
		,ERROR_MESSAGE() AS ErrorMessage,
		XACT_STATE()as state;
		ROLLBACK TRAN
END CATCH;

