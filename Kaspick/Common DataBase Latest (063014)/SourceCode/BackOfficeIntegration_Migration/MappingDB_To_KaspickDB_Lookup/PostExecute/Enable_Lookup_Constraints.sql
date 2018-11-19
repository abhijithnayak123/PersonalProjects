
 ALTER INDEX [UK_TBL_Lookup_ClientEmployee_ContactID_SubContactID_ContactRoleCode]   ON TBL_Lookup_ClientEmployee REBUILD;
 go
 ALTER INDEX [UK_TBL_Lookup_ClientDesignation_ContactID_SubContactID_ContactRoleCode] ON   TBL_Lookup_ClientDesignation	REBUILD;
 go
 ALTER INDEX [UK_TBL_Lookup_TrustParticipant_ContactID] ON   TBL_Lookup_TrustParticipant REBUILD;
 go
 ALTER INDEX [UK_TBL_Lookup_Remainderman_ContactID_CustomerAccountNumber_ContactRoleCode] ON  TBL_Lookup_Remainderman REBUILD;
 go
 ALTER INDEX [UK_TBL_Lookup_RemaindermanDesignation_ContactID_CustomerAccountNumber_ContactRoleCode] ON  TBL_Lookup_RemaindermanDesignation REBUILD;
 go
 ALTER INDEX [UK_TBL_Lookup_Donor_ContactID_CustomerAccountNumber_ContactRoleCode] ON  TBL_Lookup_Donor REBUILD;
 go
 ALTER INDEX [UK_TBL_Lookup_Trustee_ContactID_CustomerAccountNumber_ContactRoleCode] ON  TBL_Lookup_Trustee REBUILD;
 go
 ALTER INDEX [UK_TBL_Lookup_TrustAdvisor_ContactID_SubContactID_ContactRoleCode] ON  TBL_Lookup_TrustAdvisor REBUILD;
 go
 ALTER INDEX [UK_TBL_Lookup_Beneficiary_ContactID_CustomerAccountNumber_RoleCode_PaidForContactID_BeneficiaryDistributionID] ON TBL_Lookup_Beneficiary REBUILD;
 go
 ALTER INDEX [UK_TBL_Lookup_KCOStaff_ContactID] ON  TBL_Lookup_KCOStaff REBUILD;
 go
 ALTER INDEX [UK_TBL_Lookup_Client_ManagerCode_ContactID] ON  TBL_Lookup_Client REBUILD;
 go
 ALTER INDEX [UK_TBL_Lookup_Program_AllianceNumber_ContactID] ON  TBL_Lookup_Program REBUILD;
 go
 ALTER INDEX [UK_TBL_Lookup_Account_CustomerAccountNumber] ON  TBL_Lookup_Account REBUILD;
go