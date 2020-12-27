alter table [Congress].[ArticleType] alter Column [Title] nvarchar(100) null
go
alter table [Congress].[Configuration] add BackgroundColor varchar(6)  null
go
alter table [Congress].[Configuration] add BackgroundImage uniqueidentifier null
go