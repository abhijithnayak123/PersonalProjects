﻿DROP TABLE tCustomerAccounts
GO

CREATE TABLE tCustomerAccounts
	(
	rowguid uniqueidentifier NOT NULL,
	Id bigint NOT NULL IDENTITY (1000000000, 1),
	PAN bigint NOT NULL,
	Type int NOT NULL,
	Provider int NOT NULL,
	DTCreate datetime NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE tCustomerAccounts ADD CONSTRAINT
	PK_tCustomerAccounts PRIMARY KEY CLUSTERED 
	(
	rowguid
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE tCustomerAccounts SET (LOCK_ESCALATION = TABLE)
GO
