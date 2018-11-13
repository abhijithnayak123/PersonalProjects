if not exists(select * from sys.columns where Name = N'ClientId' and Object_ID = Object_ID(N'tCustomer'))
begin
Alter Table tCustomer
Add [ClientId] [bigint] NULL,
[ClientCustomerId] [bigint] NULL,
[BankId] [nvarchar](40) NULL,
[FraudScore] [int] NULL,
[AlternatePhoneType] [nvarchar](255) NULL,
[MailingAddress1] [nvarchar](255) NULL,
[MailingAddress2] [nvarchar](255) NULL,
[MailingCity] [nvarchar](255) NULL,
[MailingState] [nvarchar](255) NULL,
[MailingZipcode] [nvarchar](255) NULL
end