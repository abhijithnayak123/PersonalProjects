INSERT INTO [tWUnion_AmountTypes]([rowguid],[AmountType],[DTCreate]) VALUES(NEWID(), N'USD', GETDATE())
INSERT INTO [tWUnion_AmountTypes]([rowguid],[AmountType],[DTCreate]) VALUES(NEWID(), N'LocalCurrency', GETDATE())      
GO

INSERT INTO tWUnion_Countries(rowguid,ISOCountryCode,Name,DTCreate) VALUES(NEWID(), N'IN', N'INDIA', GETDATE())
INSERT INTO tWUnion_Countries(rowguid,ISOCountryCode,Name,DTCreate) VALUES(NEWID(), N'MX', N'MEXICO', GETDATE())
INSERT INTO tWUnion_Countries(rowguid,ISOCountryCode,Name,DTCreate) VALUES(NEWID(), N'US', N'United States', GETDATE())
GO

INSERT INTO tWUnion_CountryCurrencies(rowguid,CountryCode,CurrencyCode,DTCreate) VALUES( NEWID(), N'AF', N'AFN', GETDATE())
INSERT INTO tWUnion_CountryCurrencies(rowguid,CountryCode,CurrencyCode,DTCreate) VALUES( NEWID(), N'AU', N'AUD', GETDATE())
INSERT INTO tWUnion_CountryCurrencies(rowguid,CountryCode,CurrencyCode,DTCreate) VALUES( NEWID(), N'CN', N'CAD', GETDATE())
INSERT INTO tWUnion_CountryCurrencies(rowguid,CountryCode,CurrencyCode,DTCreate) VALUES( NEWID(), N'IN', N'INR', GETDATE())
INSERT INTO tWUnion_CountryCurrencies(rowguid,CountryCode,CurrencyCode,DTCreate) VALUES( NEWID(), N'MX', N'MXN', GETDATE())
INSERT INTO tWUnion_CountryCurrencies(rowguid,CountryCode,CurrencyCode,DTCreate) VALUES( NEWID(), N'US', N'USD', GETDATE())
GO

INSERT INTO tWUnion_CountryCurrencyDeliveryMethods(rowguid,CountryCode,CurrencyCode,SvcCode,SvcName,DTCreate) VALUES(NEWID(),'US','USD','001','Money In Minutes',GETDATE())
GO

INSERT INTO tWUnion_DeliveryOptions(rowguid,Product,Category,T_Index,Description,DTCreate) VALUES(NEWID(),'DLVSVCS_UNIVERSAL H2H1','2030','0','DLSVSVSC',GETDATE())
GO 

