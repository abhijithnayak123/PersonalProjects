if not exists( select 1 from TMessageStore where MessageKey = '1005.6003')
begin
INSERT INTO TMessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1005.6003',1,'0','A receive money transaction with the same MTCN has already been loaded to the shopping cart.',getdate(),'Please check the MTCN number and re-enter if needed.','')
end