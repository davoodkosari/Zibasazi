
GO
ALTER TABLE Payment.TempDiscount
	DROP CONSTRAINT PK_TempDiscount
GO
ALTER TABLE Payment.TempDiscount
	DROP CONSTRAINT DF__TempDiscou__Date__4924D839
GO
ALTER TABLE Payment.TempDiscount
	DROP COLUMN Date
Go
ALTER TABLE Payment.TempDiscount ADD [Id] UNIQUEIDENTIFIER NULL 
GO
Update Payment.TempDiscount set [Id]=NEWID()
GO
ALTER TABLE Payment.TempDiscount ALTER COLUMN [Id] UNIQUEIDENTIFIER NOT NULL
GO
ALTER TABLE Payment.TempDiscount ADD  CONSTRAINT [PK_TempDiscount] PRIMARY KEY ([Id])
GO





ALTER TABLE Payment.TransactionDiscount
	DROP CONSTRAINT PK_TransactionDiscount
GO
ALTER TABLE Payment.TransactionDiscount
	DROP CONSTRAINT DF__Transactio__Date__4C0144E4
GO
ALTER TABLE Payment.TransactionDiscount
	DROP COLUMN Date
GO
ALTER TABLE Payment.TransactionDiscount ADD [Id] UNIQUEIDENTIFIER NULL 
GO
Update Payment.TransactionDiscount set [Id]=NEWID()
GO
ALTER TABLE Payment.TransactionDiscount ALTER COLUMN [Id] UNIQUEIDENTIFIER NOT NULL
GO
ALTER TABLE Payment.TransactionDiscount ADD  CONSTRAINT [PK_TransactionDiscount] PRIMARY KEY ([Id])
GO
