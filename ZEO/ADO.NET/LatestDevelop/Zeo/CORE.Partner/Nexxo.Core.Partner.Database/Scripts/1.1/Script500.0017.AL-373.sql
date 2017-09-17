--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <May 5th 2015>
-- Description:	<Script to update column names>           
-- Jira ID:	<AL-373>
--===========================================================================================
/*********tAcceptedIdentifications****************/

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tAcceptedIdentifications'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tAcceptedIdentifications.id'
		,@newname = 'AcceptedIdentificationID'
		,@objtype = 'COLUMN'
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tAcceptedIdentifications'
			AND COLUMN_NAME = 'StateId'
		)
BEGIN
	EXEC sp_rename @objname = 'tAcceptedIdentifications.StateId'
		,@newname = 'StatePK'
		,@objtype = 'COLUMN'
END
GO


--2) Adding FK to tStates.StatePK
ALTER TABLE [dbo].[tAcceptedIdentifications]
	WITH CHECK ADD CONSTRAINT [FK_tAcceptedIdentifications_tStates_StatePK] FOREIGN KEY ([StatePK]) REFERENCES [dbo].tStates([StatePK]);
GO

--3) Adding new column CountryPK which will have FK to tCountries. The FK should have been on Country Code but guidelines suggest - no FK on PK.
ALTER TABLE tAcceptedIdentifications ADD CountryPK UNIQUEIDENTIFIER NULL
GO

--4) Updating new column CountryPK based on CountryCode value
UPDATE tAcceptedIdentifications
SET CountryPK = c.CountryPK
FROM tAcceptedIdentifications AI WITH (NOLOCK)
INNER JOIN tCountries c WITH (NOLOCK) ON c.code = AI.CountryCode
GO

--5) Making new column CountryPK not null
ALTER TABLE tAcceptedIdentifications
ALTER COLUMN CountryPK UNIQUEIDENTIFIER NOT NULL
GO

--6)Adding FK to tCountries.CountryPK
ALTER TABLE tAcceptedIdentifications ADD CONSTRAINT FK_tAcceptedIdentifications_tCountries_CountryPK FOREIGN KEY (CountryPK) REFERENCES tCountries (CountryPK)
GO
--7) Adding new column for PK
ALTER TABLE tAcceptedIdentifications ADD AcceptedIdentificationsPK UNIQUEIDENTIFIER NULL
GO
--8) Updating new column for PK
UPDATE tAcceptedIdentifications
SET AcceptedIdentificationsPK = NEWID()
GO
--9) Making new column for PK as not null
ALTER TABLE tAcceptedIdentifications 
ALTER COLUMN AcceptedIdentificationsPK UNIQUEIDENTIFIER NOT NULL
GO


--9) Adding new column as PK 
ALTER TABLE [dbo].[tAcceptedIdentifications] ADD PRIMARY KEY (AcceptedIdentificationsPK);
GO

--1) Adding new column of uniqueidentifier type for PK , to replace AgentID pk. this column will serve both as PK and FK.
ALTER TABLE tAgentAuthentication ADD AgentPK UNIQUEIDENTIFIER NULL
go
--2) Updating AgentPK based on AgentID in tAgentDetails
UPDATE tAgentAuthentication
SET AgentPK = AG.AgentPK
FROM tAgentAuthentication AA WITH (NOLOCK)
INNER JOIN tAgentDetails ag WITH (NOLOCK) ON AG.Agentid = AA.Agentid
go
--3) Making new column not null.

ALTER TABLE tAgentAuthentication
ALTER COLUMN AgentPK UNIQUEIDENTIFIER NOT NULL
go
--4) Dropping old PK constraint on AgentID(int type)
ALTER TABLE [dbo].tAgentAuthentication
DROP CONSTRAINT PK_tAgentAuthentication
go
--5)Adding new PK
ALTER TABLE [dbo].tAgentAuthentication ADD PRIMARY KEY (AgentPK)
go
--6) Making FK to tAgentDetails.AgentPK
ALTER TABLE tAgentAuthentication ADD CONSTRAINT FK_tAgentAuthentication_tAgentDetails_AgentPK FOREIGN KEY (AgentPK) REFERENCES tAgentDetails (AgentPK)
GO

