alter table [News].[News] add [ExpireDate] char(10)
go
alter table [News].[News] add [ExpireTime] char(5)
go


INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('A2B5D85E-4380-44D4-B25F-773232CE084F',null,N'title',N'/Congress/ManagmentPanel/GetModifyUserWorkShop',null,1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','A2B5D85E-4380-44D4-B25F-773232CE084F')
GO


INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('790BA22A-A51F-4CCE-8F2B-24F9597C572C',null,N'title',N'/Congress/User/SearchUser',null,1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','790BA22A-A51F-4CCE-8F2B-24F9597C572C')
GO



INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('0A9E32A5-DA20-480A-88A8-05945BC5F446',null,N'title',N'/Congress/ManagmentPanel/GetModifyUserHotel',null,1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','0A9E32A5-DA20-480A-88A8-05945BC5F446')
GO


INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('713DA2D6-A153-408C-920B-B06BEFE9508B',null,N'title',N'/Congress/ManagmentPanel/GetModifyUserBooth',null,1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','713DA2D6-A153-408C-920B-B06BEFE9508B')
GO

alter table [Congress].[Configuration] add AllowUserAddAuthor bit
go
update [Congress].[Configuration] set AllowUserAddAuthor=1
go
alter table [Congress].[Configuration] alter column  AllowUserAddAuthor bit not null
go

update [Security].[Menu] set Title=N'مدیریت (مقاله/اثر/ایده)' where Id='747d59a6-7396-42a7-97f7-bf42af367701'
go
update [Security].[Menu] set Title=N'گزارش تغییرات' where Id='3345c0a2-32cc-4af1-a562-ef46a64c9325'
go
update [Security].[MenuGroup] set Name=N'مقاله/ایده/اثر' where Id=3
go
alter table [Congress].[Configuration] add EnabledArticleKeyword bit
go
update [Congress].[Configuration] set EnabledArticleKeyword=1
go
alter table [Congress].[Configuration] alter column  EnabledArticleKeyword bit not null
go
