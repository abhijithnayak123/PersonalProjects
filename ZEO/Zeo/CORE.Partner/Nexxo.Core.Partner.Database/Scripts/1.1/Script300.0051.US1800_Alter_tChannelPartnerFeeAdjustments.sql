-- Author: SwarnaLakshmi S
-- Date Created: Jan 08 2015
-- Description: Adding new column PromotionType to tChannelPartnerFeeAdjustments
-- User Story ID: US1800 Task ID: 

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'PromotionType' and Object_ID = Object_ID(N'tChannelPartnerFeeAdjustments'))
BEGIN 
	ALTER TABLE tChannelPartnerFeeAdjustments
	ADD PromotionType NVARCHAR(50) NULL
END
GO