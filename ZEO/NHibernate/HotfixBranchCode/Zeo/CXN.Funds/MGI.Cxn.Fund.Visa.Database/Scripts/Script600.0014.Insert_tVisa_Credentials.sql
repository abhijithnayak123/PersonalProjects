-- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <23/08/2015>
-- Description:	<As Alloy, I need to configure products for Synovus>
-- Jira ID:		<AL-908>
-- ================================================================================

SET IDENTITY_INSERT [dbo].[tVisa_Credential] ON

INSERT 
[dbo].[tVisa_Credential]
([rowguid], [Id], [ServiceUrl], [CertificateName],
[UserName], [Password], [ClientNodeId], [CardProgramNodeId], 
[SubClientNodeId], [StockId], [ChannelPartnerId],
[DTServerCreate], [DTServerLastModified])

VALUES 
(NEWID(), 1000000001, N'https://certservicesgateway.visaonline.com/websrv_prepaid/v15_10/prepaidservices',
 N'Synovus DPS Prepaid Web Service (CTE WSI)', N'prc96702.webserv', N'Synovus1', 368769, 368770,
 -1, N'967CS001', 33, CAST(0x0000A51300C58E6B AS DateTime), NULL)
 
SET IDENTITY_INSERT [dbo].[tVisa_Credential] OFF
