alter table [Statistics].[Log] alter column [Url] nvarchar(400)
go
 update    [Statistics].[Log] 
 set 
 [Statistics].[Log].[Url]=[Statistics].[WebSite].[Url]+[Statistics].[Log].[Url] 
  
FROM            [Statistics].[Log] INNER JOIN
                         [Statistics].WebSite ON [Statistics].[Log].WebSiteId = [Statistics].WebSite.Id
						 where [Statistics].[Log].WebSiteId=[Statistics].WebSite.Id and [Statistics].[Log].Url is not null
						 go

						



