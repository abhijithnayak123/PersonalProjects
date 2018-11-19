
	UPDATE		lkp
	set			lkp.TrustAdvisorID = tv.TrustAdvisorID
	FROM		$(ExcelsiorDB)..TRUSTADVISOR tv
	INNER JOIN  $(ExcelsiorDB)..TRUSTPARTICIPANT tp ON tp.ParticipantID = tv.ParticipantID
	INNER JOIN  $(ExcelsiorDB)..TBL_EIS_EX_PARTY_RELATION pr ON pr.THIRD_PARTY_ID = tv.TrustAdvisorID
	INNER JOIN  $(ExcelsiorDB)..TBL_EIS_EX_PARTY p ON p.PARTY_ID=pr.PARTY_ID
	INNER JOIN  $(MappingDB)..TBL_TrustAdvisorLookup lkp  ON p.Party_ID=lkp.PartyID AND tv.ParticipantID=LKP.ParticipantID
