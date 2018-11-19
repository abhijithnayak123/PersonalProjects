-- Query to update the ContactIDs in PersonAccount4 for Giftwrap database using mapping table
UPDATE PerMap
SET PersonAccount4 = ContMap.ContactID
FROM $(GiftwrapDB)..tblPerson PerMap
INNER JOIN (
	SELECT Person.PersonID
		,Person.PersonAccount1
		,PartContact.CONTACTID
	FROM $(GiftwrapDB)..tblPerson Person
	LEFT OUTER JOIN $(MappingDB)..TBL_ParticipantContactLookUp PartContact
		ON PartContact.ParticipantID = Person.PersonAccount1
	WHERE PersonAccount1 NOT LIKE '%[^0-9]%'
		AND Person.PersonAccount1 IS NOT NULL
		AND Person.PersonAccount1 <> ''
	) ContMap
	ON ContMap.PersonID = PerMap.PersonID
		AND ContMap.CONTACTID IS NOT NULL