---tAgentLocationMapping
--Adding new column AgentLocationPK of uniqueidentifier type for PK
ALTER TABLE tAgentLocationMapping ADD AgentLocationPK UNIQUEIDENTIFIER NULL;
GO

UPDATE tAgentLocationMapping
SET AgentLocationPK = NEWID();
GO

ALTER TABLE tAgentLocationMapping
ALTER COLUMN AgentLocationPK UNIQUEIDENTIFIER NOT NULL;
GO

ALTER TABLE [dbo].tAgentLocationMapping
DROP CONSTRAINT PK_tAgentLocationMapping;
GO

ALTER TABLE [dbo].tAgentLocationMapping ADD PRIMARY KEY (AgentLocationPK);
GO

--Adding AgentPK column for making it FK to tAgentDetails
ALTER TABLE tAgentLocationMapping ADD AgentPK UNIQUEIDENTIFIER NULL;
GO
UPDATE tAgentLocationMapping
SET AgentPK = ag.AgentPK
FROM tAgentLocationMapping al WITH (NOLOCK)
INNER JOIN tAgentDetails ag WITH (NOLOCK) ON AG.Agentid = Al.Agentid;
GO
ALTER TABLE tAgentLocationMapping
ALTER COLUMN AgentPK UNIQUEIDENTIFIER NOT NULL;
GO
ALTER TABLE tAgentLocationMapping ADD CONSTRAINT FK_tAgentLocationMapping_tAgentDetails_AgentPK FOREIGN KEY (AgentPK) REFERENCES tAgentDetails (AgentPK)
GO

--Adding LocationPK column for making it FK to tLocations
ALTER TABLE tAgentLocationMapping ADD LocationPK UNIQUEIDENTIFIER NULL;
GO
UPDATE tAgentLocationMapping
SET LocationPK = l.LocationPK
FROM tAgentLocationMapping al WITH (NOLOCK)
INNER JOIN tLocations l WITH (NOLOCK) ON l.LocationID = Al.LocationID;
GO
--ALTER TABLE tAgentLocationMapping ALTER COLUMN LocationPK UNIQUEIDENTIFIER NOT NULL;
--not able to update as not null because of location id 1 and 33 present in the table, which lead to null values.

ALTER TABLE tAgentLocationMapping ADD CONSTRAINT FK_tAgentLocationMapping_tLocations_LocationPK FOREIGN KEY (LocationPK) REFERENCES tLocations (LocationPK)
GO

----Adding AgentPK column for making it FK to tAgentDetails
ALTER TABLE tAgentSessions ADD AgentPK UNIQUEIDENTIFIER NULL;
GO

UPDATE tAgentSessions
SET AgentPK = ad.AgentPK
FROM tAgentSessions ags WITH (NOLOCK)
INNER JOIN tAgentDetails ad WITH (NOLOCK) ON CAST(ad.AgentId as nvarchar(50))= Ags.Agentid;
GO

ALTER TABLE tAgentSessions
ALTER COLUMN AgentPK UNIQUEIDENTIFIER NOT NULL;
GO

ALTER TABLE tAgentSessions ADD CONSTRAINT FK_tAgentSessions_tAgentDetails_AgentPK FOREIGN KEY (AgentPK) REFERENCES tAgentDetails (AgentPK)
GO

--Adding a primary key constraint.
ALTER TABLE [dbo].tChannelPartnerConfig ADD PRIMARY KEY (ChannelPartnerPK);
GO

---tCheckTypes : just 8 rows, so not adding uniqueidentifier for PK.
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChannelPartnerGroups'
			AND COLUMN_NAME = 'ChannelPartnerGroupIdPK'
		)
