--- ===============================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <12/12/2016>
-- Description:	Update the foreign key in tLocationProcessorCredentials and tLocationCounterIdDetails table
-- Jira ID:		<AL-7582>
-- ================================================================================

---- Update the LocationID in tLocationProcessorCredentials

BEGIN TRY
	BEGIN TRAN;

	UPDATE LP SET LP.LocationId = L.LocationID
	FROM tLocationProcessorCredentials AS LP
	INNER JOIN tLocations AS L
	ON LP.LocationPK = L.LocationPK


	---- Update the LocationID in tLocationCounterIdDetails

	UPDATE LC SET LC.LocationId = L.LocationID
	FROM tLocationCounterIdDetails AS LC INNER JOIN tLocations AS L
	ON LC.LocationPK = L.LocationPK


	--======================================================================================

	-- ALTER LocationId(bigint) as not nullable for FK reference

	IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tLocationProcessorCredentials' AND COLUMN_NAME = 'LocationId')
	BEGIN
		ALTER TABLE tLocationProcessorCredentials 
		ALTER COLUMN LocationId BIGINT NOT NULL
	END

	-- ALTER LocationId(bigint) as not nullable for FK reference

	IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tLocationCounterIdDetails' AND COLUMN_NAME = 'LocationId')
	BEGIN
		ALTER TABLE tLocationCounterIdDetails 
		ALTER COLUMN LocationId BIGINT NOT NULL
	END

	--==========================================================================================

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