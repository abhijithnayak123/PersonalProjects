--- ===============================================================================
-- Author:		<Rizwana Shaik>
-- Create date: <1-12-2017>
-- Description:	Create procedure for getting channelPartner Id or name
-- Jira ID:		<AL-7580>

-- EXEC usp_GetChannelPartnerByChannelPartnerIdOrName NULL,34
-- ================================================================================


IF OBJECT_ID(N'usp_GetChannelPartnerByChannelPartnerIdOrName', N'P') IS NOT NULL
DROP PROCEDURE usp_GetChannelPartnerByChannelPartnerIdOrName   -- Drop the existing procedure.
GO

CREATE PROCEDURE [dbo].[usp_GetChannelPartnerByChannelPartnerIdOrName]
(
@channelPartnerName NVARCHAR(100)= NULL,
	@channelPartnerId INT = NULL
)
AS
BEGIN
	BEGIN TRY

		SELECT 
			 PC.ChannelPartnerID 
			,PC.DisableWithdrawCNP  
			,PC.CashOverCounter 
			,PC.FrankData  
			,PC.IsCheckFrank  
			,PC.IsNotesEnable  
			,PC.IsReferralSectionEnable  
			,PC.IsMGIAlloyLogoEnable  
			,PC.MasterSSN  
			,PC.IsMailingAddressEnable  
			,PC.CanEnableProfileStatus 
			,PC.CustomerMinimumAge  
			,CP.Name 
			,CP.FeesFollowCustomer 
			,CP.CashFeeDescriptionEN 
			,CP.CashFeeDescriptionES  
			,CP.DebitFeeDescriptionEN 
			,CP.DebitFeeDescriptionES  
			,CP.ConvenienceFeeCash 
			,CP.ConvenienceFeeDebit 
			,CP.ConvenienceFeeDescriptionEN  
			,CP.ConvenienceFeeDescriptionES 
			,CP.CanCashCheckWOGovtId 
			,CP.LogoFileName 
			,CP.IsEFSPartner 
			,CP.EFSClientId  
			,CP.UsePINForNonGPR  
			,CP.IsCUPartner 
			,CP.TIM  
			,CP.HasNonGPRCard 
			,CP.ManagesCash  
			,CP.AllowPhoneNumberAuthentication 
			,CP.CardPresenceVerificationConfig  
			,CP.ComplianceProgramName 
		FROM tChannelPartnerConfig PC 
		left outer join tChannelPartners CP on PC.ChannelPartnerID=CP.ChannelPartnerId
		WHERE (lower(CP.Name)= lower(@channelPartnerName)) OR (CP.ChannelPartnerId= @channelPartnerId);

	END TRY
	BEGIN CATCH	        
	-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END
