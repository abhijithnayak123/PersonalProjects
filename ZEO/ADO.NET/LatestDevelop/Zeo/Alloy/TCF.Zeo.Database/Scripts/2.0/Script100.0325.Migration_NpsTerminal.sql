BEGIN TRY
	BEGIN TRAN;

	UPDATE N
	SET N.ChannelPartnerId = C.ChannelPartnerId
	FROM tNpsTerminals AS N
	INNER JOIN tChannelPartners AS C
	ON N.ChannelPartnerPK = c.ChannelPartnerPK

	ALTER TABLE tNpsTerminals 
	ALTER COLUMN ChannelPartnerId SMALLINT  NULL 


	UPDATE N
	SET N.LocationId = L.LocationID
	FROM tNpsTerminals AS N
	INNER JOIN tLocations AS L
	ON N.LocationPK = l.LocationPK

	ALTER TABLE tNpsTerminals 
	ALTER COLUMN LocationId BIGINT NULL 

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

