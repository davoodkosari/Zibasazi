
CREATE TABLE [ContentManage].[MenuHtml](
	[Id] [uniqueidentifier] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[IsExternal] [bit] NOT NULL 
 CONSTRAINT [PK_MenuHtml] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 
go

CREATE TABLE [Congress].[CongressMenuHtml](
	[CongressId] [uniqueidentifier] NOT NULL,
	[MenuHtmlId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CongressMenuHtml] PRIMARY KEY CLUSTERED 
(
	[CongressId] ASC,
	[MenuHtmlId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Congress].[CongressMenuHtml]  WITH CHECK ADD  CONSTRAINT [FK_CongressMenuHtml_Homa] FOREIGN KEY([CongressId])
REFERENCES [Congress].[Homa] ([Id])
GO

ALTER TABLE [Congress].[CongressMenuHtml] CHECK CONSTRAINT [FK_CongressMenuHtml_Homa]
GO

ALTER TABLE [Congress].[CongressMenuHtml]  WITH CHECK ADD  CONSTRAINT [FK_CongressMenuHtml_Menu] FOREIGN KEY([MenuHtmlId])
REFERENCES [ContentManage].[MenuHtml] ([Id])
GO

ALTER TABLE [Congress].[CongressMenuHtml] CHECK CONSTRAINT [FK_CongressMenuHtml_Menu]
GO



INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('F869FE1E-E43E-4813-8E7C-76E0726A2500',null,N'html منو ها',N'/Congress/CongressMenuHtml',null,1,1,1,null,null,2)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('76179913-7fda-43f3-a253-bda1d123475f','F869FE1E-E43E-4813-8E7C-76E0726A2500')
GO


INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('35155340-A699-478F-A71A-90280D249AE7',null,N'ایجاد',N'/Congress/CongressMenuHtml/Create','F869FE1E-E43E-4813-8E7C-76E0726A2500',1,1,1,null,null,2)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('76179913-7fda-43f3-a253-bda1d123475f','35155340-A699-478F-A71A-90280D249AE7')
GO

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('935101D3-461D-42D9-91A9-A707683EED8E',null,N'ویرایش',N'/Congress/CongressMenuHtml/Edit','F869FE1E-E43E-4813-8E7C-76E0726A2500',1,0,1,null,null,2)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('76179913-7fda-43f3-a253-bda1d123475f','935101D3-461D-42D9-91A9-A707683EED8E')
GO

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('7684FEC0-C80E-4A4C-986C-DA2B6D6C0D9D',null,N'حذف',N'/Congress/CongressMenuHtml/Delete','F869FE1E-E43E-4813-8E7C-76E0726A2500',1,0,1,null,null,2)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('76179913-7fda-43f3-a253-bda1d123475f','7684FEC0-C80E-4A4C-986C-DA2B6D6C0D9D')
GO


INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('88898248-4122-4D51-84D8-460F29ED3688',null,N'لیست',N'/Congress/CongressMenuHtml/Index','F869FE1E-E43E-4813-8E7C-76E0726A2500',1,1,1,null,null,2)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('76179913-7fda-43f3-a253-bda1d123475f','88898248-4122-4D51-84D8-460F29ED3688')
GO



INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('03B3BCE4-3D3D-4891-B338-86F7BD7E2F2D',null,N'جزئیات',N'/Congress/CongressMenuHtml/Details','F869FE1E-E43E-4813-8E7C-76E0726A2500',1,0,1,null,null,2)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('76179913-7fda-43f3-a253-bda1d123475f','03B3BCE4-3D3D-4891-B338-86F7BD7E2F2D')
GO

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('3DF19B59-2C1A-4899-B57E-91243F989E7C',null,N'html منو ها',N'/ContentManager/MenuHtml',null,1,1,1,null,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('b6684ff0-3f77-4282-9458-17ec7926a1d4','3DF19B59-2C1A-4899-B57E-91243F989E7C')
GO


INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('A9EB2B9C-91E6-4BC4-91E0-F36D22B49D34',null,N'ایجاد',N'/ContentManager/MenuHtml/Create','3DF19B59-2C1A-4899-B57E-91243F989E7C',1,1,1,null,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('b6684ff0-3f77-4282-9458-17ec7926a1d4','A9EB2B9C-91E6-4BC4-91E0-F36D22B49D34')
GO

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('2BCDBEFE-EB30-4506-8D66-767811D60007',null,N'ویرایش',N'/ContentManager/MenuHtml/Edit','3DF19B59-2C1A-4899-B57E-91243F989E7C',1,0,1,null,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('b6684ff0-3f77-4282-9458-17ec7926a1d4','2BCDBEFE-EB30-4506-8D66-767811D60007')
GO

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('8202A4BE-37D2-49C6-B85F-2B57972CD27A',null,N'حذف',N'/ContentManager/MenuHtml/Delete','3DF19B59-2C1A-4899-B57E-91243F989E7C',1,0,1,null,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('b6684ff0-3f77-4282-9458-17ec7926a1d4','8202A4BE-37D2-49C6-B85F-2B57972CD27A')
GO


INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('79B87BA9-D2EA-47D3-A996-E9AEA3F05414',null,N'لیست',N'/ContentManager/MenuHtml/Index','3DF19B59-2C1A-4899-B57E-91243F989E7C',1,1,1,null,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('b6684ff0-3f77-4282-9458-17ec7926a1d4','79B87BA9-D2EA-47D3-A996-E9AEA3F05414')
GO



INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('650BEFF5-C2B5-42E1-96F7-0D383E64924E',null,N'جزئیات',N'/ContentManager/MenuHtml/Details','3DF19B59-2C1A-4899-B57E-91243F989E7C',1,0,1,null,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('b6684ff0-3f77-4282-9458-17ec7926a1d4','650BEFF5-C2B5-42E1-96F7-0D383E64924E')
GO