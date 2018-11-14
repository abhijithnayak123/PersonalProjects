declare @ComplianceProgramPK uniqueidentifier

set @ComplianceProgramPK = 'A1FFB6FE-21B6-47AC-AE8F-8B388AA3AB61'

delete from tLimits where LimitTypePK in (select rowguid from tLimitTypes where ComplianceProgramPK = @ComplianceProgramPK)
delete from tLimitTypes where ComplianceProgramPK = @ComplianceProgramPK

-- Money order, GPR withdraw and GPR balance limits
insert tLimitTypes(rowguid, ComplianceProgramPK, ClassId, TypeId, Name, Value, DTCreate)
values
('57183BB2-5101-4DC0-8E78-F4C7349E2D83', @ComplianceProgramPK, 'Product', 15, 'MoneyOrder', '4,5;187', GETDATE()),
('9C404D61-17FC-4C8A-9F0B-CC02FED10F02',@ComplianceProgramPK,'Product',15,'PurseCashOut','4,5;180',GETDATE()),
('A6E81E42-5C5F-40A2-9562-FA956355C8FC',@ComplianceProgramPK,'FundSource',17,'PurseBalance','',GETDATE())

insert tLimits(rowguid, LimitTypePK, Name, PerX, PerDay, PerNDays, NDays, IsDefault,DTCreate)
values
(NEWID(),'57183BB2-5101-4DC0-8E78-F4C7349E2D83','Synovus MoneyOrder',1000,-1,-1,-1,1,GETDATE()),
(NEWID(),'9C404D61-17FC-4C8A-9F0B-CC02FED10F02','GPR Withdrawal',2500,2500,-1,-1,1,GETDATE()),
(NEWID(),'A6E81E42-5C5F-40A2-9562-FA956355C8FC','GPR Balanace',7500,7500,-1,-1,1,GETDATE())