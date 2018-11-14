
declare @synovusCompProgPK uniqueidentifier, @debitSourceLimitTypePK uniqueidentifier

set @synovusCompProgPK = 'A1FFB6FE-21B6-47AC-AE8F-8B388AA3AB61'
set @debitSourceLimitTypePK = '2FCDD61C-8D3D-4449-91BD-855556B551AE'

insert tLimitTypes(rowguid, ComplianceProgramPK, ClassId, TypeId, Name, Value, DTCreate)
values(@debitSourceLimitTypePK, @synovusCompProgPK, 'FundSource', 3, 'GPRSource','debit',GETDATE())

insert tLimits(rowguid, LimitTypePK, Name, PerX, PerDay, PerNDays, NDays, IsDefault,DTCreate)
values(NEWID(), @debitSourceLimitTypePK, 'GPR Debit', 2500, 2500, -1, -1, 1,GETDATE())
