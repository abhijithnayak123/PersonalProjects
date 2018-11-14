--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <09-02-2016>
-- Description:	 Alter PK and FK constraints for Fee Adjustments table
-- Jira ID:		<AL-7926>
-- ================================================================================

--============ DROP FK constraints in Check Processing Fee related tables related tables======================

IF  EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'FK_tChannelPartnerFeeAdjustments_tChannelPartners') AND parent_object_id = OBJECT_ID(N'tChannelPartnerFeeAdjustments'))
BEGIN
	ALTER TABLE tChannelPartnerFeeAdjustments DROP CONSTRAINT FK_tChannelPartnerFeeAdjustments_tChannelPartners		
END
GO

IF  EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'FK_tCustomerFeeAdjustments_tChannelPartnerFeeAdjustments') AND parent_object_id = OBJECT_ID(N'tCustomerFeeAdjustments'))
BEGIN
	ALTER TABLE tCustomerFeeAdjustments DROP CONSTRAINT FK_tCustomerFeeAdjustments_tChannelPartnerFeeAdjustments		
END
GO

IF  EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'FK_tFeeAdjustmentConditions_tChannelPartnerFeeAdjustments') AND parent_object_id = OBJECT_ID(N'tFeeAdjustmentConditions'))
BEGIN
	ALTER TABLE tFeeAdjustmentConditions DROP CONSTRAINT FK_tFeeAdjustmentConditions_tChannelPartnerFeeAdjustments
	
END
GO


IF  EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'FK_tFeeAdjustmentConditions_tFeeAdjustmentConditionTypes') AND parent_object_id = OBJECT_ID(N'tFeeAdjustmentConditions'))
BEGIN
	ALTER TABLE tFeeAdjustmentConditions DROP CONSTRAINT FK_tFeeAdjustmentConditions_tFeeAdjustmentConditionTypes	
END
GO

IF  EXISTS (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'FK_tTxn_FeeAdjustments_tChannelPartnerFeeAdjustments') AND parent_object_id = OBJECT_ID(N'tTxn_FeeAdjustments'))
BEGIN
	ALTER TABLE tTxn_FeeAdjustments DROP CONSTRAINT FK_tTxn_FeeAdjustments_tChannelPartnerFeeAdjustments	
END
GO

--==============================ADD NEW COLUMN IN tTxn_FeeAdjustments TABLE================================================

-- Add new columns(FeeAdjustmentID, ChannelPartnerId) in tChannelPartnerFeeAdjustments table

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerFeeAdjustments' AND COLUMN_NAME IN ('FeeAdjustmentId', 'ChannelPartnerId'))
BEGIN
	ALTER TABLE tChannelPartnerFeeAdjustments 
	ADD FeeAdjustmentId BIGINT NOT NULL IDENTITY(1000000000,1) 

	ALTER TABLE tChannelPartnerFeeAdjustments 
	ADD ChannelPartnerId INT NULL 
END
GO

-- Add new columns(CustomerFeeAdjustmentID, FeeAdjustmentID) in tCustomerFeeAdjustments table

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomerFeeAdjustments' AND COLUMN_NAME = 'FeeAdjustmentId' )
BEGIN

	ALTER TABLE tCustomerFeeAdjustments 
	ADD FeeAdjustmentId BIGINT NULL 
END
GO

-- Add new columns(AdjConditionsID, FeeAdjustmentID) in tFeeAdjustmentConditions table

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tFeeAdjustmentConditions' AND COLUMN_NAME IN ('AdjConditionsId', 'FeeAdjustmentId'))
BEGIN
	ALTER TABLE tFeeAdjustmentConditions 
	ADD AdjConditionsId BIGINT NOT NULL IDENTITY(1000000000,1) 

	ALTER TABLE tFeeAdjustmentConditions 
	ADD FeeAdjustmentId BIGINT NULL 
END
GO



-- Add new columns(TransactionFeeAdjustmentID, FeeAdjustmentID,TransactionID) in tTxn_FeeAdjustments table

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_FeeAdjustments' AND COLUMN_NAME IN ('TransactionFeeAdjustmentId', 'FeeAdjustmentId', 'TransactionId'))
BEGIN
	ALTER TABLE tTxn_FeeAdjustments 
	ADD TransactionFeeAdjustmentId BIGINT NOT NULL IDENTITY(1000000000,1) 

	ALTER TABLE tTxn_FeeAdjustments 
	ADD FeeAdjustmentId BIGINT NULL 

	ALTER TABLE tTxn_FeeAdjustments 
	ADD TransactionId BIGINT NULL 

