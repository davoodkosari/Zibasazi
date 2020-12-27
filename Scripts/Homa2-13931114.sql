alter table [Congress].[Configuration] add CanUserReserveHotelWithoutPaymentType bit not null default 1 
go
alter table [Congress].[Configuration] add CanUserReserveWorkShopWithoutPaymentType bit not null default 1  
go
alter table [Congress].[Configuration] add CanUserReserveBoothWithoutPaymentType bit not null default 1  
go
alter table [Congress].[Configuration] add CanUserSendArticleWithoutPaymentType bit not null default 1  
go
