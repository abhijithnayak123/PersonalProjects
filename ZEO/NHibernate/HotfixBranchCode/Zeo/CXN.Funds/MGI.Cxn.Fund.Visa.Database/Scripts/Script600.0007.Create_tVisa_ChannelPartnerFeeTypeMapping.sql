
-- ==========================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <11/27/2015>
-- Description:	<DDL script to create tChannelPartnerVisaTypeMapping table>
-- Rally ID:	<AL-1639>
-- ==========================================================================

IF NOT EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID(N'[dbo].[tVisa_ChannelPartnerFeeTypeMapping]') AND TYPE in (N'U'))
BEGIN

CREATE TABLE tVisa_ChannelPartnerFeeTypeMapping
(
	ChannelPartnerFeeTypeMappingPK UNIQUEIDENTIFIER NOT NULL,
	ChannelPartnerFeeTypeId BIGINT IDENTITY(1000000000,1) NOT NULL,
	VisaFeeTypePK UNIQUEIDENTIFIER NOT NULL,
	ChannelPartnerID BIGINT NOT NULL,
	DTServerCreate DATETIME NOT NULL,
	DTServerLastModified DATETIME NULL

CONSTRAINT PK_tVisa_ChannelPartnerFeeTypeMapping PRIMARY KEY CLUSTERED 
(
	ChannelPartnerFeeTypeMappingPK ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

) ON [PRIMARY]

END
GO

-- CREATE the Foreign key reference for VisaFeeTypePK column in tVisa_ChannelPartnerFeeTypeMapping table

IF NOT EXISTS (Select 1 from SYS.FOREIGN_KEYS where Name = 'FK_tVisa_ChannelPartnerFeeTypeMapping_tVisa_FeeTypes' AND TYPE in (N'F'))
BEGIN

ALTER TABLE dbo.tVisa_ChannelPartnerFeeTypeMapping  WITH CHECK ADD  CONSTRAINT FK_tVisa_ChannelPartnerFeeTypeMapping_tVisa_FeeTypes
FOREIGN KEY(VisaFeeTypePK)
REFERENCES dbo.tVisa_FeeTypes (VisaFeeTypePK)

END
GO
