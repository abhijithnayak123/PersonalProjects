	-- ================================================================================
-- Author:		Abhijith
-- Create date: <05/14/2018>
-- Description:	<Perm Resident - UI Updates>
-- ================================================================================

IF EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'Mask' AND Object_ID = Object_ID(N'tNexxoidTypes'))
BEGIN
	ALTER TABLE tNexxoidTypes
	ALTER COLUMN Mask NVARCHAR(100)
END
GO

