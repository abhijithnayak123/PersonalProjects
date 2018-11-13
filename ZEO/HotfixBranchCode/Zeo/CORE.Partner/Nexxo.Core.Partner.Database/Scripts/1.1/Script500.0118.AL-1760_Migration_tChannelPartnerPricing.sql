--- ================================================================================
-- Author:		<Abhijith>
-- Create date: <10/28/2015>
-- Description:	<As Alloy, the pricing currently in effect for current clients will continue to exist and will be applied through the pricing engine>
-- Jira ID:		<AL-1760>
-- ================================================================================

DECLARE @pricingGroupPK UNIQUEIDENTIFIER
DECLARE @channelPartnerPK UNIQUEIDENTIFIER
DECLARE @productPK UNIQUEIDENTIFIER

----------------------------  For Client - TCF ---------------------------------------
    SET @channelPartnerPK = 
	(
		SELECT ChannelPartnerPK
		FROM tChannelPartners
		WHERE ChannelPartnerId = 34
	)
	
	----------------------------  For Check Cashing - TCF --------------------------------

	
	SET @productPK = 
	(
		SELECT rowguid
		FROM tProducts
		WHERE Name = 'ProcessCheck'
	)

	--For Check Cashing
	IF EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='CC-TCF-Government')
	BEGIN
	  
	  SET @pricingGroupPK = 
	  (	
		 SELECT PricingGroupPK
		 FROM tPricingGroups
		 WHERE PricingGroupName='CC-TCF-Government'
	  )

	  -- Printed Payroll	
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified],[DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 7, GETDATE(), NULL, GETDATE(), NULL)
	  
	  --Government
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 3, GETDATE(), NULL, GETDATE(), NULL)
	  
	   --US Treasury
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 2, GETDATE(), NULL, GETDATE(), NULL)

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

	  -- Handwritten Payroll
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 6, GETDATE(), NULL, GETDATE(), NULL)
	  
	   -- Loan/RAL
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 14, GETDATE(), NULL, GETDATE(), NULL)
	  
	  -- Money Order
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 5, GETDATE(), NULL, GETDATE(), NULL)
	  
	  -- Two Party
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 10, GETDATE(), NULL, GETDATE(), NULL)
	  
	   -- Ins/Attorney/Cashiers
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 1, GETDATE(), NULL, GETDATE(), NULL)
	 
	END

	----------------------------  For Check Cashing - TCF --------------------------------
	
	----------------------------  For Money Order - TCF --------------------------------

    SET @productPK = 
	(
		SELECT rowguid
		FROM tProducts
		WHERE Name = 'MoneyOrder'
	)

	--For Money Order
	IF EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='MO-TCF')
	BEGIN

	  SET @pricingGroupPK = 
	  (	
		 SELECT PricingGroupPK
		 FROM tPricingGroups
		 WHERE PricingGroupName='MO-TCF'
	  )

	   INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, NULL, GETDATE(), NULL, GETDATE(), NULL)
	 

	END

	----------------------------  For Money Order - TCF --------------------------------

----------------------------  For Client - TCF ---------------------------------------



----------------------------  For Client - Synovus ---------------------------------------

SET @channelPartnerPK = 
(
	SELECT ChannelPartnerPK
	FROM tChannelPartners
	WHERE ChannelPartnerId = 33
)

	----------------------------  For Check Cashing - Synovus --------------------------------
	SET @productPK = 
	(
		SELECT rowguid
		FROM tProducts
		WHERE Name = 'ProcessCheck'
	)

	--For Check Cashing
	IF EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='CC-Synovus-Government')
	BEGIN

	  SET @pricingGroupPK = 
	  (	
		 SELECT PricingGroupPK
		 FROM tPricingGroups
		 WHERE PricingGroupName='CC-Synovus-Government'
	  )

	  -- Government	
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 3, GETDATE(), NULL, GETDATE(), NULL)
	  
	  -- Printed Payroll	
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 7, GETDATE(), NULL, GETDATE(), NULL)
	  
	  -- US Treasury	
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 2, GETDATE(), NULL, GETDATE(), NULL)

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

	  -- Two Party	
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 10, GETDATE(), NULL, GETDATE(), NULL)
	  
	   -- Loan/RAL
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 14, GETDATE(), NULL, GETDATE(), NULL)
	  
	   -- Ins/Attorney/Cashiers
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 1, GETDATE(), NULL, GETDATE(), NULL)
	  
	    -- Handwritten Payroll
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 6, GETDATE(), NULL, GETDATE(), NULL)

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

	    -- Money Order
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 5, GETDATE(), NULL, GETDATE(), NULL)
	
	END

