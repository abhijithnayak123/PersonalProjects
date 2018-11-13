IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tTransactionStatus]') AND type in (N'U'))
begin
DROP TABLE tTransactionStatus
END
GO

CREATE TABLE tTransactionStatus (
[Id] [bigint] NOT NULL,
[Status] [varchar] (50) NULL
)

insert into tTransactionStatus select 1, 'Pending';
insert into tTransactionStatus select 2, 'Authorized';
insert into tTransactionStatus select 3, 'AuthorizationFailed';
insert into tTransactionStatus select 4, 'Committed';
insert into tTransactionStatus select 5, 'Failed';
insert into tTransactionStatus select 6, 'Canceled';
insert into tTransactionStatus select 7, 'Expired';
insert into tTransactionStatus select 8, 'Declined';
insert into tTransactionStatus select 9, 'Initiated';
insert into tTransactionStatus select 10, 'Hold';