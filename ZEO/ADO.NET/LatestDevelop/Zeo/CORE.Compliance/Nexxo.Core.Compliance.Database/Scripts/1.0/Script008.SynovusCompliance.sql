insert tCompliancePrograms(rowguid,Name,RunOFAC,DTCreate)
values('A1FFB6FE-21B6-47AC-AE8F-8B388AA3AB61', 'SynovusCompliance', 0, GETDATE())

insert tLimitTypes(rowguid, ComplianceProgramPK, ClassId, TypeId, Name, Value, DTCreate)
values
('57183BB2-5101-4DC0-8E78-F4C7349E2D83', 'A1FFB6FE-21B6-47AC-AE8F-8B388AA3AB61', 'Product', 15, 'MoneyOrder', '4,5;187', GETDATE())

insert tLimits(rowguid, LimitTypePK, Name, PerX, PerDay, PerNDays, NDays, IsDefault, MultipleNDaysLimits, DTCreate)
values
(NEWID(),'57183BB2-5101-4DC0-8E78-F4C7349E2D83','Synovus MoneyOrder',1000,-1,-1,-1,1,NULL,GETDATE())

insert tTransactionMinimums(rowguid,ComplianceProgramPK,TransactionType,Minimum,DTCreate)
values
(NEWID(),'A1FFB6FE-21B6-47AC-AE8F-8B388AA3AB61',5,10,getdate()),
(NEWID(),'A1FFB6FE-21B6-47AC-AE8F-8B388AA3AB61',6,50,getdate()),
(NEWID(),'A1FFB6FE-21B6-47AC-AE8F-8B388AA3AB61',2,3,getdate()),
(NEWID(),'A1FFB6FE-21B6-47AC-AE8F-8B388AA3AB61',4,10,getdate()),
(NEWID(),'A1FFB6FE-21B6-47AC-AE8F-8B388AA3AB61',8,20,getdate())
