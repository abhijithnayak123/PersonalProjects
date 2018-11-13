--===========================================================================================
-- Auther:			Raviraja
-- Date Created:	12/10/2013
-- Description:		Script for inserting channel partner details
--===========================================================================================
--script to insert ChannelPartner into  [tChannelPartners] 
INSERT INTO [dbo].[tChannelPartners]
   ([id],[Name],[FeesFollowCustomer],[CashFeeDescriptionEN],[CashFeeDescriptionES],[DebitFeeDescriptionEN]
   ,[DebitFeeDescriptionES],[ConvenienceFeeCash],[ConvenienceFeeDebit],[ConvenienceFeeDescriptionEN]
   ,[ConvenienceFeeDescriptionES],[CanCashCheckWOGovtId],[LogoFileName],[IsEFSPartner],[EFSClientId]
   ,[UsePINForNonGPR],[IsCUPartner],[HasNonGPRCard],[ManagesCash],[AllowPhoneNumberAuthentication]
   ,[rowguid],[ComplianceProgramName],[CardPresenceVerificationConfig])
VALUES
   (28,N'Carver',0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,N'Carver.png',NULL,3,NULL,1,1,0,1,
   '578AC8FB-F69C-4DBD-A502-57B1EECD41D6',N'CarverCompliance',0)
GO

--script to insert record into [tSkins] 
INSERT INTO [dbo].[tSkins]
   ([Id],[ChannelPartnerId],[ValueType],[Name],[ValueEn],[ValueEs],[DTCreate],[DTLastMod])
VALUES
	(NEWID(),28,1,N'PhoneNumber',N'718-230-2900',N'718-230-2900',GETDATE(),NULL)
GO

--script to insert Primary location into tlocations
INSERT	INTO [dbo].[tLocations] 
	([rowguid],[LocationName],[IsActive],[Address1],[Address2],[City],[State],[ZipCode],[DTCreate],
	[DTLastMod],[ChannelPartnerId],[PhoneNumber],[LocationIdentifier],[BankID],[BranchID],[TimezoneID])
VALUES	
	('5CE4DBBA-1AC9-48C3-8E85-E93F7B12B906','Carver',1,'Branch 1234','100 Main St','Sampleville','OH',
	'99999',GETDATE(),GETDATE(),28,'555.555.5555',NULL,NULL,NULL,'Eastern Standard Time')
GO 

--script to insert default terminal into tTerminals
INSERT INTO [dbo].[tTerminals] 
	([rowguid],[Name],[MacAddress],[IpAddress],[LocationPK],[NpsTerminalPK],[DTCreate],[DTLastMod],
	[ChannelPartnerPK])
VALUES	
	(NEWID(),'Carver','','10.111.109.16','5CE4DBBA-1AC9-48C3-8E85-E93F7B12B906',NULL,GETDATE(),
	GETDATE(),'578AC8FB-F69C-4DBD-A502-57B1EECD41D6')
GO

--script for inserting sysadmin user
SET IDENTITY_INSERT [dbo].[tAgentDetails] ON
GO

INSERT INTO [dbo].[tAgentDetails]
	(rowguid,Id,UserName,FirstName,LastName,FullName,IsEnabled,ManagerId,PrimaryLocationId,UserRoleId,
	UserStatusId,ChannelPartnerId,PhoneNumber,Email,Notes,DTLastLogin,DTCreate,DTLastMod) 
VALUES	
	(NEWID(),300000,'SysAdmin','System','Administrator','System Administrator',1,300000,1000000002,4,1,
	28,NULL,'',NULL,NULL,GETDATE(),GETDATE())
GO
SET IDENTITY_INSERT [dbo].[tAgentDetails] OFF
GO

INSERT INTO [dbo].[tAgentAuthentication] 
	(AgentId,UserName,PasswordHash,Salt,AuthenticationFailures,TemporaryPassword,DTLastPasswordUpdate,
	LastPasswordUpdateBy)
VALUES
	(300000,'SysAdmin','JG0mLAfWkOFl9CGCW2fs','2SwFOic=',0,1,GETDATE(),NULL)
GO

INSERT INTO [dbo].[tAgentLocationMapping] 
	(AgentId,LocationId)
VALUES
	(300000,1000000002)
GO

INSERT INTO [dbo].[tChannelPartnerConfig] 
	(ChannelPartnerID,DisableWithdrawCNP,CashOverCounter)
VALUES
	('578AC8FB-F69C-4DBD-A502-57B1EECD41D6',0,0)
GO
--===========================================================================================
