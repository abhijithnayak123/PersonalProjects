--- ================================================================================
-- Author:		<Bineesh E Raghavan>
-- Create date: <05/06/2016>
-- Description:	<Updates Synovus branch id and name >
-- Jira ID:		<AL-6553>
-- ================================================================================

UPDATE tLocations SET LocationName = '165-0001 CB and T Columbus-Uptown Center' WHERE LocationPK = '86C9A2F3-5F72-48DD-9E14-C3B45801001A' -- branch code 0001
UPDATE tLocations SET LocationName = '165-0002 CB and T Columbus-Main Post' WHERE LocationPK = '54D4323C-F1D7-4426-B262-A4432F697B3D' -- branch code 0002
UPDATE tLocations SET LocationName = '165-0003 CB and T Columbus-Tenth ST' WHERE LocationPK = 'BF529B0E-F4A4-49C7-AE5C-93B92C5A83AF' -- branch code 0003
UPDATE tLocations SET LocationName = '165-0004 CB and T Columbus-Midland Crossing', BranchID = '0004' WHERE LocationPK = '730AEB6B-1E33-4C44-BB1E-EDACEB43DD74' -- branch code 0019
UPDATE tLocations SET LocationName = '165-0005 CB and T Columbus-River Rd', BranchID = '0005' WHERE LocationPK = '27B0552A-FAAA-47E3-9C78-CD38DEBB2CD1' -- branch code 0020
UPDATE tLocations SET LocationName = '165-0006 CB and T Columbus-South Columbus' WHERE LocationPK = '921A38AC-C17D-43BC-BB5E-A0FE5CED1EF3' -- branch code 0006
UPDATE tLocations SET LocationName = '165-0007 CB and T Columbus-Midtown' WHERE LocationPK = '5822F043-7B53-4D93-8BCC-0B87F4188829' -- branch code 0007
UPDATE tLocations SET LocationName = '165-0008 CB and T Columbus-Commissary', BranchID = '0008' WHERE LocationPK = 'E0F8042E-1AF8-436F-A45D-36B23A13D277' -- branch code 0026
UPDATE tLocations SET LocationName = '165-0009 CB and T Columbus-TSYS Campus', BranchID = '0009' WHERE LocationPK = '97510A89-5F60-47BE-BE72-700F64C68F4D' -- branch code 0027
UPDATE tLocations SET LocationName = '165-0010 CB and T Columbus-East Columbus', BranchID = '0010' WHERE LocationPK = 'B6740221-2BFC-4303-A2A4-44D00D32907F' -- branch code 0029
UPDATE tLocations SET LocationName = '165-0011 CB and T Columbus-Peachtree' WHERE LocationPK = '7F516FC9-221F-4E2E-A8FC-42AD5D79292C' -- branch code 0011
UPDATE tLocations SET LocationName = '165-0012 CB and T Columbus-Bradley Park', BranchID = '0008' WHERE LocationPK = 'B8FCFA0F-5E0B-4F2E-A6C7-A80CF1702517' -- branch code 0012
UPDATE tLocations SET LocationName = '165-0013 CB and T Columbus-Columbus Park', BranchID = '0013' WHERE LocationPK = '01585DEA-A31D-43C6-AA5B-EC9AE95B56A7' -- branch code 0028

GO