--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <07-02-2018>
-- Description:	Adding a column "HiddenCheckType"
-- ================================================================================

DECLARE @maxCheckId INT = (SELECT MAX(CheckTypeId) FROM tCheckTypes)

INSERT tCheckTypes(CheckTypeId, Name, ProductProviderCode, DisplayName)
values (@maxCheckId + 1, 'Ins/Attorney/Cashiers', 202, 'Ins/Attorney/Cashiers'),
(@maxCheckId + 2, 'US Treasury',202, 'US Treasury'),
(@maxCheckId + 3, 'Government', 202, 'Government'),
(@maxCheckId + 4, 'Money Order',202, 'Money Order'),
(@maxCheckId + 5, 'Handwritten Payroll',202,'Handwritten Payroll'),
(@maxCheckId + 6, 'Printed Payroll',202,'Printed Payroll'),
(@maxCheckId + 7, 'Two Party',202, 'Two Party'),
(@maxCheckId + 8, 'Loan/RAL', 202, 'Loan/RAL')
GO

--================ Creating a New column "HiddenCheckType" in the tCheckTypes table ==================
IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'HiddenCheckType'AND Object_ID = Object_ID(N'tCheckTypes'))
BEGIN
    ALTER TABLE tCheckTypes ADD HiddenCheckType BIT NOT NULL DEFAULT 0
END
GO

UPDATE tCheckTypes 
SET HiddenCheckType = 1
WHERE Name IN ('OnUsOCMO','OnUsTRUE','OnUsOTHER') AND ProductProviderCode = 202
GO
