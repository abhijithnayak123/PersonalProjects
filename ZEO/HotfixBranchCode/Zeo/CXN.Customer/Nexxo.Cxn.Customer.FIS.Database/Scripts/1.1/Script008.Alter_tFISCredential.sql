if not exists(select * from sys.columns where Name = N'MetBankNumber' and Object_ID = Object_ID(N'tFIS_Credential'))
	alter table tFIS_Credential add MetBankNumber nvarchar(5) NULL
go

update tFIS_Credential set MetBankNumber = BankId
where MetBankNumber is null
go