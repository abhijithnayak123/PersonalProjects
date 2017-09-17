declare @AdjustmentPK uniqueidentifier, @PartnerId uniqueidentifier

set @AdjustmentPK = newid()
select @PartnerId = rowguid from tChannelPartners where id = 33

-- add fee adjustment that gives 100% off
insert tChannelPartnerFeeAdjustments(rowguid, ChannelPartnerPK, TransactionType, Name, DTStart, SystemApplied, adjustmentRate, adjustmentamount,DTCreate)
values(@AdjustmentPK, @PartnerId, 1, 'Free 2nd Check', '4/1/2014', 1, -1.00, 0, getdate())

-- add a "# transactions" condition with a "= 1" condition value
insert tFeeAdjustmentConditions(rowguid, FeeAdjustmentPK, ConditionType, CompareType, ConditionValue,DTCreate)
values(newid(), @AdjustmentPK, 4, 1,'1',getdate())