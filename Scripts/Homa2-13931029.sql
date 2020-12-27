

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES
           ('DAF01B6C-B09E-4608-8459-DB2282277439',null,N'ورود مدیر به پنل کاربر',N'/Congress/ManagmentPanel/LoginAsUser',null,1,0,1,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('4CC144EC-2D0E-4F06-A000-564F5B5669F1','DAF01B6C-B09E-4608-8459-DB2282277439')
GO
alter table [Congress].[UserRegisterPaymentType] add CanUserSelect bit not null default 1
go
alter table [Congress].[Booth] add MaxBoothOfficerCount int
go