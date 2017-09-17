-- ================================================================================
-- Author:		Kaushik Sakala
-- Create date: 07/27/2015
-- Description:	Store receiver details in transaction tables.
-- =================================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx' AND COLUMN_NAME = 'Address')
BEGIN
    ALTER TABLE [tWUnion_Trx] 
	ADD [Address] NVARCHAR(250) NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx' AND COLUMN_NAME = 'City')
BEGIN
    ALTER TABLE [tWUnion_Trx] 
	ADD City NVARCHAR(200) NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx' AND COLUMN_NAME = 'Country')
BEGIN
    ALTER TABLE [tWUnion_Trx] 
	ADD [Country] NVARCHAR(200) NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx' AND COLUMN_NAME = 'State/Province')
BEGIN
    ALTER TABLE [tWUnion_Trx] 
	ADD [State/Province] NVARCHAR(200) NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx' AND COLUMN_NAME = 'ZipCode')
BEGIN
    ALTER TABLE [tWUnion_Trx] 
	ADD ZipCode NVARCHAR(10) NULL 
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tWUnion_Trx' AND COLUMN_NAME = 'ReceiverPhoneNumber')
BEGIN
    ALTER TABLE [tWUnion_Trx] 
	ADD ReceiverPhoneNumber VARCHAR(20) NULL 
END
GO

---------------- Data Migration -------------------------------------


UPDATE
	tWUnion_Trx
SET
	Address = twr.Address,
	[State/Province]  = twr.[State/Province],
	ReceiverPhoneNumber = twr.PhoneNumber,
	ZipCode = twr.ZipCode,
	City =  twr.City
FROM 
	tWUnion_Trx twt
	INNER JOIN dbo.tWUnion_Receiver twr ON twt.WUReceiverID = twr.WUReceiverID