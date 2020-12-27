alter table [Gallery].[Gallery] add [Order] int
go
update  [Gallery].[Gallery] set [Order]=0
go
alter table [Gallery].[Gallery] alter column  [Order] int not null
go
alter table [News].[News] add Pined bit
go
update [News].[News] set Pined=0
go
alter table [News].[News] alter column  Pined bit not null
go