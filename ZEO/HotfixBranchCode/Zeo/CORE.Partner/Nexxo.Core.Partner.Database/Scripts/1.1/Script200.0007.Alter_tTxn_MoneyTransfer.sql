if not exists(select * from sys.columns where Name = N'TransferType' and Object_ID = Object_ID(N'tTxn_MoneyTransfer'))
begin

ALTER TABLE tTxn_MoneyTransfer
Add TransferType int
end

Go