BEGIN
	EXEC sp_rename @objname = 'tChannelPartnerGroups.ChannelPartnerGroupIdPK'
		,@newname = 'ChannelPartnerGroupId'
		,@objtype = 'COLUMN';
END
GO

--Adding uniqueidentifier column to be made pk instead of the current int type(ChannelPartnerGroupIdPK)
ALTER TABLE tChannelPartnerGroups ADD ChannelPartnerGroupPK UNIQUEIDENTIFIER NULL;
GO

UPDATE tChannelPartnerGroups
SET ChannelPartnerGroupPK = NEWID();
GO
ALTER TABLE tChannelPartnerGroups
ALTER COLUMN ChannelPartnerGroupPK UNIQUEIDENTIFIER NOT NULL;
GO
-- certain tables have FK on column ChannelPartnerGroupIdPK, so removing this PK is giving error.
--to rectify, adding uniqueidentified columns to the referring table and they will have a foreifn key on the new PK column
-- ie tChannelPartnerGroups.ChannelPartnerGroupPK

ALTER TABLE [dbo].tPartnerCustomerGroupSettings
DROP CONSTRAINT FK_tPartnerCustomerGroupSettings_tChannelPartnerGroups;
GO
ALTER TABLE tPartnerCustomerGroupSettings ADD ChannelPartnerGroupPK UNIQUEIDENTIFIER NULL;
GO

DISABLE Trigger ALL ON tPartnerCustomerGroupSettings; -- insert/update trigger giving error 
GO
UPDATE tPartnerCustomerGroupSettings
SET ChannelPartnerGroupPK = b.ChannelPartnerGroupPK
FROM tPartnerCustomerGroupSettings a WITH (NOLOCK)
INNER JOIN tChannelPartnerGroups b WITH (NOLOCK) ON b.ChannelPartnerGroupId= a.ChannelPartnerGroupId;
GO
----
ALTER TABLE tPartnerCustomerGroupSettings
ALTER COLUMN ChannelPartnerGroupPK UNIQUEIDENTIFIER NOT NULL;
GO
--doing the same for reffering table tProspectGroupSettings
ALTER TABLE [dbo].tProspectGroupSettings
DROP CONSTRAINT FK_tProspectGroupSettings_tChannelPartnerGroups;
GO

ALTER TABLE tProspectGroupSettings ADD ChannelPartnerGroupPK UNIQUEIDENTIFIER NULL;
GO
UPDATE tProspectGroupSettings
SET ChannelPartnerGroupPK = b.ChannelPartnerGroupPK
FROM tProspectGroupSettings a WITH (NOLOCK)
INNER JOIN tChannelPartnerGroups b WITH (NOLOCK) ON b.ChannelPartnerGroupId= a.ChannelPartnerGroupId;
GO
----

ALTER TABLE tProspectGroupSettings
ALTER COLUMN ChannelPartnerGroupPK UNIQUEIDENTIFIER NOT NULL;
GO

--dropping PK constraint on base table
ALTER TABLE [dbo].tChannelPartnerGroups
DROP CONSTRAINT PK_tChannelPartnerGroups;
GO
--making ChannelPartnerGroupPK the primary key
ALTER TABLE [dbo].tChannelPartnerGroups ADD PRIMARY KEY (ChannelPartnerGroupPK);
GO
--putting back the FK, but to the new PK instead of the old one

ALTER TABLE [dbo].[tPartnerCustomerGroupSettings]  WITH CHECK ADD  CONSTRAINT [FK_tPartnerCustomerGroupSettings_tChannelPartnerGroups_ChannelPartnerGroupPK] FOREIGN KEY([ChannelPartnerGroupPK])
REFERENCES [dbo].[tChannelPartnerGroups] ([ChannelPartnerGroupPK])
GO

ALTER TABLE [dbo].[tProspectGroupSettings]  WITH CHECK ADD  CONSTRAINT [FK_tProspectGroupSettings_tChannelPartnerGroups_ChannelPartnerGroupPK] FOREIGN KEY([ChannelPartnerGroupPK])
REFERENCES [dbo].[tChannelPartnerGroups] ([ChannelPartnerGroupPK])
GO