----------------------------  For Check Cashing - Synovus --------------------------------

----------------------------  For Money Order - Synovus --------------------------------
	SET @productPK = 
	(
		SELECT rowguid
		FROM tProducts
		WHERE Name = 'MoneyOrder'
	)


	--For Money Order
	IF EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='MO-Synovus')
	BEGIN

	 
	  SET @pricingGroupPK = 
	  (	
		 SELECT PricingGroupPK
		 FROM tPricingGroups
		 WHERE PricingGroupName='MO-Synovus'
	  )

	INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, NULL, GETDATE(), NULL, GETDATE(), NULL)


	END

----------------------------  For Money Order - Synovus --------------------------------

----------------------------  For Client - Synovus ---------------------------------------



----------------------------  For Client - Carver ---------------------------------------
 SET @channelPartnerPK = 
 (
	SELECT ChannelPartnerPK
	FROM tChannelPartners
	WHERE ChannelPartnerId = 28
 )


	----------------------------  For Check Cashing - Carver --------------------------------
	
	SET @productPK = 
	(
		SELECT rowguid
		FROM tProducts
		WHERE Name = 'ProcessCheck'
	)
	
	--For Check Cashing
	IF EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='CC-Carver-Government')
	BEGIN

	  SET @pricingGroupPK = 
	  (	
		 SELECT PricingGroupPK
		 FROM tPricingGroups
		 WHERE PricingGroupName='CC-Carver-Government'
	  )

	  -- Money Order	
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 5, GETDATE(), NULL, GETDATE(), NULL)

	   -- Loan/RAL	
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 14, GETDATE(), NULL, GETDATE(), NULL)
	   
	   -- Government
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 3, GETDATE(), NULL, GETDATE(), NULL) 
	  
	   -- Handwritten Payroll
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 6, GETDATE(), NULL, GETDATE(), NULL) 
	  
	   -- Ins/Attorney/Cashiers
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 1, GETDATE(), NULL, GETDATE(), NULL) 
	  
	   -- US Treasury
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 2, GETDATE(), NULL, GETDATE(), NULL)
	   
	   -- Printed Payroll
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 7, GETDATE(), NULL, GETDATE(), NULL) 
	  
	   -- Two Party
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 10, GETDATE(), NULL, GETDATE(), NULL)
	END

----------------------------  For Check Cashing - Carver --------------------------------

----------------------------  For Money Order - Carver --------------------------------
	SET @productPK = 
	(
		SELECT rowguid
		FROM tProducts
		WHERE Name = 'MoneyOrder'
	)

	--For Money Order
	IF EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='MO-Carver')
	BEGIN

	  SET @pricingGroupPK = 
	  (	
		 SELECT PricingGroupPK
		 FROM tPricingGroups
		 WHERE PricingGroupName='MO-Carver'
	  )

	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, NULL, GETDATE(), NULL, GETDATE(), NULL)

	END

----------------------------  For Money Order - Carver --------------------------------

----------------------------  For Client - Carver ---------------------------------------




----------------------------  For Client - MGI ---------------------------------------

