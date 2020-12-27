
CREATE TABLE [Congress].[BoothOfficer](
	[Id] [uniqueidentifier] NOT NULL,
	[BoothId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Order] [int] NOT NULL,
 CONSTRAINT [PK_BoothOfficer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[BoothId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
alter table [Congress].[CongressDefinition] add RptBoothOfficerId uniqueidentifier
go
alter table [Congress].[Homa] add [Order] int not null default 1 
