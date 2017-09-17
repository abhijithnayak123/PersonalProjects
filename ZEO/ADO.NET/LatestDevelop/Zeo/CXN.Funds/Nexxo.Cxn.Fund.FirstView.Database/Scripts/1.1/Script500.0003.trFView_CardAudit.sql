--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to alter trigger trFView_CardAudit>           
-- Jira ID:	<AL-244>
--===========================================================================================

/****** Object:  Trigger [dbo].[trFView_CardAudit]    Script Date: 4/7/2015 1:04:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER trigger [dbo].[trFView_CardAudit] on [dbo].[tFView_Card] AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tFView_Card_Aud where FViewCardID = (select FViewCardID from inserted)
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tFView_Card_Aud(
					 FViewCardPK,
                     FViewCardId,
                     CardNumber,
                     AccountNumber,
                     BSAccountNumber,
                     NameAsOnCard,
                     FirstName,
                     MiddleName,
                     LastName,
                     DateOfBirth,
                     SSNNumber,
                     GovernmentID,
                     IDNumber,
                     GovtIdExpirationDate,
                     GovtIDIssueCountry,
                     GovtIDIssueDate,
                     GovtIDIssueState,
                     AddressLine1,
                     AddressLine2,
                     City,
                     [State],
                     PostalCode,
                     HomePhoneNumber,
                     ShippingContactName,
                     ShippingAddressLine1,
                     ShippingAddressLine2,
                     ShippingCity,
                     ShippingState,
                     ShippingZipCode,
                     ExpiryDate,
                     IsActive,
                     DTActivated,
                     ActivatedBy,
                     DTDeactivated,
                     DeactivatedBy,
                     DeactivatedReason,
                     DTCreate,
                     DTLastMod,
                     RevisionNo,
                     AuditEvent,
                     DTAudit
                     )
              select  FViewCardPK, FViewCardId, CardNumber, AccountNumber, BSAccountNumber, NameAsOnCard, FirstName,  MiddleName, LastName, DateOfBirth, SSNNumber, GovernmentID, IDNumber, GovtIdExpirationDate, GovtIDIssueCountry, GovtIDIssueDate, GovtIDIssueState, AddressLine1, AddressLine2, City, [State], PostalCode, HomePhoneNumber, ShippingContactName, ShippingAddressLine1, ShippingAddressLine2, ShippingCity, ShippingState, ShippingZipCode, ExpiryDate, IsActive, DTActivated, ActivatedBy, DTDeactivated, DeactivatedBy, DeactivatedReason, DTCreate, DTLastMod,@RevisionNo,2 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
              insert into tFView_Card_Aud(
					FViewCardPK,
                     FViewCardId,
                     CardNumber,
                     AccountNumber,
                     BSAccountNumber,
                     NameAsOnCard,
                     FirstName,
                     MiddleName,
                     LastName,
                     DateOfBirth,
                     SSNNumber,
                     GovernmentID,
                     IDNumber,
                     GovtIdExpirationDate,
                     GovtIDIssueCountry,
                     GovtIDIssueDate,
                     GovtIDIssueState,
                     AddressLine1,
                     AddressLine2,
                     City,
                     [State],
                     PostalCode,
                     HomePhoneNumber,
                     ShippingContactName,
                     ShippingAddressLine1,
                     ShippingAddressLine2,
                     ShippingCity,
                     ShippingState,
                     ShippingZipCode,
                     ExpiryDate,
                     IsActive,
                     DTActivated,
                     ActivatedBy,
                     DTDeactivated,
                     DeactivatedBy,
                     DeactivatedReason,
                     DTCreate,
                     DTLastMod,
                     RevisionNo,
                     AuditEvent,
                     DTAudit
                     )
              select FViewCardPK, FViewCardId, CardNumber, AccountNumber, BSAccountNumber, NameAsOnCard, FirstName,  MiddleName, LastName, DateOfBirth, SSNNumber, GovernmentID, IDNumber, GovtIdExpirationDate, GovtIDIssueCountry, GovtIDIssueDate, GovtIDIssueState, AddressLine1, AddressLine2, City, [State], PostalCode, HomePhoneNumber, ShippingContactName, ShippingAddressLine1, ShippingAddressLine2, ShippingCity, ShippingState, ShippingZipCode, ExpiryDate, IsActive, DTActivated, ActivatedBy, DTDeactivated, DeactivatedBy, DeactivatedReason, DTCreate, DTLastMod,@RevisionNo,1 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tFView_Card_Aud(
					 FViewCardPK,
                     FViewCardId,
                     CardNumber,
                     AccountNumber,
                     BSAccountNumber,
                     NameAsOnCard,
                     FirstName,
                     MiddleName,
                     LastName,
                     DateOfBirth,
                     SSNNumber,
                     GovernmentID,
                     IDNumber,
                     GovtIdExpirationDate,
                     GovtIDIssueCountry,
                     GovtIDIssueDate,
                     GovtIDIssueState,
                     AddressLine1,
                     AddressLine2,
                     City,
                     [State],
                     PostalCode,
                     HomePhoneNumber,
                     ShippingContactName,
                     ShippingAddressLine1,
                     ShippingAddressLine2,
                     ShippingCity,
                     ShippingState,
                     ShippingZipCode,
                     ExpiryDate,
                     IsActive,
                     DTActivated,
                     ActivatedBy,
                     DTDeactivated,
                     DeactivatedBy,
                     DeactivatedReason,
                     DTCreate,
                     DTLastMod,
                     RevisionNo,
                     AuditEvent,
                     DTAudit
                     )
              select FViewCardPK, FViewCardId, CardNumber, AccountNumber, BSAccountNumber, NameAsOnCard, FirstName,  MiddleName, LastName, DateOfBirth, SSNNumber, GovernmentID, IDNumber, GovtIdExpirationDate, GovtIDIssueCountry, GovtIDIssueDate, GovtIDIssueState, AddressLine1, AddressLine2, City, [State], PostalCode, HomePhoneNumber, ShippingContactName, ShippingAddressLine1, ShippingAddressLine2, ShippingCity, ShippingState, ShippingZipCode, ExpiryDate, IsActive, DTActivated, ActivatedBy, DTDeactivated, DeactivatedBy, DeactivatedReason, DTCreate, DTLastMod,@RevisionNo,3 as AuditEvent,GETDATE() from deleted
       end
GO


