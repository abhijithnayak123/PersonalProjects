--===========================================================================================
-- Author:			<Chinar Kulkarni>
-- Date Created:	05/07/2015
-- User Story:      AL-208
-- Description:		<Script for creating new compliance program for new channel partner - Redstone >
--===========================================================================================

IF NOT EXISTS(SELECT 1 FROM tCompliancePrograms WHERE Name = 'RedstoneCompliance')
BEGIN
	INSERT INTO tCompliancePrograms (ComplianceProgramPK, Name, RunOFAC, DTCreate)
	VALUES('d734d88c-3436-4bd1-bbb8-42af3b4768b2', 'RedstoneCompliance', 0, GETDATE())
END