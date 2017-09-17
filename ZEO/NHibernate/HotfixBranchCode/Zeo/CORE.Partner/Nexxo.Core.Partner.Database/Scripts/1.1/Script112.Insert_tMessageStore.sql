INSERT INTO TMEssageStore(Rowguid,  MessageKey,  PartnerPK,  Language,  Content,  DTCreate,  AddlDetails,  Processor) VALUES
 (NewID(), '1011.2000', 1, '0', 'Create Account Failed',  GETDATE(),  'Create Account Failed', ''),
  (NewID(), '1011.2001', 1, '0', 'Context Not Found',  GETDATE(),  'Context Not Found', ''),
   (NewID(), '1011.2002', 1, '0', 'Error In Location Information',  GETDATE(),  'Error In Location Information', ''),
    (NewID(), '1011.2003', 1, '0', 'FIS Credentials Not Found',  GETDATE(),  'FIS Credentials Not Found', ''),
	 (NewID(), '1011.2004', 1, '0', 'Multiple Account Found',  GETDATE(),  'Multiple Account Found', '')
	 
GO