SET @channelPartnerPK = 
(
	SELECT ChannelPartnerPK
	FROM tChannelPartners
	WHERE ChannelPartnerId = 1
)

	----------------------------  For Check Cashing - MGI --------------------------------
   SET @productPK = 
   (
		SELECT rowguid
		FROM tProducts
		WHERE Name = 'ProcessCheck'
   )


	--For Check Cashing
	IF EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='CC-MGI-Government')
	BEGIN

	 SET @pricingGroupPK = 
	  (	
		 SELECT PricingGroupPK
		 FROM tPricingGroups
		 WHERE PricingGroupName='CC-MGI-Government'
	  )

	  -- Government
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 3, GETDATE(), NULL, GETDATE(), NULL)
	  
	  -- US Treasury
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 2, GETDATE(), NULL, GETDATE(), NULL)
	  
	   -- Printed Payroll
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 7, GETDATE(), NULL, GETDATE(), NULL)

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

	   -- Handwritten Payroll
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 6, GETDATE(), NULL, GETDATE(), NULL)
	  
	   -- Loan/RAL
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 14, GETDATE(), NULL, GETDATE(), NULL)
	  
	    -- Two Party
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 10, GETDATE(), NULL, GETDATE(), NULL)
	  
	   -- Ins/Attorney/Cashiers
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 1, GETDATE(), NULL, GETDATE(), NULL)
	  
	   -- Money Order
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 5, GETDATE(), NULL, GETDATE(), NULL)
	  
	END

	----------------------------  For Check Cashing - MGI --------------------------------
	
	----------------------------  For Money Order - MGI ----------------------------------
	
    SET @productPK = 
	(
		SELECT rowguid
		FROM tProducts
		WHERE Name = 'MoneyOrder'
	)

	--For Money Order
	IF EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='MO-TCF')
	BEGIN

	  SET @pricingGroupPK = 
	  (	
		 SELECT PricingGroupPK
		 FROM tPricingGroups
		 WHERE PricingGroupName='MO-TCF'
	  )

	   INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, NULL, GETDATE(), NULL, GETDATE(), NULL)
	 

	END
	
	----------------------------  For Money Order - MGI ----------------------------------

----------------------------  For Client - MGI ---------------------------------------



----------------------------  For Client - RedStone ---------------------------------------

SET @channelPartnerPK = 
(
	SELECT ChannelPartnerPK
	FROM tChannelPartners
	WHERE ChannelPartnerId = 35
)

	----------------------------  For Check Cashing - RedStone --------------------------------
   SET @productPK = 
   (
		SELECT rowguid
		FROM tProducts
		WHERE Name = 'ProcessCheck'
   )


	--For Check Cashing
	IF EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='CC-MGI-Government')
	BEGIN

	 SET @pricingGroupPK = 
	  (	
		 SELECT PricingGroupPK
		 FROM tPricingGroups
		 WHERE PricingGroupName='CC-MGI-Government'
	  )

	  -- Two Party
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 10, GETDATE(), NULL, GETDATE(), NULL)
	  
	  -- Loan/RAL
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 14, GETDATE(), NULL, GETDATE(), NULL)
	  
	   -- US Treasury
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 2, GETDATE(), NULL, GETDATE(), NULL)
	  
	   -- Government
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 3, GETDATE(), NULL, GETDATE(), NULL)
	  
	   -- Printed Payroll
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 7, GETDATE(), NULL, GETDATE(), NULL)


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

	   -- Money Order
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 5, GETDATE(), NULL, GETDATE(), NULL)
	  
	   -- Handwritten Payroll
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 6, GETDATE(), NULL, GETDATE(), NULL)
	  
	   -- Ins/Attorney/Cashiers
	  INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, 1, GETDATE(), NULL, GETDATE(), NULL)
	  

	END

	----------------------------  For Check Cashing - RedStone --------------------------------
	
	----------------------------  For Money Order - RedStone ----------------------------------
	
    SET @productPK = 
	(
		SELECT rowguid
		FROM tProducts
		WHERE Name = 'MoneyOrder'
	)

	--For Money Order
	IF EXISTS(SELECT 1 FROM tPricingGroups WHERE PricingGroupName='MO-TCF')
	BEGIN

	  SET @pricingGroupPK = 
	  (	
		 SELECT PricingGroupPK
		 FROM tPricingGroups
		 WHERE PricingGroupName='MO-TCF'
	  )

	   INSERT INTO tChannelPartnerPricing
	  ([ChannelPartnerPricingPK],[PricingGroupPK],[ChannelPartnerPK],[LocationPK],[ProductPK],[ProductType],
		[DTServerCreate],[DTServerLastModified], [DTTerminalCreate], [DTTerminalLastModified])
	  VALUES
	  (NEWID(),@pricingGroupPK, @channelPartnerPK, NULL, @productPK, NULL, GETDATE(), NULL, GETDATE(), NULL)
	 

	END
	
	----------------------------  For Money Order - RedStone ----------------------------------