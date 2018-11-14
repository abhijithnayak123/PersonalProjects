--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to alter view tWUnion_TrxAud>           
-- Jira ID:	<AL-244>
--===========================================================================================

/****** Object:  Trigger [dbo].[tWUnion_TrxAud]    Script Date: 4/7/2015 3:20:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ============================================================
-- Author:	   <Swarnalakshmi>
-- Create date: <09/12/2014>
-- Description:	<Alter tWunion_trx_Aud Trigger to add missing columns>
-- Rally ID:	
-- ============================================================
ALTER TRIGGER [dbo].[tWUnion_TrxAud] on [dbo].[tWUnion_Trx] AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tWUnion_Trx_Aud where 
      WUTrxID = (select WUTrxID from inserted)
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tWUnion_Trx_Aud(
						[WUTrxPK],	[WUTrxId] ,	[WUAccountPK],
						[WUnionRecipientAccountPK],	[OriginatorsPrincipalAmount],	[OriginatingCountryCode],
						[OriginatingCurrencyCode],	[TranascationType],	[PromotionsCode] ,
						[ExchangeRate],	[DestinationPrincipalAmount],	[GrossTotalAmount],
						[Charges],	[TaxAmount],[Mtcn] ,	[DTCreate],
						[DTLastMod],	[PromotionDiscount] ,	[OtherCharges],
						[MoneyTransferKey],	[AdditionalCharges],[DestinationCountryCode] ,
						[DestinationCurrencyCode] ,	[DestinationState] ,	[IsDomesticTransfer] ,
						[IsFixedOnSend],[PhoneNumber],[Url],[AgencyName],[ChannelPartnerId],
						[ProviderId] ,[TestQuestion],[TempMTCN],[ExpectedPayoutStateCode],
						[ExpectedPayoutCityName],[AuditEvent],[DTAudit],[RevisionNo],
						TestAnswer,TestQuestionAvaliable, [GCNumber] ,[SenderName],
						[PdsRequiredFlag],[DfTransactionFlag],[DeliveryServiceName],
						[DTAvailableForPickup],[RecieverFirstName],[RecieverLastName],
						[RecieverSecondLastName],[PromoCodeDescription],[PromoName],
						[PromoMessage],[PromotionError],
						[Sender_ComplianceDetails_ComplianceData_Buffer],[recordingCountryCode],
						[recordingCurrencyCode],[originating_city],[originating_state],
						[municipal_tax],[state_tax],[county_tax],[plus_charges_amount],
						[message_charge],[total_undiscounted_charges],[total_discount],
						[total_discounted_charges],[instant_notification_addl_service_charges],
						[PaySideCharges],[PaySideTax],[AmountToReceiver],[SMSNotificationFlag],
						[PersonalMessage],[DeliveryServiceDesc],[ReferenceNo],[WUCard_TotalPointsEarned],
						[OriginalTransactionID],[TransactionSubType] ,[ReasonCode],[ReasonDescription],
						[Comments],[pay_or_do_not_pay_indicator] ,[OriginalDestinationCountryCode],
						[OriginalDestinationCurrencyCode],[FilingDate],[FilingTime],[PaidDateTime],
						[AvailableForPickup],[DelayHours] ,[AvailableForPickupEST],[DeliveryOption],
						[DeliveryOptionDesc],[PromotionSequenceNo],
						[DTServerCreate],[DTServerLastMod])
						
              select	[WUTrxPK],	[WUTrxId] ,	[WUAccountPK],
						[WUnionRecipientAccountPK],	[OriginatorsPrincipalAmount],	[OriginatingCountryCode],
						[OriginatingCurrencyCode],	[TranascationType],	[PromotionsCode] ,
						[ExchangeRate],	[DestinationPrincipalAmount],	[GrossTotalAmount],
						[Charges],	[TaxAmount],[Mtcn] ,[DTCreate],
						[DTLastMod],[PromotionDiscount] ,	[OtherCharges],
						[MoneyTransferKey],	[AdditionalCharges],[DestinationCountryCode] ,
						[DestinationCurrencyCode] ,	[DestinationState] ,	[IsDomesticTransfer] ,
						[IsFixedOnSend],[PhoneNumber],[Url],[AgencyName],[ChannelPartnerId],
						[ProviderId] ,[TestQuestion], [TempMTCN],	[ExpectedPayoutStateCode],
						[ExpectedPayoutCityName], 2 as AuditEvent, GETDATE(), @RevisionNo,
					    	TestAnswer,TestQuestionAvaliable,[GCNumber] ,[SenderName],
						[PdsRequiredFlag],[DfTransactionFlag],[DeliveryServiceName],
						[DTAvailableForPickup],[RecieverFirstName],[RecieverLastName],
						[RecieverSecondLastName],[PromoCodeDescription],[PromoName],
						[PromoMessage],[PromotionError],
						[Sender_ComplianceDetails_ComplianceData_Buffer],[recordingCountryCode],
						[recordingCurrencyCode],[originating_city],[originating_state],
						[municipal_tax],[state_tax],[county_tax],[plus_charges_amount],
						[message_charge],[total_undiscounted_charges],[total_discount],
						[total_discounted_charges],[instant_notification_addl_service_charges],
						[PaySideCharges],[PaySideTax],[AmountToReceiver],[SMSNotificationFlag],
						[PersonalMessage],[DeliveryServiceDesc],[ReferenceNo],[WUCard_TotalPointsEarned],
						[OriginalTransactionID],[TransactionSubType] ,[ReasonCode],[ReasonDescription],
						[Comments],[pay_or_do_not_pay_indicator] ,[OriginalDestinationCountryCode],
						[OriginalDestinationCurrencyCode],[FilingDate],[FilingTime],[PaidDateTime],
						[AvailableForPickup],[DelayHours] ,[AvailableForPickupEST],[DeliveryOption],
						[DeliveryOptionDesc],[PromotionSequenceNo],GETDATE() , GETDATE() from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
               insert into tWUnion_Trx_Aud( WUTrxPK,	[WUTrxId] ,	[WUAccountPK],
						[WUnionRecipientAccountPK],	[OriginatorsPrincipalAmount],	[OriginatingCountryCode],
						[OriginatingCurrencyCode],	[TranascationType],	[PromotionsCode] ,
						[ExchangeRate],	[DestinationPrincipalAmount],	[GrossTotalAmount],
						[Charges],	[TaxAmount],[Mtcn] ,	[DTCreate],
						[DTLastMod],	[PromotionDiscount] ,	[OtherCharges],
						[MoneyTransferKey],	[AdditionalCharges],[DestinationCountryCode] ,
						[DestinationCurrencyCode] ,	[DestinationState] ,	[IsDomesticTransfer] ,
						[IsFixedOnSend],[PhoneNumber],[Url],[AgencyName],[ChannelPartnerId],
						[ProviderId] ,[TestQuestion],[TempMTCN],[ExpectedPayoutStateCode],
						[ExpectedPayoutCityName],[AuditEvent],[DTAudit],[RevisionNo],
						TestAnswer,TestQuestionAvaliable, [GCNumber] ,[SenderName],
						[PdsRequiredFlag],[DfTransactionFlag],[DeliveryServiceName],
						[DTAvailableForPickup],[RecieverFirstName],[RecieverLastName],
						[RecieverSecondLastName],[PromoCodeDescription],[PromoName],
						[PromoMessage],[PromotionError],
						[Sender_ComplianceDetails_ComplianceData_Buffer],[recordingCountryCode],
						[recordingCurrencyCode],[originating_city],[originating_state],
						[municipal_tax],[state_tax],[county_tax],[plus_charges_amount],
						[message_charge],[total_undiscounted_charges],[total_discount],
						[total_discounted_charges],[instant_notification_addl_service_charges],
						[PaySideCharges],[PaySideTax],[AmountToReceiver],[SMSNotificationFlag],
						[PersonalMessage],[DeliveryServiceDesc],[ReferenceNo],[WUCard_TotalPointsEarned],
						[OriginalTransactionID],[TransactionSubType] ,[ReasonCode],[ReasonDescription],
						[Comments],[pay_or_do_not_pay_indicator] ,[OriginalDestinationCountryCode],
						[OriginalDestinationCurrencyCode],[FilingDate],[FilingTime],[PaidDateTime],
						[AvailableForPickup],[DelayHours] ,[AvailableForPickupEST],[DeliveryOption],
						[DeliveryOptionDesc],[PromotionSequenceNo],[DTServerCreate],[DTServerLastMod])
						
              select  [WUTrxPK],	[WUTrxId] ,	[WUAccountPK],
						[WUnionRecipientAccountPK],	[OriginatorsPrincipalAmount],	[OriginatingCountryCode],
						[OriginatingCurrencyCode],	[TranascationType],	[PromotionsCode] ,
						[ExchangeRate],	[DestinationPrincipalAmount],	[GrossTotalAmount],
						[Charges],	[TaxAmount],[Mtcn] ,[DTCreate],
						[DTLastMod],[PromotionDiscount] ,	[OtherCharges],
						[MoneyTransferKey],	[AdditionalCharges],[DestinationCountryCode] ,
						[DestinationCurrencyCode] ,	[DestinationState] ,	[IsDomesticTransfer] ,
						[IsFixedOnSend],[PhoneNumber],[Url],[AgencyName],[ChannelPartnerId],
						[ProviderId] ,[TestQuestion], [TempMTCN],	[ExpectedPayoutStateCode],
						[ExpectedPayoutCityName], 2 as AuditEvent, GETDATE(), @RevisionNo,
					    	TestAnswer,TestQuestionAvaliable,[GCNumber] ,[SenderName],
						[PdsRequiredFlag],[DfTransactionFlag],[DeliveryServiceName],
						[DTAvailableForPickup],[RecieverFirstName],[RecieverLastName],
						[RecieverSecondLastName],[PromoCodeDescription],[PromoName],
						[PromoMessage],[PromotionError],
						[Sender_ComplianceDetails_ComplianceData_Buffer],[recordingCountryCode],
						[recordingCurrencyCode],[originating_city],[originating_state],
						[municipal_tax],[state_tax],[county_tax],[plus_charges_amount],
						[message_charge],[total_undiscounted_charges],[total_discount],
						[total_discounted_charges],[instant_notification_addl_service_charges],
						[PaySideCharges],[PaySideTax],[AmountToReceiver],[SMSNotificationFlag],
						[PersonalMessage],[DeliveryServiceDesc],[ReferenceNo],[WUCard_TotalPointsEarned],
						[OriginalTransactionID],[TransactionSubType] ,[ReasonCode],[ReasonDescription],
						[Comments],[pay_or_do_not_pay_indicator] ,[OriginalDestinationCountryCode],
						[OriginalDestinationCurrencyCode],[FilingDate],[FilingTime],[PaidDateTime],
						[AvailableForPickup],[DelayHours] ,[AvailableForPickupEST],[DeliveryOption],
						[DeliveryOptionDesc],[PromotionSequenceNo],GETDATE() , GETDATE() from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tWUnion_Trx_Aud(
						[WUTrxPK],	[WUTrxID] ,	[WUAccountPK],
						[WUnionRecipientAccountPK],	[OriginatorsPrincipalAmount],	[OriginatingCountryCode],
						[OriginatingCurrencyCode],	[TranascationType],	[PromotionsCode] ,
						[ExchangeRate],	[DestinationPrincipalAmount],	[GrossTotalAmount],
						[Charges],	[TaxAmount],[Mtcn] ,[DTCreate],
						[DTLastMod],	[PromotionDiscount] ,[OtherCharges],
						[MoneyTransferKey],	[AdditionalCharges],[DestinationCountryCode] ,
						[DestinationCurrencyCode] ,	[DestinationState] ,	[IsDomesticTransfer] ,
						[IsFixedOnSend],[PhoneNumber],[Url],[AgencyName],[ChannelPartnerId],
						[ProviderId] ,[TestQuestion],[TempMTCN],	[ExpectedPayoutStateCode],
						[ExpectedPayoutCityName],[AuditEvent],[DTAudit],[RevisionNo],
						TestAnswer,TestQuestionAvaliable,[GCNumber] ,[SenderName],
						[PdsRequiredFlag],[DfTransactionFlag],[DeliveryServiceName],
						[DTAvailableForPickup],[RecieverFirstName],[RecieverLastName],
						[RecieverSecondLastName],[PromoCodeDescription],[PromoName],	
						[PromoMessage],[PromotionError],
						[Sender_ComplianceDetails_ComplianceData_Buffer],[recordingCountryCode],
						[recordingCurrencyCode],[originating_city],[originating_state],
						[municipal_tax],[state_tax],[county_tax],[plus_charges_amount],
						[message_charge],[total_undiscounted_charges],[total_discount],
						[total_discounted_charges],[instant_notification_addl_service_charges],
						[PaySideCharges],[PaySideTax],[AmountToReceiver],[SMSNotificationFlag],
						[PersonalMessage],[DeliveryServiceDesc],[ReferenceNo],[WUCard_TotalPointsEarned],
						[OriginalTransactionID],[TransactionSubType] ,[ReasonCode],[ReasonDescription],
						[Comments],[pay_or_do_not_pay_indicator] ,[OriginalDestinationCountryCode],
						[OriginalDestinationCurrencyCode],[FilingDate],[FilingTime],[PaidDateTime],
						[AvailableForPickup],[DelayHours] ,[AvailableForPickupEST],[DeliveryOption],
						[DeliveryOptionDesc],[PromotionSequenceNo],
						[DTServerCreate],[DTServerLastMod])
						
             select [WUTrxPK],	[WUTrxID] ,	[WUAccountPK],
						[WUnionRecipientAccountPK],	[OriginatorsPrincipalAmount],	[OriginatingCountryCode],
						[OriginatingCurrencyCode],	[TranascationType],	[PromotionsCode] ,
						[ExchangeRate],	[DestinationPrincipalAmount],	[GrossTotalAmount],
						[Charges],	[TaxAmount],[Mtcn] ,[DTCreate],
						[DTLastMod],[PromotionDiscount] ,	[OtherCharges],
						[MoneyTransferKey],	[AdditionalCharges],[DestinationCountryCode] ,
						[DestinationCurrencyCode] ,	[DestinationState] ,	[IsDomesticTransfer] ,
						[IsFixedOnSend],[PhoneNumber],[Url],[AgencyName],[ChannelPartnerId],
						[ProviderId] ,[TestQuestion], [TempMTCN],	[ExpectedPayoutStateCode],
						[ExpectedPayoutCityName], 2 as AuditEvent, GETDATE(), @RevisionNo,
					    	TestAnswer,TestQuestionAvaliable,[GCNumber] ,[SenderName],
						[PdsRequiredFlag],[DfTransactionFlag],[DeliveryServiceName],
						[DTAvailableForPickup],[RecieverFirstName],[RecieverLastName],
						[RecieverSecondLastName],[PromoCodeDescription],[PromoName],
						[PromoMessage],[PromotionError],
						[Sender_ComplianceDetails_ComplianceData_Buffer],[recordingCountryCode],
						[recordingCurrencyCode],[originating_city],[originating_state],
						[municipal_tax],[state_tax],[county_tax],[plus_charges_amount],
						[message_charge],[total_undiscounted_charges],[total_discount],
						[total_discounted_charges],[instant_notification_addl_service_charges],
						[PaySideCharges],[PaySideTax],[AmountToReceiver],[SMSNotificationFlag],
						[PersonalMessage],[DeliveryServiceDesc],[ReferenceNo],[WUCard_TotalPointsEarned],
						[OriginalTransactionID],[TransactionSubType] ,[ReasonCode],[ReasonDescription],
						[Comments],[pay_or_do_not_pay_indicator] ,[OriginalDestinationCountryCode],
						[OriginalDestinationCurrencyCode],[FilingDate],[FilingTime],[PaidDateTime],
						[AvailableForPickup],[DelayHours] ,[AvailableForPickupEST],[DeliveryOption],
						[DeliveryOptionDesc],[PromotionSequenceNo],GETDATE() , GETDATE() from inserted
       END
GO


