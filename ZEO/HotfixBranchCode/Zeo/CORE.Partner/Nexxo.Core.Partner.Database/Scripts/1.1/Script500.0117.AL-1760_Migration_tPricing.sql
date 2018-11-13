--- ================================================================================
-- Author:		<Abhijith>
-- Create date: <10/28/2015>
-- Description:	<As Alloy, the pricing currently in effect for current clients will continue to exist and will be applied through the pricing engine>
-- Jira ID:		<AL-1760>
-- ================================================================================

DECLARE @pricingGroupPK UNIQUEIDENTIFIER

----------------------------  For Client - TCF ---------------------------------------

--For Check Cashing
IF EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='CC-TCF-Government')
BEGIN
  
  SET @pricingGroupPK = 
  (	
	 SELECT PricingGroupPK
	 FROM tPricingGroups
	 WHERE PricingGroupName='CC-TCF-Government'
  )

  INSERT INTO tPricing
  ([PricingPK],[PricingGroupPK],[CompareTypePK],[MinimumAmount],[MaximumAmount],
	[MinimumFee],[Value], [IsPercentage], [DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
  VALUES
  (NEWID(),@pricingGroupPK, NULL, NULL, NULL, 1, 1, 1, GETDATE(), NULL, GETDATE(), NULL)

END

--For Check Cashing
IF EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='CC-TCF-Payroll')
BEGIN
	
  SET @pricingGroupPK = 
  (	
	 SELECT PricingGroupPK
	 FROM tPricingGroups
	 WHERE PricingGroupName='CC-TCF-Payroll'
  )

  INSERT INTO tPricing
  ([PricingPK],[PricingGroupPK],[CompareTypePK],[MinimumAmount],[MaximumAmount],
	[MinimumFee],[Value], [IsPercentage], [DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
  VALUES
  (NEWID(),@pricingGroupPK, NULL, NULL, NULL, 1, 3, 1, GETDATE(), NULL, GETDATE(), NULL)
 
END


--For Money Order
IF EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='MO-TCF')
BEGIN

  SET @pricingGroupPK = 
  (	
	 SELECT PricingGroupPK
	 FROM tPricingGroups
	 WHERE PricingGroupName='MO-TCF'
  )

  INSERT INTO tPricing
  ([PricingPK],[PricingGroupPK],[CompareTypePK],[MinimumAmount],[MaximumAmount],
	[MinimumFee],[Value], [IsPercentage], [DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
  VALUES
  (NEWID(),@pricingGroupPK, NULL, NULL, NULL, NULL, 5, 0, GETDATE(), NULL, GETDATE(), NULL)
 

END

----------------------------  For Client - TCF ---------------------------------------


----------------------------  For Client - Synovus ---------------------------------------


--For Check Cashing
IF EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='CC-Synovus-Government')
BEGIN

  SET @pricingGroupPK = 
  (	
	 SELECT PricingGroupPK
	 FROM tPricingGroups
	 WHERE PricingGroupName='CC-Synovus-Government'
  )

  INSERT INTO tPricing
  ([PricingPK],[PricingGroupPK],[CompareTypePK],[MinimumAmount],[MaximumAmount],
	[MinimumFee],[Value], [IsPercentage], [DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
  VALUES
  (NEWID(),@pricingGroupPK, NULL, NULL, NULL, 1, 1.5, 1, GETDATE(), NULL, GETDATE(), NULL)

END

--For Check Cashing
IF EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='CC-Synovus-TwoParty')
BEGIN

  SET @pricingGroupPK = 
  (	
	 SELECT PricingGroupPK
	 FROM tPricingGroups
	 WHERE PricingGroupName='CC-Synovus-TwoParty'
  )

  INSERT INTO tPricing
  ([PricingPK],[PricingGroupPK],[CompareTypePK],[MinimumAmount],[MaximumAmount],
	[MinimumFee],[Value], [IsPercentage], [DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
  VALUES
  (NEWID(),@pricingGroupPK, NULL, NULL, NULL, 1, 2.9, 1, GETDATE(), NULL, GETDATE(), NULL)

END

--For Check Cashing
IF EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='CC-Synovus-MoneyOrder')
BEGIN

  SET @pricingGroupPK = 
  (	
	 SELECT PricingGroupPK
	 FROM tPricingGroups
	 WHERE PricingGroupName='CC-Synovus-MoneyOrder'
  )

  INSERT INTO tPricing
  ([PricingPK],[PricingGroupPK],[CompareTypePK],[MinimumAmount],[MaximumAmount],
	[MinimumFee],[Value], [IsPercentage], [DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
  VALUES
  (NEWID(),@pricingGroupPK, NULL, NULL, NULL, 1, 4.9, 1, GETDATE(), NULL, GETDATE(), NULL)

END


--For Money Order
IF EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='MO-Synovus')
BEGIN

 
  SET @pricingGroupPK = 
  (	
	 SELECT PricingGroupPK
	 FROM tPricingGroups
	 WHERE PricingGroupName='MO-Synovus'
  )

  INSERT INTO tPricing
  ([PricingPK],[PricingGroupPK],[CompareTypePK],[MinimumAmount],[MaximumAmount],
	[MinimumFee],[Value], [IsPercentage], [DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
  VALUES
  (NEWID(),@pricingGroupPK, NULL, NULL, NULL, NULL, 1, 0, GETDATE(), NULL, GETDATE(), NULL)


END


----------------------------  For Client - Synovus ---------------------------------------

----------------------------  For Client - Carver ---------------------------------------

--For Check Cashing
IF EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='CC-Carver-Government')
BEGIN

  SET @pricingGroupPK = 
  (	
	 SELECT PricingGroupPK
	 FROM tPricingGroups
	 WHERE PricingGroupName='CC-Carver-Government'
  )

  INSERT INTO tPricing
  ([PricingPK],[PricingGroupPK],[CompareTypePK],[MinimumAmount],[MaximumAmount],
	[MinimumFee],[Value], [IsPercentage], [DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
  VALUES
  (NEWID(),@pricingGroupPK, NULL, NULL, NULL, 2, 1.86, 1, GETDATE(), NULL, GETDATE(), NULL)


END


--For Money Order
IF EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='MO-Carver')
BEGIN

  SET @pricingGroupPK = 
  (	
	 SELECT PricingGroupPK
	 FROM tPricingGroups
	 WHERE PricingGroupName='MO-Carver'
  )

  INSERT INTO tPricing
  ([PricingPK],[PricingGroupPK],[CompareTypePK],[MinimumAmount],[MaximumAmount],
	[MinimumFee],[Value], [IsPercentage], [DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
  VALUES
  (NEWID(),@pricingGroupPK, NULL, NULL, NULL, NULL, 0.5, 0, GETDATE(), NULL, GETDATE(), NULL)

END

----------------------------  For Client - Carver ---------------------------------------


----------------------------  For Client - MGI ---------------------------------------

--For Check Cashing
IF EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='CC-MGI-Government')
BEGIN

 SET @pricingGroupPK = 
  (	
	 SELECT PricingGroupPK
	 FROM tPricingGroups
	 WHERE PricingGroupName='CC-MGI-Government'
  )

  INSERT INTO tPricing
  ([PricingPK],[PricingGroupPK],[CompareTypePK],[MinimumAmount],[MaximumAmount],
	[MinimumFee],[Value], [IsPercentage], [DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
  VALUES
  (NEWID(),@pricingGroupPK, NULL, NULL, NULL, 0, 1, 1, GETDATE(), NULL, GETDATE(), NULL)

END

--For Check Cashing
IF EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='CC-MGI-Payroll')
BEGIN

   SET @pricingGroupPK = 
  (	
	 SELECT PricingGroupPK
	 FROM tPricingGroups
	 WHERE PricingGroupName='CC-MGI-Payroll'
  )

  INSERT INTO tPricing
  ([PricingPK],[PricingGroupPK],[CompareTypePK],[MinimumAmount],[MaximumAmount],
	[MinimumFee],[Value], [IsPercentage], [DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
  VALUES
  (NEWID(),@pricingGroupPK, NULL, NULL, NULL, 0, 3, 1, GETDATE(), NULL, GETDATE(), NULL)

END

----------------------------  For Client - MGI ---------------------------------------
