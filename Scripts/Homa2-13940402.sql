

CREATE TABLE [Congress].[SecurityUser](
	[CongressId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_SecurityUser] PRIMARY KEY CLUSTERED 
(
	[CongressId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Congress].[SecurityUser]  WITH CHECK ADD  CONSTRAINT [FK_SecurityUser_Homa] FOREIGN KEY([CongressId])
REFERENCES [Congress].[Homa] ([Id])
GO

ALTER TABLE [Congress].[SecurityUser] CHECK CONSTRAINT [FK_SecurityUser_Homa]
GO

ALTER TABLE [Congress].[SecurityUser]  WITH CHECK ADD  CONSTRAINT [FK_SecurityUser_User] FOREIGN KEY([UserId])
REFERENCES [Security].[User] ([Id])
GO

ALTER TABLE [Congress].[SecurityUser] CHECK CONSTRAINT [FK_SecurityUser_User]
GO

GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES
           ('DE92526D-CA5D-4AB6-AD30-B1F5FDBED99A',null,N'کاربران همایش',N'/Congress/SecurityUser',null,1,1,1,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('4CC144EC-2D0E-4F06-A000-564F5B5669F1','DE92526D-CA5D-4AB6-AD30-B1F5FDBED99A')
GO



INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES
           ('F238BD58-07E6-4AF8-8E89-E5ABDC8035C3',null,N'ایجاد',N'/Congress/SecurityUser/Create','DE92526D-CA5D-4AB6-AD30-B1F5FDBED99A',1,1,1,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('4CC144EC-2D0E-4F06-A000-564F5B5669F1','F238BD58-07E6-4AF8-8E89-E5ABDC8035C3')
GO

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES
           ('BE01F9B6-DB2F-4D9D-8DDE-F63989207DAA',null,N'ویرایش',N'/Congress/SecurityUser/Edit','DE92526D-CA5D-4AB6-AD30-B1F5FDBED99A',1,0,1,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('4CC144EC-2D0E-4F06-A000-564F5B5669F1','BE01F9B6-DB2F-4D9D-8DDE-F63989207DAA')
GO

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES
           ('C9196DB4-B1F4-40B6-8C86-9E54ED286F52',null,N'حذف',N'/Congress/SecurityUser/Delete','DE92526D-CA5D-4AB6-AD30-B1F5FDBED99A',1,0,1,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('4CC144EC-2D0E-4F06-A000-564F5B5669F1','C9196DB4-B1F4-40B6-8C86-9E54ED286F52')
GO


INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES
           ('1E843D7C-940A-4B9A-979F-B494C363465A',null,N'لیست',N'/Congress/SecurityUser/Index','DE92526D-CA5D-4AB6-AD30-B1F5FDBED99A',1,1,1,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('4CC144EC-2D0E-4F06-A000-564F5B5669F1','1E843D7C-940A-4B9A-979F-B494C363465A')
GO



INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES
           ('C424567C-588F-4EAA-8D88-5273A2538B10',null,N'جزئیات',N'/Congress/SecurityUser/Details','DE92526D-CA5D-4AB6-AD30-B1F5FDBED99A',1,0,1,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('4CC144EC-2D0E-4F06-A000-564F5B5669F1','C424567C-588F-4EAA-8D88-5273A2538B10')
GO

