--==========================================================================
-- Author: <Kaushik S>
-- Date Created: <April 20 2015>
-- Description: <As an Alloy user, I want to see 3 separate Customer 
--              profile states named 'Active', 'Inactive' and ‘Closed’>
-- User Story ID: <AL-229>
--===========================================================================
IF EXISTS(SELECT * FROM sys.columns WHERE Name = N'ProfileStatus' AND Object_ID = Object_ID(N'tCustomers'))
BEGIN
ALTER TABLE tCustomers 
ALTER COLUMN ProfileStatus SMALLINT
END
GO