Go
ALTER TABLE [Congress].Configuration ADD GetArticleOrginalFile bit NOT NULL DEFAULT 1;
Go
ALTER TABLE [Congress].Configuration ADD ArticleOrginalWordCount smallint Null;
Go
ALTER TABLE [Congress].Configuration ADD MinArticleOrginalWordCount smallint Null;
Go
ALTER TABLE [Congress].Article ADD ArticleOrginalText ntext NULL;
GO
ALTER TABLE [Congress].Configuration ADD ArticleOrginalFileSize smallint NULL;
GO