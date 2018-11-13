if not exists(select * from sys.columns where Name = N'ReceiptLanguage' and Object_ID = Object_ID(N'tCustomers'))
begin
ALTER TABLE [dbo].[tCustomers]
ADD [ReceiptLanguage] [varchar](50) NULL,
	[ProfileStatus] [bit]
end
GO