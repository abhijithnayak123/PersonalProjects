-- ================================================================================
-- Author:		<Kaushik Sakal>
-- Create date: <06/20/2017>
-- Description:	<Ability to store error for customer registration failure>
-- ================================================================================

--Alter Table tCustomers  
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers' AND COLUMN_NAME = 'IsRCIFSuccess')
BEGIN
ALTER TABLE tCustomers
ADD IsRCIFSuccess BIT NOT NULL DEFAULT 0
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers' AND COLUMN_NAME = 'ErrorReason')
BEGIN
ALTER TABLE tCustomers
ADD ErrorReason nvarchar(MAX) NULL
END
GO

--Alter Table tCustomers_Aud  
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers_Aud' AND COLUMN_NAME = 'IsRCIFSuccess')
BEGIN
ALTER TABLE tCustomers_Aud
ADD IsRCIFSuccess BIT NOT NULL DEFAULT 0
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tCustomers_Aud' AND COLUMN_NAME = 'ErrorReason')
BEGIN
ALTER TABLE tCustomers_Aud
ADD ErrorReason nvarchar(MAX) NULL
END
GO
-- Data Migration for historical tcustomers
UPDATE dbo.tCustomers
SET
   dbo.tCustomers.IsRCIFSuccess = 1
FROM dbo.tCustomers tc
INNER JOIN dbo.tTCIS_Account tta ON tc.CustomerID = tta.CustomerID
WHERE TC.ProfileStatus = 1 AND tta.ProfileStatus = 1

GO

