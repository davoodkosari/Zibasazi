GO
alter table [Payment].[Temp] add TrackYourOrderNum bigint not null IDENTITY(10000100101,1 )
GO
alter table [Payment].[Transaction] add TrackYourOrderNum bigint  null
go