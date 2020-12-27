GO
CREATE TABLE [Security].[MenuGroup](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](70) NOT NULL,
	[OperationId] [uniqueidentifier] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[ImageId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_MenuGroup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [Security].[MenuGroup]  WITH CHECK ADD  CONSTRAINT [FK_MenuGroup_File] FOREIGN KEY([ImageId])
REFERENCES [FileManager].[File] ([Id])
GO

ALTER TABLE [Security].[MenuGroup] CHECK CONSTRAINT [FK_MenuGroup_File]
GO

ALTER TABLE [Security].[MenuGroup] ADD  CONSTRAINT [DF_MenuGroup_Enabled]  DEFAULT ((1)) FOR [Enabled]
GO

ALTER TABLE [Security].[MenuGroup]  WITH CHECK ADD  CONSTRAINT [FK_MenuGroup_Operation] FOREIGN KEY([OperationId])
REFERENCES [Security].[Operation] ([Id])
GO

ALTER TABLE [Security].[MenuGroup] CHECK CONSTRAINT [FK_MenuGroup_Operation]
GO
GO
ALTER Table [Security].[Menu] Add MenuGroupId int null
GO
ALTER TABLE [Security].[Menu] ADD CONSTRAINT
	FK_Menu_MenuGroup FOREIGN KEY
	(
	MenuGroupId
	) REFERENCES [Security].[MenuGroup]
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  SET NULL 
	
GO
ALTER TABLE [Security].[MenuGroup] ADD CONSTRAINT
	FK_MenuGroup_Operation FOREIGN KEY
	(
	OperationId
	) REFERENCES [Security].[Operation]
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO


INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES
           ('0E6CAF56-EA09-4931-A984-F083D741BB82',null,N'دسته بندی منو',N'/Security/MenuGroup',null,1,1,1,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('2A7074B0-5BB5-44A8-9B6A-2AD12F7E011D','0E6CAF56-EA09-4931-A984-F083D741BB82')
GO


INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES
           ('05ACAFA3-D6E0-4C82-9C47-41226A541F08',null,N'ایجاد',N'/Security/MenuGroup/Create','DE92526D-CA5D-4AB6-AD30-B1F5FDBED99A',1,1,1,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('2A7074B0-5BB5-44A8-9B6A-2AD12F7E011D','05ACAFA3-D6E0-4C82-9C47-41226A541F08')
GO

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES
           ('555EDD9A-78D5-4246-8D5B-0F99E6F06A92',null,N'ویرایش',N'/Security/MenuGroup/Edit','DE92526D-CA5D-4AB6-AD30-B1F5FDBED99A',1,0,1,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('2A7074B0-5BB5-44A8-9B6A-2AD12F7E011D','555EDD9A-78D5-4246-8D5B-0F99E6F06A92')
GO

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES
           ('DD41462B-15F9-4B47-AA8B-800AA2594929',null,N'حذف',N'/Security/MenuGroup/Delete','DE92526D-CA5D-4AB6-AD30-B1F5FDBED99A',1,0,1,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('2A7074B0-5BB5-44A8-9B6A-2AD12F7E011D','DD41462B-15F9-4B47-AA8B-800AA2594929')
GO


INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES
           ('78F7604C-1E0F-4B75-B2C5-0CCFBDF12ED1',null,N'لیست',N'/Security/MenuGroup/Index','DE92526D-CA5D-4AB6-AD30-B1F5FDBED99A',1,1,1,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('2A7074B0-5BB5-44A8-9B6A-2AD12F7E011D','78F7604C-1E0F-4B75-B2C5-0CCFBDF12ED1')
GO



INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES
           ('30377FFE-DDB6-4822-B781-C4A0C8335571',null,N'جزئیات',N'/Security/MenuGroup/Details','DE92526D-CA5D-4AB6-AD30-B1F5FDBED99A',1,0,1,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('2A7074B0-5BB5-44A8-9B6A-2AD12F7E011D','30377FFE-DDB6-4822-B781-C4A0C8335571')
GO

