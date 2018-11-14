if not exists(select * from sys.columns where Name = N'ClientCustomerId' and Object_ID = Object_ID(N'tTransaction'))
begin
Alter Table tTransaction
Add [ClientCustomerId] [bigint] NULL,
[ProviderResponseCode] [nvarchar](255) NULL,
[LocationID] [varchar](50) NULL,
[TransactionDescription] [varchar](200) NULL,
[CustomerName] [nvarchar](255) NULL
end