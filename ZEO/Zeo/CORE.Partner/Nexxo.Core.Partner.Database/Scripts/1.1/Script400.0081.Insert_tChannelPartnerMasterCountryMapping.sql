--===========================================================================================
-- Author:		<Rita Patel>
-- Create date: <May 13 2015>
-- Description:	<Script to insert ChannelPartnerMasterCountries list in the table>
-- Jira ID:	    <AL-499>
--===========================================================================================

------------------------------ Insert script for  SYNOVUS ------------------------------------
IF NOT EXISTS
(
	SELECT 1 FROM  tChannelPartnerMasterCountryMapping 
	WHERE MasterCountryId = 'FD8FEFA9-E3A5-4307-9AE5-CDDB2D519121' 
	AND ChannelPartnerId = 'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17'
)
 BEGIN
	INSERT INTO tChannelPartnerMasterCountryMapping(rowguid, ChannelPartnerId, MasterCountryId) VALUES	(NEWID(), 'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17', 'FD8FEFA9-E3A5-4307-9AE5-CDDB2D519121'),	(NEWID(), 'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17', '93F0F530-4433-4FA0-8831-A3A2ED821551') END