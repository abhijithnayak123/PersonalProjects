--- ===============================================================================
-- Author     :	 M.Purna Pushkal
-- Description:  Inserting the RBS details for the certain locations
-- Creatd Date:  12-02-2018
-- Story Id   :  B-12630
-- ================================================================================

DECLARE @stateCode NVARCHAR(10)


SELECT
@stateCode = Abbr 
FROM tStates
WHERE Name = 'Minnesota'
AND CountryCode = 840 --US country Code

IF NOT EXISTS(SELECT 1 FROM tSupportInformation WHERE StateCode = @stateCode)
BEGIN
	INSERT INTO tSupportInformation
	(
	    StateCode,
	    Email,
	    Phone1,
	    Phone2,
	    DTServerCreate
	)
	VALUES
	(
	    @stateCode, 
	    N'mnbbs@tcfbank.com', 
	    N'763-337-7888', 
	    NULL, 
		GETDATE()
	) 
END

SELECT
@stateCode = Abbr 
FROM tStates
WHERE Name = 'South Dakota'
AND CountryCode = 840 --US country Code

IF NOT EXISTS(SELECT 1 FROM tSupportInformation WHERE StateCode = @stateCode)
BEGIN
	INSERT INTO tSupportInformation
	(
	    StateCode,
	    Email,
	    Phone1,
	    Phone2,
	    DTServerCreate
	)
	VALUES
	(
	    @stateCode, 
	    N'mnbbs@tcfbank.com', 
	    N'763-337-7888', 
	    NULL, 
		GETDATE()
	) 
END

SELECT
@stateCode = Abbr 
FROM tStates
WHERE Name = 'Michigan'
AND CountryCode = 840 --US country Code

IF NOT EXISTS(SELECT 1 FROM tSupportInformation WHERE StateCode = @stateCode)
BEGIN
	INSERT INTO tSupportInformation
	(
	    StateCode,
	    Email,
	    Phone1,
	    Phone2,
	    DTServerCreate
	)
	VALUES
	(
	    @stateCode, 
	    N'mibss@tcfbank.com', 
	    N'734-542-2835/2837/2834/2830', 
	    NULL, 
		GETDATE()
	) 
END

SELECT
@stateCode = Abbr 
FROM tStates
WHERE Name = 'Illinois'
AND CountryCode = 840 --US country Code

IF NOT EXISTS(SELECT 1 FROM tSupportInformation WHERE StateCode = @stateCode)
BEGIN
	INSERT INTO tSupportInformation
	(
	    StateCode,
	    Email,
	    Phone1,
	    Phone2,
	    DTServerCreate
	)
	VALUES
	(
	    @stateCode, 
	    N'osdeptlk@tcfbank.com', 
	    N'630-986-4410', 
	    NULL, 
		GETDATE()
	) 
END

SELECT
@stateCode = Abbr 
FROM tStates
WHERE Name = 'Wisconsin'
AND CountryCode = 840 --US country Code

IF NOT EXISTS(SELECT 1 FROM tSupportInformation WHERE StateCode = @stateCode)
BEGIN
	INSERT INTO tSupportInformation
	(
	    StateCode,
	    Email,
	    Phone1,
	    Phone2,
	    DTServerCreate
	)
	VALUES
	(
	    @stateCode, 
	    N'osdeptlk@tcfbank.com', 
	    N'630-986-4410', 
	    NULL, 
		GETDATE()
	) 
END


SELECT
@stateCode = Abbr 
FROM tStates
WHERE Name = 'Colorado'
AND CountryCode = 840 --US country Code

IF NOT EXISTS(SELECT 1 FROM tSupportInformation WHERE StateCode = @stateCode)
BEGIN
	INSERT INTO tSupportInformation
	(
	    StateCode,
	    Email,
	    Phone1,
	    Phone2,
	    DTServerCreate
	)
	VALUES
	(
	    @stateCode, 
	    N'cosdeptm@tcfbank.com', 
	    N'720-200-2535', 
	    NULL, 
		GETDATE()
	) 
END

SELECT
@stateCode = Abbr 
FROM tStates
WHERE Name = 'Arizona'
AND CountryCode = 840 --US country Code

IF NOT EXISTS(SELECT 1 FROM tSupportInformation WHERE StateCode = @stateCode)
BEGIN
	INSERT INTO tSupportInformation
	(
	    StateCode,
	    Email,
	    Phone1,
	    Phone2,
	    DTServerCreate
	)
	VALUES
	(
	    @stateCode, 
	    N'cosdeptm@tcfbank.com', 
	    N'720-200-2535', 
	    NULL, 
		GETDATE()
	) 
END