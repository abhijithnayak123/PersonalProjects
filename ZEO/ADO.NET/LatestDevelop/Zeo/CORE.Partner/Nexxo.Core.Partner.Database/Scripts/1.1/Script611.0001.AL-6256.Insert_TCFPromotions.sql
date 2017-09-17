DECLARE @AdjustmentPK uniqueidentifier, @PartnerId uniqueidentifier

-- add  "Code" condition with condition value ZEOFromTCFBank

SET @AdjustmentPK = newid()
SELECT @PartnerId = ChannelPartnerPK
FROM tChannelPartners WHERE ChannelPartnerId = 34

insert tChannelPartnerFeeAdjustments(FeeAdjustmentPK, ChannelPartnerPK, TransactionType, Name, Description , PromotionType , DTStart, DTEnd, SystemApplied, adjustmentRate, adjustmentamount, DTServerCreate, DTServerLastModified)
values(@AdjustmentPK, @PartnerId, 1, 'ZEOFromTCFBank','Cash any approved check, up to $1,000, for no fee with ZEO.','Code', '4/18/2016', '5/24/2016', 0, 0, -30.00, getdate(), getdate())

insert tFeeAdjustmentConditions(AdjConditionsPK, FeeAdjustmentPK, ConditionTypePK, CompareTypePK, ConditionValue, DTServerCreate, DTServerLastModified)
values(newid(), @AdjustmentPK, 9, 1, 'ZEOFromTCFBank', getdate(), getdate())

insert tFeeAdjustmentConditions(AdjConditionsPK, FeeAdjustmentPK, ConditionTypePK, CompareTypePK, ConditionValue, DTServerCreate, DTServerLastModified)
values(newid(), @AdjustmentPK, 3, 8, 1000, getdate(), getdate())

-- add  "Code" condition with condition value HelloZEO

SET @AdjustmentPK = newid()
SELECT @PartnerId = ChannelPartnerPK
FROM tChannelPartners WHERE ChannelPartnerId = 34

insert tChannelPartnerFeeAdjustments(FeeAdjustmentPK, ChannelPartnerPK, TransactionType, Name, Description , PromotionType , DTStart, DTEnd, SystemApplied, adjustmentRate, adjustmentamount, DTServerCreate, DTServerLastModified)
values(@AdjustmentPK, @PartnerId, 1, 'HelloZEO','Cash any approved check, up to $1,000, for no fee with ZEO.','Code', '4/25/2016', '5/13/2016', 0, 0, -30.00, getdate(), getdate())

insert tFeeAdjustmentConditions(AdjConditionsPK, FeeAdjustmentPK, ConditionTypePK, CompareTypePK, ConditionValue, DTServerCreate, DTServerLastModified)
values(newid(), @AdjustmentPK, 9, 1, 'HelloZEO', getdate(), getdate())

insert tFeeAdjustmentConditions(AdjConditionsPK, FeeAdjustmentPK, ConditionTypePK, CompareTypePK, ConditionValue, DTServerCreate, DTServerLastModified)
values(newid(), @AdjustmentPK, 3, 8, 1000, getdate(), getdate())

-- add  "Code" condition with condition value TryZEOToday
SET @AdjustmentPK = newid()
SELECT @PartnerId = ChannelPartnerPK
FROM tChannelPartners WHERE ChannelPartnerId = 34

insert tChannelPartnerFeeAdjustments(FeeAdjustmentPK, ChannelPartnerPK, TransactionType, Name, Description , PromotionType , DTStart, DTEnd, SystemApplied, adjustmentRate, adjustmentamount, DTServerCreate, DTServerLastModified)
values(@AdjustmentPK, @PartnerId, 1, 'TryZEOToday','Cash any approved check, up to $1,000, for no fee with ZEO.','Code', '5/2/2016', '6/7/2016', 0, 0, -30.00, getdate(), getdate())

insert tFeeAdjustmentConditions(AdjConditionsPK, FeeAdjustmentPK, ConditionTypePK, CompareTypePK, ConditionValue, DTServerCreate, DTServerLastModified)
values(newid(), @AdjustmentPK, 9, 1, 'TryZEOToday', getdate(), getdate())

insert tFeeAdjustmentConditions(AdjConditionsPK, FeeAdjustmentPK, ConditionTypePK, CompareTypePK, ConditionValue, DTServerCreate, DTServerLastModified)
values(newid(), @AdjustmentPK, 3, 8, 1000, getdate(), getdate())

-- add  "Code" condition with condition value ZEOCheckCashing
SET @AdjustmentPK = newid()
SELECT @PartnerId = ChannelPartnerPK
FROM tChannelPartners WHERE ChannelPartnerId = 34

insert tChannelPartnerFeeAdjustments(FeeAdjustmentPK, ChannelPartnerPK, TransactionType, Name, Description , PromotionType , DTStart, DTEnd, SystemApplied, adjustmentRate, adjustmentamount, DTServerCreate, DTServerLastModified)
values(@AdjustmentPK, @PartnerId, 1, 'ZEOCheckCashing','Cash any approved check, up to $1,000, for no fee with ZEO.','Code', '5/9/2016', '6/14/2016', 0, 0, -30.00, getdate(), getdate())

