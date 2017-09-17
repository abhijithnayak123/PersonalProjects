-- Author: SwarnaLakshmi S
-- Date Created: Jan 17 2014
-- Description: Adding new coloum IsParked to tShoppingCart Table for Parking Transactions
-- User Story ID: US1488 Task ID: TA3851

if not exists(select * from sys.columns where Name = N'IsParked' and Object_ID = Object_ID(N'tShoppingCarts'))
begin 
ALTER TABLE tShoppingCarts
ADD IsParked BIT NOT NULL CONSTRAINT DF_tShoppingCarts_IsParked DEFAULT (0)
end
GO
