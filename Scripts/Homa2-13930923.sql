GO

CREATE TABLE [Congress].[ChipsFood](
	[Id] [uniqueidentifier] NOT NULL,
	[CongressId] [uniqueidentifier] NOT NULL,
	[Capacity] [int] NOT NULL,
		[BaseCapacity] [int] NOT NULL,
	[DaysInfo] [nvarchar](1000) NULL,
 CONSTRAINT [PK_ChipsFood] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Congress].[ChipsFood]  WITH CHECK ADD  CONSTRAINT [FK_ChipsFood_Homa] FOREIGN KEY([CongressId])
REFERENCES [Congress].[Homa] ([Id])
GO

ALTER TABLE [Congress].[ChipsFood] CHECK CONSTRAINT [FK_ChipsFood_Homa]
GO


GO

CREATE TABLE [Congress].[ChipsFoodUser](
	[ChipsFoodId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ChipsFoodUser] PRIMARY KEY CLUSTERED 
(
	[ChipsFoodId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Congress].[ChipsFoodUser]  WITH CHECK ADD  CONSTRAINT [FK_ChipsFoodUser_ChipsFood] FOREIGN KEY([ChipsFoodId])
REFERENCES [Congress].[ChipsFood] ([Id])
GO

ALTER TABLE [Congress].[ChipsFoodUser] CHECK CONSTRAINT [FK_ChipsFoodUser_ChipsFood]
GO

ALTER TABLE [Congress].[ChipsFoodUser]  WITH CHECK ADD  CONSTRAINT [FK_ChipsFoodUser_User] FOREIGN KEY([UserId])
REFERENCES [Congress].[User] ([Id])
GO

ALTER TABLE [Congress].[ChipsFoodUser] CHECK CONSTRAINT [FK_ChipsFoodUser_User]
GO

alter table [Congress].[CongressDefinition] add RptChipFoodId	uniqueidentifier


GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES
           ('BEA22F7C-04CA-4443-8AE3-E5C19BC4ED78',null,N'ژتون غذا',N'/Congress/ChipsFood',null,1,1,1,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('4CC144EC-2D0E-4F06-A000-564F5B5669F1','BEA22F7C-04CA-4443-8AE3-E5C19BC4ED78')
GO



INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES
           ('EC84603B-9B6E-41FF-B655-A266DCA6036F',null,N'ایجاد',N'/Congress/ChipsFood/Create','BEA22F7C-04CA-4443-8AE3-E5C19BC4ED78',1,1,1,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('4CC144EC-2D0E-4F06-A000-564F5B5669F1','EC84603B-9B6E-41FF-B655-A266DCA6036F')
GO

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES
           ('08EA255A-B13A-4324-95DC-546555B75C79',null,N'ویرایش',N'/Congress/ChipsFood/Edit','BEA22F7C-04CA-4443-8AE3-E5C19BC4ED78',1,0,1,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('4CC144EC-2D0E-4F06-A000-564F5B5669F1','08EA255A-B13A-4324-95DC-546555B75C79')
GO

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES
           ('22439CC5-5193-4FB6-8228-BE9787C8023D',null,N'حذف',N'/Congress/ChipsFood/Delete','BEA22F7C-04CA-4443-8AE3-E5C19BC4ED78',1,0,1,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('4CC144EC-2D0E-4F06-A000-564F5B5669F1','22439CC5-5193-4FB6-8228-BE9787C8023D')
GO


INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES
           ('CF2B2F4E-90A9-43F7-97C2-61E22166C17C',null,N'لیست',N'/Congress/ChipsFood/Index','BEA22F7C-04CA-4443-8AE3-E5C19BC4ED78',1,1,1,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('4CC144EC-2D0E-4F06-A000-564F5B5669F1','CF2B2F4E-90A9-43F7-97C2-61E22166C17C')
GO



INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES
           ('1B4E8AC8-C591-4D09-BCD4-74C14F753A89',null,N'جزئیات',N'/Congress/ChipsFood/Details','BEA22F7C-04CA-4443-8AE3-E5C19BC4ED78',1,0,1,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('4CC144EC-2D0E-4F06-A000-564F5B5669F1','1B4E8AC8-C591-4D09-BCD4-74C14F753A89')
GO


