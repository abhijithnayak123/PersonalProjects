-- Author:		<Shwetha Mohan>
-- Create date: <11/23/2015>
-- Description:	<Added a record in tNexxoIdTypes table for CANADIAN DRIVER''S LICENSE
--				 PROVINCIAL/TERRITORIAL IDENTITY CARD for ID issusing State is NULL >
-- Jira ID:		<AL-2559>
-- ================================================================================
IF NOT EXISTS (SELECT 1 FROM tNexxoIdTypes 
	WHERE NexxoIdTypePK='BA76EBF7-D10B-47D1-B25C-21B692F1C61A')
	BEGIN
		INSERT tNexxoIdTypes([NexxoIdTypePK],[NexxoIdTypeID],[Name],[Mask],[HasExpirationDate],[Country],[State],[IsActive],[CountryPK],[StatePK])
		VALUES ('BA76EBF7-D10B-47D1-B25C-21B692F1C61A',517,'CANADIAN DRIVER''S LICENSE', '^\w{4,20}$',1,'CANADA',NULL,1,'A86487D4-E575-458C-8CB6-B7C3A62EE836',NULL)
	END

IF NOT EXISTS (SELECT 1 FROM tNexxoIdTypes
	WHERE NexxoIdTypePK='81F1BC43-6DE0-402F-B476-714CE340C318')
	BEGIN
		INSERT tNexxoIdTypes([NexxoIdTypePK],[NexxoIdTypeID],[Name],[Mask],[HasExpirationDate],[Country],[State],[IsActive],[CountryPK],[StatePK])
		VALUES ('81F1BC43-6DE0-402F-B476-714CE340C318',518,'PROVINCIAL/TERRITORIAL IDENTITY CARD', '^\w{4,20}$',1,'CANADA',NULL,1,'A86487D4-E575-458C-8CB6-B7C3A62EE836',NULL)
	END

