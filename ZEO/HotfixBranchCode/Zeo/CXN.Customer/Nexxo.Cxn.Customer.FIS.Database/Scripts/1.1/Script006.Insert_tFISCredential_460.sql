INSERT [tFIS_Credential] 
(
[rowguid], 
[Id], 
[UserName], 
[Password], 
[Applicationkey], 
[ChannelKey], 
[ChannelPartnerId], 
[BankId], 
[DTCreate], 
[DTLastMod]
) VALUES 
(
NEWID(), 2, 
N'nexxouser', N'$yn1nex', 
N'5A2E3FA8-B2E1-43AF-8DAE-A07723294A47', 5, 33, N'460', GETDATE(), NULL
)
GO