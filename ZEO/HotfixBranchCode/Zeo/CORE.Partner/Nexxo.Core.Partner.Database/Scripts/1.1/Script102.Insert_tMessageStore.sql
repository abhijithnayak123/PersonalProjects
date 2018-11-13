INSERT INTO TMEssageStore(Rowguid,  MessageKey,  PArtnerPK,  Language,  Content,  DTCreate,  AddlDetails,  Processor) VALUES
 (NewID(), '1008.6008', 1, '0', 'Check Amount is less than Minimum Amount',  GETDATE(),
  'Check Amount is less than Minimum Amount', '')
    ,
  (NewID(), '1008.6009', 1, '0', 'Money Transfer Amount is less than Minimum Amount',  GETDATE(),
  'Money Trasfer Amount is less than Minimum Amount', '')
GO
