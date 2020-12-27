
CREATE TABLE [Congress].[NewsLetter](
	[CongressId] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_NewsLetter_1] PRIMARY KEY CLUSTERED 
(
	[CongressId] ASC,
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Congress].[NewsLetter]  WITH CHECK ADD  CONSTRAINT [FK_NewsLetter_Homa] FOREIGN KEY([CongressId])
REFERENCES [Congress].[Homa] ([Id])
GO

ALTER TABLE [Congress].[NewsLetter] CHECK CONSTRAINT [FK_NewsLetter_Homa]
GO

INSERT INTO [ContentManage].[Partials]
           ([Id]
           ,[Title]
           ,[Url]
           ,[Enabled]
           ,[ContextName]
           ,[OperationId]
           ,[RefId])
     VALUES
           ('6577D93B-84E8-437D-AD8B-936456743A91'
           ,N'عضویت در خبر نامه'
           ,N'/Congress/UIPanel/GetRegisterNewsLetter'
           ,1
           ,N'همایشات'
           ,'4CC144EC-2D0E-4F06-A000-564F5B5669F1'
           ,null)
GO
