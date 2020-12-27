alter table [Congress].[Pivot] add [Order] int
go
alter table [Congress].PivotCategory add [Order] int
go
alter table [Congress].[Configuration] add EnableArticlePercentage bit
go
alter table [FormGenerator].[FormStructure] add ShowFormTitle bit
go

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId])
     VALUES
           ('F06B4B68-C2E6-4534-A330-939BF0FA5CE6',null,N'تنظیمات گزارش ها',N'/Congress/ManagmentPanel/ReportResetFactory',null,1,1,1,null,null,6)
           
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('76179913-7fda-43f3-a253-bda1d123475f','F06B4B68-C2E6-4534-A330-939BF0FA5CE6')
GO

INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('76179913-7fda-43f3-a253-bda1d123475f','d0b15356-976a-40aa-90b5-1ea8a73a5324')
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('76179913-7fda-43f3-a253-bda1d123475f','c66a1217-2ff9-499e-8811-3ea97bf3c881')
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId])
     VALUES ('76179913-7fda-43f3-a253-bda1d123475f','03c3ce2a-585f-4807-8bc0-bed3095f03cd')
GO

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',null,N'title',N'/Congress/ReportPanel/Index',null,1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81')
GO

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('a1838ca7-7479-46d4-9f18-379eee338d97',null,N'title',N'/Congress/ReportPanel/AdminEditReport','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','a1838ca7-7479-46d4-9f18-379eee338d97')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('b85e9fd3-f4b3-4d25-93f0-8489bafc997d',null,N'title',N'/Congress/ReportPanel/AdminEditReport','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','b85e9fd3-f4b3-4d25-93f0-8489bafc997d')
GO

INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('bc7d768c-d99e-4671-9a9d-9698c9e054bf',null,N'title',N'/Congress/ReportPanel/GetChart','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','bc7d768c-d99e-4671-9a9d-9698c9e054bf')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('bb1eb5cf-b777-4923-9492-055605d737cd',null,N'title',N'/Congress/ReportPanel/GetPrintChart','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','bb1eb5cf-b777-4923-9492-055605d737cd')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('b16b1fd3-ba98-4575-8d75-a6296a8a60af',null,N'title',N'/Congress/ReportPanel/DesginBoothOfficerCard','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','b16b1fd3-ba98-4575-8d75-a6296a8a60af')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('8f14dd69-7eda-49cd-84ce-15d8406f8ea3',null,N'title',N'/Congress/ReportPanel/PrintBoothOfficerCards','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','8f14dd69-7eda-49cd-84ce-15d8406f8ea3')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('16647da5-31ad-4aa3-84fa-5aa7d5a7bce3',null,N'title',N'/Congress/ReportPanel/PrintBoothOfficerCard','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','16647da5-31ad-4aa3-84fa-5aa7d5a7bce3')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('4d956e72-e75a-4b11-a00a-8b0b8fed95c1',null,N'title',N'/Congress/ReportPanel/PrintBoothOfficerList','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','4d956e72-e75a-4b11-a00a-8b0b8fed95c1')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('8fea3045-c777-44a1-ac44-18057d249f2d',null,N'title',N'/Congress/ReportPanel/PrintBoothList','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','8fea3045-c777-44a1-ac44-18057d249f2d')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('ae043434-6039-49d0-8119-a2093ba7675d',null,N'title',N'/Congress/ReportPanel/PrintBoothReservList','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','ae043434-6039-49d0-8119-a2093ba7675d')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('feeb44ba-5e6d-4628-9e21-216b3264b238',null,N'title',N'/Congress/ReportPanel/DesginUserBoothReport','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','feeb44ba-5e6d-4628-9e21-216b3264b238')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('001cc7c6-a030-436c-97d5-f43f4191f679',null,N'title',N'/Congress/ReportPanel/PrintWorkShopList','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','001cc7c6-a030-436c-97d5-f43f4191f679')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('88b724b3-e1ea-49e9-944c-d721b9b4172e',null,N'title',N'/Congress/ReportPanel/PrintUserRequestWorkShops','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','88b724b3-e1ea-49e9-944c-d721b9b4172e')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('0737c5ef-3f58-47b7-9715-afeb3a497261',null,N'title',N'/Congress/ReportPanel/DesginWorkShopUserReport','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','0737c5ef-3f58-47b7-9715-afeb3a497261')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('8db7a742-df2e-4b4e-a908-9206fd55f08d',null,N'title',N'/Congress/ReportPanel/PrintUserRequestHotels','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','8db7a742-df2e-4b4e-a908-9206fd55f08d')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('4af4e3cf-fe95-4eb9-844f-d0842d3611a4',null,N'title',N'/Congress/ReportPanel/DesginUserHotelReport','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','4af4e3cf-fe95-4eb9-844f-d0842d3611a4')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('e4dda05d-fadf-4e20-96c6-9c0a72f5aa74',null,N'title',N'/Congress/ReportPanel/PrintHotelList','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','e4dda05d-fadf-4e20-96c6-9c0a72f5aa74')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('9939a91d-7fb2-4b26-8af8-553371046014',null,N'title',N'/Congress/ReportPanel/DesginChipFoodCard','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','9939a91d-7fb2-4b26-8af8-553371046014')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('df00ad31-d0e4-46c8-91e9-db9a75c0f7d4',null,N'title',N'/Congress/ReportPanel/PrintUserChipFoodCard','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','df00ad31-d0e4-46c8-91e9-db9a75c0f7d4')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('e064b5a2-550d-4b4f-839e-4ab27ff05b21',null,N'title',N'/Congress/ReportPanel/PrintChipFoodList','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','e064b5a2-550d-4b4f-839e-4ab27ff05b21')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('625e12a3-c646-412f-a064-f8710924e289',null,N'title',N'/Congress/ReportPanel/PrintUserList','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','625e12a3-c646-412f-a064-f8710924e289')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('0914574d-a16e-43a7-8b50-3e804a03dc63',null,N'title',N'/Congress/ReportPanel/DesigneUserReport','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','0914574d-a16e-43a7-8b50-3e804a03dc63')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('edb7b16b-3145-4003-be77-2392e1909ff0',null,N'title',N'/Congress/ReportPanel/SearchUserCards','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','edb7b16b-3145-4003-be77-2392e1909ff0')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('58696b3a-2bdb-457f-b99f-865c2efff920',null,N'title',N'/Congress/ReportPanel/PrintCardList','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','58696b3a-2bdb-457f-b99f-865c2efff920')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('fc4df8c8-ea2d-4767-b523-47e37c6d8ebd',null,N'title',N'/Congress/ReportPanel/PrintMiniCardList','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','fc4df8c8-ea2d-4767-b523-47e37c6d8ebd')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('6eec5676-a01e-4ec3-acb9-bcfc14fddfc8',null,N'title',N'/Congress/ReportPanel/PrintArticlesAbstract','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','6eec5676-a01e-4ec3-acb9-bcfc14fddfc8')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('d515c8a4-075a-40d0-98a2-9b90147f9074',null,N'title',N'/Congress/ReportPanel/PrintCongressCertificateList','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','d515c8a4-075a-40d0-98a2-9b90147f9074')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('f846aaad-2291-40ed-b387-6b699736496b',null,N'title',N'/Congress/ReportPanel/PrintUserInfoCardList','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','f846aaad-2291-40ed-b387-6b699736496b')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('865fbaa4-858e-45c9-a3a6-046e31069351',null,N'title',N'/Congress/ReportPanel/DesginAbstractArticle','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','865fbaa4-858e-45c9-a3a6-046e31069351')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('2697841f-5821-46cb-b92d-7f87c9509157',null,N'title',N'/Congress/ReportPanel/DsignUserInfoCard','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','2697841f-5821-46cb-b92d-7f87c9509157')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('6f3f335e-0cff-418c-a813-be7dcf16cf7e',null,N'title',N'/Congress/ReportPanel/UserCard','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','6f3f335e-0cff-418c-a813-be7dcf16cf7e')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('0a24d04a-e96f-4c8f-9ecd-56283ceb91e9',null,N'title',N'/Congress/ReportPanel/PrintUserCard','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','0a24d04a-e96f-4c8f-9ecd-56283ceb91e9')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('b6a3b88a-9add-4b4e-9feb-87871e03ca35',null,N'title',N'/Congress/ReportPanel/PrintUserCardFromAdmin','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','b6a3b88a-9add-4b4e-9feb-87871e03ca35')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('1fcc1a14-1099-412c-a829-8df4167a3f30',null,N'title',N'/Congress/ReportPanel/EmailPrintUserCard','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','1fcc1a14-1099-412c-a829-8df4167a3f30')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('c2546d51-13d4-40bd-bbc8-0e8bc3910fcd',null,N'title',N'/Congress/ReportPanel/UserCongressCertificte','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','c2546d51-13d4-40bd-bbc8-0e8bc3910fcd')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('58490fe6-782b-47da-b922-c679ecac6735',null,N'title',N'/Congress/ReportPanel/DownloaArticleZipFile','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','58490fe6-782b-47da-b922-c679ecac6735')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('832cfa6f-af9e-423e-b954-a14729d458a3',null,N'title',N'/Congress/ReportPanel/DownloadAllarticleZipFile','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','832cfa6f-af9e-423e-b954-a14729d458a3')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('1f88c009-3f84-4464-b5a6-022064cc4ff9',null,N'title',N'/Congress/ReportPanel/PrintArticleCertificate','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','1f88c009-3f84-4464-b5a6-022064cc4ff9')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('4c60823e-607a-4fd6-9b41-67ed8273cf4a',null,N'title',N'/Congress/ReportPanel/DesginArticlesReport','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','4c60823e-607a-4fd6-9b41-67ed8273cf4a')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('81d1efe0-cd6d-467d-a6e5-fbd0333606b1',null,N'title',N'/Congress/ReportPanel/PrintArticlesList','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','81d1efe0-cd6d-467d-a6e5-fbd0333606b1')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('be0d6879-5327-4056-95e4-b4092027b661',null,N'title',N'/Congress/ReportPanel/PrintAllArticleCertificate','e6568be1-c4da-4dbc-a9bb-cba0dcc43c81',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','be0d6879-5327-4056-95e4-b4092027b661')
GO
INSERT INTO [Security].[Menu]([Id],[ApplicationID],[Title],[Url],[ParentId],[Order],[Display],[Enabled],[HelpId],[ImageId],[MenuGroupId]) VALUES('7D787A14-D8BA-4A08-9F2F-A17E33B7006B',null,N'title',N'/ContentManager/Partials/Edit','d0b15356-976a-40aa-90b5-1ea8a73a5324',1,0,1,null,null,null)
GO
INSERT INTO [Security].[OperationMenu]([OperationId],[MenuId]) VALUES('76179913-7fda-43f3-a253-bda1d123475f','7D787A14-D8BA-4A08-9F2F-A17E33B7006B')
GO
