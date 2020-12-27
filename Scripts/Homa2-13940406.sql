alter table [Congress].[Configuration] add SentArticleSpecialReferee bit not null default 0
go
alter table [Congress].[Referee] add IsSpecial bit not null default 0
go

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES
           ('718BB8A0-817E-49A7-9776-0A36D618B847',null,N'دسترسی از پنل مدیریت به پنل داور',N'/Congress/ManagmentPanel/LoginAsReferee',null,2,0,1,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('4CC144EC-2D0E-4F06-A000-564F5B5669F1','718BB8A0-817E-49A7-9776-0A36D618B847')
GO
