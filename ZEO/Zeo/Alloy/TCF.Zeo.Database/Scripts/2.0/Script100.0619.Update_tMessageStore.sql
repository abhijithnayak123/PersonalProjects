--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <22-09-2017>
-- Description: Updating message from 'Zeo' to All caps
-- ================================================================================


UPDATE 
   tMessageStore 
SET
  Processor = N'ZEO'

WHERE
  Processor = 'Zeo'   


UPDATE 
   tMessageStore 
SET
  Content = N'The My WU customer name does not match the ZEO customer profile name'
WHERE
  MessageKey = '1005.100.2010'

UPDATE 
   tMessageStore 
SET
  Content = N'Error while fetching the ZEO id type'
WHERE
  MessageKey = '1001.100.3028'

UPDATE 
   tMessageStore 
SET
  Content = N'ZEO Prepaid Visa Debit Card cannot be issued.'
WHERE
  MessageKey = '1003.100.8101'
  
UPDATE 
   tMessageStore 
SET
  Content = N'Cardholder is not registered in ZEO'
WHERE
  MessageKey = '1003.100.2111'

