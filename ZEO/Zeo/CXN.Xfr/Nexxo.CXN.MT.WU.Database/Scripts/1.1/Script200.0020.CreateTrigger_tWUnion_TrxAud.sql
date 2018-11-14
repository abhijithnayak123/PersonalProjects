/****** Object:  Trigger [tWUnion_TrxAud]    Script Date: 12/10/2013 18:57:04 ******/
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_TrxAud]'))
DROP TRIGGER [dbo].[tWUnion_TrxAud]
GO


/****** Object:  Trigger [dbo].[tWUnion_TrxAud]    Script Date: 12/10/2013 18:57:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


Create trigger [dbo].[tWUnion_TrxAud] on [dbo].[tWUnion_Trx] AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tWUnion_Trx_Aud where 
       Id = (select Id from inserted)
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tWUnion_Trx_Aud(
						[rowguid],	[Id] ,	[WUnionAccountPK],
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
						[RecieverSecondLastName],[DTServerCreate],[DTServerLastMod])
              select	[rowguid],	[Id] ,	[WUnionAccountPK],
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
						[RecieverSecondLastName],GETDATE() , GETDATE() from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
               insert into tWUnion_Trx_Aud( [rowguid],	[Id] ,	[WUnionAccountPK],
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
						[RecieverSecondLastName],[DTServerCreate],[DTServerLastMod])
						
              select  [rowguid],	[Id] ,	[WUnionAccountPK],
						[WUnionRecipientAccountPK],	[OriginatorsPrincipalAmount],	[OriginatingCountryCode],
						[OriginatingCurrencyCode],	[TranascationType],	[PromotionsCode] ,
						[ExchangeRate],	[DestinationPrincipalAmount],	[GrossTotalAmount],
						[Charges],	[TaxAmount],[Mtcn] ,[DTCreate],
						[DTLastMod],[PromotionDiscount] ,	[OtherCharges],
						[MoneyTransferKey],	[AdditionalCharges],[DestinationCountryCode] ,
						[DestinationCurrencyCode] ,	[DestinationState] ,	[IsDomesticTransfer] ,
						[IsFixedOnSend],[PhoneNumber],[Url],[AgencyName],[ChannelPartnerId],
						[ProviderId] ,[TestQuestion], [TempMTCN],	[ExpectedPayoutStateCode],
						[ExpectedPayoutCityName], 1 as AuditEvent, GETDATE(), @RevisionNo,
					    TestAnswer,TestQuestionAvaliable,[GCNumber] ,[SenderName],
						[PdsRequiredFlag],[DfTransactionFlag],[DeliveryServiceName],
						[DTAvailableForPickup],[RecieverFirstName],[RecieverLastName],
						[RecieverSecondLastName],GETDATE(), GETDATE() from inserted
     end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tWUnion_Trx_Aud(
						[rowguid],	[Id] ,	[WUnionAccountPK],
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
						[RecieverSecondLastName],[DTServerCreate],[DTServerLastMod])
             select  [rowguid],	[Id] ,	[WUnionAccountPK],
						[WUnionRecipientAccountPK],	[OriginatorsPrincipalAmount],	[OriginatingCountryCode],
						[OriginatingCurrencyCode],	[TranascationType],	[PromotionsCode] ,
						[ExchangeRate],	[DestinationPrincipalAmount],	[GrossTotalAmount],
						[Charges],	[TaxAmount],[Mtcn] ,[DTCreate],
						[DTLastMod],[PromotionDiscount] ,	[OtherCharges],
						[MoneyTransferKey],	[AdditionalCharges],[DestinationCountryCode] ,
						[DestinationCurrencyCode] ,	[DestinationState] ,	[IsDomesticTransfer] ,
						[IsFixedOnSend],[PhoneNumber],[Url],[AgencyName],[ChannelPartnerId],
						[ProviderId] ,[TestQuestion], [TempMTCN],	[ExpectedPayoutStateCode],
						[ExpectedPayoutCityName], 3 as AuditEvent, GETDATE(), @RevisionNo,
					    TestAnswer,TestQuestionAvaliable,[GCNumber] ,[SenderName],
						[PdsRequiredFlag],[DfTransactionFlag],[DeliveryServiceName],
						[DTAvailableForPickup],[RecieverFirstName],[RecieverLastName],
						[RecieverSecondLastName],GETDATE(), GETDATE() from deleted
       end


GO