END
GO

--=================================================ReName the PK colum to ID column===============================================
----- ReName CompareTypePK to CompareTypeID

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tFeeAdjustmentCompareTypes' AND COLUMN_NAME IN ('CompareTypePK'))
BEGIN
	 EXEC sp_RENAME 'tFeeAdjustmentCompareTypes.CompareTypePK' , 'CompareTypeId' , 'COLUMN'	 
END
GO


-----ReName the  ConditionTypePK to ConditionTypeID

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tFeeAdjustmentConditionTypes' AND COLUMN_NAME IN ('ConditionTypePK'))
BEGIN
	 EXEC sp_RENAME 'tFeeAdjustmentConditionTypes.ConditionTypePK' , 'ConditionTypeId' , 'COLUMN'	 
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tFeeAdjustmentConditions' AND COLUMN_NAME IN ('CompareTypePK','ConditionTypePK'))
BEGIN
	  EXEC sp_RENAME 'tFeeAdjustmentConditions.CompareTypePK' , 'CompareTypeId' , 'COLUMN'
	  EXEC sp_RENAME 'tFeeAdjustmentConditions.ConditionTypePK' , 'ConditionTypeId' , 'COLUMN'
END
GO

--========================================DROP AND ADD THE PK CONSTRAINTS IN Fee related TABLE===========================================


--  Drop the PK constraint(FeeAdjustmentPK - rowguid) and add PK constraint into FeeAdjustmentID Column

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tChannelPartnerFeeAdjustments' AND OBJECT_NAME(OBJECT_ID) = 'PK_tChannelPartnerFeeAdjustments')
BEGIN

	ALTER TABLE tChannelPartnerFeeAdjustments DROP CONSTRAINT PK_tChannelPartnerFeeAdjustments
	ALTER TABLE tChannelPartnerFeeAdjustments ADD  CONSTRAINT PK_tChannelPartnerFeeAdjustments PRIMARY KEY CLUSTERED (FeeAdjustmentId)

END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tCustomerFeeAdjustments' AND OBJECT_NAME(OBJECT_ID) = 'PK_tCustomerFeeAdjustments')
BEGIN

	ALTER TABLE tCustomerFeeAdjustments DROP CONSTRAINT PK_tCustomerFeeAdjustments
	ALTER TABLE tCustomerFeeAdjustments ADD  CONSTRAINT PK_tCustomerFeeAdjustments PRIMARY KEY CLUSTERED (CustomerFeeAdjustmentsId)

END
GO

--  Drop the PK constraint(AdjConditionsPK - rowguid) and add PK constraint into AdjConditionsID Column

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tFeeAdjustmentConditions' AND OBJECT_NAME(OBJECT_ID) = 'PK_tFeeAdjustmentConditions')
BEGIN

	ALTER TABLE tFeeAdjustmentConditions DROP CONSTRAINT PK_tFeeAdjustmentConditions
	ALTER TABLE tFeeAdjustmentConditions ADD  CONSTRAINT PK_tFeeAdjustmentConditions PRIMARY KEY CLUSTERED (AdjConditionsId)
	ALTER TABLE tFeeAdjustmentConditions ALTER COLUMN AdjConditionsPK UNIQUEIDENTIFIER NULL
END
GO

--  Drop the PK constraint(TxnFeeAdjPK - rowguid) and add PK constraint into TransactionFeeAdjustmentID Column

IF EXISTS (SELECT 1 FROM sys.objects WHERE type_desc = 'PRIMARY_KEY_CONSTRAINT' AND OBJECT_NAME(parent_object_id) = 'tTxn_FeeAdjustments' AND OBJECT_NAME(OBJECT_ID) = 'PK_tTxn_FeeAdjustments')
BEGIN

	ALTER TABLE tTxn_FeeAdjustments DROP CONSTRAINT PK_tTxn_FeeAdjustments
	ALTER TABLE tTxn_FeeAdjustments ADD  CONSTRAINT PK_tTxn_FeeAdjustments PRIMARY KEY CLUSTERED (TransactionFeeAdjustmentId)

