-- ============================================================
-- Author:	   <Swarnalakshmi>
-- Create date: <09/12/2014>
-- Description:	<Alter tWunion_trx_Aud to add missing columns>
-- Rally ID:	
-- ============================================================
IF NOT EXISTS 
(
SELECT 	1   FROM   	sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx_Aud]') 
    AND name = 'PromoCodeDescription'
)
BEGIN
	ALTER TABLE tWUnion_Trx_Aud 
	ADD 
		[PromoCodeDescription] [nvarchar](80) NULL,
		[PromoName] [nvarchar](80) NULL,
		[PromoMessage] [nvarchar](80) NULL,
		[PromotionError] [nvarchar](80) NULL,
		[Sender_ComplianceDetails_ComplianceData_Buffer] [varchar](500) NULL,
		[recordingCountryCode] [nvarchar](20) NULL,
		[recordingCurrencyCode] [nvarchar](20) NULL,
		[originating_city] [nvarchar](100) NULL,
		[originating_state] [nvarchar](100) NULL,
		[municipal_tax] [decimal](18, 2) NULL,
		[state_tax] [decimal](18, 2) NULL,
		[county_tax] [decimal](18, 2) NULL,
		[plus_charges_amount] [decimal](18, 2) NULL,
		[message_charge] [decimal](18, 2) NULL,
		[total_undiscounted_charges] [decimal](18, 2) NULL,
		[total_discount] [decimal](18, 2) NULL,
		[total_discounted_charges] [decimal](18, 2) NULL,
		[instant_notification_addl_service_charges] [nvarchar](300) NULL,
		[PaySideCharges] [decimal](18, 2) NULL,
		[PaySideTax] [decimal](18, 2) NULL,
		[AmountToReceiver] [decimal](18, 2) NULL,
		[SMSNotificationFlag] [varchar](10) NULL,
		[PersonalMessage] [nvarchar](1000) NULL,
		[DeliveryServiceDesc] [varchar](100) NULL,
		[ReferenceNo] [varchar](50) NULL,
		[WUCard_TotalPointsEarned] [varchar](50) NULL,
		[OriginalTransactionID] [bigint] NULL,
		[TransactionSubType] [varchar](20) NULL,
		[ReasonCode] [varchar](20) NULL,
		[ReasonDescription] [varchar](255) NULL,
		[Comments] [varchar](50) NULL,
		[pay_or_do_not_pay_indicator] [varchar](10) NULL,
		[OriginalDestinationCountryCode] [varchar](10) NULL,
		[OriginalDestinationCurrencyCode] [varchar](10) NULL,
		[FilingDate] [varchar](10) NULL,
		[FilingTime] [varchar](10) NULL,
		[PaidDateTime] [nvarchar](50) NULL,
		[AvailableForPickup] [nvarchar](50) NULL,
		[DelayHours] [varchar](10) NULL,
		[AvailableForPickupEST] [varchar](10) NULL,
		[DeliveryOption] [varchar](20) NULL,
		[DeliveryOptionDesc] [varchar](100) NULL,
		[PromotionSequenceNo] [varchar](20) NULL
END
GO
	

