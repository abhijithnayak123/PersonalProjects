DELETE FROM TMEssageStore where MessageKey in ('1005.1000', '1005.1001', '1005.1002', '1005.1003', '1005.2000', '1005.2001', '1005.2002')

INSERT INTO TMEssageStore(Rowguid,  MessageKey,  PArtnerPK,  Language,  Content,  DTCreate,  AddlDetails,  Processor) VALUES
 (NewID(), '1005.1000', 1, '0', 'Money Transfer Transaction Not Found',  GETDATE(),
  'Money Transfer Transaction Not Found', '')
    ,
  (NewID(), '1005.1001', 1, '0', 'Money Transfer Create Failed',  GETDATE(),
  'Money Trasfer Create Failed', '')
      ,
  (NewID(), '1005.1002', 1, '0', 'Money Transfer Update Failed',  GETDATE(),
  'Money Trasfer Update Failed', '')  
      ,
  (NewID(), '1005.1003', 1, '0', 'Money Transfer Commit Failed',  GETDATE(),
  'Money Trasfer Commit Failed', '')
  ,
  (NewID(), '1005.2000', 1, '0', 'Receiver Already Existed',  GETDATE(),
  'Receiver profile already exists in Nexxo database', '')
  ,
  (NewID(), '1005.2001', 1, '0', 'Receiver Not Found',  GETDATE(),
  'Receiver Not Found', '')
  ,
  (NewID(), '1005.2002', 1, '0', 'Unknown',  GETDATE(),
  'Unknown Exception', '')