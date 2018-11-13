--===========================================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <Oct 26 2015>
-- Description:	<Script for inserting 'PASSPORT' ID Types to Countries(EAST TIMOR,PALESTINIAN TERRITORY,REUNION)>
-- Jira ID:	    <AL-2426>
--===========================================================================================

IF NOT EXISTS (SELECT 1 FROM [dbo].[tNexxoIdTypes] WHERE NexxoIdTypePK = '0A927247-D011-4611-BF3B-7C3EF30E15C4')
BEGIN
	 INSERT INTO [dbo].[tNexxoIdTypes] ([NexxoIdTypePK], [NexxoIdTypeID], [Name], [Mask], [HasExpirationDate], [Country], [State], [CountryPK], [StatePK], [IsActive])
	 VALUES ('0A927247-D011-4611-BF3B-7C3EF30E15C4', 514, 'PASSPORT', '^\w{4,20}$', 1, 'EAST TIMOR', NULL, 'C485A3A3-0E28-47F6-8FEC-C87CD06BCA67', NULL, 1)
END


IF NOT EXISTS (SELECT 1 FROM [dbo].[tNexxoIdTypes] WHERE NexxoIdTypePK = 'F2881A88-E178-4B9D-84E0-51295A65557A')
BEGIN
	 INSERT INTO [dbo].[tNexxoIdTypes] ([NexxoIdTypePK], [NexxoIdTypeID], [Name], [Mask], [HasExpirationDate], [Country], [State], [CountryPK], [StatePK], [IsActive])
     VALUES ('F2881A88-E178-4B9D-84E0-51295A65557A', 515, 'PASSPORT', '^\w{4,20}$', 1, 'PALESTINIAN TERRITORY', NULL, 'DFF1DC42-2725-48DE-B78E-212A5AB1F3A3', NULL, 1)
END


IF NOT EXISTS (SELECT 1 FROM [dbo].[tNexxoIdTypes] WHERE NexxoIdTypePK = '41F25534-4B28-4387-8530-497AFA6456D2')
BEGIN
	 INSERT INTO [dbo].[tNexxoIdTypes] ([NexxoIdTypePK], [NexxoIdTypeID], [Name], [Mask], [HasExpirationDate], [Country], [State], [CountryPK], [StatePK], [IsActive])
     VALUES ('41F25534-4B28-4387-8530-497AFA6456D2', 516, 'PASSPORT', '^\w{4,20}$', 1, 'REUNION', NULL, 'A6E9B895-55A5-4649-A1DA-32E1094301DF', NULL, 1)
END
	   