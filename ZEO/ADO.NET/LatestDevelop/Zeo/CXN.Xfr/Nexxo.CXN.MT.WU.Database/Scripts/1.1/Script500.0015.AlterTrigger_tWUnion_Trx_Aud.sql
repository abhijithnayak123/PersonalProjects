--===========================================================================================
-- Author:		<Shwetha Mohan>
-- Created date: <September 25 2015>
-- Description:	< Alter tigger to include columns CounterId,TransalatedDeliveryServiceName
--			     in tWUnion_Trx_Aud table>           
-- Jira ID:	<AL-2018>
--===========================================================================================


IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_TrxAud]'))
DROP TRIGGER [dbo].[tWUnion_TrxAud]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[tWUnion_TrxAud] on [dbo].[tWUnion_Trx] AFTER Insert, Update, Delete
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
						[Charges],	[TaxAmount],[Mtcn] ,	[DTTerminalCreate],
						[DTTerminalLastModified],	[PromotionDiscount] ,	[OtherCharges],
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
						[DTServerCreate],[DTServerLastModified],[Receiver_unv_Buffer],[Sender_unv_Buffer],
						[PrincipalAmount],[CounterId],[TransalatedDeliveryServiceName])
						
              select	[WUTrxPK],	[WUTrxId] ,	[WUAccountPK],
						[WUnionRecipientAccountPK],	[OriginatorsPrincipalAmount],	[OriginatingCountryCode],
						[OriginatingCurrencyCode],	[TranascationType],	[PromotionsCode] ,
						[ExchangeRate],	[DestinationPrincipalAmount],	[GrossTotalAmount],
						[Charges],	[TaxAmount],[Mtcn] ,[DTTerminalCreate],
						[DTTerminalLastModified],[PromotionDiscount] ,	[OtherCharges],
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
						[DeliveryOptionDesc],[PromotionSequenceNo],GETDATE() , GETDATE(),[Receiver_unv_Buffer],
						[Sender_unv_Buffer],[PrincipalAmount],[CounterId],[TransalatedDeliveryServiceName] 
						from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
               insert into tWUnion_Trx_Aud( WUTrxPK,	[WUTrxId] ,	[WUAccountPK],
						[WUnionRecipientAccountPK],	[OriginatorsPrincipalAmount],	[OriginatingCountryCode],
						[OriginatingCurrencyCode],	[TranascationType],	[PromotionsCode] ,
						[ExchangeRate],	[DestinationPrincipalAmount],	[GrossTotalAmount],
						[Charges],	[TaxAmount],[Mtcn] ,	[DTTerminalCreate],
						[DTTerminalLastModified],	[PromotionDiscount] ,	[OtherCharges],
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
						[DeliveryOptionDesc],[PromotionSequenceNo],[DTServerCreate],[DTServerLastModified],
						[Receiver_unv_Buffer],[Sender_unv_Buffer],[PrincipalAmount],
						[CounterId],[TransalatedDeliveryServiceName])
						
			select  [WUTrxPK],	[WUTrxId] ,	[WUAccountPK],
				[WUnionRecipientAccountPK],	[OriginatorsPrincipalAmount],	[OriginatingCountryCode],
				[OriginatingCurrencyCode],	[TranascationType],	[PromotionsCode] ,
				[ExchangeRate],	[DestinationPrincipalAmount],	[GrossTotalAmount],
				[Charges],	[TaxAmount],[Mtcn] ,[DTTerminalCreate],
				[DTTerminalLastModified],[PromotionDiscount] ,	[OtherCharges],
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
				[DeliveryOptionDesc],[PromotionSequenceNo],GETDATE() , GETDATE(),[Receiver_unv_Buffer],
				[Sender_unv_Buffer],[PrincipalAmount],[CounterId],[TransalatedDeliveryServiceName] 
			from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tWUnion_Trx_Aud(
					[WUTrxPK],	[WUTrxID] ,	[WUAccountPK],
					[WUnionRecipientAccountPK],	[OriginatorsPrincipalAmount],	[OriginatingCountryCode],
					[OriginatingCurrencyCode],	[TranascationType],	[PromotionsCode] ,
					[ExchangeRate],	[DestinationPrincipalAmount],	[GrossTotalAmount],
					[Charges],	[TaxAmount],[Mtcn] ,[DTTerminalCreate],
					[DTTerminalLastModified],	[PromotionDiscount] ,[OtherCharges],
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
					[DTServerCreate],[DTServerLastModified],[Receiver_unv_Buffer],[Sender_unv_Buffer],
					[PrincipalAmount],[CounterId],[TransalatedDeliveryServiceName])
						
             select [WUTrxPK],	[WUTrxID] ,	[WUAccountPK],
					[WUnionRecipientAccountPK],	[OriginatorsPrincipalAmount],	[OriginatingCountryCode],
					[OriginatingCurrencyCode],	[TranascationType],	[PromotionsCode] ,
					[ExchangeRate],	[DestinationPrincipalAmount],	[GrossTotalAmount],
					[Charges],	[TaxAmount],[Mtcn] ,[DTTerminalCreate],
					[DTTerminalLastModified],[PromotionDiscount] ,	[OtherCharges],
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
					[DeliveryOptionDesc],[PromotionSequenceNo],GETDATE() , GETDATE(),[Receiver_unv_Buffer],
					[Sender_unv_Buffer],[PrincipalAmount], [CounterId],[TransalatedDeliveryServiceName] 
					from inserted
       END
GO


