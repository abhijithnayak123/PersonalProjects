	-- ================================================================================
-- Author:		Abhijith
-- Create date: <04/17/2018>
-- Description:	<Support Permanent Resident Card as ID>
-- Jira ID:		<B-14081>
-- ================================================================================
IF EXISTS (SELECT 1 FROM tNexxoidTypes WHERE Name = 'GREEN CARD / PERMANENT RESIDENT CARD')
BEGIN
	DECLARE @nexxoIdTypeID BIGINT = 
	(
		SELECT NexxoIdTypeID 
		FROM tNexxoidTypes
		WHERE Name = 'GREEN CARD / PERMANENT RESIDENT CARD'
	) 

	UPDATE tChannelPartnerIDTypeMapping
	SET ISActive = 1
	WHERE NexxoIdTypeID = @nexxoIdTypeID AND ChannelPartnerId = 34

	UPDATE tNexxoidTypes
	SET Mask = '^[a-zA-Z]\d{11}$'
	WHERE NexxoIdTypeID = @nexxoIdTypeID

END
GO
