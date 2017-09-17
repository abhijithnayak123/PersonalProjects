IF not exists(select * from sys.columns where Name = N'LocationId' and Object_ID = Object_ID(N'tLocation'))
Begin
   ALTER TABLE [dbo].tLocation ADD LocationId varchar(50)
End
