-- Author: Swarnalakshmi Subramani
-- Date Created: Jan 08 2015
-- Description: Adding new coloum IsReferralSectionEnable to tChannelPartnerConfig Table
-- User Story ID: US1800 Task ID: 

IF NOT EXISTS(SELECT * FROM sys.columns where Name = N'IsReferralSectionEnable' and Object_ID = Object_ID(N'tChannelPartnerConfig'))
BEGIN 
	ALTER TABLE tChannelPartnerConfig
	ADD IsReferralSectionEnable BIT NOT NULL DEFAULT(0)
END
GO