INSERT INTO tWUnion_Relationships(rowguid,RelationshipName,DTCreate) VALUES (NEWID(),N'',GETDATE())
INSERT INTO tWUnion_Relationships(rowguid,RelationshipName,DTCreate) VALUES (NEWID(),N'AUNT',GETDATE())
INSERT INTO tWUnion_Relationships(rowguid,RelationshipName,DTCreate) VALUES (NEWID(),N'BOYFRIEND',GETDATE())
INSERT INTO tWUnion_Relationships(rowguid,RelationshipName,DTCreate) VALUES (NEWID(),N'BROTHER',GETDATE())
INSERT INTO tWUnion_Relationships(rowguid,RelationshipName,DTCreate) VALUES (NEWID(),N'BROTHER-IN-LAW',GETDATE())
INSERT INTO tWUnion_Relationships(rowguid,RelationshipName,DTCreate) VALUES (NEWID(),N'COUSIN',GETDATE())
INSERT INTO tWUnion_Relationships(rowguid,RelationshipName,DTCreate) VALUES (NEWID(),N'DAUGHTER',GETDATE())
INSERT INTO tWUnion_Relationships(rowguid,RelationshipName,DTCreate) VALUES (NEWID(),N'DAUGHTER-IN-LAW',GETDATE())
INSERT INTO tWUnion_Relationships(rowguid,RelationshipName,DTCreate) VALUES (NEWID(),N'FATHER',GETDATE())
INSERT INTO tWUnion_Relationships(rowguid,RelationshipName,DTCreate) VALUES (NEWID(),N'FATHER-IN-LAW',GETDATE())
INSERT INTO tWUnion_Relationships(rowguid,RelationshipName,DTCreate) VALUES (NEWID(),N'FRIEND',GETDATE())
INSERT INTO tWUnion_Relationships(rowguid,RelationshipName,DTCreate) VALUES (NEWID(),N'GIRLFRIEND',GETDATE())
INSERT INTO tWUnion_Relationships(rowguid,RelationshipName,DTCreate) VALUES (NEWID(),N'GRANDFATHER',GETDATE())
INSERT INTO tWUnion_Relationships(rowguid,RelationshipName,DTCreate) VALUES (NEWID(),N'GRANDMOTHER',GETDATE())
INSERT INTO tWUnion_Relationships(rowguid,RelationshipName,DTCreate) VALUES (NEWID(),N'HUSBAND',GETDATE())
INSERT INTO tWUnion_Relationships(rowguid,RelationshipName,DTCreate) VALUES (NEWID(),N'MOTHER',GETDATE())
INSERT INTO tWUnion_Relationships(rowguid,RelationshipName,DTCreate) VALUES (NEWID(),N'MOTHER-IN-LAW',GETDATE())
INSERT INTO tWUnion_Relationships(rowguid,RelationshipName,DTCreate) VALUES (NEWID(),N'NEPHEW',GETDATE())
INSERT INTO tWUnion_Relationships(rowguid,RelationshipName,DTCreate) VALUES (NEWID(),N'NIECE',GETDATE())
INSERT INTO tWUnion_Relationships(rowguid,RelationshipName,DTCreate) VALUES (NEWID(),N'OTHER',GETDATE())
INSERT INTO tWUnion_Relationships(rowguid,RelationshipName,DTCreate) VALUES (NEWID(),N'SISTER',GETDATE())
INSERT INTO tWUnion_Relationships(rowguid,RelationshipName,DTCreate) VALUES (NEWID(),N'SISTER-IN-LAW',GETDATE())
INSERT INTO tWUnion_Relationships(rowguid,RelationshipName,DTCreate) VALUES (NEWID(),N'SON',GETDATE())
INSERT INTO tWUnion_Relationships(rowguid,RelationshipName,DTCreate) VALUES (NEWID(),N'SON-IN-LAW',GETDATE())
INSERT INTO tWUnion_Relationships(rowguid,RelationshipName,DTCreate) VALUES (NEWID(),N'UNCLE',GETDATE())
INSERT INTO tWUnion_Relationships(rowguid,RelationshipName,DTCreate) VALUES (NEWID(),N'WIFE',GETDATE())

INSERT INTO tWUnion_States(rowguid,StateCode,Name,ISOCountryCode,DTCreate) VALUES(NEWID(),N'CA', N'CALIFORNIA', N'US', GETDATE())
INSERT INTO tWUnion_States(rowguid,StateCode,Name,ISOCountryCode,DTCreate) VALUES(NEWID(),N'HI', N'HINTON', N'CN', GETDATE())
INSERT INTO tWUnion_States(rowguid,StateCode,Name,ISOCountryCode,DTCreate) VALUES(NEWID(),N'KA', N'KARNATAKA', N'IN', GETDATE())
INSERT INTO tWUnion_States(rowguid,StateCode,Name,ISOCountryCode,DTCreate) VALUES(NEWID(),N'SI', N'SINALOA ', N'MX', GETDATE())
INSERT INTO tWUnion_States(rowguid,StateCode,Name,ISOCountryCode,DTCreate) VALUES(NEWID(),N'TL', N'TOLUCA', N'MX',GETDATE())
GO

INSERT INTO tWUnion_Cities(rowguid,Name,StateCode,DTCreate) VALUES(NEWID(),N'BANGALORE', N'KA', GETDATE())
INSERT INTO tWUnion_Cities(rowguid,Name,StateCode,DTCreate) VALUES(NEWID(),N'SAN FRANCISCO', N'CA', GETDATE())
INSERT INTO tWUnion_Cities(rowguid,Name,StateCode,DTCreate) VALUES(NEWID(),N'TOLUCA', N'TL', GETDATE())
INSERT INTO tWUnion_Cities(rowguid,Name,StateCode,DTCreate) VALUES(NEWID(),N'ALBERTA', N'HI', GETDATE())
INSERT INTO tWUnion_Cities(rowguid,Name,StateCode,DTCreate) VALUES(NEWID(),N'GARDEN STATE', N'VI', GETDATE())
INSERT INTO tWUnion_Cities(rowguid,Name,StateCode,DTCreate) VALUES(NEWID(),N'ALTAMURA', N'SI', GETDATE())
GO


