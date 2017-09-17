
CREATE TABLE tAccounts
	(
	rowguid uniqueidentifier NOT NULL,
	Id bigint NOT NULL,
	CXEId bigint NOT NULL,
	CXNId bigint NULL,
	DTCreate datetime NOT NULL,
	DTLastMod datetime NULL
	)  ON [PRIMARY]
GO
ALTER TABLE tAccounts ADD CONSTRAINT
	PK_tAccounts PRIMARY KEY CLUSTERED 
	(
	rowguid
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE tAccounts SET (LOCK_ESCALATION = TABLE)
GO