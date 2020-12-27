/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
GO
ALTER TABLE Congress.ArticleFlow ADD
	Status tinyint NULL
GO
