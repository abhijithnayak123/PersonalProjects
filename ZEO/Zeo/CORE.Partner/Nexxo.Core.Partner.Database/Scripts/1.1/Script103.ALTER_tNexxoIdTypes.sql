-- Author: Bineesh Raghavan
-- Date Created: 11/14/2013
-- Description: Adding new coloum IsActive to limit the IdTypes available Create/Edit Customer Identification screen
-- Rally ID: US1672
if not exists(select * from sys.columns where Name = N'IsActive' and Object_ID = Object_ID(N'tNexxoIdTypes'))
begin
ALTER TABLE tNexxoIdTypes
ADD IsActive BIT NOT NULL CONSTRAINT DF_tNexxoIdTypes_IsActive DEFAULT (0)
end
GO
