--- ================================================================================
-- Author:		<Abhijith>
-- Create date: <10/28/2015>
-- Description:	<As Alloy, the pricing currently in effect for current clients will continue to exist and will be applied through the pricing engine>
-- Jira ID:		<AL-1760>
-- ================================================================================

----------------------------  For Client - TCF ---------------------------------------

--For Check Cashing
IF NOT EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='CC-TCF-Government')
BEGIN

  INSERT INTO tPricingGroups
  ([PricingGroupPK],[PricingGroupName],[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
  VALUES
  (NEWID(),'CC-TCF-Government', GETDATE(), NULL, GETDATE(), NULL)

END

--For Check Cashing
IF NOT EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='CC-TCF-Payroll')
BEGIN

  INSERT INTO tPricingGroups
  ([PricingGroupPK],[PricingGroupName],[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
  VALUES
  (NEWID(),'CC-TCF-Payroll', GETDATE(), NULL, GETDATE(), NULL)

END


--For Money Order
IF NOT EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='MO-TCF')
BEGIN

  INSERT INTO tPricingGroups
  ([PricingGroupPK],[PricingGroupName],[DTServerCreate],[DTServerLastModified],
	[DTTerminalCreate], [DTTerminalLastModified])
  VALUES
  (NEWID(),'MO-TCF', GETDATE(), NULL, GETDATE(), NULL)

END

----------------------------  For Client - TCF ---------------------------------------


----------------------------  For Client - Synovus ---------------------------------------


--For Check Cashing
IF NOT EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='CC-Synovus-Government')
BEGIN

  INSERT INTO tPricingGroups
  ([PricingGroupPK],[PricingGroupName],[DTServerCreate],[DTServerLastModified],
	[DTTerminalCreate], [DTTerminalLastModified])
  VALUES
  (NEWID(),'CC-Synovus-Government', GETDATE(), NULL, GETDATE(), NULL)

END

--For Check Cashing
IF NOT EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='CC-Synovus-TwoParty')
BEGIN

  INSERT INTO tPricingGroups
  ([PricingGroupPK],[PricingGroupName],[DTServerCreate],[DTServerLastModified],
	[DTTerminalCreate], [DTTerminalLastModified])
  VALUES
  (NEWID(),'CC-Synovus-TwoParty', GETDATE(), NULL, GETDATE(), NULL)

END

--For Check Cashing
IF NOT EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='CC-Synovus-MoneyOrder')
BEGIN

  INSERT INTO tPricingGroups
  ([PricingGroupPK],[PricingGroupName],[DTServerCreate],[DTServerLastModified],
	[DTTerminalCreate], [DTTerminalLastModified])
  VALUES
  (NEWID(),'CC-Synovus-MoneyOrder', GETDATE(), NULL, GETDATE(), NULL)

END


--For Money Order
IF NOT EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='MO-Synovus')
BEGIN

  INSERT INTO tPricingGroups
  ([PricingGroupPK],[PricingGroupName],[DTServerCreate],[DTServerLastModified],
	[DTTerminalCreate], [DTTerminalLastModified])
  VALUES
  (NEWID(),'MO-Synovus', GETDATE(), NULL, GETDATE(), NULL)

END


----------------------------  For Client - Synovus ---------------------------------------

----------------------------  For Client - Carver ---------------------------------------

--For Check Cashing
IF NOT EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='CC-Carver-Government')
BEGIN

  INSERT INTO tPricingGroups
  ([PricingGroupPK],[PricingGroupName],[DTServerCreate],[DTServerLastModified],
	[DTTerminalCreate], [DTTerminalLastModified])
  VALUES
  (NEWID(),'CC-Carver-Government', GETDATE(), NULL, GETDATE(), NULL)

END


--For Money Order
IF NOT EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='MO-Carver')
BEGIN

  INSERT INTO tPricingGroups
  ([PricingGroupPK],[PricingGroupName],[DTServerCreate],[DTServerLastModified],
	[DTTerminalCreate], [DTTerminalLastModified])
  VALUES
  (NEWID(),'MO-Carver', GETDATE(), NULL, GETDATE(), NULL)

END

----------------------------  For Client - Carver ---------------------------------------


----------------------------  For Client - MGI ---------------------------------------

--For Check Cashing
IF NOT EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='CC-MGI-Government')
BEGIN

  INSERT INTO tPricingGroups
  ([PricingGroupPK],[PricingGroupName],[DTServerCreate],[DTServerLastModified],
	[DTTerminalCreate], [DTTerminalLastModified])
  VALUES
  (NEWID(),'CC-MGI-Government', GETDATE(), NULL, GETDATE(), NULL)

END

--For Check Cashing
IF NOT EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='CC-MGI-Payroll')
BEGIN

  INSERT INTO tPricingGroups
  ([PricingGroupPK],[PricingGroupName],[DTServerCreate],[DTServerLastModified],
	[DTTerminalCreate], [DTTerminalLastModified])
  VALUES
  (NEWID(),'CC-MGI-Payroll', GETDATE(), NULL, GETDATE(), NULL)

END

----------------------------  For Client - MGI ---------------------------------------
