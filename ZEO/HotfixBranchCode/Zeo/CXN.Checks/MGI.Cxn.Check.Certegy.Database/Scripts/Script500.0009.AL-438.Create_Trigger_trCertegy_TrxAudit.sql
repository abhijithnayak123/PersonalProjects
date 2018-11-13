-- ============================================================
-- Author:		<Mohammed Khaja Firoz Molla>
-- Create date: <07/03/2015>
-- Description:	<DDL script to create trCertegy_TrxAudit Trigger>
-- Jira ID:	<AL-438>
-- ============================================================
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[trCertegy_TrxAudit] on [dbo].[tCertegy_Trx] AFTER INSERT, UPDATE, DELETE
AS
       SET NOCOUNT ON
       DECLARE @RevisionNo BIGINT
       SELECT @RevisionNo = ISNULL(MAX(RevisionNo),0) + 1 FROM tCertegy_Trx_AUD WHERE CertegyTrxID = (SELECT CertegyTrxID FROM INSERTED)
             
       IF ((SELECT COUNT(*) FROM INSERTED)<>0 and (SELECT COUNT(*) from DELETED)>0)
       BEGIN
           INSERT INTO [dbo].[tCertegy_Trx_AUD]
           ([CertegyTrxPK]
           ,[CertegyTrxID]
           ,[CheckAmount]
           ,[CertegyFee]
           ,[CheckDate]
           ,[CheckNumber]
           ,[RoutingNumber]
           ,[AccountNumber]
           ,[Micr]
           ,[CertegyUID]
           ,[ApprovalNumber]
           ,[SettlementID]
           ,[ResponseCode]
           ,[SiteID]
           ,[CertegyAccountPK]
           ,[AlloySubmitType]
           ,[AlloyReturnType]
           ,[CertegySubmitType]
           ,[CertegyReturnType]
           ,[TranType]
           ,[FundsAvail]
           ,[Version]
           ,[IdType]
           ,[ExpansionType]
           ,[MICREntryType]
           ,[DeviceType]
           ,[DeviceId]
           ,[DeviceIP]
           ,[ChannelPartnerID]
           ,[IsCheckFranked]
           ,[DTTerminalCreate]
           ,[DTTerminalLastModified]
           ,[DTServerCreate]
           ,[DTServerLastModified]
		   ,[RevisionNo]
           ,[AuditEvent]
           ,[DTAudit]
           )

       SELECT [CertegyTrxPK], [CertegyTrxID], [CheckAmount], [CertegyFee], [CheckDate], [CheckNumber], [RoutingNumber], [AccountNumber], [Micr], [CertegyUID], [ApprovalNumber], [SettlementID], [ResponseCode], [SiteID], [CertegyAccountPK], [AlloySubmitType], [AlloyReturnType], [CertegySubmitType], [CertegyReturnType], [TranType], [FundsAvail],[Version], [IdType], [ExpansionType], [MICREntryType], [DeviceType], [DeviceId], [DeviceIP], [ChannelPartnerID], [IsCheckFranked], [DTTerminalCreate], [DTTerminalCreate], [DTServerCreate], [DTServerLastModified], @RevisionNo, 2 as [AuditEvent], GETDATE() FROM INSERTED
       END
       ELSE IF(SELECT COUNT(*) FROM INSERTED)>0 AND (SELECT COUNT(*) FROM DELETED)=0
       BEGIN
              INSERT INTO [dbo].[tCertegy_Trx_AUD]
           ([CertegyTrxPK]
           ,[CertegyTrxID]
           ,[CheckAmount]
           ,[CertegyFee]
           ,[CheckDate]
           ,[CheckNumber]
           ,[RoutingNumber]
           ,[AccountNumber]
           ,[Micr]
           ,[CertegyUID]
           ,[ApprovalNumber]
           ,[SettlementID]
           ,[ResponseCode]
           ,[SiteID]
           ,[CertegyAccountPK]
           ,[AlloySubmitType]
           ,[AlloyReturnType]
           ,[CertegySubmitType]
           ,[CertegyReturnType]
           ,[TranType]
           ,[FundsAvail]
           ,[Version]
           ,[IdType]
           ,[ExpansionType]
           ,[MICREntryType]
           ,[DeviceType]
           ,[DeviceId]
           ,[DeviceIP]
           ,[ChannelPartnerID]
           ,[IsCheckFranked]
           ,[DTTerminalCreate]
           ,[DTTerminalLastModified]
           ,[DTServerCreate]
           ,[DTServerLastModified]
      	   ,[RevisionNo]
           ,[AuditEvent]
           ,[DTAudit])
              SELECT [CertegyTrxPK], [CertegyTrxID], [CheckAmount], [CertegyFee], [CheckDate], [CheckNumber], [RoutingNumber], [AccountNumber], [Micr], [CertegyUID], [ApprovalNumber], [SettlementID], [ResponseCode], [SiteID], [CertegyAccountPK], [AlloySubmitType], [AlloyReturnType], [CertegySubmitType], [CertegyReturnType], [TranType], [FundsAvail],[Version], [IdType], [ExpansionType], [MICREntryType], [DeviceType], [DeviceId], [DeviceIP], [ChannelPartnerID], [IsCheckFranked], [DTTerminalCreate], [DTTerminalLastModified], [DTServerCreate], [DTServerLastModified], @RevisionNo, 1 as [AuditEvent], GETDATE() FROM INSERTED
       END
       ELSE IF(SELECT COUNT(*) FROM DELETED)>0
       BEGIN
              INSERT INTO [dbo].[tCertegy_Trx_AUD]
           ([CertegyTrxPK]
           ,[CertegyTrxID]
           ,[CheckAmount]
           ,[CertegyFee]
           ,[CheckDate]
           ,[CheckNumber]
           ,[RoutingNumber]
           ,[AccountNumber]
           ,[Micr]
           ,[CertegyUID]
           ,[ApprovalNumber]
           ,[SettlementID]
           ,[ResponseCode]
           ,[SiteID]
           ,[CertegyAccountPK]
           ,[AlloySubmitType]
           ,[AlloyReturnType]
           ,[CertegySubmitType]
           ,[CertegyReturnType]
           ,[TranType]
           ,[FundsAvail]
           ,[Version]
           ,[IdType]
           ,[ExpansionType]
           ,[MICREntryType]
           ,[DeviceType]
           ,[DeviceId]
           ,[DeviceIP]
           ,[ChannelPartnerID]
           ,[IsCheckFranked]
           ,[DTTerminalCreate]
           ,[DTTerminalLastModified]
           ,[DTServerCreate]
           ,[DTServerLastModified]
           ,[RevisionNo]
           ,[AuditEvent]
           ,[DTAudit])
          SELECT  [CertegyTrxPK], [CertegyTrxID], [CheckAmount], [CertegyFee], [CheckDate], [CheckNumber], [RoutingNumber], [AccountNumber], [Micr], [CertegyUID], [ApprovalNumber], [SettlementID], [ResponseCode], [SiteID], [CertegyAccountPK], [AlloySubmitType], [AlloyReturnType], [CertegySubmitType], [CertegyReturnType], [TranType], [FundsAvail],[Version], [IdType], [ExpansionType], [MICREntryType], [DeviceType], [DeviceId], [DeviceIP], [ChannelPartnerID], [IsCheckFranked], [DTTerminalCreate], [DTTerminalLastModified], [DTServerCreate], [DTServerLastModified], @RevisionNo, 3 as [AuditEvent], GETDATE() FROM DELETED
       END
GO


