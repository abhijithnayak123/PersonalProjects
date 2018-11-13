CREATE CLUSTERED INDEX IX_tCustomers_Aud_rowguid ON dbo.tCustomers_Aud
(
	rowguid
) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

alter table tCustomers_Aud alter column RevisionNo bigint not null
go

ALTER TABLE dbo.tCustomers_Aud ADD CONSTRAINT PK_tCustomers_Aud PRIMARY KEY NONCLUSTERED 
(
	rowguid,
	RevisionNo
) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
