alter table [Reservation].[Chair] add OwnerId uniqueidentifier
go
ALTER TABLE [Reservation].[Chair]  WITH CHECK ADD  CONSTRAINT [FK_Chair_EnterpriseNode] FOREIGN KEY([OwnerId])
REFERENCES [EnterpriseNode].[EnterpriseNode] ([Id])
GO

ALTER TABLE [Reservation].[Chair] CHECK CONSTRAINT [FK_Chair_EnterpriseNode]
GO
