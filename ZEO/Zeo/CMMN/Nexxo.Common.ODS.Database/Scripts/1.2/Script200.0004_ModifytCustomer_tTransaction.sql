IF Exists(select * from sys.columns where Name = N'ClientCustomerId' and Object_ID = Object_ID(N'tTransaction'))
Begin
	 alter table tTransaction
	 alter column  ClientCustomerId nvarchar(100)
End

IF Exists(select * from sys.columns where Name = N'ClientCustomerId' and Object_ID = Object_ID(N'tCustomer'))
Begin
	alter table tCustomer
	alter column  ClientCustomerId nvarchar(100)
End