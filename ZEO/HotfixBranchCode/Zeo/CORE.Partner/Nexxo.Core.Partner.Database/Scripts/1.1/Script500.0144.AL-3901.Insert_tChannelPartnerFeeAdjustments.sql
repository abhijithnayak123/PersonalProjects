-- ============================================================
-- Author:		Swarnalakshmi
-- Create date: Dec 29 2015
-- Description:	As Carver, i want to add a check cashing promotion
-- Rally ID:	<AL-3901>
-- ============================================================

--Promotion type:	Code
--Client	Carver
--Promotion code:	5off1000Chk
--Promotion name:	$5 Off $1000 check to non-check customers
--Product	Check
--Promotion:	5 off fee
--Promotion constraints:	
--Promotion criteria:	"Check type <> ""Gvt - US Treasury"" AND
--Min Tran Value = 1000.00 
--Start date:	1-Jan-16
--Offer earn end date:	31-Mar-16
--Offer processed end date:	31-Mar-16


DECLARE @AdjustmentPK uniqueidentifier, @PartnerId uniqueidentifier

SET @AdjustmentPK = newid()
-- Get ChannelPartnerId for Carver
SELECT @PartnerId = ChannelPartnerPK FROM tChannelPartners WHERE ChannelPartnerId = 28

IF NOT EXISTS(SELECT 1 FROM tChannelPartnerFeeAdjustments WHERE Name='5off1000Chk' AND TransactionType=1 and ChannelPartnerPK=@PartnerId)
BEGIN

-- Add Code Promotions in ChannelPartner
-- Transaction Type: 1 – Check
INSERT tChannelPartnerFeeAdjustments(FeeAdjustmentPK, ChannelPartnerPK, TransactionType, Name, Description , PromotionType , DTStart, DTEnd, SystemApplied, adjustmentRate, adjustmentamount, DTServerCreate, DTServerLastModified)
VALUES(@AdjustmentPK, @PartnerId, 1, '5off1000Chk','$5 Off $1000 check to non-check customers','Code', '01/01/2016','03/31/2016', 0, 0, -5, getdate(), getdate())

-- add  "Code" condition with condition value 5off1000Chk
-- ConditionType:  9 - Code , 
-- CompareType: 1 - equal ,
-- ConditionalValue : 5off1000Chk
INSERT tFeeAdjustmentConditions(AdjConditionsPK, FeeAdjustmentPK, ConditionTypePK, CompareTypePK, ConditionValue, DTServerCreate, DTServerLastModified)
VALUES(newid(), @AdjustmentPK, 9, 1, '5off1000Chk', getdate(), getdate())


-- add  "Transaction amount" condition 
-- ConditionType:  3 - Transaction Amount , 
-- CompareType: 7 - greater than / equal ,
-- ConditionalValue : 1000
INSERT tFeeAdjustmentConditions(AdjConditionsPK, FeeAdjustmentPK, ConditionTypePK, CompareTypePK, ConditionValue, DTServerCreate, DTServerLastModified)
VALUES(newid(), @AdjustmentPK, 3, 7, 1000, getdate(), getdate())


-- add  "Check type <> ""Gvt - US Treasury"" condition 
-- ConditionType:  5 - Check Type , 
-- CompareType: 4 - not in
-- Conditional Value : 2
INSERT tFeeAdjustmentConditions(AdjConditionsPK, FeeAdjustmentPK, ConditionTypePK, CompareTypePK, ConditionValue, DTServerCreate, DTServerLastModified)
VALUES(newid(), @AdjustmentPK, 5, 4, 2, getdate(), getdate())

END

GO



