
CREATE SCHEMA [Reservation]
GO

CREATE TABLE [Reservation].[Hall](
	[Id] [uniqueidentifier] NOT NULL,
	[Length] [int] NOT NULL,
	[Width] [int] NOT NULL,
	[ParentId] [uniqueidentifier] NULL,
	[IsExternal] [bit] NOT NULL,
	[Enabled] [bit] NOT NULL
 CONSTRAINT [PK_Hall] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reservation].[Hall] ADD  DEFAULT ((0)) FOR [IsExternal]
GO

ALTER TABLE [Reservation].[Hall]  WITH CHECK ADD  CONSTRAINT [FK_Hall_Hall] FOREIGN KEY([ParentId])
REFERENCES [Reservation].[Hall] ([Id])
GO

ALTER TABLE [Reservation].[Hall] CHECK CONSTRAINT [FK_Hall_Hall]
GO

CREATE TABLE [Reservation].[ChairType](
	[Id] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](50) NULL,
	[HallId] [uniqueidentifier] NOT NULL,
	[Enabled] [bit] NOT NULL
  CONSTRAINT [PK_ChairType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reservation].[ChairType]  WITH CHECK ADD  CONSTRAINT [FK_ChairType_Hall] FOREIGN KEY([HallId])
REFERENCES [Reservation].[Hall] ([Id])
GO

ALTER TABLE [Reservation].[ChairType] CHECK CONSTRAINT [FK_ChairType_Hall]
GO
CREATE TABLE [Reservation].[Chair](
	[Id] [uniqueidentifier] NOT NULL,
	[HallId] [uniqueidentifier] NOT NULL,
	[Number] [int] NOT NULL,
	[Row] [int] NOT NULL,
	[Column] [int] NOT NULL,
	[Status] [tinyint] NOT NULL,
	[ChairTypeId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Chair] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reservation].[Chair]  WITH CHECK ADD  CONSTRAINT [FK_Chair_ChairType] FOREIGN KEY([ChairTypeId])
REFERENCES [Reservation].[ChairType] ([Id])
GO

ALTER TABLE [Reservation].[Chair] CHECK CONSTRAINT [FK_Chair_ChairType]
GO


