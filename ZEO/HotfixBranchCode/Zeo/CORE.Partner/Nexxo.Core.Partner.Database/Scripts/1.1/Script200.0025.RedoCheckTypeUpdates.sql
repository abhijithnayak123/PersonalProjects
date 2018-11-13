update tCheckTypes set Name = 'Ins/Attorney/Cashiers' where id = 1
update tCheckTypes set Name = 'US Treasury' where id = 2
update tCheckTypes set Name = 'Government' where id = 3
update tCheckTypes set Name = 'Handwritten Payroll' where id = 6
update tCheckTypes set Name = 'Printed Payroll' where id = 7
update tCheckTypes set Name = 'RAC/Loan' where id = 14

delete from tCheckTypes where id not in (1, 2, 3, 5, 6, 7, 10, 14)