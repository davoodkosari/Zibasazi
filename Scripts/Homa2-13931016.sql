GO
alter table [Congress].[CongressDefinition] add RptMiniUserCardId uniqueidentifier
go


INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES
           ('B1331C68-4925-42B4-B15F-A38F402C7504',null,N'طراحی کارت کوچک',N'/Congress/ReportPanel/DesginMiniCard',null,1,0,1,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('4CC144EC-2D0E-4F06-A000-564F5B5669F1','B1331C68-4925-42B4-B15F-A38F402C7504')
GO