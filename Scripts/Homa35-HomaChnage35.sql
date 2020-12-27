drop table [WebDesign].[Configuration]
go

CREATE TABLE [WebDesign].[Configuration](
	[Id] [uniqueidentifier] NOT NULL,
	[MaxNewsShow] [int] NULL,
	[Enabled] [bit] NOT NULL,
	[IntroPageId] [int] NULL,
	[HeaderId] [int] NULL,
	[FooterId] [int] NULL,
	[BigSlideId] [smallint] NULL,
	[MiniSlideId] [smallint] NULL,
	[AverageSlideId] [smallint] NULL,
	[CertificateSlideId] [smallint] NULL,
	[EventsSlideId] [smallint] NULL,
	[DefaultContainerID] [uniqueidentifier] NULL,
	[DefaultHTMLID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_WebDesignConfiguration] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [WebDesign].[Container]    Script Date: 2/18/2019 3:08:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WebDesign].[Container](
	[WebId] [uniqueidentifier] NOT NULL,
	[ContainerId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Container] PRIMARY KEY CLUSTERED 
(
	[WebId] ASC,
	[ContainerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [WebDesign].[Content]    Script Date: 2/18/2019 3:08:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WebDesign].[Content](
	[WebId] [uniqueidentifier] NOT NULL,
	[ContentId] [int] NOT NULL,
 CONSTRAINT [PK_Content_1] PRIMARY KEY CLUSTERED 
(
	[WebId] ASC,
	[ContentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [WebDesign].[FAQ]    Script Date: 2/18/2019 3:08:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WebDesign].[FAQ](
	[WebId] [uniqueidentifier] NOT NULL,
	[FAQId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_FAQ_1] PRIMARY KEY CLUSTERED 
(
	[WebId] ASC,
	[FAQId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [WebDesign].[Folder]    Script Date: 2/18/2019 3:08:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WebDesign].[Folder](
	[WebId] [uniqueidentifier] NOT NULL,
	[FolderId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Folder_1] PRIMARY KEY CLUSTERED 
(
	[WebId] ASC,
	[FolderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [WebDesign].[Forms]    Script Date: 2/18/2019 3:08:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WebDesign].[Forms](
	[WebId] [uniqueidentifier] NOT NULL,
	[FormId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Forms] PRIMARY KEY CLUSTERED 
(
	[WebId] ASC,
	[FormId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [WebDesign].[Gallery]    Script Date: 2/18/2019 3:08:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WebDesign].[Gallery](
	[WebId] [uniqueidentifier] NOT NULL,
	[GalleryId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Gallery_1] PRIMARY KEY CLUSTERED 
(
	[WebId] ASC,
	[GalleryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [WebDesign].[Html]    Script Date: 2/18/2019 3:08:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WebDesign].[Html](
	[WebId] [uniqueidentifier] NOT NULL,
	[HtmlDesginId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Html] PRIMARY KEY CLUSTERED 
(
	[WebId] ASC,
	[HtmlDesginId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [WebDesign].[Language]    Script Date: 2/18/2019 3:08:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [WebDesign].[Language](
	[WebId] [uniqueidentifier] NOT NULL,
	[LanguageId] [char](5) NOT NULL,
 CONSTRAINT [PK_Language_1] PRIMARY KEY CLUSTERED 
(
	[WebId] ASC,
	[LanguageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [WebDesign].[MapDetails]    Script Date: 2/18/2019 3:08:45 PM ******/

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WebDesign].[Menu](
	[WebId] [uniqueidentifier] NOT NULL,
	[MenuId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Menu_1] PRIMARY KEY CLUSTERED 
(
	[WebId] ASC,
	[MenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [WebDesign].[News]    Script Date: 2/18/2019 3:08:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WebDesign].[News](
	[WebId] [uniqueidentifier] NOT NULL,
	[NewsId] [int] NOT NULL,
 CONSTRAINT [PK_News_1] PRIMARY KEY CLUSTERED 
(
	[WebId] ASC,
	[NewsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [WebDesign].[Resource]    Script Date: 2/18/2019 3:08:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [WebDesign].[Resource](
	[Id] [uniqueidentifier] NOT NULL,
	[WebId] [uniqueidentifier] NOT NULL,
	[LanuageId] [char](5) NOT NULL,
	[Type] [tinyint] NOT NULL,
	[FileId] [uniqueidentifier] NULL,
	[Order] [tinyint] NOT NULL,
	[Text] [nvarchar](1000) NULL,
 CONSTRAINT [PK_Resource] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [WebDesign].[Slider]    Script Date: 2/18/2019 3:08:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WebDesign].[Slider](
	[WebId] [uniqueidentifier] NOT NULL,
	[SlideId] [smallint] NOT NULL,
 CONSTRAINT [PK_Slider] PRIMARY KEY CLUSTERED 
(
	[WebId] ASC,
	[SlideId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [WebDesign].[WebSite]    Script Date: 2/18/2019 3:08:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WebDesign].[WebSite](
	[Id] [uniqueidentifier] NOT NULL,
	[InstallPath] [nvarchar](250) NOT NULL,
	[Enabled] [bit] NOT NULL,
 CONSTRAINT [PK_WebSite_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


INSERT [ContentManage].[Partials] ([Id], [Title], [Url], [ContextName], [Enabled], [OperationId], [RefId], [ContainerId]) VALUES (N'bdc7607a-a5d1-4a7b-b86e-0b53852a08b9', N'درباره ما', N'/WebDesign/UIPanel/About', NULL, 1, N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', NULL, NULL)
GO
INSERT [ContentManage].[Partials] ([Id], [Title], [Url], [ContextName], [Enabled], [OperationId], [RefId], [ContainerId]) VALUES (N'45fe43f7-2c81-4b68-b903-104129393200', N'منوی سایت', N'/WebDesign/UIPanel/GetMenu', NULL, 1, N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', NULL, NULL)
GO
INSERT [ContentManage].[Partials] ([Id], [Title], [Url], [ContextName], [Enabled], [OperationId], [RefId], [ContainerId]) VALUES (N'def47308-5ae1-4eeb-b60e-7897abfbaf53', N'هدر سایت', N'/WebDesign/UIPanel/GetHeader', NULL, 1, N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', NULL, NULL)
GO
INSERT [ContentManage].[Partials] ([Id], [Title], [Url], [ContextName], [Enabled], [OperationId], [RefId], [ContainerId]) VALUES (N'bb90684b-dcdd-4ef2-a3c6-8d57529dc52c', N'اسلاید شو بزرگ سایت', N'/WebDesign/UIPanel/GetBigSlideShow', NULL, 1, N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', NULL, NULL)
GO
INSERT [ContentManage].[Partials] ([Id], [Title], [Url], [ContextName], [Enabled], [OperationId], [RefId], [ContainerId]) VALUES (N'f676fb96-c2a2-40f2-b554-8ee34061fe53', N'اسلاید شو کوچک', N'/WebDesign/UIPanel/GetMinSlideShow', NULL, 1, N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', NULL, NULL)
GO
INSERT [ContentManage].[Partials] ([Id], [Title], [Url], [ContextName], [Enabled], [OperationId], [RefId], [ContainerId]) VALUES (N'e5f91d23-9c84-4992-8d4b-938ea42cb0c0', N'پایین صفحه سایت', N'/WebDesign/UIPanel/GetFooter', NULL, 1, N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', NULL, NULL)
GO
INSERT [ContentManage].[Partials] ([Id], [Title], [Url], [ContextName], [Enabled], [OperationId], [RefId], [ContainerId]) VALUES (N'810968b9-d063-4a0a-b4db-d827f08171de', N'اسلاید شو متوسط', N'/WebDesign/UIPanel/GetAverageSlideShow', NULL, 1, N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', NULL, NULL)
GO
INSERT [ContentManage].[Partials] ([Id], [Title], [Url], [ContextName], [Enabled], [OperationId], [RefId], [ContainerId]) VALUES (N'c9c66af9-7ec5-4a59-a73c-f17a90495c6c', N'اخبار سایت', N'/WebDesign/UIPanel/GetNews', NULL, 1, N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', NULL, NULL)
GO

INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'b6fbac20-0b49-4bd9-9782-01f5f75b5105', NULL, N'ویرایش', N'/webdesign/menu/edit', N'0b50c83e-ebce-4fe3-90fc-502badf8ce88', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'f0b67c5e-4e1e-4b5e-8ef6-0252f1dff0e0', NULL, N'ایجاد', N'/webdesign/content/create', N'e8a83acf-18d7-48b9-9d30-436257773bc8', 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'ae4b3a32-7aab-40bf-ac37-03e0eb007e42', NULL, N'قالب ها', N'/webdesign/container', NULL, 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'9e1476cb-5a81-456d-ab02-06ed6eddfd0f', NULL, N'حذف', N'/webdesign/faq/delete', N'd0bb0246-bd26-42d2-9c70-a6fc224240fc', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'9a6daba0-86cb-4aee-bd60-147b5030cb60', NULL, N'ویرایش', N'/webdesign/resource/edit', N'930518ea-14e8-4fd1-a288-5fc9b5b7b42e', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'4cecf1a8-dad7-4943-afeb-1b0183945305', NULL, N'جزئیات', N'/webdesign/news/details', N'3f065ba6-50de-4d8f-960a-9181c97d24f1', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'1749bedf-6883-4e71-bef9-1d4e3eb8a514', NULL, N'ویرایش', N'/webdesign/faq/edit', N'd0bb0246-bd26-42d2-9c70-a6fc224240fc', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'871da0a1-5c89-4eb6-9696-1d6d472a9a1c', NULL, N'حذف', N'/webdesign/language/delete', N'40178441-3c28-423a-b116-fb173e5d68a8', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'5b4a4504-eddf-404c-a180-1fbb5eb92055', NULL, N'ایجاد', N'/webdesign/menu/create', N'0b50c83e-ebce-4fe3-90fc-502badf8ce88', 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'aa63061f-078a-4696-83e3-21d15db53f22', NULL, N'ایجاد', N'/webdesign/folder/create', N'44293ee0-ca6b-4b7f-9a32-293dcc67d2d2', 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'95b5d60b-048e-4f77-bd5a-228fc330f5d6', NULL, N'لیست', N'/webdesign/language/index', N'40178441-3c28-423a-b116-fb173e5d68a8', 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'53421b5d-2bc6-4d6e-ba4b-2650003567a6', NULL, N'لیست', N'/webdesign/html/index', N'60af751d-30c8-4055-9180-e2a4d86d1611', 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'70fd6da5-28c9-4068-92e6-28f2b91cb55f', NULL, N'حذف', N'/webdesign/forms/delete', N'ececa015-b895-4fc7-a579-2bbb0f65f7ab', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'44293ee0-ca6b-4b7f-9a32-293dcc67d2d2', NULL, N'مدیریت فایل', N'/webdesign/folder', NULL, 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'ececa015-b895-4fc7-a579-2bbb0f65f7ab', NULL, N'فرم ساز', N'/webdesign/forms', NULL, 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'bb0b2d78-877a-45d4-b0ac-30bfb1a93326', NULL, N'جزئیات', N'/webdesign/resource/details', N'930518ea-14e8-4fd1-a288-5fc9b5b7b42e', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'51487a16-ef3e-4ea8-93ce-320636dec3e4', NULL, N'حذف', N'/webdesign/gallery/delete', N'6fc9bc15-9fff-48e5-b380-8c736719a335', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'6744c8c8-1e39-4c18-89a3-3610eb16a50a', NULL, N'حذف', N'/webdesign/folder/delete', N'44293ee0-ca6b-4b7f-9a32-293dcc67d2d2', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'c13066e6-2736-45ad-9826-37a12c381862', NULL, N'جزئیات', N'/webdesign/content/details', N'e8a83acf-18d7-48b9-9d30-436257773bc8', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'72c425b4-844c-4e76-a4b8-385a563789ce', NULL, N'لیست', N'/webdesign/faq/index', N'd0bb0246-bd26-42d2-9c70-a6fc224240fc', 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'f4697329-1d48-4856-9b38-38cb58147542', NULL, N'لیست', N'/webdesign/folder/index', N'44293ee0-ca6b-4b7f-9a32-293dcc67d2d2', 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'1a1e1b66-5968-4acc-b934-426a65b235cc', NULL, N'ایجاد', N'/webdesign/forms/create', N'ececa015-b895-4fc7-a579-2bbb0f65f7ab', 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'd84d5f0c-3f95-415c-ba4d-434e93c9c73a', NULL, N'ویرایش', N'/webdesign/gallery/edit', N'6fc9bc15-9fff-48e5-b380-8c736719a335', 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'e8a83acf-18d7-48b9-9d30-436257773bc8', NULL, N'محتوا', N'/webdesign/content', NULL, 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'c73e4a75-626c-4fd1-98f3-437f67fb6981', NULL, N'ویرایش', N'/webdesign/language/edit', N'40178441-3c28-423a-b116-fb173e5d68a8', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'd57e4472-741f-4366-9ed4-43bb4a44dad8', NULL, N'لیست', N'/webdesign/menu/index', N'0b50c83e-ebce-4fe3-90fc-502badf8ce88', 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'a5587710-9c84-4313-a425-49a941cc2dec', NULL, N'ویرایش', N'/webdesign/website/edit', N'7b4c8bee-32df-4509-946c-5a07d6f49eac', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'4063f44b-5a1d-42db-84d5-4a5fde02fd6c', NULL, N'حذف', N'/webdesign/news/delete', N'3f065ba6-50de-4d8f-960a-9181c97d24f1', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'a8a8ce08-150f-4182-bf5e-4a66b2659ed4', NULL, N'اسلاید شو', N'/webdesign/slide', NULL, 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'856c3958-22d7-437a-8088-4d8147b09cf4', NULL, N'ویرایش', N'/webdesign/html/edit', N'60af751d-30c8-4055-9180-e2a4d86d1611', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'0b50c83e-ebce-4fe3-90fc-502badf8ce88', NULL, N'منوها', N'/webdesign/menu', NULL, 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'0de88c5f-1cea-4935-9015-504da6cf4e4a', NULL, N'جزئیات', N'/webdesign/website/details', N'7b4c8bee-32df-4509-946c-5a07d6f49eac', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'7b4c8bee-32df-4509-946c-5a07d6f49eac', NULL, N'مدیریت وب سایت', N'/webdesign/website', NULL, 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'930518ea-14e8-4fd1-a288-5fc9b5b7b42e', NULL, N'Resource', N'/webdesign/resource', NULL, 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'791cafc3-adc8-4459-92ab-5fdf5a902969', NULL, N'جزئیات', N'/webdesign/faq/details', N'd0bb0246-bd26-42d2-9c70-a6fc224240fc', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'126961f9-b6d0-4e4a-b398-6a60abad348e', NULL, N'جزئیات', N'/webdesign/forms/details', N'ececa015-b895-4fc7-a579-2bbb0f65f7ab', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'023b5a23-8b82-4709-b4e9-6a9fa9257c46', NULL, N'ویرایش', N'/webdesign/folder/edit', N'44293ee0-ca6b-4b7f-9a32-293dcc67d2d2', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'bae9cb12-8ef1-4f0b-9140-6fd30be86721', NULL, N'طراحی صفحه پورتال', N'/WebDesign/Configuration/DesignPortal', N'860e6617-a9ca-464a-89f5-9ea3d3712a57', 1, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'df1b9268-d095-45ac-a1fa-75c94f040bf4', NULL, N'جزئیات', N'/webdesign/container/details', N'ae4b3a32-7aab-40bf-ac37-03e0eb007e42', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'8189c8d1-f6e4-422b-b017-765eb3346033', NULL, N'لیست', N'/webdesign/content/index', N'e8a83acf-18d7-48b9-9d30-436257773bc8', 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'b038d3a2-6de2-40e9-8f9f-78b35688de2e', NULL, N'حذف', N'/webdesign/menu/delete', N'0b50c83e-ebce-4fe3-90fc-502badf8ce88', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'55931e3f-8e51-4b37-9349-7bc0f59d1cfa', NULL, N'لیست', N'/webdesign/slide/index', N'a8a8ce08-150f-4182-bf5e-4a66b2659ed4', 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'3a190516-875b-4c1d-b293-7cc89b40dac6', NULL, N'جزئیات', N'/webdesign/html/details', N'60af751d-30c8-4055-9180-e2a4d86d1611', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'765f4a34-d30e-4f62-ae0f-7ec1907df1e7', NULL, N'ویرایش', N'/webdesign/slide/edit', N'a8a8ce08-150f-4182-bf5e-4a66b2659ed4', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'e5e9de8d-8dcf-4e42-bbee-838a2619252a', NULL, N'ایجاد', N'/webdesign/html/create', N'60af751d-30c8-4055-9180-e2a4d86d1611', 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'df17a5c5-ac67-48ed-b308-84e574da1418', NULL, N'ویرایش', N'/webdesign/forms/edit', N'ececa015-b895-4fc7-a579-2bbb0f65f7ab', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'3d5f8797-4ac8-42c6-a557-8c1c19c6d6da', NULL, N'لیست', N'/webdesign/website/index', N'7b4c8bee-32df-4509-946c-5a07d6f49eac', 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'6fc9bc15-9fff-48e5-b380-8c736719a335', NULL, N'گالری تصاویر', N'/webdesign/gallery', NULL, 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'3f065ba6-50de-4d8f-960a-9181c97d24f1', 0, N'اخبار', N'/webdesign/news', NULL, 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'5d50c780-ed95-4091-bd50-99197687f107', NULL, N'حذف', N'/webdesign/container/delete', N'ae4b3a32-7aab-40bf-ac37-03e0eb007e42', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'9d451f70-092b-4e06-9328-9997ecfbb5ec', NULL, N'حذف', N'/webdesign/resource/delete', N'930518ea-14e8-4fd1-a288-5fc9b5b7b42e', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'eb0cff9e-39f3-4818-bea5-99ae81a25966', NULL, N'لوک آپ منو', N'/webdesign/menu/lookupmenu', N'0b50c83e-ebce-4fe3-90fc-502badf8ce88', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'83d6cfe8-7967-4549-b6df-9de2e4d88180', NULL, N'جزئیات', N'/webdesign/menu/details', N'0b50c83e-ebce-4fe3-90fc-502badf8ce88', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'860e6617-a9ca-464a-89f5-9ea3d3712a57', NULL, N'تنظیمات وب سایت', N'/webdesign/configuration/getconfiguration', NULL, 1, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'e081e2b2-a17f-4beb-88a8-a22f6f3c0281', NULL, N'ایجاد', N'/webdesign/slide/create', N'a8a8ce08-150f-4182-bf5e-4a66b2659ed4', 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'd0bb0246-bd26-42d2-9c70-a6fc224240fc', NULL, N'FAQ', N'/webdesign/faq', NULL, 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'1cf57a69-af31-4560-bd46-aad5f4a9eacd', NULL, N'ویرایش', N'/webdesign/news/edit', N'3f065ba6-50de-4d8f-960a-9181c97d24f1', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'738926fe-5353-43d7-a627-ab4eab89832a', NULL, N'ایجاد', N'/webdesign/faq/create', N'd0bb0246-bd26-42d2-9c70-a6fc224240fc', 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'126980ff-2b49-47a0-a201-ae205abdb5d4', NULL, N'ایجاد', N'/webdesign/language/create', N'40178441-3c28-423a-b116-fb173e5d68a8', 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'167ef311-c050-429f-9ea0-ae7a54f4ada8', NULL, N'جزئیات', N'/webdesign/language/details', N'40178441-3c28-423a-b116-fb173e5d68a8', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'0ed66054-1688-4fe5-b97b-b01e8258a3ee', NULL, N'ایجاد', N'/webdesign/news/create', N'3f065ba6-50de-4d8f-960a-9181c97d24f1', 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'99504924-379a-4670-be5b-b416f345b4bd', NULL, N'ایجاد', N'/webdesign/resource/create', N'930518ea-14e8-4fd1-a288-5fc9b5b7b42e', 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'60021781-8f33-4b99-ae83-b6b7dbc22af5', NULL, N'لیست', N'/webdesign/gallery/index', N'6fc9bc15-9fff-48e5-b380-8c736719a335', 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'369af718-7e71-4dea-bd44-bdddda2b709c', NULL, N'حذف', N'/webdesign/content/delete', N'e8a83acf-18d7-48b9-9d30-436257773bc8', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'72d7e901-f418-43a9-99b8-c1e1704b2054', NULL, N'ایجاد', N'/webdesign/configuration/create', N'860e6617-a9ca-464a-89f5-9ea3d3712a57', 1, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'0e48b0b8-4cef-4bf3-897c-c29316f1c82e', NULL, N'لیست', N'/webdesign/news/index', N'3f065ba6-50de-4d8f-960a-9181c97d24f1', 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'4195a6e3-106e-44f2-bb3d-c29c1bdff996', NULL, N'حذف', N'/webdesign/website/delete', N'7b4c8bee-32df-4509-946c-5a07d6f49eac', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'9ddfe172-af5d-4df2-8e02-c42bb85bbbb0', NULL, N'ایجاد', N'/webdesign/website/create', N'7b4c8bee-32df-4509-946c-5a07d6f49eac', 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'1ace406b-d24e-4610-87b4-cb6a1e18b8ce', NULL, N'ایجاد', N'/webdesign/gallery/create', N'6fc9bc15-9fff-48e5-b380-8c736719a335', 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'32ff0335-5e9f-46cf-8c46-ce292dca90a0', NULL, N'جزئیات', N'/webdesign/folder/details', N'44293ee0-ca6b-4b7f-9a32-293dcc67d2d2', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'c3ceb818-5d44-4ee9-866b-d3125d3fb58a', NULL, N'لیست', N'/webdesign/container/index', N'ae4b3a32-7aab-40bf-ac37-03e0eb007e42', 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'94cbf072-d272-4cad-bc70-def91e497fd9', NULL, N'ویرایش', N'/webdesign/container/edit', N'ae4b3a32-7aab-40bf-ac37-03e0eb007e42', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'60af751d-30c8-4055-9180-e2a4d86d1611', NULL, N'HTML', N'/webdesign/html', NULL, 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'4527532b-b2c7-44d2-87cd-e43f9adb12af', NULL, N'ویرایش', N'/webdesign/content/edit', N'e8a83acf-18d7-48b9-9d30-436257773bc8', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'c4f679aa-530d-4f19-891b-e5161ae1ddcd', NULL, N'لیست', N'/webdesign/resource/index', N'930518ea-14e8-4fd1-a288-5fc9b5b7b42e', 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'cf9d61ee-678e-4749-9498-ed0ca31a2254', NULL, N'لیست', N'/webdesign/forms/index', N'ececa015-b895-4fc7-a579-2bbb0f65f7ab', 0, 1, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'43dab0da-31f9-44c3-86f0-ef3bf82a4432', NULL, N'ویرایش', N'/webdesign/configuration/edit', N'860e6617-a9ca-464a-89f5-9ea3d3712a57', 1, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'd89ad8d3-c4af-41e6-a6f9-f5548260210a', NULL, N'جزئیات', N'/webdesign/gallery/details', N'6fc9bc15-9fff-48e5-b380-8c736719a335', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'21c1a65f-6c39-49ae-9301-f6af4e5628f1', NULL, N'حذف', N'/webdesign/slide/delete', N'a8a8ce08-150f-4182-bf5e-4a66b2659ed4', 0, 0, 1, NULL, NULL, NULL)
GO
INSERT [Security].[Menu] ([Id], [ApplicationID], [Title], [Url], [ParentId], [Order], [Display], [Enabled], [HelpId], [ImageId], [MenuGroupId]) VALUES (N'40178441-3c28-423a-b116-fb173e5d68a8', NULL, N'زبان', N'/webdesign/language', NULL, 0, 1, 1, NULL, NULL, NULL)
GO

INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'b6fbac20-0b49-4bd9-9782-01f5f75b5105')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'f0b67c5e-4e1e-4b5e-8ef6-0252f1dff0e0')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'ae4b3a32-7aab-40bf-ac37-03e0eb007e42')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'9e1476cb-5a81-456d-ab02-06ed6eddfd0f')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'9a6daba0-86cb-4aee-bd60-147b5030cb60')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'4cecf1a8-dad7-4943-afeb-1b0183945305')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'1749bedf-6883-4e71-bef9-1d4e3eb8a514')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'871da0a1-5c89-4eb6-9696-1d6d472a9a1c')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'5b4a4504-eddf-404c-a180-1fbb5eb92055')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'aa63061f-078a-4696-83e3-21d15db53f22')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'95b5d60b-048e-4f77-bd5a-228fc330f5d6')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'53421b5d-2bc6-4d6e-ba4b-2650003567a6')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'70fd6da5-28c9-4068-92e6-28f2b91cb55f')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'44293ee0-ca6b-4b7f-9a32-293dcc67d2d2')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'ececa015-b895-4fc7-a579-2bbb0f65f7ab')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'bb0b2d78-877a-45d4-b0ac-30bfb1a93326')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'51487a16-ef3e-4ea8-93ce-320636dec3e4')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'6744c8c8-1e39-4c18-89a3-3610eb16a50a')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'c13066e6-2736-45ad-9826-37a12c381862')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'72c425b4-844c-4e76-a4b8-385a563789ce')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'f4697329-1d48-4856-9b38-38cb58147542')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'1a1e1b66-5968-4acc-b934-426a65b235cc')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'd84d5f0c-3f95-415c-ba4d-434e93c9c73a')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'e8a83acf-18d7-48b9-9d30-436257773bc8')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'c73e4a75-626c-4fd1-98f3-437f67fb6981')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'd57e4472-741f-4366-9ed4-43bb4a44dad8')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'a5587710-9c84-4313-a425-49a941cc2dec')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'4063f44b-5a1d-42db-84d5-4a5fde02fd6c')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'a8a8ce08-150f-4182-bf5e-4a66b2659ed4')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'856c3958-22d7-437a-8088-4d8147b09cf4')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'0b50c83e-ebce-4fe3-90fc-502badf8ce88')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'0de88c5f-1cea-4935-9015-504da6cf4e4a')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'7b4c8bee-32df-4509-946c-5a07d6f49eac')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'930518ea-14e8-4fd1-a288-5fc9b5b7b42e')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'791cafc3-adc8-4459-92ab-5fdf5a902969')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'126961f9-b6d0-4e4a-b398-6a60abad348e')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'023b5a23-8b82-4709-b4e9-6a9fa9257c46')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'bae9cb12-8ef1-4f0b-9140-6fd30be86721')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'df1b9268-d095-45ac-a1fa-75c94f040bf4')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'8189c8d1-f6e4-422b-b017-765eb3346033')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'b038d3a2-6de2-40e9-8f9f-78b35688de2e')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'55931e3f-8e51-4b37-9349-7bc0f59d1cfa')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'3a190516-875b-4c1d-b293-7cc89b40dac6')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'765f4a34-d30e-4f62-ae0f-7ec1907df1e7')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'e5e9de8d-8dcf-4e42-bbee-838a2619252a')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'df17a5c5-ac67-48ed-b308-84e574da1418')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'3d5f8797-4ac8-42c6-a557-8c1c19c6d6da')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'6fc9bc15-9fff-48e5-b380-8c736719a335')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'3f065ba6-50de-4d8f-960a-9181c97d24f1')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'5d50c780-ed95-4091-bd50-99197687f107')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'9d451f70-092b-4e06-9328-9997ecfbb5ec')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'eb0cff9e-39f3-4818-bea5-99ae81a25966')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'83d6cfe8-7967-4549-b6df-9de2e4d88180')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'860e6617-a9ca-464a-89f5-9ea3d3712a57')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'e081e2b2-a17f-4beb-88a8-a22f6f3c0281')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'd0bb0246-bd26-42d2-9c70-a6fc224240fc')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'1cf57a69-af31-4560-bd46-aad5f4a9eacd')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'738926fe-5353-43d7-a627-ab4eab89832a')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'126980ff-2b49-47a0-a201-ae205abdb5d4')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'167ef311-c050-429f-9ea0-ae7a54f4ada8')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'0ed66054-1688-4fe5-b97b-b01e8258a3ee')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'99504924-379a-4670-be5b-b416f345b4bd')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'60021781-8f33-4b99-ae83-b6b7dbc22af5')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'369af718-7e71-4dea-bd44-bdddda2b709c')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'72d7e901-f418-43a9-99b8-c1e1704b2054')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'0e48b0b8-4cef-4bf3-897c-c29316f1c82e')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'4195a6e3-106e-44f2-bb3d-c29c1bdff996')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'9ddfe172-af5d-4df2-8e02-c42bb85bbbb0')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'1ace406b-d24e-4610-87b4-cb6a1e18b8ce')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'32ff0335-5e9f-46cf-8c46-ce292dca90a0')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'c3ceb818-5d44-4ee9-866b-d3125d3fb58a')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'94cbf072-d272-4cad-bc70-def91e497fd9')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'60af751d-30c8-4055-9180-e2a4d86d1611')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'4527532b-b2c7-44d2-87cd-e43f9adb12af')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'c4f679aa-530d-4f19-891b-e5161ae1ddcd')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'cf9d61ee-678e-4749-9498-ed0ca31a2254')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'43dab0da-31f9-44c3-86f0-ef3bf82a4432')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'd89ad8d3-c4af-41e6-a6f9-f5548260210a')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'21c1a65f-6c39-49ae-9301-f6af4e5628f1')
GO
INSERT [Security].[OperationMenu] ([OperationId], [MenuId]) VALUES (N'349883c3-5fc3-4e4a-bf41-8da6be7f8e61', N'40178441-3c28-423a-b116-fb173e5d68a8')
GO
ALTER TABLE [WebDesign].[Configuration]  WITH CHECK ADD  CONSTRAINT [FK_Configuration_WebSite] FOREIGN KEY([Id])
REFERENCES [WebDesign].[WebSite] ([Id])
GO
ALTER TABLE [WebDesign].[Configuration] CHECK CONSTRAINT [FK_Configuration_WebSite]
GO
ALTER TABLE [WebDesign].[Container]  WITH CHECK ADD  CONSTRAINT [FK_Container_Container1] FOREIGN KEY([ContainerId])
REFERENCES [ContentManage].[Container] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [WebDesign].[Container] CHECK CONSTRAINT [FK_Container_Container1]
GO
ALTER TABLE [WebDesign].[Container]  WITH CHECK ADD  CONSTRAINT [FK_Container_WebSite] FOREIGN KEY([WebId])
REFERENCES [WebDesign].[WebSite] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [WebDesign].[Container] CHECK CONSTRAINT [FK_Container_WebSite]
GO
ALTER TABLE [WebDesign].[Content]  WITH CHECK ADD  CONSTRAINT [FK_Content_Content1] FOREIGN KEY([ContentId])
REFERENCES [ContentManage].[Content] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [WebDesign].[Content] CHECK CONSTRAINT [FK_Content_Content1]
GO
ALTER TABLE [WebDesign].[Content]  WITH CHECK ADD  CONSTRAINT [FK_Content_WebSite] FOREIGN KEY([WebId])
REFERENCES [WebDesign].[WebSite] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [WebDesign].[Content] CHECK CONSTRAINT [FK_Content_WebSite]
GO
ALTER TABLE [WebDesign].[FAQ]  WITH CHECK ADD  CONSTRAINT [FK_FAQ_FAQ1] FOREIGN KEY([FAQId])
REFERENCES [FAQ].[FAQ] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [WebDesign].[FAQ] CHECK CONSTRAINT [FK_FAQ_FAQ1]
GO
ALTER TABLE [WebDesign].[FAQ]  WITH CHECK ADD  CONSTRAINT [FK_FAQ_WebSite] FOREIGN KEY([WebId])
REFERENCES [WebDesign].[WebSite] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [WebDesign].[FAQ] CHECK CONSTRAINT [FK_FAQ_WebSite]
GO
ALTER TABLE [WebDesign].[Folder]  WITH CHECK ADD  CONSTRAINT [FK_Folder_Folder1] FOREIGN KEY([FolderId])
REFERENCES [FileManager].[Folder] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [WebDesign].[Folder] CHECK CONSTRAINT [FK_Folder_Folder1]
GO
ALTER TABLE [WebDesign].[Folder]  WITH CHECK ADD  CONSTRAINT [FK_Folder_WebSite] FOREIGN KEY([WebId])
REFERENCES [WebDesign].[WebSite] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [WebDesign].[Folder] CHECK CONSTRAINT [FK_Folder_WebSite]
GO
ALTER TABLE [WebDesign].[Forms]  WITH CHECK ADD  CONSTRAINT [FK_Forms_FormStructure] FOREIGN KEY([FormId])
REFERENCES [FormGenerator].[FormStructure] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [WebDesign].[Forms] CHECK CONSTRAINT [FK_Forms_FormStructure]
GO
ALTER TABLE [WebDesign].[Forms]  WITH CHECK ADD  CONSTRAINT [FK_Forms_WebSite] FOREIGN KEY([WebId])
REFERENCES [WebDesign].[WebSite] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [WebDesign].[Forms] CHECK CONSTRAINT [FK_Forms_WebSite]
GO
ALTER TABLE [WebDesign].[Gallery]  WITH CHECK ADD  CONSTRAINT [FK_Gallery_Gallery1] FOREIGN KEY([GalleryId])
REFERENCES [Gallery].[Gallery] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [WebDesign].[Gallery] CHECK CONSTRAINT [FK_Gallery_Gallery1]
GO
ALTER TABLE [WebDesign].[Gallery]  WITH CHECK ADD  CONSTRAINT [FK_Gallery_WebSite] FOREIGN KEY([WebId])
REFERENCES [WebDesign].[WebSite] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [WebDesign].[Gallery] CHECK CONSTRAINT [FK_Gallery_WebSite]
GO
ALTER TABLE [WebDesign].[Html]  WITH CHECK ADD  CONSTRAINT [FK_Html_HtmlDesgin] FOREIGN KEY([HtmlDesginId])
REFERENCES [ContentManage].[HtmlDesgin] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [WebDesign].[Html] CHECK CONSTRAINT [FK_Html_HtmlDesgin]
GO
ALTER TABLE [WebDesign].[Html]  WITH CHECK ADD  CONSTRAINT [FK_Html_WebSite] FOREIGN KEY([WebId])
REFERENCES [WebDesign].[WebSite] ([Id])
GO
ALTER TABLE [WebDesign].[Html] CHECK CONSTRAINT [FK_Html_WebSite]
GO
ALTER TABLE [WebDesign].[Language]  WITH CHECK ADD  CONSTRAINT [FK_Language_Language1] FOREIGN KEY([LanguageId])
REFERENCES [Common].[Language] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [WebDesign].[Language] CHECK CONSTRAINT [FK_Language_Language1]
GO
ALTER TABLE [WebDesign].[Language]  WITH CHECK ADD  CONSTRAINT [FK_Language_WebSite] FOREIGN KEY([WebId])
REFERENCES [WebDesign].[WebSite] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [WebDesign].[Language] CHECK CONSTRAINT [FK_Language_WebSite]
GO
ALTER TABLE [WebDesign].[Menu]  WITH CHECK ADD  CONSTRAINT [FK_Menu_Menu1] FOREIGN KEY([MenuId])
REFERENCES [ContentManage].[Menu] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [WebDesign].[Menu] CHECK CONSTRAINT [FK_Menu_Menu1]
GO
ALTER TABLE [WebDesign].[Menu]  WITH CHECK ADD  CONSTRAINT [FK_Menu_WebSite] FOREIGN KEY([WebId])
REFERENCES [WebDesign].[WebSite] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [WebDesign].[Menu] CHECK CONSTRAINT [FK_Menu_WebSite]
GO
ALTER TABLE [WebDesign].[News]  WITH CHECK ADD  CONSTRAINT [FK_News_News1] FOREIGN KEY([NewsId])
REFERENCES [News].[News] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [WebDesign].[News] CHECK CONSTRAINT [FK_News_News1]
GO
ALTER TABLE [WebDesign].[News]  WITH CHECK ADD  CONSTRAINT [FK_News_WebSite] FOREIGN KEY([WebId])
REFERENCES [WebDesign].[WebSite] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [WebDesign].[News] CHECK CONSTRAINT [FK_News_WebSite]
GO
ALTER TABLE [WebDesign].[Resource]  WITH CHECK ADD  CONSTRAINT [FK_Resource_WebSite] FOREIGN KEY([WebId])
REFERENCES [WebDesign].[WebSite] ([Id])
GO
ALTER TABLE [WebDesign].[Resource] CHECK CONSTRAINT [FK_Resource_WebSite]
GO
ALTER TABLE [WebDesign].[Slider]  WITH CHECK ADD  CONSTRAINT [FK_Slider_Slide] FOREIGN KEY([SlideId])
REFERENCES [Slider].[Slide] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [WebDesign].[Slider] CHECK CONSTRAINT [FK_Slider_Slide]
GO
ALTER TABLE [WebDesign].[Slider]  WITH CHECK ADD  CONSTRAINT [FK_Slider_WebSite] FOREIGN KEY([WebId])
REFERENCES [WebDesign].[WebSite] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [WebDesign].[Slider] CHECK CONSTRAINT [FK_Slider_WebSite]
GO
ALTER TABLE [WebDesign].[Configuration] ADD FavIcon uniqueidentifier NULL

