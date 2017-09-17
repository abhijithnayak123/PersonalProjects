-- Author: SwarnaLakshmi S
-- Date Created: Jan 08 2015
-- Description: Altering Trigger for tShoppingCart Table -column changes
-- User Story ID: US1800 Task ID: 


IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[trShoppingCartsAudit]'))
DROP TRIGGER [dbo].[trShoppingCartsAudit]
GO

create trigger [dbo].[trShoppingCartsAudit] on [dbo].[tShoppingCarts] AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tShoppingCarts_Aud where cartRowguid = (select cartRowguid from inserted)
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tShoppingCarts_Aud(
					 cartRowguid,
                     Id,
                     Active,
                     DTCreate,
                     DTLastMod,
                     CustomerPK,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     IsReferral,
                     [Status],
                     IsParked)
              select cartRowguid,Id,Active,DTCreate,DTLastMod,CustomerPK,@RevisionNo,2 as AuditEvent,GETDATE(),IsReferral,[Status],IsParked from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
              insert into tShoppingCarts_Aud(
					  cartRowguid,
                     Id,
                     Active,
                     DTCreate,
                     DTLastMod,
                     CustomerPK,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     IsReferral,
                     [Status],
                     IsParked)
              select cartRowguid,Id,Active,DTCreate,DTLastMod,CustomerPK,@RevisionNo,1 as AuditEvent,GETDATE(),IsReferral,[Status],IsParked from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tShoppingCarts_Aud(
					  cartRowguid,
                     Id,
                     Active,
                     DTCreate,
                     DTLastMod,
                     CustomerPK,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     IsReferral,
                     [Status],
                     IsParked)
              select cartRowguid,Id,Active,DTCreate,DTLastMod,CustomerPK,@RevisionNo,3 as AuditEvent,GETDATE(),IsReferral,[Status],IsParked from deleted
       end
GO


