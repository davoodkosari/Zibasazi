GO

CREATE TABLE [Congress].[CongressHall](
	[CongressId] [uniqueidentifier] NOT NULL,
	[HallId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CongressHall] PRIMARY KEY CLUSTERED 
(
	[CongressId] ASC,
	[HallId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Congress].[CongressHall]  WITH CHECK ADD  CONSTRAINT [FK_CongressHall_Homa] FOREIGN KEY([CongressId])
REFERENCES [Congress].[Homa] ([Id])
GO

ALTER TABLE [Congress].[CongressHall] CHECK CONSTRAINT [FK_CongressHall_Homa]
GO




GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES
           ('3C86495E-9520-46C6-A194-8D4CF80AD8E0',null,N'سالن همایش',N'/Congress/CongressHall',null,1,1,1,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('4CC144EC-2D0E-4F06-A000-564F5B5669F1','3C86495E-9520-46C6-A194-8D4CF80AD8E0')
GO



INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES
           ('C2BCC3A0-B3FC-4F3B-BB57-B82B984EE67E',null,N'ایجاد',N'/Congress/CongressHall/Create','3C86495E-9520-46C6-A194-8D4CF80AD8E0',1,1,1,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('4CC144EC-2D0E-4F06-A000-564F5B5669F1','C2BCC3A0-B3FC-4F3B-BB57-B82B984EE67E')
GO

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES
           ('9010B56C-56E8-49B1-87A7-CF49250BA3DF',null,N'ویرایش',N'/Congress/CongressHall/Edit','3C86495E-9520-46C6-A194-8D4CF80AD8E0',1,0,1,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('4CC144EC-2D0E-4F06-A000-564F5B5669F1','9010B56C-56E8-49B1-87A7-CF49250BA3DF')
GO

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES
           ('0E966D4A-9FDF-45D0-B42E-1242DD38716C',null,N'حذف',N'/Congress/CongressHall/Delete','3C86495E-9520-46C6-A194-8D4CF80AD8E0',1,0,1,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('4CC144EC-2D0E-4F06-A000-564F5B5669F1','0E966D4A-9FDF-45D0-B42E-1242DD38716C')
GO


INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES
           ('00126629-D50F-4CBE-A954-9EE28A54C91F',null,N'لیست',N'/Congress/CongressHall/Index','3C86495E-9520-46C6-A194-8D4CF80AD8E0',1,1,1,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('4CC144EC-2D0E-4F06-A000-564F5B5669F1','00126629-D50F-4CBE-A954-9EE28A54C91F')
GO



INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId])
     VALUES
           ('1DF465F3-FB24-4845-A064-65C6AD5E2199',null,N'جزئیات',N'/Congress/CongressHall/Details','3C86495E-9520-46C6-A194-8D4CF80AD8E0',1,0,1,null,null)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('4CC144EC-2D0E-4F06-A000-564F5B5669F1','1DF465F3-FB24-4845-A064-65C6AD5E2199')
GO






