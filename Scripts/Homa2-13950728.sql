
ALTER TABLE Congress.Article ADD
	EditDate char(10) NULL
GO
ALTER TABLE Congress.Article ADD
	EditTime char(5) NULL

	GO

	ALTER TABLE Congress.Article ADD
	IsArchive bit NULL

	GO