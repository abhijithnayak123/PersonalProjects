alter table tTxn_Check add BaseFee money null
alter table tTxn_Check add DiscountApplied money null
go
alter table tTxn_Funds add BaseFee money null
alter table tTxn_Funds add DiscountApplied money null
go
alter table tTxn_MoneyOrder add BaseFee money null
alter table tTxn_MoneyOrder add DiscountApplied money null
go
update tTxn_Check set BaseFee = Fee, DiscountApplied = 0
update tTxn_Funds set BaseFee = Fee, DiscountApplied = 0
update tTxn_MoneyOrder set BaseFee = Fee, DiscountApplied = 0
