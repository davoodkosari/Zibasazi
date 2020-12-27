alter table [Congress].[CongressDefinition] add RptArticleId uniqueidentifier
go
alter table [Congress].[CongressDefinition] add RptUserId uniqueidentifier
go

alter table [Congress].[CongressDefinition] add RptWorkShopUserId uniqueidentifier
go

alter table [Congress].[CongressDefinition] add RptHotelUserId uniqueidentifier
go

alter table [Congress].[CongressDefinition] add RptUserBoothId uniqueidentifier
go
update [Security].[Menu] set Url='/Congress/ReportPanel/UserCards' where Id='1917156D-A5B1-416F-8E44-C2FD1AE9244D'
go


