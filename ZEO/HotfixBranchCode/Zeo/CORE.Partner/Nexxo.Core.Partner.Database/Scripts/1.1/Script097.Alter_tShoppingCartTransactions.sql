ALTER TABLE dbo.tShoppingCartTransactions
ADD 
	cartTxnRowguid UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
	CartItemStatus VARCHAR(50)
GO

ALTER TABLE dbo.tShoppingCartTransactions
ADD CONSTRAINT PK_tShoppingCartTransactions_cartTxnRowguid PRIMARY KEY (cartTxnRowguid)
GO

ALTER TABLE dbo.tShoppingCartTransactions
ADD CONSTRAINT FK_tShoppingCartTransactions_tShoppingCart FOREIGN KEY (cartRowguid) REFERENCES tShoppingCarts(cartRowguid)
GO

ALTER TABLE tShoppingCarts
ADD Status VARCHAR(50)
GO