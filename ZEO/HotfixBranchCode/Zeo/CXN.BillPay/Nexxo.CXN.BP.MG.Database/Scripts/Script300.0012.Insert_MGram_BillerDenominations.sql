-- ============================================================
-- Author:		<Ratheesh PK>
-- Create date: <17/09/2014>
-- Description:	<BillerLimits insertion. Data copied from  table creation> 
-- Rally ID:	<NA>
-- ============================================================

DELETE FROM [dbo].[tMGram_BillerDenomination]

INSERT INTO tMGram_BillerDenomination 
VALUES (NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='3941'),25.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='3941'),50.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='3941'),75.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='3941'),100.00)

INSERT INTO tMGram_BillerDenomination 
VALUES (NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7272'),25.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7272'),50.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7272'),100.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7272'),30.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7272'),70.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7272'),15.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7272'),60.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7272'),80.00)

INSERT INTO tMGram_BillerDenomination 
VALUES (NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7575'),25.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7575'),40.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7575'),45.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7575'),50.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7575'),60.00)

INSERT INTO tMGram_BillerDenomination 
VALUES (NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7592'),50.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7592'),70.00)

INSERT INTO tMGram_BillerDenomination 
VALUES (NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7673'),15.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7673'),25.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7673'),50.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7673'),60.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7673'),90.00)

INSERT INTO tMGram_BillerDenomination 
VALUES (NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7870'),5.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7870'),10.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7870'),20.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7870'),30.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7870'),50.00)

INSERT INTO tMGram_BillerDenomination 
VALUES (NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7936'),5.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7936'),10.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7936'),20.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7936'),30.00)

INSERT INTO tMGram_BillerDenomination 
VALUES (NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7938'),10.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7938'),15.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7938'),20.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7938'),30.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7938'),50.00)

INSERT INTO tMGram_BillerDenomination 
VALUES (NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7941'),5.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7941'),10.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7941'),20.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7941'),30.00)

INSERT INTO tMGram_BillerDenomination 
VALUES (NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7942'),10.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7942'),15.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7942'),20.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7942'),30.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7942'),50.00)

INSERT INTO tMGram_BillerDenomination 
VALUES (NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7944'),10.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7944'),15.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7944'),20.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7944'),30.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7944'),50.00)

INSERT INTO tMGram_BillerDenomination 
VALUES (NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7946'),5.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7946'),10.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7946'),20.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7946'),30.00)

INSERT INTO tMGram_BillerDenomination 
VALUES (NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7962'),5.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7962'),10.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7962'),20.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7962'),30.00)

INSERT INTO tMGram_BillerDenomination 
VALUES (NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7967'),10.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7967'),15.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7967'),20.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7967'),30.00)
,(NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='7967'),50.00)

INSERT INTO tMGram_BillerDenomination 
VALUES (NEWID(),(select rowguid from tMGram_BillerLimit where receiveCode='1193'),5.00)
