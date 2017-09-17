
--- ================================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <06/23/2016>
-- Description:	Add Error/Exception handling Framework for Terminal, Locations, Agent and Channel Partner
-- Jira ID:		<AL-7305, AL-7306, AL-7307, AL-7308>
-- ================================================================================
--select * from tMessageStore WHERE MessageKey like '1000.100.30%' or MessageKey like '1000.100.40%' 


-- Error message for AGENT/USER

DELETE FROM tMessageStore WHERE MessageKey like '1000.100.30%' or MessageKey like '1000.100.40%' 
GO

INSERT INTO [dbo].[tMessageStore]([MessageStorePK], [MessageKey], [PartnerPK], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES
(NEWID(), '1000.100.3000', 1, 0, 'Error occurred while creating the Agent Session', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3001', 1, 0, 'Error occurred while retrieving the Agent Session', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3002', 1, 0, 'Error occurred while updating the Agent Session', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3003', 1, 0, 'Error occurred while creating the user', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3004', 1, 0, 'Error occurred while updating the user', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3005', 1, 0, 'User not found',	'Please verify inforamtion entered and retry.  Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3006', 1, 0, 'Error occurred while retrieving the user',	'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3007', 1, 0, 'Error occurred while checking the user''s permission', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.4000', 1, 0, 'Error occurred while creating the Agent Session', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.4001', 1, 0, 'Error occurred while retrieving the Agent Session', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.4002', 1, 0, 'Error occurred while updating the Agent Session', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.4003', 1, 0, 'Error occurred while creating the user', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2)
GO


-- Error message for LOCATIONS
DELETE FROM tMessageStore WHERE MessageKey like '1000.100.32%' or MessageKey like '1000.100.42%' or MessageKey = '1000.100.8090'
GO
INSERT INTO [dbo].[tMessageStore]([MessageStorePK], [MessageKey], [PartnerPK], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES

(NEWID(), '1000.100.3200',	1, 0, 'Error occurred while creating the location', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3201',	1, 0, 'Location Name already exists', 'This location name already exist.  Please enter a different name and submit', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3202',	1, 0, 'Error occurred while updating the location', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3203',	1, 0, 'Error occurred while retrieving the locations', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3204',	1, 0, 'Bank Id  and Branch Id is already exists', 'This branch ID already exist.  Please enter a different branch ID and submit', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3205',	1, 0, 'Error occurred while retrieving location counter Id', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3206',	1, 0, 'Location counter Id status update failed', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3207',	1, 0, 'Duplicate location ID. Verify the Location ID entered', 'This location ID already exist.  Please enter a different location ID and submit', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3208',	1, 0, 'Error occurred while retrieving location processor credentials', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3209',	1, 0, 'Error occurred while creating location processor credentials', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.4200',	1, 0, 'Error occurred while creating the location', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.4201',	1, 0, 'Location Name already exists', 'This location name already exist.  Please enter a different location name and submit', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.4202',	1, 0, 'Error occurred while updating the location', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.4203',	1, 0, 'Error occurred while retrieving the locations', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.4204',	1, 0, 'Error occurred while updating location counter Id status', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.4205',	1, 0, 'Error occurred while retrieving location processor credentials', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.4206',	1, 0, 'Error occurred while creating location processor credentials', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.8090',	1, 0, 'Location Name already exists', 'This location name already exist.  Please enter a different name and submit', GETDATE(), 'MGiAlloy', 2)
GO


-- Error message for CHANNELPARTNER

DELETE FROM tMessageStore WHERE MessageKey like '1000.100.31%' or MessageKey like '1000.100.41%' 
GO

INSERT INTO [dbo].[tMessageStore]([MessageStorePK], [MessageKey], [PartnerPK], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES
(NEWID(), '1000.100.3100',	1, 0, 'Error occurred while retrieving Channel Partner', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3101',	1, 0, 'Error occurred while retrieving locations for the ChannelPartner', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3102',	1, 0, 'Channel Partner Group not found', 'Please contact technical support team with this error message for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3103',	1, 0, 'Error occurred while creating the ChannelPartner Group', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3104',	1, 0, 'Error occurred while updating the ChannelPartner Group', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3105',	1, 0, 'Error occurred while retrieving the ChannelPartner Group', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3106',	1, 0, 'Error occurred while retrieving the ChannelPartner certificate information', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3107',	1, 0, 'Error occurred while retrieving tips and offers', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.4100',	1, 0, 'Error occurred while retrieving Channel Partner', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.4101',	1, 0, 'Error occurred while retrieving locations for the ChannelPartner', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.4102',	1, 0, 'Error occurred while retrieving the ChannelPartner Group', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.4103',	1, 0, 'Error occurred while retrieving tips and offers', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.4104',	1, 0, 'Error occurred while retrieving the ChannelPartner certificate information', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2)
GO


-- Error message for TERMINAL
DELETE FROM tMessageStore WHERE MessageKey like '1000.100.33%' or MessageKey like '1000.100.43%' or MessageKey in('1000.100.8091','1000.100.8092')
GO

INSERT INTO [dbo].[tMessageStore]([MessageStorePK], [MessageKey], [PartnerPK], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES
(NEWID(), '1000.100.3300',	1, 0, 'Error occurred while creating the terminal', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3301',	1, 0, 'Error occurred while updating the terminal', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3302',	1, 0, 'Error occurred while retrieving the terminal', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3303',	1, 0, 'Error occurred while creating the NPS terminal', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3304',	1, 0, 'Error occurred while updating the NPS terminal', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.3305',	1, 0, 'Error occurred while retrieving the NPS terminal', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.4300',	1, 0, 'Error occurred while creating the terminal', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.4301',	1, 0, 'Error occurred while updating the terminal', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.4302',	1, 0, 'Error occurred while retrieving the terminal', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.4303',	1, 0, 'Error occurred while creating the NPS terminal', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.4304',	1, 0, 'Error occurred while updating the NPS terminal', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.4305',	1, 0, 'Error occurred while retrieving the NPS terminal', 'This operation could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.8091',	1, 0, 'PS terminal not setup correctly', 'PS needs to be setup by System Administrator before continuing with MGi Alloy operation', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1000.100.8092',	1, 0, 'MGI Alloy terminal not setup correctly', 'Please setup terminal by clicking Hardware -> Terminal', GETDATE(), 'MGiAlloy', 2)

-- Default error message
DELETE FROM tMessageStore WHERE MessageKey like '1000.100.9999' 
GO
INSERT INTO [dbo].[tMessageStore]([MessageStorePK], [MessageKey], [PartnerPK], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES

(NEWID(), '1000.100.9999',	1, 0, 'This operation could not be completed', 'Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
