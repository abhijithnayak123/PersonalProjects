	update b
	set    BeneficiaryDistributionID=null,PaidForContactID=null
	from   $(MappingDB)..TBL_BeneficiaryLookup	b


	--SQL Query: 
	UPDATE LookupProxy
	SET			paidforcontactid = LookupBene.ContactiD
	FROM		$(MappingDB)..TBL_BeneficiaryLookup LookupBene
	INNER JOIN	$(MappingDB)..TBL_BeneficiaryLookup LookupProxy
		  ON	LookupBene.CustomerAccountNumber = LookupProxy.CustomerAccountNumber
		  AND   LookupBene.BeneficiaryID = LookupProxy.K1BeneficiaryID

	UPDATE  $(MappingDB)..TBL_BeneficiaryLookup
	SET		PaidForContactID = ContactID
	WHERE	PaidForContactID IS NULL

	update		bene
	set			Bene.BeneficiaryDistributionID=BD.BeneficiaryDistributionID								
	from		$(MappingDB)..TBL_BeneficiaryLookup	 Bene  
	inner Join	$(InnotrustDB)..BeneficiaryDistributions bd on Bene.contactid=PayeeID and bd.contactid=Bene.PaidForContactID	
	inner Join	$(InnotrustDB)..RemittanceInstructions   RI on bd.InstructionID = RI.InstructionID AND Bene.CustomerAccountNumber=RI.CustomerAccountNumber
	where		Bene.rolecode in(10,21)--Proxy Recipient/Beneficiary
	