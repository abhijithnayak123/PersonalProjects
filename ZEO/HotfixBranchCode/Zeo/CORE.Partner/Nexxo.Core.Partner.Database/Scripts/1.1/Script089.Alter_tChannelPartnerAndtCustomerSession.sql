---- US1458 Card Present Flag

if not exists(select * from sys.columns 
            where Name = N'CardPresent' and Object_ID = Object_ID(N'tCustomerSessions'))
begin
ALTER TABLE [tCustomerSessions]
	ADD CardPresent BIT NULL 
	CONSTRAINT Const_CustSession DEFAULT 0
end
GO

if not exists(select * from sys.columns 
            where Name = N'CardPresenceVerificationConfig' and Object_ID = Object_ID(N'tChannelPartners'))
begin
ALTER TABLE  [tChannelPartners]
	ADD CardPresenceVerificationConfig INT NULL
end
GO