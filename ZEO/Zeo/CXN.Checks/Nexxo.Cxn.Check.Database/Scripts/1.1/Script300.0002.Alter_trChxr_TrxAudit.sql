--===========================================================================================
-- Auther:			SwarnaLakshmi
-- Date Created:	23rd june 2014
-- Description:		Script for alter tchxr_trx_Audit Trigger
--===========================================================================================
IF EXISTS(SELECT name FROM sysobjects WHERE name = 'trChxr_TrxAudit')
begin
      exec('ALTER trigger trChxr_TrxAudit on tChxr_Trx AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tChxr_Trx_Aud where Id = (select Id from inserted)
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tChxr_Trx_Aud(
                              rowguid,
                     Id,
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
              select rowguid, Id, Amount, ChexarAmount, ChexarFee, CheckDate, CheckNumber, RoutingNumber, AccountNumber, Micr, Latitude, Longitude, InvoiceId, TicketId, WaitTime, [Status], ChexarStatus, DeclineCode, [Message], Location, ChxrAccountPK, DTCreate, DTLastMod, SubmitType, ReturnType,@RevisionNo,2 as AuditEvent,GETDATE(),ChannelPartnerID,DTServerCreate,DTServerLastMod,IsCheckFranked from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
              insert into tChxr_Trx_Aud(
                              rowguid,
                     Id,
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
              select rowguid, Id, Amount, ChexarAmount, ChexarFee, CheckDate, CheckNumber, RoutingNumber, AccountNumber, Micr, Latitude, Longitude, InvoiceId, TicketId, WaitTime, [Status], ChexarStatus, DeclineCode, [Message], Location, ChxrAccountPK, DTCreate, DTLastMod, SubmitType, ReturnType,@RevisionNo,1 as AuditEvent,GETDATE(),ChannelPartnerID,DTServerCreate,DTServerLastMod,IsCheckFranked from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tChxr_Trx_Aud(
                              rowguid,
                     Id,
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
              select rowguid, Id, Amount, ChexarAmount, ChexarFee, CheckDate, CheckNumber, RoutingNumber, AccountNumber, Micr, Latitude, Longitude, InvoiceId, TicketId, WaitTime, [Status], ChexarStatus, DeclineCode, [Message], Location, ChxrAccountPK, DTCreate, DTLastMod, SubmitType, ReturnType,@RevisionNo,3 as AuditEvent,GETDATE(),ChannelPartnerID,DTServerCreate,DTServerLastMod,IsCheckFranked from deleted
       end')
end
GO
