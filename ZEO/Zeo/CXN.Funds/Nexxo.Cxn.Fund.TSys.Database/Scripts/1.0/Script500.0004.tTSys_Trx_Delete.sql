--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to alter trigger tTSys_Trx_Delete>           
-- Jira ID:	<AL-244>
--===========================================================================================

/****** Object:  Trigger [dbo].[tTSys_Trx_Delete]    Script Date: 4/7/2015 1:30:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [dbo].[tTSys_Trx_Delete] ON [dbo].[tTSys_Trx] AFTER DELETE
AS
BEGIN
	SET NOCOUNT ON
	insert tTSys_Trx_Aud(TSysTrxPK,TSysTrxId,TSysAccountPK,TransactionType,Amount,Fee,Description,DTLocalTransaction,DTTransmission,Status,
	ErrorCode,ErrorMsg,ConfirmationId,Balance,DTCreate,DTLastMod,ChannelPartnerID, DTServerCreate, DTServerLastMod,
	RevisionNo, AuditEvent, DTAudit)
	SELECT i.TSysTrxPK,TSysTrxId,i.TSysAccountPK,TransactionType,Amount,Fee,Description,DTLocalTransaction,DTTransmission,Status,
	ErrorCode,ErrorMsg,ConfirmationId,Balance,DTCreate,DTLastMod,ChannelPartnerID, DTServerCreate, DTServerLastMod,
	isnull(a.MaxRev,0) + 1, 3, GETDATE()
	from deleted i 
		left outer join (
			select TSysAccountPK, MAX(RevisionNo) as MaxRev
			from tTSys_Account_Aud
			group by TSysAccountPK
		) a on i.TSysAccountPK = a.TSysAccountPK
END
GO


