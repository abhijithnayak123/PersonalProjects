-- ================================================================================
-- Author:		<Abhijith Nayak>
-- Create date: <11/20/2015>
-- Description:	<As MGiAlloy, I want to remove US territories from the ID country list>
-- Jira ID:		<AL-3035>
-- ================================================================================
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChannelPartnerIDTypeMapping'
		  AND COLUMN_NAME = 'IsActive'
		)
BEGIN
	-- Updating the IsActive to false to not show the countries in the Issuing Country Dropdown.
	UPDATE CPM
	SET CPM.IsActive = 0
	FROM tChannelPartnerIDTypeMapping CPM
		INNER JOIN tChannelPartners cp ON CPM.ChannelPartnerId = cp.ChannelPartnerPK
		INNER JOIN tNexxoIdTypes NT ON CPM.NexxoIdTypeId = NT.NexxoIdTypePK
		INNER JOIN tMasterCountries MC  ON MC.rowguid = NT.CountryPK
	WHERE cp.Name IN ('TCF', 'Carver', 'MGI', 'Redstone')  AND MC.Name IN ('Puerto Rico','Guam','Northern Mariana Islands','American Samoa', 'VIRGIN ISLANDS-US')
	
	-- Updating the IsActive to false to not show the states in the Issuing State Dropdown.
	UPDATE CPM
	SET CPM.IsActive = 0
	FROM tChannelPartnerIDTypeMapping CPM
		INNER JOIN tChannelPartners cp ON CPM.ChannelPartnerId = cp.ChannelPartnerPK
		INNER JOIN tNexxoIdTypes NT ON CPM.NexxoIdTypeId = NT.NexxoIdTypePK
		INNER JOIN tMasterCountries MC  ON MC.rowguid = NT.CountryPK
		INNER JOIN tStates s ON S.StatePK = NT.StatePK
	WHERE cp.Name IN ('TCF', 'Carver', 'MGI', 'Redstone', 'Synovus') AND s.Name IN ('FEDERATED STATES OF MICRONESIA', 'MARSHALL ISLANDS','PALAU')

END
GO