﻿delete from tlimits where LimitTypePK in (select rowguid from tLimitTypes where ComplianceProgramPK = '57183BB2-5101-4DC0-8E78-F4C7349E2D83')
delete from tlimittypes where rowguid = (select rowguid from tLimitTypes where ComplianceProgramPK = '57183BB2-5101-4DC0-8E78-F4C7349E2D83')
delete from tlimits where LimitTypePK = (select rowguid from tLimitTypes where ComplianceProgramPK = 'A1FFB6FE-21B6-47AC-AE8F-8B388AA3AB61')
delete from tLimitTypes where rowguid = (select rowguid from tLimitTypes where ComplianceProgramPK = 'A1FFB6FE-21B6-47AC-AE8F-8B388AA3AB61')

--select * from tLimitTypes
insert tLimitTypes(rowguid, ComplianceProgramPK, ClassId, TypeId, Name, Value, DTCreate)

values

('71AD3865-23F0-4138-A4F6-A67E36EAB9A7', 'A1FFB6FE-21B6-47AC-AE8F-8B388AA3AB61', 'Total', 1, 'All', NULL, GETDATE()),

('C1FB1750-D6C2-469E-8194-EACF37088C48', 'A1FFB6FE-21B6-47AC-AE8F-8B388AA3AB61', 'FundSourceCash', 2, 'FundingSourceCash', 'cash', GETDATE()),

('00ED3F34-C581-4734-82A3-31D06CE150D6', 'A1FFB6FE-21B6-47AC-AE8F-8B388AA3AB61', 'FundSource', 9, 'FundingSourceCheck', 'check', GETDATE()),

('986C2947-C415-4AAA-BD23-964C78EBF24F', 'A1FFB6FE-21B6-47AC-AE8F-8B388AA3AB61', 'PaymentType', 13, 'PaymentTypeMoneyTransfer', '1,2,3', GETDATE()),

('C6A72CE1-69C5-4C32-8B17-56429F3FFC05', 'A1FFB6FE-21B6-47AC-AE8F-8B388AA3AB61', 'Product', 15, 'ExoPurseCashLoad', '4,5;181', GETDATE()),

('2645996D-F20E-48B7-9284-BB83759F10DD', 'A1FFB6FE-21B6-47AC-AE8F-8B388AA3AB61', 'Balance', 17, 'FundingSourcePurseBalance', NULL, GETDATE()),

('6B1AFFE5-AE85-4364-BE66-44865C42832E', 'A1FFB6FE-21B6-47AC-AE8F-8B388AA3AB61', 'Product', 15, 'ExoPurseCashOut', '4,5;180', GETDATE()),

('45C4D82A-0CD5-4BC6-B05A-F1F548BC6517', 'A1FFB6FE-21B6-47AC-AE8F-8B388AA3AB61', 'PaymentType', 11, 'PaymentTypeAll', '1,2,3,4,5,8,9;-10', GETDATE()),

('50EBA1DA-4F8F-4F80-9DCB-B8EE81047007', 'A1FFB6FE-21B6-47AC-AE8F-8B388AA3AB61', 'PaymentType', 12, 'PaymentTypeBillPayAndTopUps', '4,5,9;-8,9,10', GETDATE()),

('86B3787B-13B9-4A50-8353-4840D7B3B539', 'A1FFB6FE-21B6-47AC-AE8F-8B388AA3AB61', 'Product', 15, 'ProductMoneyOrder', '4,5;177,187', GETDATE())

insert tLimits(rowguid, LimitTypePK, Name, PerX, PerDay, PerNDays, NDays, IsDefault, MultipleNDaysLimits, DTCreate)

values

(NEWID(),'71AD3865-23F0-4138-A4F6-A67E36EAB9A7','Synovus Total',9999,9999,9999,30,1,NULL,GETDATE()),

(NEWID(),'C1FB1750-D6C2-469E-8194-EACF37088C48','Synovus Source Cash',2999,2999,9999,30,1,NULL,GETDATE()),

(NEWID(),'00ED3F34-C581-4734-82A3-31D06CE150D6','Synovus Check',9999,9999,NULL,30,1,NULL,GETDATE()),

(NEWID(),'986C2947-C415-4AAA-BD23-964C78EBF24F','Synovus Money Transfer',995,995,4995,30,1,'4:2995',GETDATE()),

(NEWID(),'C6A72CE1-69C5-4C32-8B17-56429F3FFC05','Synovus Purse Cash Load',5000,5000,5000,30,1,NULL,GETDATE()),

(NEWID(),'2645996D-F20E-48B7-9284-BB83759F10DD','Synovus Funding Balance',10000,10000,NULL,NULL,1,NULL,GETDATE()),

(NEWID(),'6B1AFFE5-AE85-4364-BE66-44865C42832E','Synovus Purse Cash Out',9999,9999,9999,30,1,NULL,GETDATE()),

(NEWID(),'45C4D82A-0CD5-4BC6-B05A-F1F548BC6517','Synovus PaymentTypeAll',9999,9999,9999,30,1,NULL,GETDATE()),

(NEWID(),'50EBA1DA-4F8F-4F80-9DCB-B8EE81047007','Synovus PaymentTypeBillPayAndTopUps',500,1000,9999,30,1,NULL,GETDATE()),

(NEWID(),'86B3787B-13B9-4A50-8353-4840D7B3B539','Synovus PaymentTypeBillPayAndTopUps',900,900,9999,30,1,'2:1800;7:2999',GETDATE())