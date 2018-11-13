
ALTER TABLE [dbo].[tChxr_Trx] ADD  ChannelPartnerID bigint
GO

ALTER TABLE [dbo].tChxr_Trx_Aud ADD ChannelPartnerID bigint
GO

ALTER trigger [dbo].[trChxr_TrxAudit] on [dbo].[tChxr_Trx] AFTER Insert, Update, Delete
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
                     DTAudit,ChannelPartnerID
                     )
              select rowguid, Id, Amount, ChexarAmount, ChexarFee, CheckDate, CheckNumber, RoutingNumber, AccountNumber, Micr, Latitude, Longitude, InvoiceId, TicketId, WaitTime, [Status], ChexarStatus, DeclineCode, [Message], Location, ChxrAccountPK, DTCreate, DTLastMod, SubmitType, ReturnType,@RevisionNo,2 as AuditEvent,GETDATE(),ChannelPartnerID from inserted
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
                     DTAudit,ChannelPartnerID
                     )
              select rowguid, Id, Amount, ChexarAmount, ChexarFee, CheckDate, CheckNumber, RoutingNumber, AccountNumber, Micr, Latitude, Longitude, InvoiceId, TicketId, WaitTime, [Status], ChexarStatus, DeclineCode, [Message], Location, ChxrAccountPK, DTCreate, DTLastMod, SubmitType, ReturnType,@RevisionNo,1 as AuditEvent,GETDATE(),ChannelPartnerID from inserted
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
                     DTAudit,ChannelPartnerID
                     )
              select rowguid, Id, Amount, ChexarAmount, ChexarFee, CheckDate, CheckNumber, RoutingNumber, AccountNumber, Micr, Latitude, Longitude, InvoiceId, TicketId, WaitTime, [Status], ChexarStatus, DeclineCode, [Message], Location, ChxrAccountPK, DTCreate, DTLastMod, SubmitType, ReturnType,@RevisionNo,3 as AuditEvent,GETDATE(),ChannelPartnerID from deleted
       end
	   GO
