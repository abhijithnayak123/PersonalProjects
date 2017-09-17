--===========================================================================================
-- Auther:			Ratheesh
-- Date Created:	01/9/2014
-- Description:		Alter table tMGram_Catalog 
--===========================================================================================


IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tMGram_Catalog' AND COLUMN_NAME = 'Keywords')
BEGIN
	ALTER TABLE tMGram_Catalog 
	ALTER COLUMN Keywords [VARCHAR](max) NULL

END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tMGram_Catalog' AND COLUMN_NAME = 'Poe_Svc_Msg_ES_Text')
BEGIN
	ALTER TABLE tMGram_Catalog 
	ALTER COLUMN Poe_Svc_Msg_ES_Text [VARCHAR](max) NULL

END
GO


IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tMGram_Catalog' AND COLUMN_NAME = 'Poe_Svc_Msg_EN_Text')
BEGIN
	ALTER TABLE tMGram_Catalog 
	ALTER COLUMN Poe_Svc_Msg_EN_Text [VARCHAR](max) NULL

END
GO
