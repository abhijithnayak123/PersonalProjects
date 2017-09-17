declare @AdjustmentPK uniqueidentifier, @PartnerId uniqueidentifier

set @AdjustmentPK = newid()
select @PartnerId = rowguid from tChannelPartners where id = 33

-- add fee adjustment that adds $1
insert tChannelPartnerFeeAdjustments(rowguid, ChannelPartnerPK, TransactionType, Name, DTStart, SystemApplied, adjustmentRate, adjustmentamount,DTCreate)
values(@AdjustmentPK, @PartnerId, 1, 'First Check $1 surcharge', '4/1/2014', 1, 0, 1, getdate())

-- add a "# transactions" condition with a "= 0" condition value
insert tFeeAdjustmentConditions(rowguid, FeeAdjustmentPK, ConditionType, CompareType, ConditionValue,DTCreate)
values(newid(), @AdjustmentPK, 4, 1,'0',getdate())