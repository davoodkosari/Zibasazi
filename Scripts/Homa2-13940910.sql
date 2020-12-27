alter table [FormGenerator].[FormData] add DataFileId [uniqueidentifier]  
ALTER TABLE [FormGenerator].[FormData]  WITH CHECK ADD  CONSTRAINT [FK_FormData_File] FOREIGN KEY([DataFileId])
REFERENCES [FileManager].[File] ([Id])
GO

alter table [Congress].[Configuration] add MerchantId varchar(50) 
go

