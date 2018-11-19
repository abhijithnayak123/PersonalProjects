
SET NOCOUNT ON

	---- Participant  -> Contact mapping lookup migration script
	--INSERT INTO TBL_ParticipantContactLookUP (
	--	ParticipantID
	--	,CUSTOMERACCOUNTNUMBER
	--	,CONTACTID
	--	)
	--SELECT DISTINCT part.ParticipantID
	--	,part.AdventID
	--	,part.ParticipantID
	--FROM $(ExcelsiorDB)..V_EIS_EX_TRUSTPARTICIPANT_TYPE part

	---- Participant and Beneficary -> Contact mapping lookup migration script
	--INSERT INTO TBL_PartBeneContactLookUP
	--SELECT DISTINCT part.ParticipantID
	--	,ben.BeneficiaryID
	--	,CONTACTID
	--	,CASE 
	--		WHEN ben.BeneficiaryID IS NOT NULL
	--			THEN 24
	--		ELSE 21
	--		END
	--	,part.AdventID
	--FROM $(ExcelsiorDB)..V_EIS_EX_TRUSTPARTICIPANT_TYPE part
	--INNER JOIN TBL_ParticipantContactLookUP lkup ON lkup.ParticipantID = part.ParticipantID
	--LEFT OUTER JOIN $(ExcelsiorDB)..beneficiary ben ON ben.ParticipantID = part.ParticipantID
	--	AND part.AdventID = lkup.customeraccountnumber

-- TBL_InstructionLookUp insert script
	INSERT INTO TBL_InstructionLookUp
	SELECT DISTINCT adventid
		,accountid
	FROM $(ExcelsiorDB)..DEFERREDGIFTACCOUNT

-- TBL_TransactionLookUp insert script
	INSERT INTO TBL_TransactionLookUp
	SELECT DISTINCT PaymentID
		,PaymentID
	FROM $(ExcelsiorDB)..BENPAYMENT

-- TBL_DocumentNoLookUp insert script
	INSERT INTO TBL_DocumentNoLookUp
	SELECT DISTINCT PaymentID
		,PaymentID
	FROM $(ExcelsiorDB)..BENPAYMENT

---- TBL_BeneficiaryDistributionLookUp insert script
--	INSERT INTO TBL_BeneficiaryDistributionLookUp
--	SELECT DISTINCT BeneficiaryID
--		,BeneficiaryID
--	FROM $(ExcelsiorDB)..Beneficiary


------------------------------------------------------------

---- TBL_AccountTypeMapping insert script	
--SET NOCOUNT ON

--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('FDN','990')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('TMCF','1099')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('GAP','1099')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('PR69','1099')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('PR69','1099')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('PR69','1099')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('GAP','CGA')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('GAPR','CGA')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('GAPP','CGA')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('CORP','CORP')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('CRAT','CRAT')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('CRUT','CRUT')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('NIMU','CRUTMU')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('NICT','CRUTNI')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('DAF','D-A FUND')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('QPE','DCA')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('DDF','D-D FUND')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('ENDQ','D-D FUND')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('END','ENDOW')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('ENDQ','ENDOW')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('ENDT','ENDOW')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('EST','ESTATE')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('GREV','GRANTOR-OWNER')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('GLAT','LEAD-FIXED')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('NLAT','LEAD-FIXED')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('GLUT','LEAD-PCT')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('NLUT','LEAD-PCT')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('IRRV','NQI')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('GIRV','NQI')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('CLR','OTHER')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('GENLEND','OTHER')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('RET','OTHER')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('NQP','OTHER')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('ENDQ','OTHER')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('PIF','PIF')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('PR69','PRE-69')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('SIMP','PRIV-EXMT')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('ROTH','PRIV-EXMT')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('IRA','PRIV-EXMT')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('RET','PRIV-EXMT')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('SEP','PRIV-EXMT')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('PSP','PRIV-EXMT')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('PTA','PRIV-TAX')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('PSHIP','PRIV-TAX')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('SIMP','PRIV-TAX')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('ASST','TMC-ASSOC')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('BRCT','TMC-BRANCH')
--INSERT INTO  $(MappingDB)..TBL_AccountTypeMapping ( InnotrustAccountTypeCode,ExcelsiorAccountTypeCode) VALUES ('TMCF','TMC-FUND')

--------------------------------------------------------------

