-- ============================================================
-- Author:		<Bineesh E Raghavan>
-- Create date: <06/01/2015>
-- Description:	<Script to insert records to tChannelPartnerCertificate table>
-- Rally ID:	<AL-388>
-- ============================================================

IF NOT EXISTS (SELECT 1 FROM tChannelPartnerCertificate)
BEGIN
	INSERT tChannelPartnerCertificate (ChannelPartnerCertificatePK, ChannelPartnerPK, Issuer, ThumbPrint, DTCreate) VALUES 
	(NEWID(), '6D7E785F-7BDD-42C8-BC49-44536A1885FC', 'TCF-Okta', '8BE85F904A293BD4463BB98F18BDA5E33B6A9883', GETDATE()),
	(NEWID(), '578AC8FB-F69C-4DBD-A502-57B1EECD41D6', 'Carver-Okta', '8BE85F904A293BD4463BB98F18BDA5E33B6A9883', GETDATE()),
	(NEWID(), 'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17', 'Synovus-Okta', '8BE85F904A293BD4463BB98F18BDA5E33B6A9883', GETDATE()),
	(NEWID(), '10F2865B-DBC5-4A0B-983C-62E0A0574354', 'MGI-Okta', '8BE85F904A293BD4463BB98F18BDA5E33B6A9883', GETDATE()),
	(NEWID(), 'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6', 'Redstone-Okta', '8BE85F904A293BD4463BB98F18BDA5E33B6A9883', GETDATE())
END
GO