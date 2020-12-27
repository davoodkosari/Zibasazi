

CREATE TABLE [Congress].[PivotCategory](
	[Id] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CongressId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Congress.PivotCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [Congress].[PivotCategory]  WITH CHECK ADD  CONSTRAINT [FK_PivotCategory_Homa] FOREIGN KEY([CongressId])
REFERENCES [Congress].[Homa] ([Id])
GO

ALTER TABLE [Congress].[PivotCategory] CHECK CONSTRAINT [FK_PivotCategory_Homa]
GO

ALTER TABLE [Congress].[Pivot] ADD [PivotCategoryId] [uniqueidentifier] NULL

GO

ALTER TABLE [Congress].[Pivot]  WITH CHECK ADD  CONSTRAINT [FK_Pivot_PivotCategory] FOREIGN KEY([PivotCategoryId])
REFERENCES [Congress].[PivotCategory] ([Id])
GO

ALTER TABLE [Congress].[Pivot] CHECK CONSTRAINT [FK_Pivot_PivotCategory]

GO
Alter table [Congress].[Article] ADD IsShare bit NOT NULL DEFAULT (0) 
GO
ALTER TABLE [Congress].[Configuration] ADD EnableArticleComment bit NOT NULL Default(0)
GO
alter table [Congress].[ArticleAuthors] add [Percentage] tinyint NULL
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('5FFFEC50-C094-42B4-B2FE-4C26CDA2C43B',null,N'گروه محور',N'/congress/pivotcategory',null,12,1,1,null,null,3)   
GO

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('e92a2176-b5cc-4bf7-959f-9d1122782227',null,N'ایجاد',N'/Congress/pivotcategory/Create','5fffec50-c094-42b4-b2fe-4c26cda2c43b',1,1,1,null,null,2)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('76179913-7fda-43f3-a253-bda1d123475f','e92a2176-b5cc-4bf7-959f-9d1122782227')
GO

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('5086a229-83d8-4299-8610-3dd5d819f55a',null,N'ویرایش',N'/Congress/pivotcategory/Edit','5fffec50-c094-42b4-b2fe-4c26cda2c43b',1,0,1,null,null,2)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('76179913-7fda-43f3-a253-bda1d123475f','5086a229-83d8-4299-8610-3dd5d819f55aE')
GO

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('4278205f-bf49-42a3-aad6-cd91c39ff615',null,N'حذف',N'/Congress/pivotcategory/Delete','5fffec50-c094-42b4-b2fe-4c26cda2c43b',1,0,1,null,null,2)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('76179913-7fda-43f3-a253-bda1d123475f','4278205f-bf49-42a3-aad6-cd91c39ff615')
GO


INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('90887fde-ff08-43ff-869a-2df1ff7e68cb',null,N'لیست',N'/Congress/pivotcategory/Index','5fffec50-c094-42b4-b2fe-4c26cda2c43b',1,1,1,null,null,2)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('76179913-7fda-43f3-a253-bda1d123475f','90887fde-ff08-43ff-869a-2df1ff7e68cb')
GO



INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('53c7c8e8-42d9-47ba-9bd3-8732190d9b9a',null,N'جزئیات',N'/Congress/pivotcategory/Details','5fffec50-c094-42b4-b2fe-4c26cda2c43b',1,0,1,null,null,2)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('76179913-7fda-43f3-a253-bda1d123475f','53c7c8e8-42d9-47ba-9bd3-8732190d9b9a')
GO


CREATE TABLE [Congress].[ArticleUserComment](
	[Id] [uniqueidentifier] NOT NULL,
	[ArticleId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](150) NULL,
	[Description] [nvarchar](max) NULL,
	[SaveDate] [char](10) NOT NULL,
	[SaveTime] [char](5) NOT NULL,
	[ConfirmAdmin] [bit] NOT NULL,
	[IsLike] [bit] NULL CONSTRAINT [DF__ArticleUs__IsLik__76818E95]  DEFAULT ((0)),
	[IP] [varchar](15) NOT NULL,
 CONSTRAINT [PK_ArticleUserComment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [Congress].[ArticleUserComment]  WITH CHECK ADD  CONSTRAINT [FK_ArticleUserComment_Article] FOREIGN KEY([ArticleId])
REFERENCES [Congress].[Article] ([Id])
GO

ALTER TABLE [Congress].[ArticleUserComment] CHECK CONSTRAINT [FK_ArticleUserComment_Article]
GO

alter table [Slider].[SlideItem] add StartDate char(10) NULL
go
alter table [Slider].[SlideItem] add FinishDate char(10) NULL
go
CREATE TABLE [Congress].[CustomMessage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CongressId] [uniqueidentifier] NOT NULL,
	[Type] [tinyint] NOT NULL,
 CONSTRAINT [PK_Congress.CustomMessage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Congress].[CustomMessage]  WITH CHECK ADD  CONSTRAINT [FK_CustomMessage_Homa] FOREIGN KEY([CongressId])
REFERENCES [Congress].[Homa] ([Id])
GO




CREATE TABLE [FormGenerator].[FormEvaluation](
	[ControlId] [varchar](300) NOT NULL,
	[Weight] [float] NULL,
	[OpinionCount] [int] NULL,
	[MinScale] [int] NULL,
	[MaxScale] [int] NULL,
	 CONSTRAINT [PK_FormEvaluation] PRIMARY KEY CLUSTERED 
(
	[ControlId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


CREATE TABLE [dbo].[Tracker](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RefId] [nvarchar](200) NULL,
	[RefTitle] [nvarchar](500) NULL,
	[IpAddress] [nvarchar](50) NULL,
	[UserName] [nvarchar](50) NULL,
	[Date] [char](10) NULL,
	[Time] [char](10) NULL,
	[ObjectName] [nvarchar](100) NULL,
	[FieldDesc] [nvarchar](100) NULL,
	[FieldName] [nvarchar](50) NULL,
	[OldVal] [nvarchar](200) NULL,
	[NewValue] [nvarchar](200) NULL,
	[Operation] [nvarchar](200) NULL,
	[MasterRefId] [nvarchar](200) NULL,
	[MasterObjectRefId] [nvarchar](200) NULL,
	[RootId] [nvarchar](200) NULL,
	[RootTitle] [nvarchar](200) NULL,
 CONSTRAINT [PK_Track] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


alter table [Congress].[RefereeCartable] add Score float NULL
GO
INSERT INTO [Security].[Menu]
           ([Id]
           ,[ApplicationID]
           ,[Title]
           ,[Url]
           ,[ParentId]
           ,[Order]
           ,[Display]
           ,[Enabled]
           ,[HelpId]
           ,[ImageId]
           ,[MenuGroupId])
     VALUES
           ('13F3D77C-E8CA-433A-83C5-F15917A3396F'
           ,0
           ,N'GetModal'
           ,'/CommonUI/GetModal'
           ,NULL
           ,2
           ,0
           ,1
           ,NULL
           ,NULL
           ,NULL)

GO

INSERT INTO [Security].[OperationMenu]
           ([OperationId]
           ,[MenuId])
     VALUES
           ('76179913-7fda-43f3-a253-bda1d123475f'
           ,'13F3D77C-E8CA-433A-83C5-F15917A3396F')
GO
update  [Security].[Menu] set [Url]=N'/ContentManager/UIDesginPanel/Index' where Id='7e207db4-d666-4d01-8e35-a214f517e6bd'
go
update [Security].[OperationMenu] set [OperationId]='76179913-7fda-43f3-a253-bda1d123475f' where [OperationId]='4cc144ec-2d0e-4f06-a000-564f5b5669f1' 
and [MenuId] not in (select [MenuId] from [Security].[OperationMenu]  where [OperationId]='76179913-7fda-43f3-a253-bda1d123475f')
go
update [ContentManage].[Partials] set [OperationId]='76179913-7fda-43f3-a253-bda1d123475f' where [OperationId]='4cc144ec-2d0e-4f06-a000-564f5b5669f1' 
and Id not in (select Id from [ContentManage].[Partials]   where [OperationId]='76179913-7fda-43f3-a253-bda1d123475f')
go
delete [Security].[Operation] where Id='4cc144ec-2d0e-4f06-a000-564f5b5669f1'