--===========================================================================================
-- Author:			<Chinar Kulkarni>
-- Date Created:	05/07/2015
-- User Story:      AL-208
-- Description:		<Script for fixing the NexxoIdTypeID column values for the new ID Types
--===========================================================================================


 UPDATE tNexxoIdTypes SET NexxoIdTypeID = 487 WHERE Name = 'FEDERAL EMPLOYEE ID' AND Country = 'UNITED STATES'


 UPDATE tNexxoIdTypes SET NexxoIdTypeID = 488 WHERE Name = 'NYC ID/BENEFITS ID' AND Country = 'UNITED STATES'