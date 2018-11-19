

 	;WITH cte
     AS (SELECT ROW_NUMBER() OVER (PARTITION BY ManagerCode,ContactId
								       ORDER BY ( SELECT 0)) RN
         FROM   TBL_Lookup_Client)
	DELETE FROM cte
	WHERE  RN > 1

go 
 	;WITH cte
     AS (SELECT ROW_NUMBER() OVER (PARTITION BY SubContactId,ContactId,ContactRoleCode
								       ORDER BY ( SELECT 0)) RN
         FROM   TBL_Lookup_ClientEmployee)
	DELETE FROM cte
	WHERE  RN > 1
	

go

 	;WITH cte
     AS (SELECT ROW_NUMBER() OVER (PARTITION BY SubContactId,ContactId,ContactRoleCode
								       ORDER BY ( SELECT 0)) RN
         FROM   TBL_Lookup_ClientDesignation)
	DELETE FROM cte
	WHERE  RN > 1

go

 	;WITH cte
     AS (SELECT ROW_NUMBER() OVER (PARTITION BY AllianceNumber,ContactId
								       ORDER BY ( SELECT 0)) RN
         FROM   TBL_Lookup_Program)
	DELETE FROM cte
	WHERE  RN > 1

go

 	;WITH cte
     AS (SELECT ROW_NUMBER() OVER (PARTITION BY ContactId,CustomerAccountNumber,ContactRoleCode
								       ORDER BY ( SELECT 0)) RN
         FROM   TBL_Lookup_Donor)
	DELETE FROM cte
	WHERE  RN > 1

go
	 ;WITH cte
     AS (SELECT ROW_NUMBER() OVER (PARTITION BY CustomerAccountNumber,ContactID,ContactRoleCode
								       ORDER BY ( SELECT 0)) RN
         FROM   TBL_Lookup_Trustee)
	DELETE FROM cte
	WHERE  RN > 1

go

	 ;WITH cte
     AS (SELECT ROW_NUMBER() OVER (PARTITION BY ContactID,CustomerAccountNumber,RoleCode,PaidForContactID,BeneficiaryDistributionID
								       ORDER BY ( SELECT 0)) RN
         FROM   TBL_Lookup_Beneficiary)
	DELETE FROM cte
	WHERE  RN > 1

go

	;WITH cte
     AS (SELECT ROW_NUMBER() OVER (PARTITION BY CustomerAccountNumber,ContactID,ContactRoleCode
								       ORDER BY ( SELECT 0)) RN
         FROM   TBL_Lookup_RemaindermanDesignation)
	DELETE FROM cte
	WHERE  RN >1


go--

	;WITH cte
     AS (SELECT ROW_NUMBER() OVER (PARTITION BY ContactID,CustomerAccountNumber,ContactRoleCode
								       ORDER BY ( SELECT 0)) RN
         FROM   TBL_Lookup_Remainderman)
	DELETE FROM cte
	WHERE  RN >1

go--

	;WITH cte
     AS (SELECT ROW_NUMBER() OVER (PARTITION BY SubContactId,ContactId,ContactRoleCode
								       ORDER BY ( SELECT 0)) RN
         FROM   TBL_Lookup_TrustAdvisor)
	DELETE FROM cte
	WHERE  RN >1

go

	;WITH cte
     AS (SELECT ROW_NUMBER() OVER (PARTITION BY ContactId
								       ORDER BY ( SELECT 0)) RN
         FROM   TBL_Lookup_TrustParticipant)
	DELETE FROM cte
	WHERE  RN >1

go
	;WITH cte
     AS (SELECT ROW_NUMBER() OVER (PARTITION BY trustadvisorid
								       ORDER BY ( SELECT 0)) RN
         FROM   tbl_lookup_trustadvisor)
	DELETE FROM cte
	WHERE  RN >1
go

	;WITH cte
     AS (SELECT ROW_NUMBER() OVER (PARTITION BY ContactID
								       ORDER BY ( SELECT 0)) RN
         FROM   [TBL_Lookup_KCOStaff])
	DELETE FROM cte
	WHERE  RN >1