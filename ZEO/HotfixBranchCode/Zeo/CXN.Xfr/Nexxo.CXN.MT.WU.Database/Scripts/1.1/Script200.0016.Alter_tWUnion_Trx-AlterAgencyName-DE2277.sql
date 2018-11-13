IF exists(select * from sys.columns where Name = N'AgencyName' and Object_ID = Object_ID(N'tWUnion_Trx'))
Begin
   ALTER TABLE tWUnion_Trx ALTER COLUMN AgencyName varchar(200)
End