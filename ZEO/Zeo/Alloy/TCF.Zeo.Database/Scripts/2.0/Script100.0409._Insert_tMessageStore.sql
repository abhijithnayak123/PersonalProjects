--- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <03/14/2017>
-- Description:	 Insert into tmessage store
-- ================================================================================

-- Error message for AGENT
DELETE FROM tMessageStore WHERE MessageKey like '1000.100.30%' or MessageKey like '1000.100.40%' 
GO

INSERT INTO [dbo].[tMessageStore]([MessageKey], [ChannelPartnerId], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES
('1000.100.3000',1,'0','Error occurred while creating the Agent Session','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.3001',1,'0','Error occurred while retrieving the Agent Session','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.4000',1,'0','Error occurred while creating the Agent Session','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.4001',1,'0','Error occurred while retrieving the Agent Session','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.4002',1,'0','Error occurred while updating the Agent Session','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.4003',1,'0','Error occurred while creating the user','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2)

-- Error message for LOCATIONS
DELETE FROM tMessageStore WHERE MessageKey like '1000.100.32%' or MessageKey like '1000.100.42%' or MessageKey = '1000.100.8090'
GO

INSERT INTO [dbo].[tMessageStore]([MessageKey], [ChannelPartnerId], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES
('1000.100.3200',1,'0','Error occurred while creating the location','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.3202',1,'0','Error occurred while updating the location','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.3203',1,'0','Error occurred while retrieving the locations','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.3205',1,'0','Error occurred while retrieving location counter Id','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.3206',1,'0',' Error occurred while retrieving location processor credentials','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.3207',1,'0','Duplicate location ID. Verify the Location ID entered','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.3208',1,'0','Error occurred while retrieving location processor credentials.','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.3209',1,'0','Error occurred while creating location processor credentials.','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.3210',1,'0','Error occurred while creating counter id for customer session.','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.4200',1,'0','Error occurred while creating the location','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.4201',1,'0','Location Name already exists.','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.4202',1,'0','Error occurred while updating the location','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.4203',1,'0','Error occurred while retrieving the locations','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.4204',1,'0','Error occurred while updating location counter Id status','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.4205',1,'0','Error occurred while retrieving location processor credentials.','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.4206',1,'0','Error occurred while creating location processor credentials.','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.4207',1,'0','Error occurred while creating customerSessiom counterId','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.4208',1,'0','Location with LocationName already exist','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.4209',1,'0','Location with Bank/Branch Id already exist','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2)


-- Error message for CHANNELPARTNER

DELETE FROM tMessageStore WHERE MessageKey like '1000.100.31%' or MessageKey like '1000.100.41%' 
GO

INSERT INTO [dbo].[tMessageStore]([MessageKey], [ChannelPartnerId], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES
('1000.100.3105',1,'0','Error occurred while retrieving the ChannelPartner Group','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.3106',1,'0','Error occurred while retrieving the ChannelPartner certificate information','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.3107',1,'0','Error occurred while retrieving tips and offers','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.3108',1,'0','Error while retrieving channel partner configuration','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.3109',1,'0','Error while retrieving channel partner providers','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.4100',1,'0','Error occurred while retrieving Channel Partner.','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.4101',1,'0','Error occurred while retrieving locations for the ChannelPartner','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.4102',1,'0','Error occurred while retrieving the ChannelPartner Group','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.4103',1,'0','Error occurred while retrieving tips and offers','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.4104',1,'0','Error occurred while retrieving the ChannelPartner certificate information','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2)

-- Error message for TERMINAL
DELETE FROM tMessageStore WHERE MessageKey like '1000.100.33%' or MessageKey like '1000.100.43%' or MessageKey in('1000.100.8091','1000.100.8092')
GO

INSERT INTO [dbo].[tMessageStore]([MessageKey], [ChannelPartnerId], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES

('1000.100.3300',1,'0','Error occurred while creating the terminal','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.3301',1,'0','Error occurred while updating the terminal','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.3302',1,'0','Error occurred while retrieving the terminal','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.3303',1,'0','Error occurred while creating the NPS terminal','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.3304',1,'0','Error occurred while updating the NPS terminal','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.3305',1,'0','Error occurred while retrieving the NPS terminal','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.4300',1,'0','Error occurred while creating the terminal','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.4301',1,'0','Error occurred while updating the terminal','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.4302',1,'0','Error occurred while retrieving the terminal','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.4303',1,'0','Error occurred while creating the NPS terminal','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.4304',1,'0','Error occurred while updating the NPS terminal','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.4305',1,'0','Error occurred while retrieving the NPS terminal','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2),
('1000.100.4306',1,'0','Error occurred while NPS terminal diagnostic','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2)


-- Generic Error Message.
DELETE FROM tMessageStore WHERE MessageKey like '1000.100.9999' 
GO
INSERT INTO [dbo].[tMessageStore]([MessageKey], [ChannelPartnerId], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES
('1000.100.9999',1,'0', 'This operation could not be completed', 'Please contact your technical support team for more information', GETDATE(), 'Zeo', 2)

--Error Messages for MoneyOrder

DELETE FROM tMessageStore WHERE MessageKey like '1006.100.%'
GO

INSERT INTO tMessageStore (MessageKey, [ChannelPartnerId], Language, Content, AddlDetails, DTServerCreate, Processor, Type)
VALUES
('1006.100.1000',1,'0',' MoneyOrder Create Failed','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1006.100.1001',1,'0',' MoneyOrder Update Failed','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1006.100.1003',1,'0',' MoneyOrder Not Found','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1006.100.1004',1,'0','MoneyOrder status update Failed','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1006.100.1005',1,'0','MoneyOrder Fee Update Failed','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1006.100.6000',1,'0','Maximum amount allowed for this transaction is {0}','Please reduce amount and try again.', GETDATE(),'Zeo',2),
('1006.100.6001',1,'0',' Minimum amount required for this transaction is {0}','Please increase the amount and try again.', GETDATE(),'Zeo',2),
('1006.100.6002',1,'0',' Check Print Template Not Found','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1006.100.6003',1,'0',' Money Order has already been issued.and as printed','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1006.100.6004',1,'0',' Error occurred while resubmitting the transaction','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1006.100.6006',1,'0',' Error occurred while Adding transaction.','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1006.100.6007',1,'0','Error occurred while updating transaction.','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1006.100.6008',1,'0',' Error occurred while committing transaction','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1006.100.6009',1,'0',' Error occurred while fetching transaction.','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1006.100.6010',1,'0',' Receipt Templates Not Found.','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1006.100.6011',1,'0',' Error occurred while doing money order diagnostic.','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1006.100.6013',1,'0','MoneyOrder status update Failed','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1006.100.6014',1,'0','MoneyOrder Get fee failed','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1006.100.6015',1,'0','MoneyOrder promo code invalid','Please verify that the PROMO CODE is in all caps and correct and try again.', GETDATE(),'Zeo',2),
('1006.100.8000',1,'0',' Money Order Scanning Error','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1006.100.8001',1,'0',' The Money Order Check Number was not scanned correctly','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1006.100.8002',1,'0',' Service connectivity could not be established.','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1006.100.8003',1,'0','Printing Money Order check could not be completed and printer returned error message more information','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1006.100.8004',1,'0','Printer could not be detected','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2)


-- Error messages for BillPay
DELETE FROM tMessageStore WHERE MessageKey like '1004.%'
GO

INSERT INTO [dbo].[tMessageStore]([MessageKey], [ChannelPartnerId], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES

('1004.100.1000',1,'0','Error occurred while staging the billpay transaction','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.1001',1,'0','Error occurred while updating the billpay transaction','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.1004',1,'0','Error occurred while creating or updating the favourite biller','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.1005',1,'0','Error occurred while retrieving the billers','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.1006',1,'0','Error occurred while retrieving favorite biller','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.1007',1,'0','Error occurred while deleting favorite biller','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.2001',1,'0','Invalid account number','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.2002',1,'0','Certificate not found for WU partner integration','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.2005',1,'0','Error occurred while getting banner message','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.2006',1,'0','Error occurred while getting past biller','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.2052',1,'0','Error while enrolling goldcard details','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.2053',1,'0','Error in card lookup','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.2079',1,'0','Error occurred while getting WU credentials','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.2101',1,'0','BillPay cannot proceed as validation has failed','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.2102',1,'0','BillPay cannot proceed as transaction cannot be committed','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.2103',1,'0','Location cannot be retrieved','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.2104',1,'0','Delivery methods cannot be retrieved','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.2105',1,'0','Biller message cannot be retrieved','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.2106',1,'0','Biller fields cannot be retrieved','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.2107',1,'0','Biller cannot be retrieved','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.2109',1,'0','Account creation failed','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.2114',1,'0','Provider import has failed','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.2115',1,'0','Counter id cannot be found.','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.2116',1,'0','Counter id cannot be found','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.2117',1,'0','BillPay cannot proceed as Gold card points update failed','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.2118',1,'0','BillPay cannot proceed as account retrieval has failed','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.2119',1,'0','Transaction retrieval has failed','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.2120',1,'0','Retrieval of fee has failed','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.2121',1,'0','Biller message cannot be retrieved','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.2122',1,'0','Biller fields cannot be retrieved','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.2123',1,'0','Account update failed','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.2124',1,'0','Card Info retrieval has failed','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.6000',1,'0','Maximum amount allowed for this transaction is {0} ','Please reduce amount and try again.', GETDATE(),'Zeo',2),
('1004.100.6001',1,'0','Minimum amount required for this transaction is {0}','Please increase the amount and try again.', GETDATE(),'Zeo',2),
('1004.100.6003',1,'0','Error occurred while updating Transaction','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.6004',1,'0','Error occurred while deleting favorite biller','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.6005',1,'0','Error occurred while canceling transaction','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.6009',1,'0','Error occurred while adding past biller','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.6010',1,'0','Error occurred while fetching card details','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.6012',1,'0','Error occurred while fetching transaction','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.6013',1,'0','Error occurred while fetching favorite biller','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.6015',1,'0','Error occurred while fetching biller message','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.6016',1,'0','Error occurred while fetching fee','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.6017',1,'0','Error occurred while fetching biller location','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.6018',1,'0','Error occurred while committing the transaction','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.6020',1,'0','Error occurred while validating the transaction','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.6023',1,'0','Error occurred while submitting the transaction','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.6024',1,'0','Error occurred while retrieving the biller details','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.6025',1,'0','Error occurred while retrieving biller provider attributes','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.100.9999',1,'0','This operation could not be completed','Please contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.401',1,'0','','Please contact the IT Service Desk at 763-337-6600.', GETDATE(),'Western Union',2),
('1004.401.5000',1,'0','This operation could not be completed ','Please try again. If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.401.5001',1,'0','This operation could not be completed ','Please try again. If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.401.5002',1,'0','This operation could not be completed ','Please try again. If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.401.5003',1,'0','This operation has timed out','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1004.401.R5643',1,'0','','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Western Union',2),
('1004.401.T0415',1,'0','','Please add SSN/ITIN to customer profile and then resubmit transaction.', GETDATE(),'Western Union',2),
('1004.401.T0425',1,'0','','Please verify the ID number in the customer profile is correct and try again.', GETDATE(),'Western Union',2),
('1004.401.T0505',1,'0','','Please add SSN/ITIN to customer profile and then resubmit transaction.', GETDATE(),'Western Union',2),
('1004.401.T0525',1,'0','','Please change Occupation description to be more specific in the customer profile and try again.', GETDATE(),'Western Union',2),
('1004.401.T0695',1,'0','','Please add SSN/ITIN to customer profile and then resubmit transaction.', GETDATE(),'Western Union',2),
('1004.401.T0749',1,'0','','Please add SSN/ITIN to customer profile and then resubmit transaction.', GETDATE(),'Western Union',2),
('1004.401.T0997',1,'0','','Counter ID is not set up in the Western Union system. Please contact the IT Service Desk at 763-337-6600.', GETDATE(),'Western Union',2),
('1004.401.T3006',1,'0','','Please verify the promotion code and try again.', GETDATE(),'Western Union',2),
('1004.401.T3251',1,'0','','Please contact the IT Service Desk at 763-337-6600.', GETDATE(),'Western Union',2),
('1004.401.T3254',1,'0','','Please contact the IT Service Desk at 763-337-6600.', GETDATE(),'Western Union',2),
('1004.401.T4832',1,'0','','Please verify the biller account number and try again.If problem persists, contact IT Service Desk at 763-337-6600', GETDATE(),'Western Union',2),
('1004.401.T4996',1,'0','','Please contact the IT Service Desk at 763-337-6600.', GETDATE(),'Western Union',2),
('1004.401.T5004',1,'0','','Please verify the biller account number and try again. If problem persists, contact IT Service Desk at 763-337-6600', GETDATE(),'Western Union',2),
('1004.401.T5005',1,'0','','Please verify the biller account number and try again. If problem persists, contact IT Service Desk at 763-337-6600', GETDATE(),'Western Union',2),
('1004.401.T5013',1,'0','','Please verify the biller account number and try again.   If problem persists, contact IT Service Desk at 763-337-6600', GETDATE(),'Western Union',2),
('1004.401.T5425',1,'0','','Please contact the IT Service Desk at 763-337-6600.', GETDATE(),'Western Union',2),
('1004.401.T5684',1,'0','','Please change Occupation description to be more specific in the customer profile and try again.', GETDATE(),'Western Union',2),
('1004.401.T6008',1,'0','','Please add SSN/ITIN to customer profile and then resubmit transaction.', GETDATE(),'Western Union',2),
('1004.401.T6009',1,'0','','Please add SSN/ITIN to customer profile and then resubmit transaction.', GETDATE(),'Western Union',2),
('1004.401.T6015',1,'0','','Please verify the sender''s country of birth and try again.', GETDATE(),'Western Union',2),
('1004.401.T6750',1,'0','','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Western Union',2),
('1004.401.T6752',1,'0','','Please verify the biller account number and try again.   If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Western Union',2),
('1004.401.T6785',1,'0','','Please contact the IT Service Desk at 763-337-6600.', GETDATE(),'Western Union',2),
('1004.401.T6789',1,'0','','Please contact the IT Service Desk at 763-337-6600.', GETDATE(),'Western Union',2),
('1004.401.U3003',1,'0','','Please contact the IT Service Desk at 763-337-6600.', GETDATE(),'Western Union',2),
('1004.401.U9035',1,'0','','A duplicate bill pay transaction was submitted within 60 minutes. Change the amount or try again at a later time.', GETDATE(),'Western Union',2)


-- Error messages for Funds

DELETE FROM tMessageStore WHERE MessageKey like '1003.%'
GO

INSERT INTO [dbo].[tMessageStore]([MessageKey], [ChannelPartnerId], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES
('1003.100.2001',1,'0','Account Not Found','Please contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.2003',1,'0','Transaction Not Found','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.2010',1,'0','Card Activation Failed','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.2011',1,'0','SSN empty or not matching Visa records','Please add SSN/ITIN to customer profile and then resubmit transaction.', GETDATE(),'Zeo',2),
('1003.100.2013',1,'0','Card Already Registered ','Please use a new card and try again.', GETDATE(),'Zeo',2),
('1003.100.2054',1,'0','Transaction Update Failed','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600', GETDATE(),'Zeo',2),
('1003.100.2063',1,'0','Card expiration date not matching our records','Please correct the card expiration date and try again.', GETDATE(),'Zeo',2),
('1003.100.2090',1,'0','Card number not valid','Please verify the card number and try again.', GETDATE(),'Zeo',2),
('1003.100.2106',1,'0','Pseudo DDA not valid','Please verify the Pseudo DDA on the packet and try again.', GETDATE(),'Zeo',2),
('1003.100.2109',1,'0','Card Already Issued','Please use a Valid/New card.', GETDATE(),'Zeo',2),
('1003.100.2113',1,'0','Error occurred while retrieving Card Information','Please contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.2114',1,'0','Error occurred while retrieving Card Balance','Please contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.2115',1,'0','Error occurred while loading to Card.','Please contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.2116',1,'0','Error occurred while withdrawing from Card.','Please contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.2117',1,'0','Error occurred while retrieving Card Transaction History.','Please contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.2118',1,'0','Error occurred while closing Card Account.','Please contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.2119',1,'0','Error occurred while updating Card Status.','Please contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.2120',1,'0','Error occurred while ordering Companion Card.','Please contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.2121',1,'0','Error occurred while replacing Card.','Please contact the IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.2122',1,'0','Error occurred while creating  card account for customer','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.2123',1,'0','Fund transaction has invalid data','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.2124',1,'0','Card Account cannot be retrieved','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.2125',1,'0','Failed to update Card Account','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.2126',1,'0','Error occurred while closing an Account Closure','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.2127',1,'0','Shipping details  cannot be retrieved','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.2128',1,'0','Error occurred while retrieving funds fee','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.2129',1,'0','Fund cannot proceed as transaction cannot be committed','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.2130',1,'0','Error while staging the transaction','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.6005',1,'0','Error while staging the transaction','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.6012',1,'0','Error occurred while creating funds account','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.6014',1,'0','Error occurred while retrieving Card Balance','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.6015',1,'0','Error occurred while retrieving prepaid transaction','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.6016',1,'0','Error occurred while retrieving funds fee','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.6017',1,'0','Error occurred while updating account','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.6018',1,'0','Error occurred while retrieving minimum load amount','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.6019',1,'0','Error occurred while cancelling the transaction','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.6020',1,'0','Error occurred while retrieving card transaction history','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.6021',1,'0','Error occurred while closing the account','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.6022',1,'0','Error occurred while updating the card status','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.6023',1,'0','Error occurred while replacing card','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.6024',1,'0','Error occurred while retrieving the shipping types','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.6025',1,'0','Error occurred while retrieving the shipping fee','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.6026',1,'0','Error occurred while associating the card','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.6027',1,'0','Error occurred while retrieving the card maintenance fee','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.6028',1,'0','Error occurred while card withdraw','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.6029',1,'0','Error occurred while card activate','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.6030',1,'0','Error occurred while card load','Please try again. If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.6031',1,'0','Error occurred while companion card order','Please try again. If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.103.5000',1,'0','Display message from VISA As-Is','Please try again. If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'VisaDPS',2),
('1003.103.5001',1,'0','Display message from VISA As-Is','Please try again. If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'VisaDPS',2),
('1003.103.5002',1,'0','Display message from VISA As-Is','Please try again. If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'VisaDPS',2),
('1003.103.5003',1,'0','Display message from VISA As-Is','Please try again. If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'VisaDPS',2),
('1003.103.Error code from Visa',1,'0','Display message from VISA As-Is','Please contact the IT Service Desk at 763-337-6600.', GETDATE(),'VisaDPS',2),
('1003.103.9999',1,'0','This operation could not be completed','Please try again. If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.6032',1,'0','Error occurred while committing funds','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.6033',1,'0','Error occurred while retrieving the card information','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.6034',1,'0','Account not found','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.3600',1,'0','Error occurred while creating funds transaction','Please try again. If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.3601',1,'0','Error occurred while updating funds transaction','Please try again. If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.3602',1,'0','Error occurred while committing fund transaction','Please try again. If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.3603',1,'0','Error occurred while updating amount','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2),
('1003.100.3604',1,'0','Error occurred while retrieving the funds transaction','Please try again. If problem persists, contact IT Service Desk at 763-337-6600.', GETDATE(),'Zeo',2)