END
GO



--IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'FK_tChannelPartnerFeeAdjustments_tChannelPartners') AND parent_object_id = OBJECT_ID(N'tChannelPartnerFeeAdjustments'))
--BEGIN

--	ALTER TABLE tChannelPartnerFeeAdjustments  WITH CHECK ADD  CONSTRAINT FK_tChannelPartnerFeeAdjustments_tChannelPartners FOREIGN KEY(ChannelPartnerId)
--	REFERENCES tChannelPartners (ChannelPartnerId)
	
--END
--GO

-- Drop the FK constraint (FeeAdjustmentPK - RowGuid) create the FK constraint for FeeAdjustmentID

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'FK_tCustomerFeeAdjustments_tChannelPartnerFeeAdjustments') AND parent_object_id = OBJECT_ID(N'tCustomerFeeAdjustments'))
BEGIN

	ALTER TABLE tCustomerFeeAdjustments  WITH CHECK ADD  CONSTRAINT FK_tCustomerFeeAdjustments_tChannelPartnerFeeAdjustments FOREIGN KEY(FeeAdjustmentId)
	REFERENCES tChannelPartnerFeeAdjustments (FeeAdjustmentId)
	
END
GO

-- Drop the FK constraint (FeeAdjustmentPK - RowGuid) create the FK constraint for FeeAdjustmentID

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'FK_tFeeAdjustmentConditions_tChannelPartnerFeeAdjustments') AND parent_object_id = OBJECT_ID(N'tFeeAdjustmentConditions'))
BEGIN

    ALTER TABLE tFeeAdjustmentConditions  WITH CHECK ADD  CONSTRAINT FK_tFeeAdjustmentConditions_tChannelPartnerFeeAdjustments FOREIGN KEY(FeeAdjustmentId)
	REFERENCES tChannelPartnerFeeAdjustments (FeeAdjustmentId)
	
END
GO

-- Drop the FK constraint (FeeAdjustmentPK - RowGuid) create the FK constraint for FeeAdjustmentID

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'FK_tTxn_FeeAdjustments_tChannelPartnerFeeAdjustments') AND parent_object_id = OBJECT_ID(N'tTxn_FeeAdjustments'))
BEGIN
	
	ALTER TABLE tTxn_FeeAdjustments  WITH CHECK ADD  CONSTRAINT FK_tTxn_FeeAdjustments_tChannelPartnerFeeAdjustments FOREIGN KEY(FeeAdjustmentId)
	REFERENCES tChannelPartnerFeeAdjustments (FeeAdjustmentId)
	
END
GO

--- Making all column to NULLABLE

IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_FeeAdjustments' AND COLUMN_NAME = 'TxnFeeAdjPK')
BEGIN
	ALTER TABLE tTxn_FeeAdjustments ALTER COLUMN TxnFeeAdjPK UNIQUEIDENTIFIER NULL 
END
GO

IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_FeeAdjustments' AND COLUMN_NAME = 'TxnPK')
BEGIN
	ALTER TABLE tTxn_FeeAdjustments ALTER COLUMN TxnPK UNIQUEIDENTIFIER NULL 
END
GO

IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tTxn_FeeAdjustments' AND COLUMN_NAME = 'FeeAdjustmentPK')
BEGIN
	ALTER TABLE tTxn_FeeAdjustments ALTER COLUMN FeeAdjustmentPK UNIQUEIDENTIFIER NULL 
END
GO

IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomerFeeAdjustments' AND COLUMN_NAME = 'CustomerFeeAdjustmentsPK')
BEGIN
	ALTER TABLE tCustomerFeeAdjustments ALTER COLUMN CustomerFeeAdjustmentsPK UNIQUEIDENTIFIER NULL 
END
GO

IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomerFeeAdjustments' AND COLUMN_NAME = 'FeeAdjustmentPK')
BEGIN
	ALTER TABLE tCustomerFeeAdjustments ALTER COLUMN FeeAdjustmentPK UNIQUEIDENTIFIER NULL 
END
GO

IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tFeeAdjustmentConditions' AND COLUMN_NAME = 'FeeAdjustmentPK')
BEGIN
	ALTER TABLE tFeeAdjustmentConditions ALTER COLUMN FeeAdjustmentPK UNIQUEIDENTIFIER NULL 
END
GO
