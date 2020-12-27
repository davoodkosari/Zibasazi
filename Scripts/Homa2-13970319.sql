GO

INSERT INTO [Security].[Menu]
           ([Id]
           ,[ApplicationID]
           ,[Title]
           ,[Url]
           ,[ParentId]
           ,[Order]
           ,[Display]
           ,[Enabled]
           ,[HelpId]
           ,[ImageId]
           ,[MenuGroupId])
     VALUES
           ('276985F8-4C28-4F37-94A9-CBBC99B2DC3D'
           ,0
           ,N'مقاله'
           ,'/congress/referee/lookuparticleassign'
           ,NULL
           ,2
           ,1
           ,1
           ,NULL
           ,NULL
           ,NULL)

GO

INSERT INTO [Security].[OperationMenu]
           ([OperationId]
           ,[MenuId])
     VALUES
           ('76179913-7fda-43f3-a253-bda1d123475f'
           ,'276985F8-4C28-4F37-94A9-CBBC99B2DC3D')
GO



alter table [Congress].[Configuration] add DefaultContrainerId uniqueidentifier
go
UPDATE [Congress].[Configuration] 
SET  [Congress].[Configuration].DefaultContrainerId =p.DefaultContrainerId
FROM [Congress].[Configuration]  AS R
INNER JOIN [Congress].[ConfigurationContent]   AS P 
       ON R.CongressId = P.ConfigurationId 
	   
	   go
	   alter table [Congress].[ConfigurationContent] drop column DefaultContrainerId 
go
alter table [Congress].[Configuration] add DefaultHtmlId uniqueidentifier
go
UPDATE [Congress].[Configuration] 
SET  [Congress].[Configuration].DefaultHtmlId =P2.Id
FROM [Congress].[Configuration]  AS R
INNER JOIN [Congress].[CongressHtml]   AS P 
       ON R.CongressId = P.CongressId 
	   INNER JOIN [ContentManage].[HtmlDesgin]  AS P2 
       ON P.HtmlDesginId= P2.Id and P2.[Enabled]=1
	   
	   go
	  
go
alter table [Congress].[Configuration] add DefaultMenuHtmlId uniqueidentifier
go
UPDATE [Congress].[Configuration] 
SET  [Congress].[Configuration].DefaultMenuHtmlId =P2.Id
FROM [Congress].[Configuration]  AS R
INNER JOIN [Congress].[CongressMenuHtml]   AS P 
       ON R.CongressId = P.CongressId 
	   INNER JOIN [ContentManage].[MenuHtml]  AS P2 
       ON P.MenuHtmlId= P2.Id and P2.[Enabled]=1
	   
	   go
	  
go

alter table  [ContentManage].[PartialLoad] add HasContainer bit
go
update [ContentManage].[PartialLoad] set HasContainer=1
go
alter table  [ContentManage].[PartialLoad] alter column HasContainer bit not null
go

update  [ContentManage].[PartialLoad] set [HasContainer]=0 where PartialId='a518adcb-9332-4094-9d50-0464ccaf36d6' or PartialId='152ef7cf-611c-4ca3-9569-97ffbc30711c'
or PartialId='05b4d18b-ccbd-426c-a48d-c8303d53db9b' or PartialId='59144559-b787-40d9-b55a-eb2969e5d24b' or PartialId='b7f292ce-2e72-4246-9ccd-219d7bd2dafe' 

go
