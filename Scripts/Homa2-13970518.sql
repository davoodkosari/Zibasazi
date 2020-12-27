Alter Table [Congress].[Configuration] Add AccessToUserImportFromExcel bit Null
go
update [Congress].[Configuration] set AccessToUserImportFromExcel=1
go
Alter Table [Congress].[Configuration] alter column AccessToUserImportFromExcel bit  not null
go