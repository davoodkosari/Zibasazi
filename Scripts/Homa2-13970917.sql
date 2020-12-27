

GO
ALTER TABLE [Congress].Configuration ADD ThemeColorURL nvarchar(200) NULL
GO

--13971012
GO

update [Common].[LanguageContent] set Value=N'درباره' where Value =N'درباره همایش'

go
update [Common].[LanguageContent] set Value=N'تقویم' where [Key]='ContentManage.Menu.908d4d98-ac98-462e-b66a-be68de308aa3.Text'
go
update [Common].[LanguageContent] set Value=N'زبان' where [Key]='ContentManage.Partials.2B0E0753-CA81-43FF-8924-838624ECC8C3.Title'
go
update [Common].[LanguageContent] set Value=N'پوستر' where [Key]='ContentManage.Partials.61765F6C-C872-4D7B-83DC-FDFBEBEBEF7B.Title'
go
update [Common].[LanguageContent] set Value=N'اخبار' where [Key]='ContentManage.Partials.8735A75E-E02F-4241-9AE4-F6CCD7449863.Title'
go
update [Common].[LanguageContent] set Value=N'محورها' where [Key]='ContentManage.Partials.EA09A23F-4245-426E-8108-75D253568F2B.Title'
go
update [Common].[LanguageContent] set Value=N'هدر' where [Key]='ContentManage.Partials.FD9169C5-7724-4BF3-9280-A248253DA286.Title'

GO

