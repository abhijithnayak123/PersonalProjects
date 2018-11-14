-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <12/04/2014>
-- Description:	<Sample insert script for tVisa_Credential table.>
-- Rally ID:	<US2154>
-- ============================================================
INSERT tVisa_Credential
 (
	 [rowguid], 
	 [ServiceUrl], 
	 [CertificateName], 
	 [UserName], 
	 [Password], 
	 [ClientNodeId], 
	 [CardProgramNodeId], 
	 [SubClientNodeId], 
	 [StockId], 
	 [ChannelPartnerId], 
	 [DTCreate]
 ) 
 VALUES 
 (
	 NEWID(), 
	 N'https://certservicesgateway.visaonline.com/websrv_prepaid/v14_10/prepaidservices', 
	 N'TCF Nexxo Web Services (CTE WSI)', 
	 N'prc1279.webserv', 
	 N'pKzWRV24r4', 
	 12081, 
	 250012, 
	 -1, 
	 N'127CS201', 
	 34, 
	 GETDATE()
 )
 GO