-- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <07/21/2015>
-- Description:	<As an engineer, I want to implement ADO.Net for Terminal module>
-- Jira ID:		<AL-7583>
-- ================================================================================

BEGIN TRY
	BEGIN TRAN;

		UPDATE T
		SET T.LocationId = L.LocationID
		FROM tTerminals AS T
		INNER JOIN tLocations AS L
		ON T.LocationPK = L.LocationPK

		ALTER TABLE tTerminals 
		ALTER COLUMN LocationId BIGINT NOT NULL


		UPDATE T
		SET T.NpsTerminalId = N.NpsTerminalId
		FROM tTerminals AS T
		INNER JOIN tNpsTerminals AS N
		ON T.NpsTerminalPK = N.NpsTerminalPK

		ALTER TABLE tTerminals 
		ALTER COLUMN NpsTerminalId BIGINT NULL 

		UPDATE T
		SET T.ChannelPartnerId = C.ChannelPartnerId
		FROM tTerminals AS T
		INNER JOIN tChannelPartners AS C
		ON T.ChannelPartnerPK = c.ChannelPartnerPK

		ALTER TABLE tTerminals 
		ALTER COLUMN NpsTerminalId BIGINT NULL 


		UPDATE A
		SET  A.TerminalID = T.TerminalID 
		FROM tTerminals AS T
		INNER JOIN tAgentSessions AS  A
		ON T.TerminalPK = A.TerminalPK

		ALTER TABLE tAgentSessions 
		ALTER COLUMN TerminalId BIGINT NULL 

		IF NOT EXISTS (
				SELECT 1
				FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
				WHERE TABLE_NAME = 'tTerminals'
				AND CONSTRAINT_NAME = 'IX_tTerminals_NameChannelPartner'
		)
		BEGIN
			ALTER TABLE [dbo].[tTerminals] ADD  CONSTRAINT [IX_tTerminals_NameChannelPartner] UNIQUE NONCLUSTERED 
			(
				[Name] ASC,
				[LocationId] ASC,
				[ChannelPartnerId] ASC
			)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF,
			 SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, 
			 ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
		 END

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

