/*
   Thursday, September 15, 201612:08:08 PM
   User: sa
   Server: 10.1.1.15
   Database: culcur14_new
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/

BEGIN TRANSACTION
GO
ALTER TABLE Congress.CongressDefinition ADD
	RptUserInfoCardId uniqueidentifier NULL
GO
COMMIT

