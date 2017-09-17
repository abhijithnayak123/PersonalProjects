--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <09-19-2016>
-- Description:	 Migration script for limits
-- Jira ID:		<AL-8195>
-- ================================================================================

BEGIN TRY
  BEGIN TRAN

	UPDATE L SET L.ComplianceProgramID = C.ComplianceProgramID FROM tLimitTypes AS L
	   INNER JOIN tCompliancePrograms AS C ON L.ComplianceProgramPK = C.ComplianceProgramPK
	   
	UPDATE L SET L.LimitTypeID = LT.LimitTypeID FROM tLimits AS L
	   INNER JOIN tLimitTypes AS LT ON L.LimitTypePK = LT.LimitTypePK

	UPDATE LF SET LF.ComplianceProgramID = CP.ComplianceProgramID FROM tLimitFailures AS LF
	   INNER JOIN tCompliancePrograms AS CP ON LF.ComplianceProgramPK = CP.ComplianceProgramPK
	
	--As per the Enum value change in Alloy code
	UPDATE tLimitTypes SET Name = 'ProcessCheck' WHERE Name = 'Check'

	IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tLimitTypes' AND COLUMN_NAME = 'ComplianceProgramID')
	BEGIN
		ALTER TABLE tLimitTypes 
		ALTER COLUMN ComplianceProgramId BIGINT NOT NULL 
	END

	-- ALTER column (LimitTypeID) as NOT NULL in tLimits table

	IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tLimits' AND COLUMN_NAME = 'LimitTypeID')
	BEGIN
		ALTER TABLE tLimits 
		ALTER COLUMN LimitTypeId BIGINT NOT NULL 
	END

	-- ALTER COLUMN (ComplianceProgramID) AS NOT NULL in tLimits table

	IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tLimitFailures' AND COLUMN_NAME = 'ComplianceProgramID')
	BEGIN
		ALTER TABLE tLimitFailures 
		ALTER COLUMN ComplianceProgramId BIGINT NOT NULL 
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



