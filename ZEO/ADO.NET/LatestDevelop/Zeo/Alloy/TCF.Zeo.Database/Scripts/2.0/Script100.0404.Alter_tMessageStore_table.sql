--- ===============================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <14-03-2017>
-- Description:	Alter tMessageStore
-- ================================================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tMessageStore' AND COLUMN_NAME = 'MessageStorePK' AND COLUMNPROPERTY(OBJECT_ID('dbo.tMessageStore'), 'MessageStorePK', 'IsRowGuidCol') = 1)
BEGIN
    ALTER TABLE tMessageStore
	ALTER COLUMN MessageStorePK DROP ROWGUIDCOL
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tMessageStore' AND COLUMN_NAME = 'MessageStorePK')
BEGIN
    ALTER TABLE tMessageStore
	ALTER COLUMN MessageStorePK UNIQUEIDENTIFIER NULL
END
GO

----- Rename PartnerPK to ChannelPartnerId
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tMessageStore' AND COLUMN_NAME = 'PartnerPK' )
BEGIN
	 EXEC SP_RENAME 'tMessageStore.PartnerPK','ChannelPartnerId','COLUMN'
END
GO 