GO

ALTER TABLE [WebDesign].[Configuration] ADD CONSTRAINT
	FK_Configuration_File FOREIGN KEY
	(
	FavIcon
	) REFERENCES [FileManager].[File]
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO


CREATE TABLE [WebDesign].[MenuHtml](
	[WebSiteId] [uniqueidentifier] NOT NULL,
	[MenuHtmlId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_MenuHtml] PRIMARY KEY CLUSTERED 
(
	[WebSiteId] ASC,
	[MenuHtmlId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [WebDesign].[MenuHtml]  WITH CHECK ADD  CONSTRAINT [FK_MenuHtml_WebSite] FOREIGN KEY([WebSiteId])
REFERENCES [WebDesign].[WebSite] ([Id])
GO

ALTER TABLE [WebDesign].[MenuHtml] CHECK CONSTRAINT [FK_MenuHtml_WebSite]
GO

ALTER TABLE [WebDesign].[MenuHtml]  WITH CHECK ADD  CONSTRAINT [FK_MenuHtml_Menu] FOREIGN KEY([MenuHtmlId])
REFERENCES [ContentManage].[MenuHtml] ([Id])
GO

ALTER TABLE [WebDesign].[MenuHtml] CHECK CONSTRAINT [FK_MenuHtml_Menu]
GO



INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('9DA4FF99-B595-4501-A931-B53A2FB4507D',null,N'html منو ها',N'/WebDesign/MenuHtml',null,1,1,1,null,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('349883c3-5fc3-4e4a-bf41-8da6be7f8e61','9DA4FF99-B595-4501-A931-B53A2FB4507D')
GO


INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('0082534A-2B6D-4394-A1D0-88A6DE5A4EC3',null,N'ایجاد',N'/WebDesign/MenuHtml/Create','F869FE1E-E43E-4813-8E7C-76E0726A2500',1,1,1,null,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('349883c3-5fc3-4e4a-bf41-8da6be7f8e61','0082534A-2B6D-4394-A1D0-88A6DE5A4EC3')
GO

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('DA8D5F41-ED17-4134-9908-72F58299C846',null,N'ویرایش',N'/WebDesign/MenuHtml/Edit','F869FE1E-E43E-4813-8E7C-76E0726A2500',1,0,1,null,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('349883c3-5fc3-4e4a-bf41-8da6be7f8e61','DA8D5F41-ED17-4134-9908-72F58299C846')
GO

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('90C1A424-231F-4075-82B5-2D4130B5DEE0',null,N'حذف',N'/WebDesign/MenuHtml/Delete','F869FE1E-E43E-4813-8E7C-76E0726A2500',1,0,1,null,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('349883c3-5fc3-4e4a-bf41-8da6be7f8e61','90C1A424-231F-4075-82B5-2D4130B5DEE0')
GO


INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('EFEFE250-C355-47D2-A7EB-0D0217E77F6E',null,N'لیست',N'/WebDesign/MenuHtml/Index','F869FE1E-E43E-4813-8E7C-76E0726A2500',1,1,1,null,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('349883c3-5fc3-4e4a-bf41-8da6be7f8e61','EFEFE250-C355-47D2-A7EB-0D0217E77F6E')
GO



INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('CE779A9D-0827-483B-A98C-D16BE44C485B',null,N'جزئیات',N'/WebDesign/MenuHtml/Details','F869FE1E-E43E-4813-8E7C-76E0726A2500',1,0,1,null,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('349883c3-5fc3-4e4a-bf41-8da6be7f8e61','CE779A9D-0827-483B-A98C-D16BE44C485B')


	 go


	 alter table [WebDesign].[Configuration] add DefaultMenuHtmlId uniqueidentifier
go



go
alter table [Congress].[Configuration] add CanUserRentDeskWithoutPaymentType bit not null default 0
go


alter table [Congress].[Configuration] add HasDesk bit not null default 0
go
alter table [Congress].[Configuration] add DeskRentStartDate char(10)
go
alter table [Congress].[Configuration] add DeskRentEndDate char(10)
go
alter table [Congress].[Configuration] add DeskRezerveStartDate char(10)
go
alter table [Congress].[Configuration] add DeskRezerveEndDate char(10)
go
CREATE TABLE [Congress].[Desk](
	[Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
	[Title] nvarchar(500) NULL,
	[Address] nvarchar(500) NULL,
	[CongressId] [uniqueidentifier] NULL,
    [Capacity] int NULL,
)ON [PRIMARY]
ALTER TABLE [Congress].[Desk]  WITH CHECK ADD  CONSTRAINT [FK_Desk_Homa] FOREIGN KEY([CongressId])
REFERENCES [Congress].[Homa] ([Id])
GO

ALTER TABLE [Congress].[Desk] CHECK CONSTRAINT [FK_Desk_Homa]
GO
CREATE TABLE [Congress].[DeskTime](
	[Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
	[DeskId] [uniqueidentifier] NULL,
	[B2BDate] char(10) NULL,
	[B2BTimeFrom] char(5) NULL,
	[B2BTimeTo] char(5) NULL,
	[ValidCost] varchar(15) NULL,
 
)ON [PRIMARY]
ALTER TABLE [Congress].[DeskTime]  WITH CHECK ADD  CONSTRAINT [FK_DeskTime_Desk] FOREIGN KEY([DeskId])
REFERENCES [Congress].[Desk] ([Id])
GO

ALTER TABLE [Congress].[DeskTime] CHECK CONSTRAINT [FK_DeskTime_Desk]
GO
CREATE TABLE [Congress].[DeskRent](
	[Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
	[DeskTimeId] [uniqueidentifier] NULL,
	[UserId] [uniqueidentifier] NULL,
	[Status] [tinyint]  NULL,
	[RegisterDate] char(10) NULL,
	[TransactionId] [uniqueidentifier] NULL,
	[TempId] [uniqueidentifier] NULL,
 
)ON [PRIMARY]
ALTER TABLE [Congress].[DeskRent]  WITH CHECK ADD  CONSTRAINT [FK_DeskRent_DeskTime] FOREIGN KEY([DeskTimeId])
REFERENCES [Congress].[DeskTime] ([Id])
GO

ALTER TABLE [Congress].[DeskRent] CHECK CONSTRAINT [FK_DeskRent_DeskTime]
GO
ALTER TABLE [Congress].[DeskRent]  WITH CHECK ADD  CONSTRAINT [FK_DeskRent_User] FOREIGN KEY([UserId])
REFERENCES [Congress].[User] ([Id])
GO

ALTER TABLE [Congress].[DeskRent] CHECK CONSTRAINT [FK_DeskRent_User]
GO
CREATE TABLE [Congress].[DeskRentTime](
	[Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
	[DeskRentId] [uniqueidentifier] NULL,
	[TimeFrom] char(5)  NULL,
	[TimeTo] char(5) NULL,
	[DeskDate] char(10) NULL,

)ON [PRIMARY]
ALTER TABLE [Congress].[DeskRentTime]  WITH CHECK ADD  CONSTRAINT [FK_DeskRentTime_DeskRent] FOREIGN KEY([DeskRentId])
REFERENCES [Congress].[DeskRent] ([Id])
GO

ALTER TABLE [Congress].[DeskRentTime] CHECK CONSTRAINT [FK_DeskRentTime_DeskRent]
GO
CREATE TABLE [Congress].[DeskReserve](
	[Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
	[UserId] [uniqueidentifier] NULL,
	[Status] [tinyint]  NULL,
	[Description] nvarchar(500) NULL,
	[DeskRentTimeId] [uniqueidentifier] NULL,

)ON [PRIMARY]
ALTER TABLE [Congress].[DeskReserve]  WITH CHECK ADD  CONSTRAINT [FK_DeskReserve_DeskRentTime] FOREIGN KEY([DeskRentTimeId])
REFERENCES [Congress].[DeskRentTime] ([Id])
GO

ALTER TABLE [Congress].[DeskReserve] CHECK CONSTRAINT [FK_DeskReserve_DeskRentTime]
GO
ALTER TABLE [Congress].[DeskReserve]  WITH CHECK ADD  CONSTRAINT [FK_DeskReserve_User] FOREIGN KEY([UserId])
REFERENCES [Congress].[User] ([Id])
GO

ALTER TABLE [Congress].[DeskReserve] CHECK CONSTRAINT [FK_DeskReserve_User]
GO





exec sp_executesql N'INSERT INTO [Security].[Menu] ([Id] ,[ApplicationID] ,[Title] ,[Url] ,[ParentId] ,[Order] ,[Display] ,[Enabled] ,[HelpId] ,[ImageId]) VALUES (@1, @2, @3, @4, @5, @6, @7, @8, @9, @10)',N'@1 uniqueidentifier ,@2 tinyint ,@3 nvarchar(250) ,@4 nvarchar(100) ,@5 uniqueidentifier ,@6 tinyint ,@7 bit ,@8 bit ,@9 uniqueidentifier ,@10 uniqueidentifier',@1='d97c01fc-0a5b-429c-8a96-64fcd29119ee', @2=NULL, @3=N'میز', @4=N'/congress/desk', @5=NULL, @6=0, @7=1, @8=1, @9=NULL, @10=NULL
exec sp_executesql N'INSERT INTO [Security].[Menu] ([Id] ,[ApplicationID] ,[Title] ,[Url] ,[ParentId] ,[Order] ,[Display] ,[Enabled] ,[HelpId] ,[ImageId]) VALUES (@1, @2, @3, @4, @5, @6, @7, @8, @9, @10)',N'@1 uniqueidentifier ,@2 tinyint ,@3 nvarchar(250) ,@4 nvarchar(100) ,@5 uniqueidentifier ,@6 tinyint ,@7 bit ,@8 bit ,@9 uniqueidentifier ,@10 uniqueidentifier',@1='482a5c8e-8345-403a-b267-999a36f6359b', @2=NULL, @3=N'ایجاد', @4=N'/congress/desk/create', @5='d97c01fc-0a5b-429c-8a96-64fcd29119ee', @6=0, @7=1, @8=1, @9=NULL, @10=NULL
exec sp_executesql N'INSERT INTO [Security].[Menu] ([Id] ,[ApplicationID] ,[Title] ,[Url] ,[ParentId] ,[Order] ,[Display] ,[Enabled] ,[HelpId] ,[ImageId]) VALUES (@1, @2, @3, @4, @5, @6, @7, @8, @9, @10)',N'@1 uniqueidentifier ,@2 tinyint ,@3 nvarchar(250) ,@4 nvarchar(100) ,@5 uniqueidentifier ,@6 tinyint ,@7 bit ,@8 bit ,@9 uniqueidentifier ,@10 uniqueidentifier',@1='fa79e0fe-1e7d-431b-920a-b9023dee42a2', @2=NULL, @3=N'لیست', @4=N'/congress/desk/index', @5='d97c01fc-0a5b-429c-8a96-64fcd29119ee', @6=0, @7=1, @8=1, @9=NULL, @10=NULL
exec sp_executesql N'INSERT INTO [Security].[Menu] ([Id] ,[ApplicationID] ,[Title] ,[Url] ,[ParentId] ,[Order] ,[Display] ,[Enabled] ,[HelpId] ,[ImageId]) VALUES (@1, @2, @3, @4, @5, @6, @7, @8, @9, @10)',N'@1 uniqueidentifier ,@2 tinyint ,@3 nvarchar(250) ,@4 nvarchar(100) ,@5 uniqueidentifier ,@6 tinyint ,@7 bit ,@8 bit ,@9 uniqueidentifier ,@10 uniqueidentifier',@1='ee903781-fb14-4a36-b341-d3b7f8525b97', @2=NULL, @3=N'جزییات', @4=N'/congress/desk/details', @5='fa79e0fe-1e7d-431b-920a-b9023dee42a2', @6=0, @7=0, @8=1, @9=NULL, @10=NULL
exec sp_executesql N'INSERT INTO [Security].[Menu] ([Id] ,[ApplicationID] ,[Title] ,[Url] ,[ParentId] ,[Order] ,[Display] ,[Enabled] ,[HelpId] ,[ImageId]) VALUES (@1, @2, @3, @4, @5, @6, @7, @8, @9, @10)',N'@1 uniqueidentifier ,@2 tinyint ,@3 nvarchar(250) ,@4 nvarchar(100) ,@5 uniqueidentifier ,@6 tinyint ,@7 bit ,@8 bit ,@9 uniqueidentifier ,@10 uniqueidentifier',@1='025ed734-1555-4b6e-a5a9-e571e12c994e', @2=NULL, @3=N'ویرایش', @4=N'/congress/desk/edit', @5='fa79e0fe-1e7d-431b-920a-b9023dee42a2', @6=0, @7=0, @8=1, @9=NULL, @10=NULL
exec sp_executesql N'INSERT INTO [Security].[Menu] ([Id] ,[ApplicationID] ,[Title] ,[Url] ,[ParentId] ,[Order] ,[Display] ,[Enabled] ,[HelpId] ,[ImageId]) VALUES (@1, @2, @3, @4, @5, @6, @7, @8, @9, @10)',N'@1 uniqueidentifier ,@2 tinyint ,@3 nvarchar(250) ,@4 nvarchar(100) ,@5 uniqueidentifier ,@6 tinyint ,@7 bit ,@8 bit ,@9 uniqueidentifier ,@10 uniqueidentifier',@1='6f268cd0-2cc7-402b-b6ee-5b2c6dbf8935', @2=NULL, @3=N'ساعت', @4=N'/congress/desktime/index', @5='fa79e0fe-1e7d-431b-920a-b9023dee42a2', @6=0, @7=0, @8=1, @9=NULL, @10=NULL
exec sp_executesql N'INSERT INTO [Security].[OperationMenu] ([OperationId] ,[MenuId]) VALUES (@1, @2)',N'@1 uniqueidentifier ,@2 uniqueidentifier',@1='4cc144ec-2d0e-4f06-a000-564f5b5669f1', @2='d97c01fc-0a5b-429c-8a96-64fcd29119ee'
exec sp_executesql N'INSERT INTO [Security].[OperationMenu] ([OperationId] ,[MenuId]) VALUES (@1, @2)',N'@1 uniqueidentifier ,@2 uniqueidentifier',@1='4cc144ec-2d0e-4f06-a000-564f5b5669f1', @2='482a5c8e-8345-403a-b267-999a36f6359b'
exec sp_executesql N'INSERT INTO [Security].[OperationMenu] ([OperationId] ,[MenuId]) VALUES (@1, @2)',N'@1 uniqueidentifier ,@2 uniqueidentifier',@1='4cc144ec-2d0e-4f06-a000-564f5b5669f1', @2='fa79e0fe-1e7d-431b-920a-b9023dee42a2'
exec sp_executesql N'INSERT INTO [Security].[OperationMenu] ([OperationId] ,[MenuId]) VALUES (@1, @2)',N'@1 uniqueidentifier ,@2 uniqueidentifier',@1='4cc144ec-2d0e-4f06-a000-564f5b5669f1', @2='ee903781-fb14-4a36-b341-d3b7f8525b97'
exec sp_executesql N'INSERT INTO [Security].[OperationMenu] ([OperationId] ,[MenuId]) VALUES (@1, @2)',N'@1 uniqueidentifier ,@2 uniqueidentifier',@1='4cc144ec-2d0e-4f06-a000-564f5b5669f1', @2='025ed734-1555-4b6e-a5a9-e571e12c994e'
exec sp_executesql N'INSERT INTO [Security].[OperationMenu] ([OperationId] ,[MenuId]) VALUES (@1, @2)',N'@1 uniqueidentifier ,@2 uniqueidentifier',@1='4cc144ec-2d0e-4f06-a000-564f5b5669f1', @2='6f268cd0-2cc7-402b-b6ee-5b2c6dbf8935'



go
alter table [Congress].[Configuration] add MaxDeskRentPerUser smallint
go
alter table [Congress].[Configuration] add DeskReserveInformType tinyint
go
alter table [Congress].[Configuration] add DeskRentInformType tinyint
go


alter table [Congress].[DeskRent] add Title nvarchar(100)
go
alter table [Congress].[DeskReserve] add Title nvarchar(50)
go
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE Congress.DeskTime
	DROP CONSTRAINT FK_DeskTime_Desk
GO
ALTER TABLE Congress.Desk SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'Congress.Desk', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'Congress.Desk', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'Congress.Desk', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE Congress.DeskRent
	DROP CONSTRAINT FK_DeskRent_DeskTime
GO
ALTER TABLE Congress.DeskTime ADD CONSTRAINT
	FK_DeskTime_Desk FOREIGN KEY
	(
	DeskId
	) REFERENCES Congress.Desk
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE Congress.DeskTime SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'Congress.DeskTime', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'Congress.DeskTime', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'Congress.DeskTime', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE Congress.DeskRentTime
	DROP CONSTRAINT FK_DeskRentTime_DeskRent
GO
ALTER TABLE Congress.DeskRent ADD CONSTRAINT
	FK_DeskRent_DeskTime FOREIGN KEY
	(
	DeskTimeId
	) REFERENCES Congress.DeskTime
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE Congress.DeskRent SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'Congress.DeskRent', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'Congress.DeskRent', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'Congress.DeskRent', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE Congress.DeskReserve
	DROP CONSTRAINT FK_DeskReserve_DeskRentTime
GO
ALTER TABLE Congress.DeskRentTime ADD CONSTRAINT
	FK_DeskRentTime_DeskRent FOREIGN KEY
	(
	DeskRentId
	) REFERENCES Congress.DeskRent
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE Congress.DeskRentTime SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'Congress.DeskRentTime', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'Congress.DeskRentTime', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'Congress.DeskRentTime', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE Congress.DeskReserve ADD CONSTRAINT
	FK_DeskReserve_DeskRentTime FOREIGN KEY
	(
	DeskRentTimeId
	) REFERENCES Congress.DeskRentTime
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE Congress.DeskReserve SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'Congress.DeskReserve', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'Congress.DeskReserve', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'Congress.DeskReserve', 'Object', 'CONTROL') as Contr_Per 
go

CREATE TABLE [WebDesign].[WebSiteAlias](
	[Id] [uniqueidentifier] NOT NULL,
	[WebSiteId] [uniqueidentifier] NOT NULL,
	[Url] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_WebSiteAlias] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [WebDesign].[WebSiteAlias]  WITH CHECK ADD  CONSTRAINT [FK_WebSiteAlias_WebSite] FOREIGN KEY([WebSiteId])
REFERENCES [WebDesign].[WebSite] ([Id])
GO

ALTER TABLE [WebDesign].[WebSiteAlias] CHECK CONSTRAINT [FK_WebSiteAlias_WebSite]
GO
