--- ================================================================================
-- Author:		<Namit>
-- Create date: <07/22/2015>
-- Description:	<As Carver, I want to accept non-US passports and EA/green cards>
-- Jira ID:		<AL-618>
-- ================================================================================
 
UPDATE  tChannelPartnerIDTypeMapping
SET
IsActive = 1  
WHERE 
NexxoIdTypeId ='E14EE00D-BDD8-4AA2-A06D-8946A633F28D' and ChannelPartnerId = '578AC8FB-F69C-4DBD-A502-57B1EECD41D6'


UPDATE  tChannelPartnerIDTypeMapping
SET
IsActive = 1
WHERE
NexxoIdTypeId ='6B86C00A-F5E7-40AE-8C39-FE497D630BBB' and ChannelPartnerId = '578AC8FB-F69C-4DBD-A502-57B1EECD41D6'