/*-- [Create Schema] --*/
CREATE SCHEMA [CrossPlatform] 

GO
/*-- [Create ContentCategories Table] **/
CREATE TABLE [CrossPlatform].[ContentCategories](
	[Id] [uniqueidentifier] NOT NULL,
	[CongressId] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](max) NULL,
	[OrderCategory] [int] NULL,
	[Description] [nvarchar](50) NULL,
	[Image] [uniqueidentifier] NULL,
 CONSTRAINT [PK_ContentCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/*-- [Create Contents Table] --*/
CREATE TABLE [CrossPlatform].[Contents](
	[Id] [uniqueidentifier] NOT NULL,
	[CongressId] [uniqueidentifier] NOT NULL,
	[CategoryId] [uniqueidentifier] NOT NULL,
	[Subject] [nvarchar](max) NULL,
	[Body] [nvarchar](max) NULL,
	[Summary] [nvarchar](max) NULL,
	[ObserverCount] [int] NULL,
	[RecordDate] [nvarchar](max) NULL,
	[RecordTime] [nvarchar](max) NULL,
	[Image] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Contents] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/*-- [Create SyncAdapter Table] --*/
CREATE TABLE [CrossPlatform].[SyncAdapter](
	[Id] [uniqueidentifier] NOT NULL,
	[SourceId] [varchar](100) NULL,
	[CongressId] [uniqueidentifier] NOT NULL,
	[TableName] [varchar](100) NULL,
	[VersionId] [bigint] IDENTITY(1,1) NOT NULL,
	[Script] [nvarchar](max) NULL,
	[Type] [int] NULL,
	[Deprecated] [bit] NULL,
	[UserId] [uniqueidentifier] NULL,
	[RecordDate] [datetime] NULL,
 CONSTRAINT [PK_SyncAdapter_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


/*-- [Create Foreign Keys] --*/
GO
ALTER TABLE [CrossPlatform].[Contents]  WITH CHECK ADD  CONSTRAINT [FK_Contents_ContentCategories] FOREIGN KEY([CategoryId])
REFERENCES [CrossPlatform].[ContentCategories] ([Id])
GO
ALTER TABLE [CrossPlatform].[Contents] CHECK CONSTRAINT [FK_Contents_ContentCategories]
GO
