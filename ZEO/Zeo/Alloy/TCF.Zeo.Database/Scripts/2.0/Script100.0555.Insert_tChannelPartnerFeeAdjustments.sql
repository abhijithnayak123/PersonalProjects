--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <13-06-2017>
-- Description:	Create the script to add "THREETHENFREE" promotion.
-- Jira ID:		<AL-7705>
-- ================================================================================

-- Add new conditions to get transaction count after promotion applied.

IF NOT EXISTS(SELECT 1 FROM tFeeAdjustmentConditionTypes WHERE ConditionTypeId = 10)
BEGIN

  INSERT INTO tFeeAdjustmentConditionTypes
              (
			   ConditionTypeId,
			   Name
			  )
       VALUES (
	           10,
               'Aggregate'
			  )
END


DECLARE @feeAdjustmentId BIGINT
 
-- Add Manual promo code ('THREETHENFREE') to tChannelPartnerFeeAdjustments  

IF NOT EXISTS(SELECT 1 FROM tChannelPartnerFeeAdjustments WHERE Name = 'THREETHENFREE' AND SystemApplied = 0 AND PromotionType = 'Code' AND DTStart = '2017-08-16 00:00:00.000' AND DTEnd = '2017-09-30 00:00:00.000')
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
           ,'THREETHENFREE'
           ,NULL
           ,'2017-08-16'
           ,'2017-09-30'
           ,0
           ,0
           ,0
           ,GETDATE()
           ,NULL
           ,'Code'
           ,34
		   )

END

SELECT @feeAdjustmentId = FeeAdjustmentId FROM tChannelPartnerFeeAdjustments WHERE Name = 'THREETHENFREE' AND SystemApplied = 0  AND PromotionType = 'Code' AND DTStart = '2017-08-16 00:00:00.000' AND DTEnd = '2017-09-30 00:00:00.000'

-- 1. Add conditions to validate promo code

IF NOT EXISTS( SELECT 1 FROM tFeeAdjustmentConditions WHERE FeeAdjustmentId = @feeAdjustmentId AND ConditionTypeId = 1 AND CompareTypeId = 1)
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
           ,1
           ,1
           ,'THREETHENFREE'
           ,GETDATE()
		   ,NULL
           ,@feeAdjustmentId
		   )

END

--  3. Add condition to validate transaction count after appliying the promo code.

IF NOT EXISTS( SELECT 1 FROM tFeeAdjustmentConditions WHERE FeeAdjustmentId = @feeAdjustmentId AND ConditionTypeId = 10 AND CompareTypeId = 6)
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
           ,10
           ,6
           ,'3'
           ,GETDATE()
		   ,NULL
           ,@feeAdjustmentId
		   )
END

-- Add System Applied Promocode for 4th check free after applied the promocode "THREETHENFREE".

IF NOT EXISTS(SELECT 1 FROM tChannelPartnerFeeAdjustments WHERE Name = 'THREETHENFREE' AND SystemApplied = 0 AND DTStart = '2017-10-01 00:00:00.000' AND DTEnd = '2017-10-30 00:00:00.000')
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
           ,'THREETHENFREE'
           ,NULL
           ,'2017-10-01'
           ,'2017-10-30'
           ,0
           ,0
           ,0
           ,GETDATE()
           ,NULL
           ,'Code'
           ,34
		   )

END

SELECT @feeAdjustmentId = FeeAdjustmentId FROM tChannelPartnerFeeAdjustments WHERE Name = 'THREETHENFREE' AND DTStart = '2017-10-01 00:00:00.000' AND DTEnd = '2017-10-30 00:00:00.000'   

--1. Add group promo code condition.

IF NOT EXISTS( SELECT 1 FROM tFeeAdjustmentConditions WHERE FeeAdjustmentId = @feeAdjustmentId AND ConditionTypeId = 1)
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
           ,1
           ,1
           ,'THREETHENFREE'
           ,GETDATE()
		   ,NULL
           ,@feeAdjustmentId
		   )

END

--  3.Add condition to validate transaction count after appliying the promo code.

IF NOT EXISTS( SELECT 1 FROM tFeeAdjustmentConditions WHERE FeeAdjustmentId = @feeAdjustmentId AND ConditionTypeId = 10 AND CompareTypeId = 7)
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
           ,10
           ,7
           ,'1'
           ,GETDATE()
		   ,NULL
           ,@feeAdjustmentId
		   )
END

--  4.Add condition to validate transaction count after appliying the promo code.

IF NOT EXISTS( SELECT 1 FROM tFeeAdjustmentConditions WHERE FeeAdjustmentId = @feeAdjustmentId AND ConditionTypeId = 10 AND CompareTypeId = 6)
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
           ,10
           ,6
           ,'3'
           ,GETDATE()
		   ,NULL
           ,@feeAdjustmentId
		   )
END

IF NOT EXISTS(SELECT 1 FROM tChannelPartnerFeeAdjustments WHERE Name = 'THREETHENFREE' AND DTStart = '2017-08-16 00:00:00.000' AND DTEnd = '2017-10-30 00:00:00.000')
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
           ,'THREETHENFREE'
           ,NULL
           ,'2017-08-16'
           ,'2017-10-30'
           ,0
           ,-1
           ,0
           ,GETDATE()
           ,NULL
           ,'Code'
           ,34
		   )

END

SELECT @feeAdjustmentId = FeeAdjustmentId FROM tChannelPartnerFeeAdjustments WHERE Name = 'THREETHENFREE' AND DTStart = '2017-08-16 00:00:00.000' AND DTEnd = '2017-10-30 00:00:00.000' 

--1. Add group promo code condition.

IF NOT EXISTS( SELECT 1 FROM tFeeAdjustmentConditions WHERE FeeAdjustmentId = @feeAdjustmentId AND ConditionTypeId = 1)
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
           ,1
           ,1
           ,'THREETHENFREE'
           ,GETDATE()
		   ,NULL
           ,@feeAdjustmentId
		   )

END

-- 1. Add conditions to validate the amount (<=1500)

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

-- 2. Add conditions to validate the No of transactions after promocode, it will verify the 4th check to be free. 

IF NOT EXISTS( SELECT 1 FROM tFeeAdjustmentConditions WHERE FeeAdjustmentId = @feeAdjustmentId AND ConditionTypeId = 10 AND CompareTypeId = 1)
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
           ,10
           ,1
           ,'3'
           ,GETDATE()
		   ,NULL
           ,@feeAdjustmentId
		   )
END


-- Add Group promo in tChannelPartnerGroups table

IF NOT EXISTS( select 1 from tChannelPartnerGroups where Name = 'THREETHENFREE')
BEGIN

INSERT INTO [dbo].[tChannelPartnerGroups]
           ([Name]
           ,[DTServerCreate]
           ,[DTServerLastModified]
           ,[ChannelPartnerID])
     VALUES
           (
		   'THREETHENFREE'
           ,GETDATE()
           ,NULL
           ,34
		   )

END
GO