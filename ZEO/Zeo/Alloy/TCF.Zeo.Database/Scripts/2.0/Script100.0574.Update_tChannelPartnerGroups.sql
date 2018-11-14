-- ================================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <26/06/2017>
-- Description:	<As part of review command, modified DTEnd date>
-- ================================================================================

	UPDATE tChannelPartnerGroups set DTEnd = '2017-09-30 23:59:59' WHERE Name = 'THREETHENFREE'


	UPDATE tChannelPartnerFeeAdjustments set DTEnd = '2017-09-30 23:59:59' WHERE DTStart = '2017-08-16 00:00:00.000' AND DTEnd = '2017-09-30 00:00:00.000'	AND Name = 'THREETHENFREE'
	UPDATE tChannelPartnerFeeAdjustments set DTEnd = '2017-10-30 23:59:59' WHERE DTStart = '2017-10-01 00:00:00.000' AND DTEnd = '2017-10-30 00:00:00.000'	AND Name = 'THREETHENFREE'
	UPDATE tChannelPartnerFeeAdjustments set DTEnd = '2017-10-30 23:59:59' WHERE DTStart = '2017-08-16 00:00:00.000' AND DTEnd = '2017-10-30 00:00:00.000'  AND Name = 'THREETHENFREE'