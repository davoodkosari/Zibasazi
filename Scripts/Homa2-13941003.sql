alter table [ContentManage].[Partials] add [ContainerId] uniqueidentifier
go
ALTER TABLE [ContentManage].[Partials]  WITH CHECK ADD  CONSTRAINT [FK_Partials_Container] FOREIGN KEY([ContainerId])
REFERENCES [ContentManage].[Container] ([Id])
GO
ALTER TABLE [ContentManage].[Partials] CHECK CONSTRAINT [FK_Partials_Container]
GO
alter table [Congress].[ConfigurationContent] add DefaultContrainerId uniqueidentifier
go
Create Schema Graphic
go
CREATE TABLE [Graphic].[Theme](
	[Id] [uniqueidentifier] NOT NULL,
	[Enabled] [bit] NOT NULL,
 CONSTRAINT [PK_Theme_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
CREATE TABLE [Graphic].[Resource](
	[Id] [uniqueidentifier] NOT NULL,
	[ThemeId] [uniqueidentifier] NOT NULL,
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

ALTER TABLE [Graphic].[Resource]  WITH CHECK ADD  CONSTRAINT [FK_Resource_Theme] FOREIGN KEY([ThemeId])
REFERENCES [Graphic].[Theme] ([Id])
GO

ALTER TABLE [Graphic].[Resource] CHECK CONSTRAINT [FK_Resource_Theme]
GO

ALTER TABLE [News].[News] ADD [Visible] bit
GO

ALTER TABLE [Security].[MenuGroup] ADD [ImageId] uniqueidentifier
GO

INSERT INTO [Security].[MenuGroup]
           ([Name]
           ,[OperationId]
           ,[Enabled]
           ,[ImageId])
     VALUES
           (N'طراحی سایت و محتوا'
           ,'4CC144EC-2D0E-4F06-A000-564F5B5669F1'
           ,1
           ,Null)
GO
INSERT INTO [Security].[MenuGroup]
           ([Name]
           ,[OperationId]
           ,[Enabled]
           ,[ImageId])
     VALUES
           (N'مدیریت مقالات '
           ,'4CC144EC-2D0E-4F06-A000-564F5B5669F1'
           ,1
           ,Null)
GO
INSERT INTO [Security].[MenuGroup]
           ([Name]
           ,[OperationId]
           ,[Enabled]
           ,[ImageId])
     VALUES
           (N'مدیریت فعالیت های جانبی'
           ,'4CC144EC-2D0E-4F06-A000-564F5B5669F1'
           ,1
           ,Null)
GO
INSERT INTO [Security].[MenuGroup]
           ([Name]
           ,[OperationId]
           ,[Enabled]
           ,[ImageId])
     VALUES
           (N'مدیریت ثبت نام و شرکت کنندگان'
           ,'4CC144EC-2D0E-4F06-A000-564F5B5669F1'
           ,1
           ,Null)
GO
INSERT INTO [Security].[MenuGroup]
           ([Name]
           ,[OperationId]
           ,[Enabled]
           ,[ImageId])
     VALUES
           (N'تنظیمات و مدیریت  همایش ها'
           ,'4CC144EC-2D0E-4F06-A000-564F5B5669F1'
           ,1
           ,Null)
GO
INSERT INTO [Security].[MenuGroup]
           ([Name]
           ,[OperationId]
           ,[Enabled]
           ,[ImageId])
     VALUES
           (N'اطلاع رسانی'
           ,'4CC144EC-2D0E-4F06-A000-564F5B5669F1'
           ,1
           ,Null)
GO

INSERT INTO [Security].[MenuGroup]
           ([Name]
           ,[OperationId]
           ,[Enabled]
           ,[ImageId])
     VALUES
           (N'سایر امکانات'
           ,'4CC144EC-2D0E-4F06-A000-564F5B5669F1'
           ,1
           ,Null)
GO


UPDATE [Security].[Menu]
   SET 
      [MenuGroupId] = 2
 WHERE id in(
'8D995FFA-EC82-41E1-AD66-D6EB85BC1352',
'F1984671-55D1-4588-BF6D-E384440E6A1D',
'B29E759F-F516-4A2E-BA3F-C27275AC8DAD',
'93858BC7-60E9-4F46-AD9C-824779184A6B',
'CF666D94-FA7C-496E-96E6-749DA1D6CF6F',
'0EAE3116-28C5-4456-96B7-5CB662DB92A9',
'F2E6DB56-AD0B-4A1F-A405-4B283B979AA4',
'444EF088-E864-4BCA-ABAC-1889F10459B8',
'98FAE187-7AE5-4346-8746-03CC2589F5FB',
'9F9FDE96-D796-40A4-ACEB-3B691652A881',
'C0CFA801-6FB9-48A4-9D08-2ED0FBE0141B')
GO

UPDATE [Security].[Menu]
   SET 
      [MenuGroupId] = 3
 WHERE id in(
 '771ED905-0609-4495-BC3E-31718B0D59DD',
'75F0BD91-AFB9-464E-B9BA-7A96398FF05A',
'747D59A6-7396-42A7-97F7-BF42AF367701',
'AFB1EFB6-C372-4559-BD5A-CFA6B1A21014')
GO







UPDATE [Security].[Menu]
   SET 
      [MenuGroupId] = 4
 WHERE id in(
'D59EE61B-F8AB-4493-9540-D82654C6E3B7',
'08221B48-F945-4FC6-A1F1-7800C5081BF2',
'48F73B66-6F29-448B-8B0B-6826921D5B80',
'24C13CE2-C08D-4EE3-AACF-4F013208E21C',
'619ECFD2-8478-4A5F-9E60-48F4549783AE',
'C784BD07-3577-4696-B5C4-472AC236E84E')
GO

UPDATE [Security].[Menu]
   SET 
      [MenuGroupId] = 5
 WHERE id in(
 'CFF9E127-96C6-4A4F-A7FC-0C20F6BCDA3A',
'4D23283F-FB39-403B-890A-01A4E764DEBB',
'0D8F3D3B-C7A2-4120-B356-5F83AA930742',
'00C7A7BA-2D4C-4E55-ADCF-EA463BF468F5',
'1917156D-A5B1-416F-8E44-C2FD1AE9244D',
'E77E56A6-F69A-4A29-AAB8-B97B3FB4DF51')
GO

UPDATE [Security].[Menu]
   SET 
      [MenuGroupId] = 6
 WHERE id in(
'7C28919A-A847-48F0-9839-181E0E2F41E3',
'DE92526D-CA5D-4AB6-AD30-B1F5FDBED99A',
'6054354F-D75E-458C-89FF-2035899AA829')
GO

UPDATE [Security].[Menu]
   SET 
      [MenuGroupId] = 7
 WHERE id in(
'E741602C-0667-4F8B-B8FD-BE106FEC2B8B',
'CEC427DA-122F-482C-9282-B7C205E70B6B')
GO

UPDATE [Security].[Menu]
   SET 
      [MenuGroupId] = 8
 WHERE id in(
'37345B07-133C-4052-8E58-AAE0C860B6F7',
'D7E3C986-9A7A-4B1A-A696-5E2ECD1C398F',
'26901ACD-465D-418B-9D74-6B8794FF3CD8',
'3C86495E-9520-46C6-A194-8D4CF80AD8E0',
'54EA867F-A0B9-4944-B414-24F993E3A36E',
'3B7FDCBE-054C-4D68-AE04-11353B26E41E',
'44DEB3D6-B3DA-494D-B9AE-17DDCCF17640',
'BEA22F7C-04CA-4443-8AE3-E5C19BC4ED78')
GO

exec sp_executesql N'INSERT INTO [Security].[OperationMenu] ([OperationId] ,[MenuId]) VALUES (@1, @2)',N'@1 uniqueidentifier ,@2 uniqueidentifier',@1='76179913-7fda-43f3-a253-bda1d123475f', @2='7521228f-1ef3-4016-81b0-45312c0d4c8e'
go
