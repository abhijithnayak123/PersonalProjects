/****** Object:  Trigger [tTxn_MoneyTransfer_CommitAudit]    Script Date: 12/10/2013 18:40:36 ******/
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[tTxn_MoneyTransfer_CommitAudit]'))
DROP TRIGGER [dbo].[tTxn_MoneyTransfer_CommitAudit]
GO


/****** Object:  Trigger [dbo].[tTxn_MoneyTransfer_CommitAudit]    Script Date: 12/10/2013 18:40:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


Create trigger [dbo].[tTxn_MoneyTransfer_CommitAudit] on [dbo].[tTxn_MoneyTransfer_Commit] AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tTxn_MoneyTransfer_Commit_Aud
       where Id = (select Id from inserted)
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tTxn_MoneyTransfer_Commit_Aud(
					[rowguid],[Id],[Amount],[Fee],[AccountPK],	[Status] , [ConfirmationNumber],[ReceiverName],
					[Destination],[DTCreate],[DTLastMod],[DTServerCreate],[DTServerLastMod],
                     RevisionNo, AuditEvent,DTAudit)
              select  [rowguid],[Id],[Amount],[Fee],[AccountPK], [Status] , [ConfirmationNumber],[ReceiverName],
					[Destination],[DTCreate],[DTLastMod],[DTServerCreate],[DTServerLastMod],
					 @RevisionNo,2 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
               insert into tTxn_MoneyTransfer_Commit_Aud(
					 [rowguid],[Id],[Amount],[Fee],[AccountPK], [Status] , [ConfirmationNumber],[ReceiverName],
					[Destination],[DTCreate],[DTLastMod],[DTServerCreate],[DTServerLastMod],
                     RevisionNo, AuditEvent,DTAudit)
              select [rowguid],[Id],[Amount],[Fee],[AccountPK], [Status] , [ConfirmationNumber],[ReceiverName],
					[Destination],[DTCreate],[DTLastMod],[DTServerCreate],[DTServerLastMod],
					 @RevisionNo,1 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tTxn_MoneyTransfer_Commit_Aud(
					 [rowguid], [Id], [Amount], [Fee],[AccountPK],[Status],
					 [ConfirmationNumber],[ReceiverName],[Destination],
					 [DTCreate],[DTLastMod],[DTServerCreate],[DTServerLastMod],
                     RevisionNo, AuditEvent,DTAudit)
              select [rowguid],[Id],[Amount],[Fee],[AccountPK], [Status] , [ConfirmationNumber],[ReceiverName],
					[Destination],[DTCreate],[DTLastMod],[DTServerCreate],[DTServerLastMod],
					 @RevisionNo,3 as AuditEvent,GETDATE() from deleted
       end

GO


