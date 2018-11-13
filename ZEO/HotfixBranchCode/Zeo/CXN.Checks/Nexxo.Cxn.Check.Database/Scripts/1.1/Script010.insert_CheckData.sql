SET IDENTITY_INSERT [tChxr_Partner] ON
INSERT [tChxr_Partner] ([rowguid], [Id], [Name], [URL], [DTCreate], [DTLastMod]) VALUES 
(N'86fd3e44-c31e-46c5-8b01-17923e0d892e', 1000000000, N'MyPartner', N'http://beta.chexar.net/webservice/', CAST(0x0000A1390010C110 AS DateTime), NULL)
SET IDENTITY_INSERT [tChxr_Partner] OFF


SET IDENTITY_INSERT [tChxr_Identity] ON
INSERT [tChxr_Identity] ([rowguid], [Id], [Location], [BranchUsername], [BranchPassword], [EmpUsername], [EmpPassword], [ChxrPartnerPK], [DTCreate], [DTLastMod]) VALUES 
( NEWID(), 1000000003, N'Nexxo', N'nexxo', N'oxxen1', N'nexxotest', N'Nexxo123', N'86fd3e44-c31e-46c5-8b01-17923e0d892e', CAST(0x0000A1390010C110 AS DateTime), NULL)
SET IDENTITY_INSERT [tChxr_Identity] OFF
