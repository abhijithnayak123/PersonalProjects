--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <09-04-2017>
-- Description: update the content from 'Alloy' to 'Zeo' in tMessageStore table.
-- ================================================================================


 UPDATE 
   tMessageStore 
 SET 
    Content = 'The My WU customer name does not match the zeo customer profile name'
 WHERE 
   MessageKey = '1005.100.2010' 
 
UPDATE
 tMessageStore 
SET 
 Content = 'Cardholder is not registered in Zeo' 
WHERE 
 MessageKey = '1003.100.2111' 
 
UPDATE 
 tMessageStore
SET 
 Content = 'Error while fetching the zeo id type' 
WHERE
 MessageKey = '1001.100.3028' 
