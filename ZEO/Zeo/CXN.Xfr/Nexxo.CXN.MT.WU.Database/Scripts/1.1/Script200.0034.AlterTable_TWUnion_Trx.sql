IF not exists(select * from sys.columns where Name = N'pay_or_do_not_pay_indicator' and Object_ID = Object_ID(N'tWUnion_Trx'))
Begin
   ALTER TABLE [dbo].tWUnion_Trx ADD pay_or_do_not_pay_indicator varchar(10)
End