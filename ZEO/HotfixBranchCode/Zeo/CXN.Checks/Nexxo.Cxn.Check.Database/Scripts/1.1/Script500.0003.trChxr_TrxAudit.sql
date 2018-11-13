--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to alter trigger tChxr_Trx>           
-- Jira ID:	<AL-244>
--===========================================================================================

/****** Object:  Trigger [dbo].[trChxr_TrxAudit]    Script Date: 4/7/2015 12:48:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER trigger [dbo].[trChxr_TrxAudit] on [dbo].[tChxr_Trx] AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tChxr_Trx_Aud where ChxrTrxID = (select ChxrTrxID from inserted)
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tChxr_Trx_Aud(
                              ChxrTrxPK,
                     ChxrTrxID,
                     Amount,
                     ChexarAmount,
                     ChexarFee,
                     CheckDate,
                     CheckNumber,
                     RoutingNumber,
                     AccountNumber,
                     Micr,
                     Latitude,
                     Longitude,
                     InvoiceId,
                     TicketId,
                     WaitTime,
                     [Status],
                     ChexarStatus,
                     DeclineCode,
                     [Message],
                     Location,
                     ChxrAccountPK,
                     DTCreate,
                     DTLastMod,
                     SubmitType,
                     ReturnType,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     ChannelPartnerID,
                     DTServerCreate,
                     DTServerLastMod,
                     IsCheckFranked
                     )
              select ChxrTrxPK, ChxrTrxID, Amount, ChexarAmount, ChexarFee, CheckDate, CheckNumber, RoutingNumber, AccountNumber, Micr, Latitude, Longitude, InvoiceId, TicketId, WaitTime, [Status], ChexarStatus, DeclineCode, [Message], Location, ChxrAccountPK, DTCreate, DTLastMod, SubmitType, ReturnType,@RevisionNo,2 as AuditEvent,GETDATE(),ChannelPartnerID,DTServerCreate,DTServerLastMod,IsCheckFranked from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
              insert into tChxr_Trx_Aud(
                              ChxrTrxPK,
                     ChxrTrxID,
                     Amount,
                     ChexarAmount,
                     ChexarFee,
                     CheckDate,
                     CheckNumber,
                     RoutingNumber,
                     AccountNumber,
                     Micr,
                     Latitude,
                     Longitude,
                     InvoiceId,
                     TicketId,
                     WaitTime,
                     [Status],
                     ChexarStatus,
                     DeclineCode,
                     [Message],
                     Location,
                     ChxrAccountPK,
                     DTCreate,
                     DTLastMod,
                     SubmitType,
                     ReturnType,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     ChannelPartnerID,
                     DTServerCreate,
                     DTServerLastMod,
                     IsCheckFranked
                     )
              select ChxrTrxPK, ChxrTrxID, Amount, ChexarAmount, ChexarFee, CheckDate, CheckNumber, RoutingNumber, AccountNumber, Micr, Latitude, Longitude, InvoiceId, TicketId, WaitTime, [Status], ChexarStatus, DeclineCode, [Message], Location, ChxrAccountPK, DTCreate, DTLastMod, SubmitType, ReturnType,@RevisionNo,1 as AuditEvent,GETDATE(),ChannelPartnerID,DTServerCreate,DTServerLastMod,IsCheckFranked from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tChxr_Trx_Aud(
                              ChxrTrxPK,
                     ChxrTrxID,
                     Amount,
                     ChexarAmount,
                     ChexarFee,
                     CheckDate,
                     CheckNumber,
                     RoutingNumber,
                     AccountNumber,
                     Micr,
                     Latitude,
                     Longitude,
                     InvoiceId,
                     TicketId,
                     WaitTime,
                     [Status],
                     ChexarStatus,
                     DeclineCode,
                     [Message],
                     Location,
                     ChxrAccountPK,
                     DTCreate,
                     DTLastMod,
                     SubmitType,
                     ReturnType,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     ChannelPartnerID,
                     DTServerCreate,
                     DTServerLastMod,
                     IsCheckFranked
                     )
              select ChxrTrxPK, ChxrTrxID, Amount, ChexarAmount, ChexarFee, CheckDate, CheckNumber, RoutingNumber, AccountNumber, Micr, Latitude, Longitude, InvoiceId, TicketId, WaitTime, [Status], ChexarStatus, DeclineCode, [Message], Location, ChxrAccountPK, DTCreate, DTLastMod, SubmitType, ReturnType,@RevisionNo,3 as AuditEvent,GETDATE(),ChannelPartnerID,DTServerCreate,DTServerLastMod,IsCheckFranked from deleted
       end
GO


