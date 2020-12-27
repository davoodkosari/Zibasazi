
CREATE TABLE [Congress].[Resource](
	[Id] [uniqueidentifier] NOT NULL,
	[CongressId] [uniqueidentifier] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[UseLayoutId] [varchar](30) NOT NULL,
	[Type] [tinyint] NOT NULL,
	[Order] [tinyint] NOT NULL,
 CONSTRAINT [PK_CongressResource] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Congress].[Resource]  WITH CHECK ADD  CONSTRAINT [FK_Resource_Homa] FOREIGN KEY([CongressId])
REFERENCES [Congress].[Homa] ([Id])
GO

ALTER TABLE [Congress].[Resource] CHECK CONSTRAINT [FK_Resource_Homa]
GO



INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('8786ED3B-0E0E-460B-968B-B8F72457958C',null,N'مدیریت منابع',N'/Congress/Resource',null,1,1,1,null,null,2)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('76179913-7fda-43f3-a253-bda1d123475f','8786ED3B-0E0E-460B-968B-B8F72457958C')
GO


INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('2CE819E1-FD64-4D0B-8B94-0369C998107F',null,N'ایجاد',N'/Congress/Resource/Create','8786ED3B-0E0E-460B-968B-B8F72457958C',1,1,1,null,null,2)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('76179913-7fda-43f3-a253-bda1d123475f','2CE819E1-FD64-4D0B-8B94-0369C998107F')
GO

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('BF5EEF84-9235-46CE-8CBB-0EC3EEAEA382',null,N'ویرایش',N'/Congress/Resource/Edit','8786ED3B-0E0E-460B-968B-B8F72457958C',1,0,1,null,null,2)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('76179913-7fda-43f3-a253-bda1d123475f','BF5EEF84-9235-46CE-8CBB-0EC3EEAEA382')
GO

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('6F6F2A8E-3878-423D-82F0-9284174E3E1F',null,N'حذف',N'/Congress/Resource/Delete','8786ED3B-0E0E-460B-968B-B8F72457958C',1,0,1,null,null,2)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('76179913-7fda-43f3-a253-bda1d123475f','6F6F2A8E-3878-423D-82F0-9284174E3E1F')
GO


INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('79DB2EA9-32FF-4726-A1C2-CF452B84E5E4',null,N'لیست',N'/Congress/Resource/Index','8786ED3B-0E0E-460B-968B-B8F72457958C',1,1,1,null,null,2)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('76179913-7fda-43f3-a253-bda1d123475f','79DB2EA9-32FF-4726-A1C2-CF452B84E5E4')
GO



INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('09F17214-04D4-48D5-9CBF-317E1EE9113C',null,N'جزئیات',N'/Congress/Resource/Details','8786ED3B-0E0E-460B-968B-B8F72457958C',1,0,1,null,null,2)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('76179913-7fda-43f3-a253-bda1d123475f','09F17214-04D4-48D5-9CBF-317E1EE9113C')
GO


