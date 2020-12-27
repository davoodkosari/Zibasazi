/*
   Wednesday, February 15, 20173:58:43 PM
   User: sa
   Server: .
   Database: AlzahraHoma
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
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
ALTER TABLE ContentManage.[Content]
	DROP CONSTRAINT FK_Content_Section
GO
ALTER TABLE ContentManage.Section SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'ContentManage.Section', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'ContentManage.Section', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'ContentManage.Section', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE ContentManage.[Content]
	DROP CONSTRAINT FK_Content_Menu
GO
ALTER TABLE ContentManage.Menu SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'ContentManage.Menu', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'ContentManage.Menu', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'ContentManage.Menu', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE ContentManage.[Content]
	DROP CONSTRAINT DF_Content_Enabled
GO
ALTER TABLE ContentManage.[Content]
	DROP CONSTRAINT DF_Content_VisitCount
GO
ALTER TABLE ContentManage.[Content]
	DROP CONSTRAINT DF__Content__HasCont__075714DC
GO
ALTER TABLE ContentManage.[Content]
	DROP CONSTRAINT DF__Content__IsExter__084B3915
GO
CREATE TABLE ContentManage.Tmp_Content
	(
	Id int NOT NULL IDENTITY (1, 1),
	PageName nvarchar(200) NOT NULL,
	Title nvarchar(500) NULL,
	Keyword nvarchar(500) NULL,
	Description nvarchar(1000) NULL,
	Enabled bit NOT NULL,
	StartDate char(10) NULL,
	ExpireDate char(10) NULL,
	Link nvarchar(300) NULL,
	IsMenu bit NOT NULL,
	MenuId uniqueidentifier NULL,
	VisitCount int NOT NULL,
	Text ntext NULL,
	Subject nvarchar(500) NULL,
	Abstract nvarchar(MAX) NULL,
	SectionId int NULL,
	IsSection bit NOT NULL,
	HasContainer bit NOT NULL,
	ContainerId uniqueidentifier NULL,
	UserScript nvarchar(MAX) NULL,
	IsExternal bit NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE ContentManage.Tmp_Content SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE ContentManage.Tmp_Content ADD CONSTRAINT
	DF_Content_Enabled DEFAULT ((1)) FOR Enabled
GO
ALTER TABLE ContentManage.Tmp_Content ADD CONSTRAINT
	DF_Content_VisitCount DEFAULT ((0)) FOR VisitCount
GO
ALTER TABLE ContentManage.Tmp_Content ADD CONSTRAINT
	DF__Content__HasCont__075714DC DEFAULT ((1)) FOR HasContainer
GO
ALTER TABLE ContentManage.Tmp_Content ADD CONSTRAINT
	DF__Content__IsExter__084B3915 DEFAULT ((0)) FOR IsExternal
GO
SET IDENTITY_INSERT ContentManage.Tmp_Content ON
GO
IF EXISTS(SELECT * FROM ContentManage.[Content])
	 EXEC('INSERT INTO ContentManage.Tmp_Content (Id, PageName, Title, Keyword, Description, Enabled, StartDate, ExpireDate, Link, IsMenu, MenuId, VisitCount, Text, Subject, Abstract, SectionId, IsSection, HasContainer, ContainerId, UserScript, IsExternal)
		SELECT Id, PageName, Title, Keyword, Description, Enabled, StartDate, ExpireDate, Link, IsMenu, MenuId, VisitCount, Text, Subject, Abstract, SectionId, IsSection, HasContainer, ContainerId, CONVERT(nvarchar(MAX), UserScript), IsExternal FROM ContentManage.[Content] WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT ContentManage.Tmp_Content OFF
GO
ALTER TABLE ContentManage.ContentContent
	DROP CONSTRAINT FK_ContentContent_Content
GO
ALTER TABLE Congress.CongressContent
	DROP CONSTRAINT FK_CongressContent_Content
GO
DROP TABLE ContentManage.[Content]
GO
EXECUTE sp_rename N'ContentManage.Tmp_Content', N'Content', 'OBJECT' 
GO
ALTER TABLE ContentManage.[Content] ADD CONSTRAINT
	PK_Content PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE ContentManage.[Content] ADD CONSTRAINT
	FK_Content_Menu FOREIGN KEY
	(
	MenuId
	) REFERENCES ContentManage.Menu
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE ContentManage.[Content] ADD CONSTRAINT
	FK_Content_Section FOREIGN KEY
	(
	SectionId
	) REFERENCES ContentManage.Section
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
select Has_Perms_By_Name(N'ContentManage.[Content]', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'ContentManage.[Content]', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'ContentManage.[Content]', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE Congress.CongressContent ADD CONSTRAINT
	FK_CongressContent_Content FOREIGN KEY
	(
	ContentId
	) REFERENCES ContentManage.[Content]
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE Congress.CongressContent SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'Congress.CongressContent', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'Congress.CongressContent', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'Congress.CongressContent', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE ContentManage.ContentContent ADD CONSTRAINT
	FK_ContentContent_Content FOREIGN KEY
	(
	Id
	) REFERENCES ContentManage.[Content]
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE ContentManage.ContentContent SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'ContentManage.ContentContent', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'ContentManage.ContentContent', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'ContentManage.ContentContent', 'Object', 'CONTROL') as Contr_Per 