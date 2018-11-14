-- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <08/31/2015>
-- Description:	<To update the regular expression for 
-- 1. Canadian Driver's License
-- 2. Provincial/Territorial Identity Card for Canada>
-- Jira ID:		<AL-1323>
-- ================================================================================
 

-- ID Type : Canadian Driver's License

UPDATE tNexxoIdTypes SET Mask = '^\w{4,20}$' WHERE NexxoIdTypeID = 489; 
UPDATE tNexxoIdTypes SET Mask = '^\w{4,20}$' WHERE NexxoIdTypeID = 490; 
UPDATE tNexxoIdTypes SET Mask = '^\w{4,20}$' WHERE NexxoIdTypeID = 491; 
UPDATE tNexxoIdTypes SET Mask = '^\w{4,20}$' WHERE NexxoIdTypeID = 492;  
UPDATE tNexxoIdTypes SET Mask = '^\w{4,20}$' WHERE NexxoIdTypeID = 493;  
UPDATE tNexxoIdTypes SET Mask = '^\w{4,20}$' WHERE NexxoIdTypeID = 494;  
UPDATE tNexxoIdTypes SET Mask = '^\w{4,20}$' WHERE NexxoIdTypeID = 495;  
UPDATE tNexxoIdTypes SET Mask = '^\w{4,20}$' WHERE NexxoIdTypeID = 496; 
UPDATE tNexxoIdTypes SET Mask = '^\w{4,20}$' WHERE NexxoIdTypeID = 497; 
UPDATE tNexxoIdTypes SET Mask = '^\w{4,20}$' WHERE NexxoIdTypeID = 498;
UPDATE tNexxoIdTypes SET Mask = '^\w{4,20}$' WHERE NexxoIdTypeID = 499;
UPDATE tNexxoIdTypes SET Mask = '^\w{4,20}$' WHERE NexxoIdTypeID = 500;
UPDATE tNexxoIdTypes SET Mask = '^\w{4,20}$' WHERE NexxoIdTypeID = 501; 

-- ID Type :Provincial/Territorial Identity Card

UPDATE tNexxoIdTypes SET Mask = '^\w{4,20}$' WHERE NexxoIdTypeID = 502;  
UPDATE tNexxoIdTypes SET Mask = '^\w{4,20}$' WHERE NexxoIdTypeID = 503;  
UPDATE tNexxoIdTypes SET Mask = '^\w{4,20}$' WHERE NexxoIdTypeID = 504;  
UPDATE tNexxoIdTypes SET Mask = '^\w{4,20}$' WHERE NexxoIdTypeID = 505;  
UPDATE tNexxoIdTypes SET Mask = '^\w{4,20}$' WHERE NexxoIdTypeID = 506;  
UPDATE tNexxoIdTypes SET Mask = '^\w{4,20}$' WHERE NexxoIdTypeID = 507;  
UPDATE tNexxoIdTypes SET Mask = '^\w{4,20}$' WHERE NexxoIdTypeID = 508;  
UPDATE tNexxoIdTypes SET Mask = '^\w{4,20}$' WHERE NexxoIdTypeID = 509;  
UPDATE tNexxoIdTypes SET Mask = '^\w{4,20}$' WHERE NexxoIdTypeID = 510;  
UPDATE tNexxoIdTypes SET Mask = '^\w{4,20}$' WHERE NexxoIdTypeID = 511;  
UPDATE tNexxoIdTypes SET Mask = '^\w{4,20}$' WHERE NexxoIdTypeID = 512;  
UPDATE tNexxoIdTypes SET Mask = '^\w{4,20}$' WHERE NexxoIdTypeID = 513;  
UPDATE tNexxoIdTypes SET Mask = '^\w{4,20}$' WHERE NexxoIdTypeID = 514;  

