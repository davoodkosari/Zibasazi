Alter Table [Congress].[Configuration] Add RequireArticleTypeForCertificate bit
go
update [Congress].[Configuration] set RequireArticleTypeForCertificate=0
go
Alter Table [Congress].[Configuration] alter column  RequireArticleTypeForCertificate bit not null
go
Alter Table [Congress].[Configuration] Add ArticleCertificateState tinyint
go
update [Congress].[Configuration] set ArticleCertificateState=2
go
