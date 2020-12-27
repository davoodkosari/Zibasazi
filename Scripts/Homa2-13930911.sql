alter table [Congress].[User] add Number bigint not null  IDENTITY(100000,1)


INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES ('54EA867F-A0B9-4944-B414-24F993E3A36E',null,N'مهمان',N'/Congress/Guest',null,0,1,1,null,null)
GO


INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('4CC144EC-2D0E-4F06-A000-564F5B5669F1','54EA867F-A0B9-4944-B414-24F993E3A36E')
GO

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES ('A59EB617-AD49-4A41-BBE9-EA840FDF3B13',null,N'لیست',N'/Congress/Guest/Index','54EA867F-A0B9-4944-B414-24F993E3A36E',3,1,1,null,null)
GO


INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('4CC144EC-2D0E-4F06-A000-564F5B5669F1','A59EB617-AD49-4A41-BBE9-EA840FDF3B13')
GO

