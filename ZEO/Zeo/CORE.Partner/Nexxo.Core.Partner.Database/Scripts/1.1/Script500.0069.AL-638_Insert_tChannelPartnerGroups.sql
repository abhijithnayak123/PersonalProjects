--===========================================================================================
-- Auther:			<Chinar Kulkarni>
-- Date Created:	<22-July-2015>
-- Description:		<Script for Inserting new Groups for TCF>
-- Jira ID:			<AL-638>
--===========================================================================================

IF NOT EXISTS(SELECT 1 FROM tChannelPartnerGroups WHERE Name = 'CommercialPayroll')
BEGIN
	INSERT INTO tChannelPartnerGroups(Name, ChannelPartnerPK, DTServerCreate, ChannelPartnerGroupPK)
	VALUES('CommercialPayroll', N'6d7e785f-7bdd-42c8-bc49-44536a1885fc', GETDATE(), NEWID())
END

IF NOT EXISTS(SELECT 1 FROM tChannelPartnerGroups WHERE Name = 'InstorePayroll')
BEGIN
	INSERT INTO tChannelPartnerGroups(Name, ChannelPartnerPK, DTServerCreate, ChannelPartnerGroupPK)
	VALUES('InstorePayroll', N'6d7e785f-7bdd-42c8-bc49-44536a1885fc', GETDATE(), NEWID())
END