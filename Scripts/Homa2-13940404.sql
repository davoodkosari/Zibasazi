CREATE TABLE [Congress].[HomaAlias](
	[Id] [uniqueidentifier] NOT NULL,
	[CongressId] [uniqueidentifier] NOT NULL,
	[Url] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_HomaAlias] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Congress].[HomaAlias]  WITH CHECK ADD  CONSTRAINT [FK_HomaAlias_Homa] FOREIGN KEY([CongressId])
REFERENCES [Congress].[Homa] ([Id])
GO

ALTER TABLE [Congress].[HomaAlias] CHECK CONSTRAINT [FK_HomaAlias_Homa]
GO
alter table [Congress].[Homa] alter column [Description] nvarchar(max) 
go


