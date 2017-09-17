alter table tWUnion_Account
add NameType varchar(200),
FirstName varchar(250),
LastName varchar(250),
[Address] varchar(250),
City varchar(250),
[State] varchar(250),
PostalCode varchar(250),
PreferredCustomerAccountNumber varchar(250),
PreferredCustomerLevelCode varchar(250),
Email varchar(250),
ContactPhone varchar(250),
MobilePhone varchar(250),
SmsNotificationFlag varchar(250)
Go

alter table tWUnion_Trx
add PromotionDiscount bigint,
OtherCharges bigint,
MoneyTransferKey varchar(100),
AdditionalCharges bigint,
DestinationCountryCode varchar(100),
DestinationCurrencyCode varchar(100),
DestinationState varchar(100),
IsDomesticTransfer bit,
IsFixedOnSend bit
Go

alter table tWUnion_Trx
alter column WUnionRecipientAccountPK uniqueidentifier null