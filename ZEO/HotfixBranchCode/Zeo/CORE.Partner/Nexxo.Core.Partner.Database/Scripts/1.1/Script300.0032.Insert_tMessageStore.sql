--===========================================================================================
-- Author:		<SwarnaLakshmi>
-- Create date: <12/2/2014>
-- Description:	<Synovus - MEssage Store - Records for CounterID Related Exception>
-- Rally ID:	<US2028>
--===========================================================================================

IF NOT Exists( select 1 from TMessageStore where MessageKey = '1010.3216' and PartnerPK=1)
BEGIN
INSERT INTO TMessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) 
VALUES(NewID(),'1010.3216',1,'0','Customer Session CounterID Create Failed.',getdate(),'Customer Session CounterID Create Failed.','')
END

GO

IF NOT Exists( select 1 from TMessageStore where MessageKey = '1010.3206' and PartnerPK=1)
BEGIN
INSERT INTO TMessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) 
VALUES(NewID(),'1010.3206',1,'0','Location CounterID Not Found.',getdate(),'Location CounterID Not Found.','')
END

GO

IF NOT Exists( select 1 from TMessageStore where MessageKey = '1010.3207' and PartnerPK=1)
BEGIN
INSERT INTO TMessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) 
VALUES(NewID(),'1010.3207',1,'0','Location CounterID Status Update Failed.',getdate(),'Location CounterID Status Update Failed.','')
END

GO

IF NOT Exists( select 1 from TMessageStore where MessageKey = '1005.2004' and PartnerPK=1)
BEGIN
INSERT INTO TMessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) 
VALUES(NewID(),'1005.2004',1,'0','Invalid CounterID.',getdate(),'Invalid CounterID.','')
END

GO

IF NOT Exists( select 1 from TMessageStore where MessageKey = '1010.4100' and PartnerPK=1)
BEGIN
INSERT INTO TMessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) 
VALUES(NewID(),'1010.4100',1,'0','Customer Session Not Found.',getdate(),'Customer Session Not Found.','')
END

GO