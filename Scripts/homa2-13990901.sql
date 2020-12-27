

CREATE TABLE [Congress].[CongressType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Enable] [bit] NOT NULL,
 CONSTRAINT [PK_CongressType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



alter table  [Congress].[Homa] add CongressTypeId int
go


ALTER TABLE [Congress].[Homa]  WITH CHECK ADD  CONSTRAINT [FK_Congress_CongressType] FOREIGN KEY([CongressTypeId])
REFERENCES [Congress].[CongressType] ([Id])
GO

ALTER TABLE [Congress].[Homa] CHECK CONSTRAINT [FK_Congress_CongressType]

GO

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('CF691B97-F7EC-4024-8490-5D1AFE2B82F1',null,N'نوع رویداد',N'/congress/CongressType',null,12,1,1,null,null,6)   
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('76179913-7fda-43f3-a253-bda1d123475f','CF691B97-F7EC-4024-8490-5D1AFE2B82F1')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('141344C0-639F-4868-8285-765D855A89E6',null,N'ایجاد',N'/Congress/CongressType/Create','CF691B97-F7EC-4024-8490-5D1AFE2B82F1',1,1,1,null,null,6)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('76179913-7fda-43f3-a253-bda1d123475f','141344C0-639F-4868-8285-765D855A89E6')
GO

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('EDCC26D7-7B4B-4DB8-ACA5-7E14759A7B81',null,N'ویرایش',N'/Congress/CongressType/Edit','CF691B97-F7EC-4024-8490-5D1AFE2B82F1',1,0,1,null,null,6)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('76179913-7fda-43f3-a253-bda1d123475f','EDCC26D7-7B4B-4DB8-ACA5-7E14759A7B81')
GO

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('F1C6D531-A851-4C08-B300-A745E4570DE9',null,N'حذف',N'/Congress/CongressType/Delete','CF691B97-F7EC-4024-8490-5D1AFE2B82F1',1,0,1,null,null,6)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('76179913-7fda-43f3-a253-bda1d123475f','F1C6D531-A851-4C08-B300-A745E4570DE9')
GO


INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('A96556F6-366C-47D8-884A-35BB870EC3A8',null,N'لیست',N'/Congress/CongressType/Index','CF691B97-F7EC-4024-8490-5D1AFE2B82F1',1,1,1,null,null,6)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('76179913-7fda-43f3-a253-bda1d123475f','A96556F6-366C-47D8-884A-35BB870EC3A8')
GO



INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('F885E360-019A-4961-8E2B-B726F8006551',null,N'جزئیات',N'/Congress/CongressType/Details','CF691B97-F7EC-4024-8490-5D1AFE2B82F1',1,0,1,null,null,6)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('76179913-7fda-43f3-a253-bda1d123475f','F885E360-019A-4961-8E2B-B726F8006551')
GO

alter table [Congress].[Homa] add IsDefaultForConfig bit 
go
update  [Congress].[Homa] set IsDefaultForConfig=0
go
alter table [Congress].[Homa] alter column  IsDefaultForConfig bit not null
go
update [Security].[MenuGroup] set [Name]=N'آثار' where Id=3
go