

CREATE TABLE [Congress].[UserForms](
	[CongressId] [uniqueidentifier] NOT NULL,
	[FormId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_UserForms] PRIMARY KEY CLUSTERED 
(
	[CongressId] ASC,
	[FormId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


GO
Alter Table [Congress].[Configuration] Add HasUserForms bit
GO

INSERT INTO [Common].[LanguageContent]
           ([Key]
           ,[Value]
           ,[LanguageId]
           ,[IsDefault])
    (select 'ContentManage.Partials.'+cast([ContentManage].[Partials].Id as nvarchar(100))+'.Title',[Title],'fa-IR',1 from [ContentManage].[Partials])
GO

alter table [ContentManage].[Partials] alter  column [Title] nvarchar(150)