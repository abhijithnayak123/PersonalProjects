Alter table tCustomerGovernmentIdDetails
add IssueDate DateTime

Alter table tCustomerProfiles
add IsMailingAddressDifferent bit,
MailingAddress1 nvarchar(255),
MailingAddress2 nvarchar(255),
MailingCity nvarchar(255),
MailingState nvarchar(255),
MailingZipCode nvarchar(255)