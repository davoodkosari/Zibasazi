Alter Table [Congress].[Configuration] Add HasFinancialOperation bit
go
INSERT INTO [Common].[LanguageContent]
           ([Key]
           ,[Value]
           ,[LanguageId]
           ,[IsDefault])
    (select 'FormGenerator.FormData.'+cast([FormGenerator].[FormData].Id as nvarchar(100))+'.Data',[Data],'fa-IR',1 from [FormGenerator].[FormData])
GO

