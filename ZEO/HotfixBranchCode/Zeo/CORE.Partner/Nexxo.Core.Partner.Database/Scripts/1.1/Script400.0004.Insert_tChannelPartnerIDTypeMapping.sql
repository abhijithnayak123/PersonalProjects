--===========================================================================================
-- Auther:			<Ashok Kumar>
-- Date Created:	<22-Oct-2014>
-- Description:		<Script for insert data into tChannelPartnerIDTypeMapping>
-- Rally ID:		<TA5684>
--===========================================================================================

TRUNCATE TABLE tChannelPartnerIDTypeMapping
GO

INSERT INTO tChannelPartnerIDTypeMapping
(
	ChannelPartnerId, 
	NexxoIdTypeId,
	IsActive 
)
SELECT 
	tc.rowguid, 
	tn.rowguid, 
	tn.IsActive 
FROM tChannelPartners tc
	CROSS JOIN tNexxoIdTypes tn

GO
