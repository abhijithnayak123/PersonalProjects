IF Exists(select * from sys.columns where Name = N'LocationId' and Object_ID = Object_ID(N'tLocation'))
Begin
  alter table tLocation alter column LocationId varchar(50)
End