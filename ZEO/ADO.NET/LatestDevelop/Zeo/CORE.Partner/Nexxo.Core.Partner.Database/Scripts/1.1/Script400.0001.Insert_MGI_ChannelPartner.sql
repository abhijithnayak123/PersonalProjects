--===========================================================================================
-- Auther:			Ratheesh
-- Date Created:	07-Oct-2014
-- Description:		Script for insert MGI channel partner details
-- Rally ID:		
--===========================================================================================
--script to insert ChannelPartner into  [tChannelPartners] 

IF NOT EXISTS (SELECT [rowguid] FROM [dbo].[tChannelPartners] WHERE [rowguid] = '10F2865B-DBC5-4A0B-983C-62E0A0574354')
BEGIN
	INSERT INTO [dbo].[tChannelPartners]
	   ([id],[Name],[FeesFollowCustomer],[CashFeeDescriptionEN],[CashFeeDescriptionES],[DebitFeeDescriptionEN]
	   ,[DebitFeeDescriptionES],[ConvenienceFeeCash],[ConvenienceFeeDebit],[ConvenienceFeeDescriptionEN]
	   ,[ConvenienceFeeDescriptionES],[CanCashCheckWOGovtId],[LogoFileName],[IsEFSPartner],[EFSClientId]
	   ,[UsePINForNonGPR],[IsCUPartner],[HasNonGPRCard],[ManagesCash],[AllowPhoneNumberAuthentication]
	   ,[rowguid],[TIM],[ComplianceProgramName],[CardPresenceVerificationConfig])
	VALUES
	   (1,N'MGI',0,NULL,NULL,NULL
	   ,NULL,NULL,NULL,NULL
	   ,NULL,0,N'MGI.png',0,3
	   ,0,1,0,0,1,
	   '10F2865B-DBC5-4A0B-983C-62E0A0574354',3,N'MGICompliance',0)


	INSERT INTO [dbo].[tChannelPartnerConfig] 
		([ChannelPartnerID],[DisableWithdrawCNP],[CashOverCounter])
	VALUES
		('10F2865B-DBC5-4A0B-983C-62E0A0574354',0,0)

	--script to insert record into [tSkins] 
	INSERT INTO [dbo].[tSkins]
	   ([Id],[ChannelPartnerId],[ValueType],[Name],[ValueEn],[ValueEs],[DTCreate],[DTLastMod])
	VALUES
		(NEWID(),1,1,N'PhoneNumber',N'800-328-5678',N'800-955-7777',GETDATE(),NULL)

	--script to insert Primary location into tlocations
	INSERT	INTO [dbo].[tLocations] 
		([rowguid],[LocationName],[IsActive],[Address1],[Address2],[City],[State],[ZipCode],[DTCreate],
		[DTLastMod],[ChannelPartnerId],[PhoneNumber],[LocationIdentifier],[BankID],[BranchID],[TimezoneID])
	VALUES	
		('AC553817-1B7C-42A4-9C81-DCB8CA590A3E','MoneyGram International',1,'2828 N. Harwood St, Floor 15',NULL,'Dallas','TX','75201',GETDATE(),
		GETDATE(),1,'800-328-5678','0','99','1','Central Standard Time')

	--script to insert default terminal into tTerminals
	INSERT INTO [dbo].[tTerminals] 
		([rowguid],[Name],[MacAddress],[IpAddress],[LocationPK],[NpsTerminalPK],[DTCreate],[DTLastMod],
		[ChannelPartnerPK])
	VALUES	
		(NEWID(),'MGI','','10.111.109.16','AC553817-1B7C-42A4-9C81-DCB8CA590A3E',NULL,GETDATE(),GETDATE(),
		'10F2865B-DBC5-4A0B-983C-62E0A0574354')

	DECLARE @PrimaryLocationId [bigint]
	SELECT  @PrimaryLocationId=[ID] FROM [dbo].[tLocations] 
	WHERE [rowguid]='AC553817-1B7C-42A4-9C81-DCB8CA590A3E'

	--script for inserting sysadmin user
	SET IDENTITY_INSERT [dbo].[tAgentDetails] ON
	INSERT INTO [dbo].[tAgentDetails]
		([rowguid],[Id],[UserName],[FirstName],[LastName],[FullName],[IsEnabled],[ManagerId],[PrimaryLocationId],[UserRoleId],
		[UserStatusId],[ChannelPartnerId],[PhoneNumber],[Email],[Notes],[DTLastLogin],[DTCreate],[DTLastMod]) 
	VALUES	
		(NEWID(),500000,'SysAdmin','System','Administrator','System Administrator',1,500000,@PrimaryLocationId,4,
		1,1,NULL,'',NULL,NULL,GETDATE(),GETDATE())
	SET IDENTITY_INSERT [dbo].[tAgentDetails] OFF

	--pswd: Sysadmin@123
	INSERT INTO [dbo].[tAgentAuthentication] 
		([AgentId],[UserName],[PasswordHash],[Salt],[AuthenticationFailures],[TemporaryPassword],[DTLastPasswordUpdate],[LastPasswordUpdateBy])
	VALUES
		(500000,'SysAdmin','gE+e0p+F4jUxmATMxKQs','2SwFOic=',0,1,GETDATE(),NULL)

	INSERT INTO [dbo].[tAgentLocationMapping] 
		([AgentId],[LocationId])
	VALUES
		(500000,@PrimaryLocationId)
END
GO
--===========================================================================================
