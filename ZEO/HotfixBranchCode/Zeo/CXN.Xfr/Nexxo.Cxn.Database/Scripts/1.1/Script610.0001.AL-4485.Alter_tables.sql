-- ================================================================================
-- Author:		<Rogy Eapen>
-- Create date: <18/01/2016>
-- Description:	<As Engineering, I need to have consistent naming convention for Id
--				and PK columns>
-- Jira ID:		<AL-4485>
-- ================================================================================

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWesternUnionCountryCurrencyDeliveryMethods'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWesternUnionCountryCurrencyDeliveryMethods.id'
		,@newname = 'WUCountryCurrencyDeliveryMethodsID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWesternUnionDeliveryOptions'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWesternUnionDeliveryOptions.id'
		,@newname = 'WUCountryCurrencyDeliveryOptionsID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWesternUnionLogs'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWesternUnionLogs.id'
		,@newname = 'WULogsID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWesternUnionPaymentMethods'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWesternUnionPaymentMethods.id'
		,@newname = 'WUPaymentMethodsID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWesternUnionPickupDetails'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWesternUnionPickupDetails.id'
		,@newname = 'WUPickupDetailsID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWesternUnionPickupMethods'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWesternUnionPickupMethods.id'
		,@newname = 'WUPickupMethodsID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWesternUnionReceiverProfiles'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	ALTER TABLE tWesternUnionReceiverProfiles 
	ALTER COLUMN id 
	DROP ROWGUIDCOL
	
	EXEC sp_rename @objname = 'tWesternUnionReceiverProfiles.id'
		,@newname = 'WUReceiverProfilesID'
		,@objtype = 'COLUMN';

	ALTER TABLE tWesternUnionReceiverProfiles 
	ALTER COLUMN WUReceiverProfilesID 
	ADD ROWGUIDCOL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_NameTypeMapping'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_NameTypeMapping.rowguid'
		,@newname = 'WUNameTypeMappingPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_NameTypeMapping'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_NameTypeMapping.id'
		,@newname = 'WUNameTypeMappingID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_PaymentMethods'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_PaymentMethods.rowguid'
		,@newname = 'WUPaymentMethodsPK'
		,@objtype = 'COLUMN';
END
GO


IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_PaymentMethods'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_PaymentMethods.id'
		,@newname = 'WUPaymentMethodsID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_PickupDetails'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_PickupDetails.rowguid'
		,@newname = 'WUPickupDetailsPK'
		,@objtype = 'COLUMN';
END
GO


IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_PickupDetails'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_PickupDetails.id'
		,@newname = 'WUPickupDetailsID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_PickupMethods'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_PickupMethods.rowguid'
		,@newname = 'WUPickupMethodsPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_PickupMethods'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_PickupMethods.id'
		,@newname = 'WUPickupMethodsID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Recipient_Account'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	ALTER TABLE tWUnion_Recipient_Account 
	ALTER COLUMN rowguid 
	DROP ROWGUIDCOL
	
	EXEC sp_rename @objname = 'tWUnion_Recipient_Account.rowguid'
		,@newname = 'WURecipientAccountPK'
		,@objtype = 'COLUMN';

	ALTER TABLE tWUnion_Recipient_Account 
	ALTER COLUMN WURecipientAccountPK 
	ADD ROWGUIDCOL
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Recipient_Account'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Recipient_Account.id'
		,@newname = 'WURecipientAccountID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Relationships'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Relationships.rowguid'
		,@newname = 'WURelationshipsPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tWUnion_Relationships'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tWUnion_Relationships.id'
		,@newname = 'WURelationshipsID'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tXRecipientProfiles'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	ALTER TABLE tXRecipientProfiles 
	ALTER COLUMN id 
	DROP ROWGUIDCOL
	
	EXEC sp_rename @objname = 'tXRecipientProfiles.id'
		,@newname = 'XRecipientProfilesID'
		,@objtype = 'COLUMN';

	ALTER TABLE tXRecipientProfiles 
	ALTER COLUMN XRecipientProfilesID 
	ADD ROWGUIDCOL
END
GO
