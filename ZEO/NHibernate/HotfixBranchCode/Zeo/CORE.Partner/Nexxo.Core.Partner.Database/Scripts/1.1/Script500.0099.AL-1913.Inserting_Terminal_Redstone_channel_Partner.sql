-- ================================================================================
-- Author:		<Nitish Biradar>
-- Create date: <09/15/2015>
-- Description:	<Insert records to [tTerminals] for Redstone channel partner >
-- Jira ID:		<AL-1913>
-- ================================================================================


IF NOT EXISTS(SELECT 1 FROM [dbo].[tTerminals] WHERE [ChannelPartnerPK] = 'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6')

BEGIN

INSERT INTO [dbo].[tTerminals] 
		([TerminalPK],[Name],[MacAddress],[IpAddress],[LocationPK],[NpsTerminalPK],[DTServerCreate],[DTServerLastModified],[ChannelPartnerPK],[DTTerminalCreate],[DTTerminalLastModified])
VALUES	
		(NEWID(),'Redstone','','10.111.109.16','acca94c7-a862-49f6-9b49-e438f1333054',NULL,GETDATE(),GETDATE(),'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6',GETDATE(),NULL)
END	