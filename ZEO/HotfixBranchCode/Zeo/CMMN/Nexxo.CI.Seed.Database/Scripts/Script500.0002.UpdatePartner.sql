USE [$DBPrifix$_PTNR]
update tChannelPartners set TIM=3
--Updating for Synovus carver and TCF
update tLocations set bankID=300 ,BranchID='0120', NoOfCounterIDs = 3
where LocationPK in('BC46F466-16D3-47B9-97CC-A9F95E2A2CCB','5CE4DBBA-1AC9-48C3-8E85-E93F7B12B906','CB0AFFF8-9404-4C22-B282-F2160D901C93')

update tLocationProcessorCredentials set UserName = '13139925', Password = '13139925' 
	where locationId in('BC46F466-16D3-47B9-97CC-A9F95E2A2CCB','5CE4DBBA-1AC9-48C3-8E85-E93F7B12B906','CB0AFFF8-9404-4C22-B282-F2160D901C93') and ProviderId = 200
update tLocationProcessorCredentials set Identifier = '13139925' 
	where locationId in('BC46F466-16D3-47B9-97CC-A9F95E2A2CCB','5CE4DBBA-1AC9-48C3-8E85-E93F7B12B906','CB0AFFF8-9404-4C22-B282-F2160D901C93') and ProviderId in (401, 301)
update tLocationProcessorCredentials set Identifier = '13139925' 
	where locationId in('BC46F466-16D3-47B9-97CC-A9F95E2A2CCB','5CE4DBBA-1AC9-48C3-8E85-E93F7B12B906','CB0AFFF8-9404-4C22-B282-F2160D901C93') and ProviderId = 102

-- Insert time Zone to synovus default location
update tLocations set [TimezoneID]='Eastern Standard Time'  
where LocationPK ='BC46F466-16D3-47B9-97CC-A9F95E2A2CCB'

UPDATE [tAgentAuthentication] Set [TemporaryPassword]=0 

