	-- ================================================================================
-- Author:		Nitish Biradar
-- Create date: <04/27/2018>
-- Description:	<Perm Resident - UI Updates>
-- Jira ID:		<B-15882>
-- ================================================================================
IF EXISTS (SELECT 1 FROM tNexxoidTypes WHERE Name = 'GREEN CARD / PERMANENT RESIDENT CARD')
BEGIN
	DECLARE @nexxoIdTypeID BIGINT = 
	(
		SELECT NexxoIdTypeID 
		FROM tNexxoidTypes
		WHERE Name = 'GREEN CARD / PERMANENT RESIDENT CARD'
	) 

	UPDATE tNexxoidTypes
	SET Name = 'LEGAL PERMANENT RESIDENT CARD'
	WHERE NexxoIdTypeID = @nexxoIdTypeID

END
GO
