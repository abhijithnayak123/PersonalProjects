	-- ================================================================================
-- Author:		Abhijith
-- Create date: <05/14/2018>
-- Description:	<Perm Resident - UI Updates>
-- ================================================================================

IF EXISTS (SELECT 1 FROM tNexxoidTypes WHERE Name = 'LEGAL PERMANENT RESIDENT CARD')
BEGIN

	UPDATE tNexxoidTypes
	SET Mask = '^[ A-Za-z0-9_@./#&+-]{0,30}$'
	WHERE Name = 'LEGAL PERMANENT RESIDENT CARD'

END
GO
