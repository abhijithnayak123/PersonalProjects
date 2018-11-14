--===========================================================================================
-- Auther:			Raviraja
-- Date Created:	02/06/2014
-- Description:		Script for insert TCF channel partner details
-- Rally ID:		US1610-TCF Whitelabel,US1825-TIM
--===========================================================================================
--script to insert ChannelPartner into  [tChannelPartners] 
INSERT INTO [dbo].[tChannelPartners]
   ([id],[Name],[FeesFollowCustomer],[CashFeeDescriptionEN],[CashFeeDescriptionES],[DebitFeeDescriptionEN]
   ,[DebitFeeDescriptionES],[ConvenienceFeeCash],[ConvenienceFeeDebit],[ConvenienceFeeDescriptionEN]
   ,[ConvenienceFeeDescriptionES],[CanCashCheckWOGovtId],[LogoFileName],[IsEFSPartner],[EFSClientId]
   ,[UsePINForNonGPR],[IsCUPartner],[HasNonGPRCard],[ManagesCash],[AllowPhoneNumberAuthentication]
   ,[rowguid],[TIM],[ComplianceProgramName],[CardPresenceVerificationConfig])
VALUES
   (34,N'TCF',0,NULL,NULL,NULL
   ,NULL,NULL,NULL,NULL
   ,NULL,0,N'TCF.png',0,3
   ,0,1,0,0,1,
   '6D7E785F-7BDD-42C8-BC49-44536A1885FC',3,N'TCFCompliance',0)


INSERT INTO [dbo].[tChannelPartnerConfig] 
	([ChannelPartnerID],[DisableWithdrawCNP],[CashOverCounter])
VALUES
	('6D7E785F-7BDD-42C8-BC49-44536A1885FC',0,0)

--script to insert record into [tSkins] 
INSERT INTO [dbo].[tSkins]
   ([Id],[ChannelPartnerId],[ValueType],[Name],[ValueEn],[ValueEs],[DTCreate],[DTLastMod])
VALUES
	(NEWID(),34,1,N'PhoneNumber',N'800-823-2265',N'800-823-2265',GETDATE(),NULL)

--script to insert Primary location into tlocations
INSERT	INTO [dbo].[tLocations] 
	([rowguid],[LocationName],[IsActive],[Address1],[Address2],[City],[State],[ZipCode],[DTCreate],
	[DTLastMod],[ChannelPartnerId],[PhoneNumber],[LocationIdentifier],[BankID],[BranchID],[TimezoneID])
VALUES	
	('CB0AFFF8-9404-4C22-B282-F2160D901C93','TCF Service Desk',1,'801 Marquette Avenue',NULL,'Minneapolis','MN','55402',GETDATE(),
	GETDATE(),34,'612-661-6600','0','99','1','Central Standard Time')

--script to insert default terminal into tTerminals
INSERT INTO [dbo].[tTerminals] 
	([rowguid],[Name],[MacAddress],[IpAddress],[LocationPK],[NpsTerminalPK],[DTCreate],[DTLastMod],
	[ChannelPartnerPK])
VALUES	
	(NEWID(),'TCF','','10.111.109.16','CB0AFFF8-9404-4C22-B282-F2160D901C93',NULL,GETDATE(),GETDATE(),
	'6D7E785F-7BDD-42C8-BC49-44536A1885FC')

DECLARE @PrimaryLocationId [bigint]
SELECT  @PrimaryLocationId=[ID] FROM [dbo].[tLocations] 
WHERE [rowguid]='CB0AFFF8-9404-4C22-B282-F2160D901C93'

--script for inserting sysadmin user
SET IDENTITY_INSERT [dbo].[tAgentDetails] ON
INSERT INTO [dbo].[tAgentDetails]
	([rowguid],[Id],[UserName],[FirstName],[LastName],[FullName],[IsEnabled],[ManagerId],[PrimaryLocationId],[UserRoleId],
	[UserStatusId],[ChannelPartnerId],[PhoneNumber],[Email],[Notes],[DTLastLogin],[DTCreate],[DTLastMod]) 
VALUES	
	(NEWID(),400000,'SysAdmin','System','Administrator','System Administrator',1,400000,@PrimaryLocationId,4,
	1,34,NULL,'',NULL,NULL,GETDATE(),GETDATE())
SET IDENTITY_INSERT [dbo].[tAgentDetails] OFF

--pswd: Sysadmin@123
INSERT INTO [dbo].[tAgentAuthentication] 
	([AgentId],[UserName],[PasswordHash],[Salt],[AuthenticationFailures],[TemporaryPassword],[DTLastPasswordUpdate],[LastPasswordUpdateBy])
VALUES
	(400000,'SysAdmin','gE+e0p+F4jUxmATMxKQs','2SwFOic=',0,1,GETDATE(),NULL)

INSERT INTO [dbo].[tAgentLocationMapping] 
	([AgentId],[LocationId])
VALUES
	(400000,@PrimaryLocationId)
GO
--===========================================================================================