-- Inserting Connect DBData
USE [$DBPrifix$_CXN]
SET IDENTITY_INSERT [tFISConnectsDb] ON
INSERT [tFISConnectsDb] ([CustomerNumber], [PrimaryPhoneNumber], [Secondaryphone], [CustomerTaxNumber], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [DriversLicenseNumber], [MothersMaidenName], [Gender], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [FISConnectsDbID], [FISConnectsPK]) VALUES (N'10', N'0706568112', N'0706568113', N'111111111', N'CAGE', N'BYRON', N'     ', NULL, N'444 GOSPEL AVE', N'COLUMBUS', N'GA', N'31909', CAST(0x00005D3900000000 AS DateTime), N'111222333', N'Mother', NULL, N'1000125790', N'460', N'13139925', CAST(0x0000A2910102E36E AS DateTime), 1000000000, N'2ac33ca2-c82e-4c21-a000-cf275ddc046f')
INSERT [tFISConnectsDb] ([CustomerNumber], [PrimaryPhoneNumber], [Secondaryphone], [CustomerTaxNumber], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [DriversLicenseNumber], [MothersMaidenName], [Gender], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [FISConnectsDbID], [FISConnectsPK]) VALUES (N'58', N'   7066161', N'0000000000', N'945612345', N'OWENS', N'HAZEL', N'     ', NULL, N'2140 COUNTY RD 49', N'KENNEDY', N'AL', N'35574', CAST(0x00003DBE00000000 AS DateTime), N'445659641', NULL, NULL, N'1000125791', N'460', N'13139925', CAST(0x0000A2910102E37D AS DateTime), 1000000001, N'35e0a6b7-28ff-4147-bc8d-38584ed16224')
INSERT [tFISConnectsDb] ([CustomerNumber], [PrimaryPhoneNumber], [Secondaryphone], [CustomerTaxNumber], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [DriversLicenseNumber], [MothersMaidenName], [Gender], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [FISConnectsDbID], [FISConnectsPK]) VALUES (N'384', N'0706258147', N'7062581471', N'111222333', N'BROWN', N'JAN', N'     ', NULL, N'111 MIDDLE ST', N'COLUMBUS', N'GA', N'31905', CAST(0x000041CD00000000 AS DateTime), N'555444654', N'Smith', NULL, N'1000125792', N'460', N'13139925', CAST(0x0000A2910102E38C AS DateTime), 1000000002, N'1184f7e6-ed51-4ce0-888e-6e34ad35b0ea')
INSERT [tFISConnectsDb] ([CustomerNumber], [PrimaryPhoneNumber], [Secondaryphone], [CustomerTaxNumber], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [DriversLicenseNumber], [MothersMaidenName], [Gender], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [FISConnectsDbID], [FISConnectsPK]) VALUES (N'1446', N'0700632199', N'7006321999', N'111223233', N'BRADY', N'CAROL', N'     ', NULL, N'5516 DAVY JONES WAY', N'WATERTOWN', N'MA', N'02471', CAST(0x0000627300000000 AS DateTime), N'555555555', N'Smith', NULL, N'1000125793', N'460', N'13139925', CAST(0x0000A2910102E39A AS DateTime), 1000000003, N'0f2b786c-01de-46a7-a4b2-5682f3964534')
INSERT [tFISConnectsDb] ([CustomerNumber], [PrimaryPhoneNumber], [Secondaryphone], [CustomerTaxNumber], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [DriversLicenseNumber], [MothersMaidenName], [Gender], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [FISConnectsDbID], [FISConnectsPK]) VALUES (N'805', N'0706327888', N'0706649420', N'111223333', N'KENT', N'CLARK', NULL, NULL, N'205 RADIATION BLVD', N'COLUMBUS', N'GA', N'31906', CAST(0x0000678400000000 AS DateTime), N'987654321', N'Smith', NULL, N'1000125794', N'460', N'13139925', CAST(0x0000A2910102E3A9 AS DateTime), 1000000004, N'c6db1f5a-2f88-4293-b727-835ccdc82459')
INSERT [tFISConnectsDb] ([CustomerNumber], [PrimaryPhoneNumber], [Secondaryphone], [CustomerTaxNumber], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [DriversLicenseNumber], [MothersMaidenName], [Gender], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [FISConnectsDbID], [FISConnectsPK]) VALUES (N'2372', N'0321321321', N'0321313213', N'122121212', N'MILLER', N'KURT', N'     ', NULL, N'1234 21ST ST', N'COLUMBUS', N'GA', N'31904', CAST(0x0000833700000000 AS DateTime), N'456450000', N'ASDFSADF            ', NULL, N'1000125795', N'460', N'13139925', CAST(0x0000A2910102E3B7 AS DateTime), 1000000005, N'c25e8423-460f-4730-9445-c87e6a70cc4f')
INSERT [tFISConnectsDb] ([CustomerNumber], [PrimaryPhoneNumber], [Secondaryphone], [CustomerTaxNumber], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [DriversLicenseNumber], [MothersMaidenName], [Gender], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [FISConnectsDbID], [FISConnectsPK]) VALUES (N'1554', N'0224224224', N'0222422244', N'123123444', N'CLAMPETT', N'JED', N'     ', NULL, N'101 MONEY DR', N'BEVERLY HILLS', N'CA', N'90210', CAST(0x000031EF00000000 AS DateTime), N'123123444', N'DAVIS               ', NULL, N'1000125796', N'460', N'13139925', CAST(0x0000A2910102E3C6 AS DateTime), 1000000006, N'73e3bd05-5988-472c-8f74-ba1256c04c61')
INSERT [tFISConnectsDb] ([CustomerNumber], [PrimaryPhoneNumber], [Secondaryphone], [CustomerTaxNumber], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [DriversLicenseNumber], [MothersMaidenName], [Gender], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [FISConnectsDbID], [FISConnectsPK]) VALUES (N'1395', N'0850455126', N'0850455915', N'123569872', N'RUNNER', N'ROAD', N'     ', NULL, N'25 DISNEY LN', N'ORLANDO', N'FL', N'32091', CAST(0x0000403400000000 AS DateTime), N'123569872', N'COYOTE              ', NULL, N'1000125797', N'460', N'13139925', CAST(0x0000A2910102E3D4 AS DateTime), 1000000007, N'4c1eec03-62f5-4f10-9f2e-07a0673ab49d')
INSERT [tFISConnectsDb] ([CustomerNumber], [PrimaryPhoneNumber], [Secondaryphone], [CustomerTaxNumber], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [DriversLicenseNumber], [MothersMaidenName], [Gender], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [FISConnectsDbID], [FISConnectsPK]) VALUES (N'3', N'0206392200', N'7066441616', N'147852369', N'MERRITT', N'WENDEE', NULL, NULL, N'1000 S ROYAL POINCIANA BLVD', N'MIAMI', N'FL', N'33166', CAST(0x0000307D00000000 AS DateTime), N'147852369', N'BRASSELL            ', NULL, N'1000125798', N'460', N'13139925', CAST(0x0000A2910102E3E3 AS DateTime), 1000000008, N'5d96eb89-e0e2-4b08-b7c5-fc1ecf5e81e3')
INSERT [tFISConnectsDb] ([CustomerNumber], [PrimaryPhoneNumber], [Secondaryphone], [CustomerTaxNumber], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [DriversLicenseNumber], [MothersMaidenName], [Gender], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [FISConnectsDbID], [FISConnectsPK]) VALUES (N'255', N'0706323897', N'0706563998', N'147855555', N'BRADY', N'L', NULL, NULL, N'10 MAPLE DR', N'WATERTOWN', N'MA', N'02471', CAST(0x00006C3B00000000 AS DateTime), N'147855555', N'JONES               ', NULL, N'1000125799', N'460', N'13139925', CAST(0x0000A2910102E3F1 AS DateTime), 1000000009, N'4b64257a-985b-4bcc-a084-7fa7ea821f5a')
INSERT [tFISConnectsDb] ([CustomerNumber], [PrimaryPhoneNumber], [Secondaryphone], [CustomerTaxNumber], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [DriversLicenseNumber], [MothersMaidenName], [Gender], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [FISConnectsDbID], [FISConnectsPK]) VALUES (N'273', N'0123777774', N'0706999552', N'222552222', N'MOTT', N'KELLY', N'     ', NULL, N'2 EASY ST', N'SAVANNAH', N'GA', N'31904', CAST(0x000064F200000000 AS DateTime), N'222552222', N'BELL                ', NULL, N'1000125800', N'460', N'13139925', CAST(0x0000A2910102E400 AS DateTime), 1000000010, N'60639d92-e7a2-4b98-8e57-eb2b6eb7b3db')
INSERT [tFISConnectsDb] ([CustomerNumber], [PrimaryPhoneNumber], [Secondaryphone], [CustomerTaxNumber], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [DriversLicenseNumber], [MothersMaidenName], [Gender], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [FISConnectsDbID], [FISConnectsPK]) VALUES (N'2117', N'5656565656', N'0232323236', N'232323233', N'JONES', N'BOB', N'     ', NULL, N'2324 HOPE AVE', N'COLUMBUS', N'GA', N'31906', CAST(0x000066F500000000 AS DateTime), N'232323233', N'HALL                ', NULL, N'1000125801', N'460', N'13139925', CAST(0x0000A2910102E40E AS DateTime), 1000000011, N'9cf5ff3e-baca-4350-87a1-2511bd5411c1')
INSERT [tFISConnectsDb] ([CustomerNumber], [PrimaryPhoneNumber], [Secondaryphone], [CustomerTaxNumber], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [DriversLicenseNumber], [MothersMaidenName], [Gender], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [FISConnectsDbID], [FISConnectsPK]) VALUES (N'2637', N'0706562954', N'7065629544', N'251111511', N'BUNNY', N'BUGS', N'     ', NULL, N'1255 CARROT LN', N'COLUMBUS', N'GA', N'31909', CAST(0x0000806800000000 AS DateTime), N'251111511', N'STAR                ', NULL, N'1000125802', N'460', N'13139925', CAST(0x0000A2910102E41D AS DateTime), 1000000012, N'6a45ba80-f0f8-4f9d-a7aa-d500919ea3e3')
INSERT [tFISConnectsDb] ([CustomerNumber], [PrimaryPhoneNumber], [Secondaryphone], [CustomerTaxNumber], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [DriversLicenseNumber], [MothersMaidenName], [Gender], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [FISConnectsDbID], [FISConnectsPK]) VALUES (N'4707', N'0706232823', N'0706291977', N'251256540', N'GREEN', N'ANN', N'L', NULL, N'1156 HERMITAGE RD', N'ROME', N'GA', N'30161', CAST(0x0000355700000000 AS DateTime), N'251256540', N'HOWELL              ', NULL, N'1000125803', N'460', N'13139925', CAST(0x0000A2910102E42C AS DateTime), 1000000013, N'fbb3ec09-e035-4455-b3fb-f630ced5150b')
INSERT [tFISConnectsDb] ([CustomerNumber], [PrimaryPhoneNumber], [Secondaryphone], [CustomerTaxNumber], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [DriversLicenseNumber], [MothersMaidenName], [Gender], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [FISConnectsDbID], [FISConnectsPK]) VALUES (N'22', N'0850729555', N'8507295555', N'252045367', N'SAVAGE', N'BETTY', NULL, NULL, N'1000 RIVER RD', N'COLUMBUS', N'GA', N'31904', CAST(0x000035A600000000 AS DateTime), N'555555555', N'NAME                ', NULL, N'1000125804', N'460', N'13139925', CAST(0x0000A2910102E43A AS DateTime), 1000000014, N'4bcabb71-dcca-46f9-b562-0a9b5f62d471')
INSERT [tFISConnectsDb] ([CustomerNumber], [PrimaryPhoneNumber], [Secondaryphone], [CustomerTaxNumber], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [DriversLicenseNumber], [MothersMaidenName], [Gender], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [FISConnectsDbID], [FISConnectsPK]) VALUES (N'1749', N'0229931962', N'2299385262', N'260648411', N'MINICK', N'SALLIE', N'F    ', NULL, N'150 MURPHY MILL RD', N'AMERICUS', N'GA', N'31709', CAST(0x00006BE800000000 AS DateTime), N'123598711', N'DUNN                ', NULL, N'1000125805', N'460', N'13139925', CAST(0x0000A2910102E448 AS DateTime), 1000000015, N'b94892f3-228d-4c10-9ce7-d7867bfd390d')
INSERT [tFISConnectsDb] ([CustomerNumber], [PrimaryPhoneNumber], [Secondaryphone], [CustomerTaxNumber], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [DriversLicenseNumber], [MothersMaidenName], [Gender], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [FISConnectsDbID], [FISConnectsPK]) VALUES (N'378', N'0706323100', N'7063231000', N'357357357', N'WHITE', N'SNOW', N'     ', NULL, N'120 DISNEY LN', N'COLUMBUS', N'GA', N'31909', CAST(0x0000561700000000 AS DateTime), N'357357357', N'ADAMS               ', NULL, N'1000125806', N'460', N'13139925', CAST(0x0000A2910102E457 AS DateTime), 1000000016, N'65f86129-69d5-4de9-8105-11a56efd89c1')
INSERT [tFISConnectsDb] ([CustomerNumber], [PrimaryPhoneNumber], [Secondaryphone], [CustomerTaxNumber], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [DriversLicenseNumber], [MothersMaidenName], [Gender], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [FISConnectsDbID], [FISConnectsPK]) VALUES (N'1775', N'0706563735', N'7065637355', N'400000000', N'YOUNG', N'DAVID', N'MICHA', NULL, N'6814 SPRINGLAKE DR', N'COLUMBUS', N'GA', N'31909', CAST(0x0000697C00000000 AS DateTime), N'405052153', N'RICHARD             ', NULL, N'1000125807', N'460', N'13139925', CAST(0x0000A2910102E465 AS DateTime), 1000000017, N'69b4a823-b3c0-4e4e-8108-cae22f5aef72')
INSERT [tFISConnectsDb] ([CustomerNumber], [PrimaryPhoneNumber], [Secondaryphone], [CustomerTaxNumber], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [DriversLicenseNumber], [MothersMaidenName], [Gender], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [FISConnectsDbID], [FISConnectsPK]) VALUES (N'1775', N'0706563733', N'7065637359', N'405052153', N'JAMES', N'DAVID', N'MICHA', NULL, N'6815 SPRINGLAKE DR', N'COLUMBUS', N'GA', N'31909', CAST(0x0000697C00000000 AS DateTime), N'405052153', NULL, NULL, N'1000125808', N'460', N'13139925', CAST(0x0000A2910102E474 AS DateTime), 1000000018, N'9398f732-7b92-462d-aaa1-917e336be541')
INSERT [tFISConnectsDb] ([CustomerNumber], [PrimaryPhoneNumber], [Secondaryphone], [CustomerTaxNumber], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [DriversLicenseNumber], [MothersMaidenName], [Gender], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [FISConnectsDbID], [FISConnectsPK]) VALUES (N'4200', N'0334292123', N'2058684868', N'417986913', N'ARGO', N'KRYSTEL', N'M', NULL, N'1111 SAXON LN', N'PHENIX CITY', N'AL', N'36867', CAST(0x00006AE100000000 AS DateTime), N'655451234', N'HARRIS              ', NULL, N'1000125809', N'460', N'13139925', CAST(0x0000A2910102E482 AS DateTime), 1000000019, N'6e804ba9-4291-4e98-a9fe-692ad6cf2de9')
SET IDENTITY_INSERT [tFISConnectsDb] OFF
SET IDENTITY_INSERT [tCCISConnectsDb] ON
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00111', N'012232345', N'6503233245', N'6503233246', N'Bond', N'James', NULL, NULL, N'10 Park Ave', N'Concord', N'California', N'94520', CAST(0x0000733E00000000 AS DateTime), N'MOMA', N'D1237690', NULL, NULL, NULL, CAST(0x0000A32700000000 AS DateTime), 1000000000, N'b679f706-c53e-4d7e-b120-aed5dcb730fd', N'Male')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00044690', N'123456112', N'2124800791', NULL, N'DOGA', N'BADA', NULL, NULL, N'161 LEXINGTON AVE', N'NEW YORK', N'NY', N'10035', CAST(0x0000500500000000 AS DateTime), N'ADOWNY', N'456123456', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000001, N'7ec8e02d-cfaa-4eab-8d20-6818ef4b8715', N'MALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00044680', N'232264848', N'6465887931', NULL, N'GATESA', N'WILLIAMA', NULL, NULL, N'2151 ALEXANDER AVE', N'BRONX', N'NY', N'10454', CAST(0x0000505D00000000 AS DateTime), N'MANCHEZ', N'112244552', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000002, N'4974fcc8-8925-42aa-9c98-4c87126ca766', N'MALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00044720', N'421456123', N'3473667171', NULL, N'FLINTSONEA', N'WILMAA', NULL, NULL, N'12121 PARK AVE', N'NEW YORK', N'NY', N'10027', CAST(0x00005F2B00000000 AS DateTime), N'ROCKY', N'555123456', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000003, N'b33f40a4-f7d7-4650-a51b-7c2d93ab57d9', N'FEMALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00044710', N'456123741', N'2123457891', NULL, N'ROBERTSA', N'LEEA', NULL, NULL, N'1251 125TH STREET', N'NEW YORK', N'NY', N'10035', CAST(0x00007B9900000000 AS DateTime), N'ROBERTSA', N'456369999', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000004, N'b7de1645-31f2-4308-8f99-4c1aeae713fe', N'MALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00044700', N'741741123', N'2124567891', NULL, N'AQUINASA', N'THOMASA', NULL, NULL, N'110 1125TH ST', N'NEW YORK', N'NY', N'10035', CAST(0x00006C8B00000000 AS DateTime), N'CANDYS', N'789753789', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000005, N'c7f21791-5734-4e62-9404-0437fa225685', N'MALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00045780', N'268957413', N'3698542471', NULL, N'YEAHLA', N'PAULA', NULL, NULL, N'123 1RUE DU SOLEIL', N'LUNE', N'NY', N'10458', CAST(0x0000253800000000 AS DateTime), N'LOUISEAP', N'136987584', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000006, N'089c939a-5324-4838-a6d6-8a839fa64a4c', N'MALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00044690', N'123456112', N'2124800792', NULL, N'GODA', N'DAB', NULL, NULL, N'1601 LEXINGTON AVE', N'NEW YORK', N'NY', N'10035', CAST(0x0000517300000000 AS DateTime), N'DOWNES', N'456123456', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000007, N'686ca47e-0e95-4025-a9ff-c370b3cb9646', N'MALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00044680', N'232264848', N'6465887932', NULL, N'STAGE', N'MILLIAM', NULL, NULL, N'2151 ALEXANDER AVE', N'BRONX', N'NY', N'10454', CAST(0x000051CA00000000 AS DateTime), N'SANCHEZA', N'112244552', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000008, N'8a4f1b70-2103-431e-bbd8-8087c2f66dbe', N'MALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00044720', N'421456123', N'3473667172', NULL, N'STONE', N'WILMAA', NULL, NULL, N'12121 PARK AVE', N'NEW YORK', N'NY', N'10027', CAST(0x0000609800000000 AS DateTime), N'ROCKSAP', N'555123456', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000009, N'60349e30-f5b1-47c0-9daa-54ce39654693', N'FEMALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00044710', N'456123741', N'2123457892', NULL, N'ROBERTS', N'EEL', NULL, NULL, N'1251 125TH STREET', N'NEW YORK', N'NY', N'10035', CAST(0x00007D0600000000 AS DateTime), N'GONZAL', N'456369999', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000010, N'5b1e5b55-d07d-4517-b282-dfad4cae54e3', N'MALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00044700', N'741741123', N'2124567892', NULL, N'AQUINAS', N'TOMAS', NULL, NULL, N'1101 125TH ST', N'NEW YORK', N'NY', N'10035', CAST(0x00006DF900000000 AS DateTime), N'HANDY', N'789753789', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000011, N'4fff6efc-2d06-4646-a69d-e5058e371f49', N'MALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00045780', N'268957413', N'3698542472', NULL, N'HEALY', N'LAUP', NULL, NULL, N'1123 RUE DU SOLEIL', N'LUNE', N'NY', N'10458', CAST(0x000026A500000000 AS DateTime), N'POUISE', N'136987584', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000012, N'aca5835a-4efa-4b63-9eed-928df864ee3d', N'MALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00044690', N'123456112', N'2124800793', NULL, N'DOGBA', N'BADBA', NULL, NULL, N'1602 LEXINGTON AVE', N'NEW YORK', N'NY', N'10035', CAST(0x000052E000000000 AS DateTime), N'DOW', N'456123456', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000013, N'b506ff7a-b562-4aec-ad2b-1a0aa52c5b90', N'MALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00044680', N'232264848', N'6465887933', NULL, N'GATESMAC', N'WILLIAM', NULL, NULL, N'2152 ALEXANDER AVE', N'BRONX', N'NY', N'10454', CAST(0x0000533700000000 AS DateTime), N'HEZ', N'112244552', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000014, N'da5e1edc-4b7e-4748-aaf8-363983218ecb', N'MALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00044720', N'421456123', N'3473667173', NULL, N'FLINTSONEY', N'WILMAC', NULL, NULL, N'12122  PARK AVE', N'NEW YORK', N'NY', N'10027', CAST(0x0000620600000000 AS DateTime), N'ROC', N'555123456', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000015, N'ad70d4e3-7740-4ccc-bc4c-5e32eb3d75e2', N'FEMALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00044710', N'456123741', N'2123457893', NULL, N'ROBS', N'FLEE', NULL, NULL, N'1252 125TH STREET', N'NEW YORK', N'NY', N'10035', CAST(0x00007E7400000000 AS DateTime), N'ROB', N'456369999', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000016, N'c20a0974-3c05-4b3f-ad30-9a1cdd50dd2d', N'MALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00044700', N'741741123', N'2124567893', NULL, N'AQUINASA', N'HOMAS', NULL, NULL, N'1102 125TH ST', N'NEW YORK', N'NY', N'10035', CAST(0x00006F6600000000 AS DateTime), N'ANDY', N'789753789', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000017, N'd719a1a7-da9e-4921-a730-c3090d197906', N'MALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00045780', N'268957413', N'3698542473', NULL, N'YEAHLAS', N'PAULAS', NULL, NULL, N'1232 RUE DU SOLEIL', N'LUNE', N'NY', N'10458', CAST(0x0000281200000000 AS DateTime), N'LOU', N'136987584', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000018, N'cee75f72-854f-4db3-87b9-8e0aefd5f755', N'MALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00044690', N'123456112', N'2124800794', NULL, N'OSBORNE', N'JOE', NULL, NULL, N'1603  LEXINGTON AVE', N'NEW YORK', N'NY', N'10035', CAST(0x0000544D00000000 AS DateTime), N'DO', N'456123456', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000019, N'411052b9-3dce-402c-bb43-73fdbc3c99d0', N'MALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00044680', N'232264848', N'6465887934', NULL, N'GATES', N'BILL', NULL, NULL, N'2153 ALEXANDER AVE', N'BRONX', N'NY', N'10454', CAST(0x000054A400000000 AS DateTime), N'SAN', N'112244552', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000020, N'89d8e464-e504-4d9d-b37b-f5937f3d33c0', N'MALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00044720', N'421456123', N'3473667174', NULL, N'FLINTSONE', N'HILLARY', NULL, NULL, N'12123 PARK AVE', N'NEW YORK', N'NY', N'10027', CAST(0x0000637300000000 AS DateTime), N'OCKS', N'555123456', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000021, N'2f02d511-242a-45bc-9ef8-45418383a9aa', N'FEMALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00044710', N'456123741', N'2123457894', NULL, N'ROBERTS', N'CAYOTE', NULL, NULL, N'1253 125TH STREET', N'NEW YORK', N'NY', N'10035', CAST(0x00007FE100000000 AS DateTime), N'BERTS', N'456369999', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000022, N'a84ba02b-73b1-4a51-b172-2dee0d1901f5', N'MALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00044700', N'741741123', N'2124567894', NULL, N'AQUINAS', N'JOHN', NULL, NULL, N'110 3125TH ST', N'NEW YORK', N'NY', N'10035', CAST(0x000070D300000000 AS DateTime), N'LOOPY', N'789753789', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000023, N'9ef55b87-9ea6-42ec-b30c-9f742991d569', N'MALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00045780', N'268957413', N'3698542474', NULL, N'BITUIN', N'AMANDO', NULL, NULL, N'1233 RUE DU SOLEIL', N'LUNE', N'NY', N'10458', CAST(0x0000298000000000 AS DateTime), N'SUS', N'136987584', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000024, N'b0a9f020-0a1b-4d03-a8a9-d1714a8b2442', N'MALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00044690', N'123456112', N'2124800795', NULL, N'DeNERO', N'ROBERT', NULL, NULL, N'1604 LEXINGTON AVE', N'NEW YORK', N'NY', N'10035', CAST(0x00004BBE00000000 AS DateTime), N'NUIN', N'456123456', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000025, N'a97fe8e9-e189-4056-b0dd-164ed84e0a0c', N'MALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00044680', N'232264848', N'6465887935', NULL, N'STONE', N'SHARON', NULL, NULL, N'2154 ALEXANDER AVE', N'BRONX', N'NY', N'10454', CAST(0x00004C1500000000 AS DateTime), N'SANT', N'112244552', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000026, N'16fcf8ba-612d-40bf-a9e5-71580e6c6bcd', N'FEMALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00044720', N'421456123', N'3473667175', NULL, N'TRAVELS', N'GULLIVER', NULL, NULL, N'12142 PARK AVE', N'NEW YORK', N'NY', N'10027', CAST(0x00005AE300000000 AS DateTime), N'ROCKSANA', N'555123456', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000027, N'63d20ff2-17af-4550-b67d-e2b24559b906', N'FEMALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00044710', N'456123741', N'2123457895', NULL, N'McPHERSON', N'TRACY', NULL, NULL, N'1245 125TH STREET', N'NEW YORK', N'NY', N'10035', CAST(0x0000775100000000 AS DateTime), N'ROBERTSANA', N'456369999', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000028, N'fcb7c6b9-696c-4e1d-8c85-f10a5651a499', N'FEMALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00044700', N'741741123', N'2124567895', NULL, N'BLANEY', N'AMANDA', NULL, NULL, N'14410 125TH ST', N'NEW YORK', N'NY', N'10035', CAST(0x000069B100000000 AS DateTime), N'CANDYSONA', N'789753789', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000029, N'a5fea3b0-c305-46dd-be61-044d67726564', N'FEMALE')
INSERT [tCCISConnectsDb] ([CustomerNumber], [CustomerTaxNumber], [PrimaryPhoneNumber], [SecondaryPhone], [LastName], [FirstName], [MiddleName], [MiddleName2], [AddressStreet], [AddressCity], [AddressState], [ZipCode], [DOB], [MothersMaidenName], [DriversLicenseNumber], [ExternalKey], [MetBankNumber], [ProgramId], [DTCreate], [CCISConnectsID], [CCISConnectsPK], [Gender]) VALUES (N'00045780', N'268957413', N'3698542476', NULL, N'ABDUL', N'PAULA', NULL, NULL, N'1423 RUE DU SOLEIL', N'LUNE', N'NY', N'10458', CAST(0x000020F000000000 AS DateTime), N'LOUISEPHILIP', N'136987584', NULL, NULL, NULL, CAST(0x0000A3370127F57E AS DateTime), 1000000030, N'7a8af265-1cd2-4991-91ba-a3fa443e4124', N'FEMALE')
SET IDENTITY_INSERT [tCCISConnectsDb] OFF
