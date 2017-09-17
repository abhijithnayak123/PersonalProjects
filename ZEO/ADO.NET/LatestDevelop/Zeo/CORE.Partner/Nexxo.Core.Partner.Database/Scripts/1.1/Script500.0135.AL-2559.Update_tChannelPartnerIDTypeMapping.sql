	-- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <11/23/2015>
-- Description:	<Added a record in tChannelPartnerIDTypeMapping table for CANADIAN DRIVER''S LICENSE
--				 PROVINCIAL/TERRITORIAL IDENTITY CARD for ID issusing State is NULL >
-- Jira ID:		<AL-2559>
-- ================================================================================

--CARVER
IF NOT EXISTS(SELECT 1 FROM tChannelPartnerIDTypeMapping
	WHERE  ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6'
	AND  NexxoIdTypeId='BA76EBF7-D10B-47D1-B25C-21B692F1C61A')
	BEGIN
		INSERT tChannelPartnerIDTypeMapping(rowguid,ChannelPartnerId,NexxoIdTypeId,IsActive)
		VALUES(NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','BA76EBF7-D10B-47D1-B25C-21B692F1C61A',1)
	END

IF NOT EXISTS(SELECT 1 FROM tChannelPartnerIDTypeMapping 
	WHERE  ChannelPartnerId='578AC8FB-F69C-4DBD-A502-57B1EECD41D6'
	AND NexxoIdTypeId='81F1BC43-6DE0-402F-B476-714CE340C318')
	BEGIN
		INSERT tChannelPartnerIDTypeMapping(rowguid,ChannelPartnerId,NexxoIdTypeId,IsActive)
		VALUES(NEWID(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6','81F1BC43-6DE0-402F-B476-714CE340C318',1)
	END




