IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_NameTypeMapping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tWUnion_NameTypeMapping](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[ISOCountryCode] [varchar](20) NULL,
	[Name] [varchar](200) NULL,
	[DTCreate] [datetime] NULL,
	[DTLastMod] [datetime] NULL,
	[NameType] [varchar](5) NULL,
 CONSTRAINT [PK_tWUnion_NameTypeMapping] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
insert into tWUnion_NameTypeMapping (rowguid,ISOCountryCode,Name,DTCreate,NameType) values (NEWID(),'CR','Costa Rica',GETDATE(),'M')
	insert into tWUnion_NameTypeMapping (rowguid,ISOCountryCode,Name,DTCreate,NameType) values (NEWID(),'HN','Honduras',GETDATE(),'M')
	insert into tWUnion_NameTypeMapping (rowguid,ISOCountryCode,Name,DTCreate,NameType) values (NEWID(),'VE','Venezuela',GETDATE(),'M')
	insert into tWUnion_NameTypeMapping (rowguid,ISOCountryCode,Name,DTCreate,NameType) values (NEWID(),'PE','Peru',GETDATE(),'M')
	insert into tWUnion_NameTypeMapping (rowguid,ISOCountryCode,Name,DTCreate,NameType) values (NEWID(),'DO','Dominican',GETDATE(),'M')
	insert into tWUnion_NameTypeMapping (rowguid,ISOCountryCode,Name,DTCreate,NameType) values (NEWID(),'PY','Paraguay',GETDATE(),'M')
	insert into tWUnion_NameTypeMapping (rowguid,ISOCountryCode,Name,DTCreate,NameType) values (NEWID(),'NI','Nicaragua',GETDATE(),'M')
	insert into tWUnion_NameTypeMapping (rowguid,ISOCountryCode,Name,DTCreate,NameType) values (NEWID(),'UY','Uruguay',GETDATE(),'M')
	insert into tWUnion_NameTypeMapping (rowguid,ISOCountryCode,Name,DTCreate,NameType) values (NEWID(),'CL','Chile',GETDATE(),'M')
	insert into tWUnion_NameTypeMapping (rowguid,ISOCountryCode,Name,DTCreate,NameType) values (NEWID(),'SV','El Salvador',GETDATE(),'M')
	insert into tWUnion_NameTypeMapping (rowguid,ISOCountryCode,Name,DTCreate,NameType) values (NEWID(),'MX','Mexico',GETDATE(),'M')
	insert into tWUnion_NameTypeMapping (rowguid,ISOCountryCode,Name,DTCreate,NameType) values (NEWID(),'PA','Panama',GETDATE(),'M')
	insert into tWUnion_NameTypeMapping (rowguid,ISOCountryCode,Name,DTCreate,NameType) values (NEWID(),'CO','Colombia',GETDATE(),'M')
END
GO 