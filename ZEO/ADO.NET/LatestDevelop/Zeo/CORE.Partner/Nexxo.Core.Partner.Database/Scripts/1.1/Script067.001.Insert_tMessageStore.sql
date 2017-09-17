

INSERT INTO TMEssageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.2000',1,'0','Location Ingo username not set in the context',getdate(),'Location Ingo username not set in the context','')
update tmessagestore set content = 'Missing Image', Addldetails = 'One or more check images missing' where messagekey = '1002.2002' and partnerpk = 1
update tmessagestore set content = 'Chexar Credentials Not Found', Addldetails = 'Chexar Credentials Not Found' where messagekey = '1002.2004' and partnerpk = 1
update tmessagestore set content = 'Chexar Login Failed', Addldetails = 'Chexar Login Failed' where messagekey = '1002.2005' and partnerpk = 1
update tmessagestore set content = 'Chexar Badge Not Created', Addldetails = 'Chexar Badge Not Created' where messagekey = '1002.2006' and partnerpk = 1
update tmessagestore set content = 'Chexar Invoice Not Created', Addldetails = 'Chexar Invoice Not Created' where messagekey = '1002.2007' and partnerpk = 1
update tmessagestore set content = 'Chexar Check Type Not Found', Addldetails = 'Chexar Check Type Not Found' where messagekey = '1002.2009' and partnerpk = 1
update tmessagestore set content = 'PartnerId not set in the context', Addldetails = 'PartnerId not set in the context' where messagekey = '1002.2011' and partnerpk = 1

INSERT INTO TMEssageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.2012',1,'0','TimeZone not found in the context',getdate(),'TimeZone not found in the context','')
INSERT INTO TMEssageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.-2',1,'0','Check too old to cash',getdate(),'Check too old to cash','')
INSERT INTO TMEssageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.-3',1,'0','Check post dated',getdate(),'Check post dated','')
INSERT INTO TMEssageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.-1',1,'0','Customer on hold',getdate(),'Customer on hold','')

INSERT INTO TMEssageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1004.2416',1,'0','Western Union counter Id is not available or has not been correctly setup',getdate(),'Western Union Error','')
