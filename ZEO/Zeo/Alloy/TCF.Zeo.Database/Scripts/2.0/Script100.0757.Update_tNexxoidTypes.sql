	-- ================================================================================
-- Author:		Abhijith
-- Create date: <05/15/2018>
-- Description:	<Perm Resident - UI Updates>
-- ================================================================================

IF EXISTS (SELECT 1 FROM tNexxoidTypes WHERE Name = 'LEGAL PERMANENT RESIDENT CARD')
BEGIN

	UPDATE tNexxoidTypes
	SET Name = 'PERMANENT RESIDENT CARD'
	WHERE Name = 'LEGAL PERMANENT RESIDENT CARD'

END
GO
