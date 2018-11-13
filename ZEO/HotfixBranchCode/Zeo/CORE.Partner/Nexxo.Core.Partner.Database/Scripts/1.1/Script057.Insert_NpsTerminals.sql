-- adding PhoneNumber column to tLocations
INSERT INTO tNpsTerminals (rowguid, Name, Description, IPAddress, Port, LocationPK, Status, DTCreate)
VALUES (NEWID(), 'DMS Bay', 'EPSON printer/scanner located in DMS bay', '172.18.100.29', 8732,'0AD068FB-88A6-48D4-9081-FC5A73F9A187','Not Available', GETDATE())
GO
