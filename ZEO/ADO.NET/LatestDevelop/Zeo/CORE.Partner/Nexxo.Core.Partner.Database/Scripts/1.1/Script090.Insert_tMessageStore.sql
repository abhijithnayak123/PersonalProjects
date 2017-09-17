if not exists(select 1 from TMessageStore where MessageKey = '1010.3008')
begin
INSERT INTO TMEssageStore(Rowguid,  MessageKey,  PArtnerPK,  Language,  Content,  DTCreate,  AddlDetails,  Processor) VALUES
   (NewID(), '1010.3008', 1, '0', 'Locations Not Found For Channel Partner', GETDATE(), 'Locations Not Found For Channel Partner', '')
,  (NewID(), '1002.6001', 1, '0', 'Location Not Set', GETDATE(), 'Location Not Set', '')
,  (NewID(), '1003.6008', 1, '0', 'Location Not Set', GETDATE(), 'Location Not Set', '')
,  (NewID(), '1003.6009', 1, '0', 'Processor Not Set', GETDATE(), 'Processor Not Set', '')
,  (NewID(), '1010.4101', 1, '0', 'Location Not Set', GETDATE(), 'Location Not Set', '')
,  (NewID(), '1010.4102', 1, '0', 'Processor Not Set', GETDATE(), 'Processor Not Set', '')
,  (NewID(), '1002.2011', 1, '0', 'Partner Not Set', GETDATE(), 'Partner Not Set', '')
,  (NewID(), '1001.6041', 1, '0', 'Location Not Set', GETDATE(), 'Location Not Set', '')
,  (NewID(), '1001.6042', 1, '0', 'Processor Not Set', GETDATE(), 'Processor Not Set', '')
,  (NewID(), '1008.6007', 1, '0', 'Load Amount is less than Minimum Load Amount',  GETDATE(),  'Load Amount is less than Minimum Load Amount', '')
end
GO

UPDATE 
	tMessageStore 
SET 
	Content = 'UserName Already Exists', 
	AddlDetails='UserName Already Exists' 
WHERE 
	MessageKey = '1010.4201'
GO
