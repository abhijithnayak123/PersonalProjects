--===========================================================================================
-- Author:		SwarnaLakshmi S
-- Create date: Feb 27th 2015
-- Description:	<Script for updating ProviderIds in tAccounts table >
-- Jira ID:	AL-95
--===========================================================================================
--
--Fund ProviderIds
--FirstView = 101, TSys = 102,Visa = 103

--ChannelPartnerIds
-- TCF	6D7E785F-7BDD-42C8-BC49-44536A1885FC
-- Carver	578AC8FB-F69C-4DBD-A502-57B1EECD41D6
-- MGI	10F2865B-DBC5-4A0B-983C-62E0A0574354
-- Synovus	EC6AAAE3-2BA7-4E0B-898E-D296DB432C17
-- Centris	2D1CE3EE-C2AD-4EAE-9926-D35E2CC76372


-- Update TCF ChannelPartner Fund Account with ProviderID Visa 103
UPDATE tAcct Set tAcct.ProviderId = 103 
From tAccounts tAcct inner join tPartnerCustomers tPartCust
on tAcct.CustomerPK = tPartCust.rowguid 
Where tPartCust.ChannelPartnerId = '6D7E785F-7BDD-42C8-BC49-44536A1885FC' and ProviderId=101


-- Update Carver ChannelPartner Fund Account with ProviderID Tsys 102
UPDATE tAcct set tAcct.ProviderId = 102
From tAccounts tAcct inner join tPartnerCustomers tPartCust
on tAcct.CustomerPK = tPartCust.rowguid 
Where tPartCust.ChannelPartnerId = '578AC8FB-F69C-4DBD-A502-57B1EECD41D6' and ProviderId=101

-- Update Synovus ChannelPartner Fund Account with ProviderID Tsys 102
Update tAcct set tAcct.ProviderId = 102
From tAccounts tAcct inner join tPartnerCustomers tPartCust
on tAcct.CustomerPK = tPartCust.rowguid 
Where tPartCust.ChannelPartnerId = 'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17' and ProviderId=101


-- Update Centris ChannelPartner Fund Account with ProviderID FirstView 101
Update tAcct set tacct.ProviderId = 101
From tAccounts tAcct inner join tPartnerCustomers tPartCust
on tAcct.CustomerPK = tPartCust.rowguid 
Where tPartCust.ChannelPartnerId = '2D1CE3EE-C2AD-4EAE-9926-D35E2CC76372' and ProviderId=101

GO
--===========================================================================================