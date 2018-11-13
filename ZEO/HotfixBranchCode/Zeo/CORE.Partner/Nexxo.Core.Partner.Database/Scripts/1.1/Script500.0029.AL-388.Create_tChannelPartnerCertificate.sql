-- ============================================================
-- Author:		<Bineesh E Raghavan>
-- Create date: <06/01/2015>
-- Description:	<Script to create tChannelPartnerCertificate table>
-- Rally ID:	<AL-388>
-- ============================================================

IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tChannelPartnerCertificate]') AND type in (N'U'))
	DROP TABLE [dbo].[tChannelPartnerCertificate]
GO

CREATE TABLE tChannelPartnerCertificate
(
	ChannelPartnerCertificatePK UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	ChannelPartnerCertificateId BIGINT IDENTITY(1000000000,1) NOT NULL,
	ChannelPartnerPK UNIQUEIDENTIFIER NOT NULL,
	Issuer NVARCHAR(255) NOT NULL,
	ThumbPrint NVARCHAR(255) NOT NULL,
	DTCreate DATETIME  NOT NULL DEFAULT GETDATE(),
	DTLastMod DATETIME NULL,
	CONSTRAINT FK_tChannelPartnerCertificate_tChannelPartners_ChannelPartnerPK FOREIGN KEY (ChannelPartnerPK) REFERENCES tChannelPartners(ChannelPartnerPK)
)
GO
