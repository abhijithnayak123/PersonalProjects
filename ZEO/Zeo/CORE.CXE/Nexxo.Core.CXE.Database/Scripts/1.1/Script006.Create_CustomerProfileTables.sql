    create table tCustomerProfiles (
        id UNIQUEIDENTIFIER not null,
       PAN BIGINT null,
       FirstName NVARCHAR(255) null,
       MiddleName NVARCHAR(255) null,
       LastName NVARCHAR(255) null,
       LastName2 NVARCHAR(255) null,
       MothersMaidenName NVARCHAR(255) null,
       DOB DATETIME null,
       Address1 NVARCHAR(255) null,
       Address2 NVARCHAR(255) null,
       City NVARCHAR(255) null,
       State NVARCHAR(255) null,
       ZipCode NVARCHAR(255) null,
       Phone1 NVARCHAR(255) null,
       Phone1Type NVARCHAR(255) null,
       Phone1Provider NVARCHAR(255) null,
       Phone2 NVARCHAR(255) null,
       Phone2Type NVARCHAR(255) null,
       Phone2Provider NVARCHAR(255) null,
       SSN NVARCHAR(255) null,
       TaxpayerId NVARCHAR(255) null,
       DoNotCall BIT null,
       SMSEnabled BIT null,
       MarketingSMSEnabled BIT null,
       ChannelPartnerId INT null,
       DTCreate DATETIME null,
       DTLastMod DATETIME null,
       primary key (id)
    )

    create table tCustomerGovernmentIdDetails (
        CustomerProfileId UNIQUEIDENTIFIER not null,
       PAN BIGINT null,
       IdentificationTypeId INT null,
       Identification NVARCHAR(255) null,
       ExpirationDate DATETIME null,
       DTCreate DATETIME null,
       DTLastMod DATETIME null,
       primary key (CustomerProfileId)
    )

    create table tCustomerEmploymentDetails (
        CustomerProfileId UNIQUEIDENTIFIER not null,
       PAN BIGINT null,
       Occupation NVARCHAR(255) null,
       Employer NVARCHAR(255) null,
       EmployerPhone NVARCHAR(255) null,
       DTCreate DATETIME null,
       DTLastMod DATETIME null,
       primary key (CustomerProfileId)
    )

    alter table tCustomerGovernmentIdDetails 
        add constraint FK47342B047B3856E 
        foreign key (CustomerProfileId) 
        references tCustomerProfiles

    alter table tCustomerEmploymentDetails 
        add constraint FK2AF0DFC27B3856E 
        foreign key (CustomerProfileId) 
        references tCustomerProfiles
