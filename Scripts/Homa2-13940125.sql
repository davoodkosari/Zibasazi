alter table [ContentManage].[Menu] add IsVertical bit not null default 0
go
INSERT INTO [ContentManage].[Partials]
           ([Id]
           ,[Title]
           ,[Url]
           ,[Enabled]
           ,[ContextName]
           ,[OperationId]
           ,[RefId])
     VALUES
           ('BDA710EA-A593-4AA7-A57F-046DECAB8C43',
           N'منوی عمودی',N'/Congress/UIPanel/GetMenuVetical',1,N'همایشات','4CC144EC-2D0E-4F06-A000-564F5B5669F1',null)
GO
update  [ContentManage].[Partials] set url=N'/Congress/UIPanel/GetMenuHorizontal', [Title]=N'منوی افقی' where url=N'/Congress/UIPanel/GetMenu'
go


