
GO

ALTER TABLE [Congress].[Configuration] ADD FavIcon uniqueidentifier NULL

GO

ALTER TABLE [Congress].[Configuration] ADD CONSTRAINT
	FK_Configuration_File FOREIGN KEY
	(
	FavIcon
	) REFERENCES [FileManager].[File]
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO