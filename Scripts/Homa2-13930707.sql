alter table [Payment].[Temp] add ParentId	uniqueidentifier
go
ALTER TABLE [Payment].[TempDiscount] ADD [Date] datetime not null default getdate() 
go
ALTER TABLE [Payment].[TempDiscount]  DROP CONSTRAINT [PK_TempDiscount]
go
ALTER TABLE [Payment].[TempDiscount] ADD CONSTRAINT  [PK_TempDiscount] PRIMARY KEY ([TempId],[DiscountTypeId],[Date])
go


ALTER TABLE [Payment].[TransactionDiscount] ADD [Date] datetime not null default getdate() 
go
ALTER TABLE [Payment].[TransactionDiscount]  DROP CONSTRAINT [PK_TransactionDiscount]
go
ALTER TABLE [Payment].[TransactionDiscount] ADD CONSTRAINT  [PK_TransactionDiscount] PRIMARY KEY ([TransactionId],[DiscountTypeId],[Date])
go