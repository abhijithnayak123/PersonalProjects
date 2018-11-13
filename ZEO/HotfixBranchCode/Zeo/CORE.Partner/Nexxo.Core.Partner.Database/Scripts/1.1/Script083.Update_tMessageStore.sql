update tMessageStore 
set AddlDetails='The customer''s profile has been marked as inactive. 
Please call Nexxo for more information.  Dial 866-340-6392'
where MessageKey='1008.5000'
go

INSERT INTO TMEssageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1003.6007 ',1,'0','Load Amount Less Than Minimum Load Amount',getdate(),'Load Amount Less Than Minimum Load Amount','')
INSERT INTO TMEssageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.6000 ',1,'0','Check Amount Less Than Minimum Amount',getdate(),'Check Amount Less Than Minimum Amount','')