insert tFeeAdjustmentConditions(AdjConditionsPK, FeeAdjustmentPK, ConditionTypePK, CompareTypePK, ConditionValue, DTServerCreate, DTServerLastModified)
values(newid(), @AdjustmentPK, 9, 1, 'ZEOCheckCashing', getdate(), getdate())

insert tFeeAdjustmentConditions(AdjConditionsPK, FeeAdjustmentPK, ConditionTypePK, CompareTypePK, ConditionValue, DTServerCreate, DTServerLastModified)
values(newid(), @AdjustmentPK, 3, 8, 1000, getdate(), getdate())

-- add  "Code" condition with condition value AskMeAboutZEO
SET @AdjustmentPK = newid()
SELECT @PartnerId = ChannelPartnerPK
FROM tChannelPartners WHERE ChannelPartnerId = 34

insert tChannelPartnerFeeAdjustments(FeeAdjustmentPK, ChannelPartnerPK, TransactionType, Name, Description , PromotionType , DTStart, DTEnd, SystemApplied, adjustmentRate, adjustmentamount, DTServerCreate, DTServerLastModified)
values(@AdjustmentPK, @PartnerId, 1, 'AskMeAboutZEO','Cash any approved check, up to $1,000, for no fee with ZEO.','Code', '5/23/2016', '6/28/2016', 0, 0, -30.00, getdate(), getdate())

insert tFeeAdjustmentConditions(AdjConditionsPK, FeeAdjustmentPK, ConditionTypePK, CompareTypePK, ConditionValue, DTServerCreate, DTServerLastModified)
values(newid(), @AdjustmentPK, 9, 1, 'AskMeAboutZEO', getdate(), getdate())

insert tFeeAdjustmentConditions(AdjConditionsPK, FeeAdjustmentPK, ConditionTypePK, CompareTypePK, ConditionValue, DTServerCreate, DTServerLastModified)
values(newid(), @AdjustmentPK, 3, 8, 1000, getdate(), getdate())

-- add  "Code" condition with condition value TryZEO
SET @AdjustmentPK = newid()
SELECT @PartnerId = ChannelPartnerPK
FROM tChannelPartners WHERE ChannelPartnerId = 34

insert tChannelPartnerFeeAdjustments(FeeAdjustmentPK, ChannelPartnerPK, TransactionType, Name, Description , PromotionType , DTStart, DTEnd, SystemApplied, adjustmentRate, adjustmentamount, DTServerCreate, DTServerLastModified)
values(@AdjustmentPK, @PartnerId, 1, 'TryZEO','Cash any approved check, up to $1,000, for no fee with ZEO.','Code', '5/31/2016', '6/10/2016', 0, 0, -30.00, getdate(), getdate())

insert tFeeAdjustmentConditions(AdjConditionsPK, FeeAdjustmentPK, ConditionTypePK, CompareTypePK, ConditionValue, DTServerCreate, DTServerLastModified)
values(newid(), @AdjustmentPK, 9, 1, 'TryZEO', getdate(), getdate())

insert tFeeAdjustmentConditions(AdjConditionsPK, FeeAdjustmentPK, ConditionTypePK, CompareTypePK, ConditionValue, DTServerCreate, DTServerLastModified)
values(newid(), @AdjustmentPK, 3, 8, 1000, getdate(), getdate())

-- add  "Code" condition with condition value ZeoCashesChecks
SET @AdjustmentPK = newid()
SELECT @PartnerId = ChannelPartnerPK
FROM tChannelPartners WHERE ChannelPartnerId = 34

insert tChannelPartnerFeeAdjustments(FeeAdjustmentPK, ChannelPartnerPK, TransactionType, Name, Description , PromotionType , DTStart, DTEnd, SystemApplied, adjustmentRate, adjustmentamount, DTServerCreate, DTServerLastModified)
values(@AdjustmentPK, @PartnerId, 1, 'ZeoCashesChecks','Cash any approved check, up to $1,000, for no fee with ZEO.','Code', '4/1/2016', '12/31/2016', 0, 0, -30.00, getdate(), getdate())

insert tFeeAdjustmentConditions(AdjConditionsPK, FeeAdjustmentPK, ConditionTypePK, CompareTypePK, ConditionValue, DTServerCreate, DTServerLastModified)
values(newid(), @AdjustmentPK, 9, 1, 'ZeoCashesChecks', getdate(), getdate())

insert tFeeAdjustmentConditions(AdjConditionsPK, FeeAdjustmentPK, ConditionTypePK, CompareTypePK, ConditionValue, DTServerCreate, DTServerLastModified)
values(newid(), @AdjustmentPK, 3, 8, 1000, getdate(), getdate())



GO 
