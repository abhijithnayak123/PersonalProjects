  --- ================================================================================
-- Author:		<RAJKUMAR M>
-- Created date: <12/15/2015>
-- Description:	<Script to Update Column value in NexxoIdTypeID>           
-- Jira ID:	<AL-3022>
-- ================================================================================

BEGIN
	-- updating NexxoIdTypeID column value from 465 to 519
	UPDATE [dbo].[tNexxoIdTypes] 
	SET 
		[NexxoIdTypeID] = 519 
	WHERE 
		[NexxoIdTypePK]= '6B024196-7BBA-486B-B561-D6FCBDAB0C61'
		AND [CountryPk] = '2B60C245-978F-4825-8BFC-8F74F67F2C59'	-- "LAOS" / PASSPORT

	-- updating NexxoIdTypeID column value from 466 to 520
	UPDATE [dbo].[tNexxoIdTypes] 
	SET 
		[NexxoIdTypeID] = 520 
	WHERE 
	[NexxoIdTypePK]= '82A110C2-864C-4C4F-87B8-603F18CC7C59'
	AND CountryPk = '52AC1FB4-5E6F-4246-8AE4-3B9E31189329'		    -- "RUSSIA" / PASSPORT

	-- updating NexxoIdTypeID column value from 514 to 521
	UPDATE [dbo].[tNexxoIdTypes] 
	SET 
		[NexxoIdTypeID] = 521 
	WHERE 
		[NexxoIdTypePK]= 'F3B7621B-5EC6-407D-8256-2ADEE9434504'
		AND CountryPk = 'A86487D4-E575-458C-8CB6-B7C3A62EE836'		 -- "CANADA" / PROVINCIAL/TERRITORIAL IDENTITY CARD

	-- updating NexxoIdTypeID column value from 'Null' to 522
	UPDATE [dbo].[tNexxoIdTypes] 
	SET
		[NexxoIdTypeID] = 522
	WHERE 
		[NexxoIdTypePK]= '0F2CA0EC-7268-4A51-BFA1-19A0825FC4DA'       --- "UNITED STATES" / FEDERAL EMPLOYEE ID


	-- updating NexxoIdTypeID column value from 'Null' to 523
	UPDATE [dbo].[tNexxoIdTypes] 
	SET
		 [NexxoIdTypeID] = 523
	WHERE 
		[NexxoIdTypePK]= '65C9D477-8615-4694-B6D4-4838F3473BB2'       ---"UNITED STATES" / NYC ID/BENEFITS ID

END