--for aud table, creating a nullable column, old records will not be updated, update trigger will take care of the new records.

ALTER TABLE tPartnerCustomerGroupSettings_AUD ADD ChannelPartnerGroupPK UNIQUEIDENTIFIER NULL;
GO

----tChannelPartnerSMTPDetails : no data in prod, need more info for FK.
ALTER TABLE tChannelPartnerSMTPDetails ADD ChannelPartnerSMTPPK UNIQUEIDENTIFIER NOT NULL;
GO
ALTER TABLE [dbo].tChannelPartnerSMTPDetails
DROP CONSTRAINT PK_tChannelPartnerSMTPDetails;
ALTER TABLE [dbo].tChannelPartnerSMTPDetails ADD PRIMARY KEY (ChannelPartnerSMTPPK);
GO

--tContactTypes just 5 rows so not putting in PK uniqueidentifier

--tCustomerPreferedProducts: no data in prod, more info needed for FK
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerPreferedProducts'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tCustomerPreferedProducts.rowguid'
		,@newname = 'CustomerProductPK'
		,@objtype = 'COLUMN';
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerPreferedProducts'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tCustomerPreferedProducts.id'
		,@newname = 'CustomerProductID'
		,@objtype = 'COLUMN';
END
GO

---tCustomerSessionCounterIdDetails : need to know why seperate table, customersessionid column from tCustomerSessions, just add counter id?
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tCustomerSessionCounterIdDetails'
			AND COLUMN_NAME = 'CustomerSessionId'
		)
BEGIN
	EXEC sp_rename @objname = 'tCustomerSessionCounterIdDetails.CustomerSessionId'
		,@newname = 'CustomerSessionPK'
		,@objtype = 'COLUMN';
END
GO


----tCustomerSessionShoppingCarts : need to know if uniqueidentifier PK has to be added.

----tFeeAdjustmentCompareTypes Compare type is tinyint-8rows,not changing.


----tLedgerEntries : no data in prod, need more info for FK
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tLedgerEntries'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tLedgerEntries.rowguid'
		,@newname = 'LedgerEntriesPK'
		,@objtype = 'COLUMN';
END 
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tLedgerEntries'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tLedgerEntries.id'
		,@newname = 'LedgerEntriesID'
		,@objtype = 'COLUMN';
END 
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tLedgerEntries'
			AND COLUMN_NAME = 'LedgerTransactionRowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tLedgerEntries.LedgerTransactionRowguid'
		,@newname = 'LedgerTransactionPK'
		,@objtype = 'COLUMN';
END 
GO

----tLedgerTransactions: no data in prod, need more info for FK
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tLedgerTransactions'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tLedgerTransactions.rowguid'
		,@newname = 'LedgerTransactionsPK'
		,@objtype = 'COLUMN';
END 
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tLedgerTransactions'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tLedgerTransactions.id'
		,@newname = 'LedgerTransactionsID'
		,@objtype = 'COLUMN';
END 
GO

---tLocationCounterIdDetails
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tLocationCounterIdDetails'
			AND COLUMN_NAME = 'rowguid'
		)
BEGIN
	EXEC sp_rename @objname = 'tLocationCounterIdDetails.rowguid'
		,@newname = 'LocationCounterIdDetailPK'
		,@objtype = 'COLUMN';
END 
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tLocationCounterIdDetails'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tLocationCounterIdDetails.id'
		,@newname = 'LocationCounterIdDetailID'
		,@objtype = 'COLUMN';
END 
GO

---tNotificationHosts : just 12 rows, not adding uniqueidentifier PK
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tNotificationHosts'
			AND COLUMN_NAME = 'id'
		)
BEGIN
	EXEC sp_rename @objname = 'tNotificationHosts.id'
		,@newname = 'NotificationHostIdPK'
		,@objtype = 'COLUMN';
END 
GO

--after updated trigger
ENABLE Trigger ALL ON tPartnerCustomerGroupSettings;
GO