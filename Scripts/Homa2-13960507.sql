/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/

GO
ALTER TABLE Congress.RefereeCartable ADD
	Id uniqueidentifier NOT NULL CONSTRAINT DF_RefereeCartable_Id DEFAULT newid() ,
	IsActive bit NOT NULL CONSTRAINT DF_RefereeCartable_IsActive DEFAULT 1,
	InsertDate smalldatetime NULL CONSTRAINT DF_RefereeCartable_InsertDate DEFAULT GETDATE()
GO

ALTER TABLE Congress.RefereeCartable
	DROP CONSTRAINT PK_RefereeCartable
GO
ALTER TABLE Congress.RefereeCartable ADD CONSTRAINT
	PK_RefereeCartable_1 PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO





