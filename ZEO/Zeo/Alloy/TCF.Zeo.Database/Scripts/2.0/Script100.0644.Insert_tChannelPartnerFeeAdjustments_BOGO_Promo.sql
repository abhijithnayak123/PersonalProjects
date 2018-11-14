--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <03-11-2017>
-- Description:	Create the script to add "BOGO" promotion.
-- Version ONE ID:		<B-09843>
-- ================================================================================

-- Add new conditions to get transaction count after promotion applied.

IF NOT EXISTS(SELECT 1 FROM tFeeAdjustmentConditionTypes WHERE ConditionTypeId = 11)
BEGIN

  INSERT INTO tFeeAdjustmentConditionTypes
              (
			   ConditionTypeId,
			   Name
			  )
       VALUES (
	           11,
               'CommittedTransactionCount'
			  )
END


DECLARE @feeAdjustmentId BIGINT
 
-- Add Manual promo code ('BOGO') to tChannelPartnerFeeAdjustments  

IF NOT EXISTS(SELECT 1 FROM tChannelPartnerFeeAdjustments WHERE Name = 'BOGO' AND SystemApplied = 0 AND PromotionType = 'Code' AND DTStart = '2018-01-01 00:00:00.000' AND DTEnd = '2018-04-15 00:00:00.000')
BEGIN  

INSERT INTO [dbo].[tChannelPartnerFeeAdjustments]
           (
            [TransactionType]
           ,[Name]
           ,[Description]
           ,[DTStart]
           ,[DTEnd]
           ,[SystemApplied]
           ,[AdjustmentRate]
           ,[AdjustmentAmount]
           ,[DTServerCreate]
           ,[DTServerLastModified]
           ,[PromotionType]
           ,[ChannelPartnerId])
     VALUES
           (
            1
           ,'BOGO'
           ,'Buy one, get one free -1'
           ,'2018-01-01'
           ,'2018-04-15'
           ,0
           ,0
           ,0
           ,GETDATE()
           ,NULL
           ,'Code'
           ,34
		   )

END

SELECT @feeAdjustmentId = FeeAdjustmentId FROM tChannelPartnerFeeAdjustments WHERE Name = 'BOGO' AND SystemApplied = 0  AND PromotionType = 'Code' AND DTStart = '2018-01-01 00:00:00.000' AND DTEnd = '2018-04-15 00:00:00.000'

-- 1. Add conditions to validate promo code

IF NOT EXISTS( SELECT 1 FROM tFeeAdjustmentConditions WHERE FeeAdjustmentId = @feeAdjustmentId AND ConditionTypeId = 9 AND CompareTypeId = 1)
BEGIN

INSERT INTO [dbo].[tFeeAdjustmentConditions]
           (
            [Description]
           ,[ConditionTypeId]
           ,[CompareTypeId]
           ,[ConditionValue]
           ,[DTServerCreate]
           ,[DTServerLastModified]
           ,[FeeAdjustmentId])
     VALUES
           (
            ''
           ,9
           ,1
           ,'BOGO'
           ,GETDATE()
		   ,NULL
           ,@feeAdjustmentId
		   )

END

--  2. Add condition to validate transaction count for this promo code. It should be applied only once for the customer

IF NOT EXISTS( SELECT 1 FROM tFeeAdjustmentConditions WHERE FeeAdjustmentId = @feeAdjustmentId AND ConditionTypeId = 11 AND CompareTypeId = 6)
BEGIN
INSERT INTO [dbo].[tFeeAdjustmentConditions]
           (
            [Description]
           ,[ConditionTypeId]
           ,[CompareTypeId]
           ,[ConditionValue]
           ,[DTServerCreate]
           ,[DTServerLastModified]
           ,[FeeAdjustmentId])
     VALUES
           (
            ''
           ,11
           ,6
           ,'1'
           ,GETDATE()
		   ,NULL
           ,@feeAdjustmentId
		   )
END

-- Add System Applied Promocode for 2nd check free after applied the promocode "BOGO".

IF NOT EXISTS(SELECT 1 FROM tChannelPartnerFeeAdjustments WHERE Name = 'BOGO' AND SystemApplied = 1 AND DTStart = '2018-01-01 00:00:00.000' AND DTEnd = '2018-05-15 00:00:00.000')
BEGIN  

INSERT INTO [dbo].[tChannelPartnerFeeAdjustments]
           (
            [TransactionType]
           ,[Name]
           ,[Description]
           ,[DTStart]
           ,[DTEnd]
           ,[SystemApplied]
           ,[AdjustmentRate]
           ,[AdjustmentAmount]
           ,[DTServerCreate]
           ,[DTServerLastModified]
           ,[ChannelPartnerId])
     VALUES
           (
            1
           ,'BOGO'
           ,'Buy one, get one free -2'
           ,'2018-01-01'
           ,'2018-05-15'
           ,1
           ,-1
           ,0
           ,GETDATE()
           ,NULL
           ,34
		   )

END

SELECT @feeAdjustmentId = FeeAdjustmentId FROM tChannelPartnerFeeAdjustments WHERE Name = 'BOGO' AND DTStart = '2018-01-01 00:00:00.000' AND DTEnd = '2018-05-15 00:00:00.000'   

--  1. Add condition to validate committed transaction count for this promocode.

IF NOT EXISTS( SELECT 1 FROM tFeeAdjustmentConditions WHERE FeeAdjustmentId = @feeAdjustmentId AND ConditionTypeId = 11 AND CompareTypeId = 1)
BEGIN
INSERT INTO [dbo].[tFeeAdjustmentConditions]
           (
            [Description]
           ,[ConditionTypeId]
           ,[CompareTypeId]
           ,[ConditionValue]
           ,[DTServerCreate]
           ,[DTServerLastModified]
           ,[FeeAdjustmentId])
     VALUES
           (
            ''
           ,11
           ,1
           ,'1'
           ,GETDATE()
		   ,NULL
           ,@feeAdjustmentId
		   )
END

-- 2. Add conditions to validate the amount (<=1500)

IF NOT EXISTS( SELECT 1 FROM tFeeAdjustmentConditions WHERE FeeAdjustmentId = @feeAdjustmentId AND ConditionTypeId = 3 AND CompareTypeId = 8)
BEGIN

INSERT INTO [dbo].[tFeeAdjustmentConditions]
           (
            [Description]
           ,[ConditionTypeId]
           ,[CompareTypeId]
           ,[ConditionValue]
           ,[DTServerCreate]
           ,[DTServerLastModified]
           ,[FeeAdjustmentId])
     VALUES
           (
		    ''
           ,3
           ,8
           ,'1500'
           ,GETDATE()
		   ,NULL
           ,@feeAdjustmentId
		   )

END

GO