USE SEMINAR_METADATA
GO

-- create dataflow table for DDS 

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DataFlow_DDS]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
  DROP TABLE [dbo].[DataFlow_DDS]
GO

CREATE TABLE [dbo].[DataFlow_DDS] (
  [ID] int IDENTITY(1, 1) NOT NULL,
  [TableName] NVARCHAR(500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
  [LSET] DATETIME NULL,
  [CET] DATETIME NULL,
)
ON [PRIMARY]
GO

-- Definition for indices : 
ALTER TABLE [dbo].[DataFlow_DDS]
ADD CONSTRAINT [PK_DataFlow_DDS] 
PRIMARY KEY CLUSTERED ([ID])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

-- CREATE TRIGGER
CREATE TRIGGER INSERT_DATAFLOWDDS ON [dbo].[DataFlow_DDS] AFTER INSERT
AS 
BEGIN
	UPDATE [dbo].[DataFlow_DDS]
	SET LSET = GETDATE(), CET = GETDATE()
	WHERE ID IN (SELECT DISTINCT ID FROM inserted)
END
GO

SET IDENTITY_INSERT [dbo].[DataFlow_DDS] ON
-- SET IDENTITY_INSERT [dbo].[DataFlow_DDS] OFF
INSERT INTO [dbo].[DataFlow_DDS](ID, TableName, LSET, CET)
	VALUES (1, 'BRAND', null, NULL)
INSERT INTO [dbo].[DataFlow_DDS](ID, TableName, LSET, CET)
	VALUES (2, 'PRODUCT',null, NULL)
INSERT INTO [dbo].[DataFlow_DDS](ID, TableName, LSET, CET)
	VALUES (3, 'CATEGORY', null, NULL)
INSERT INTO [dbo].[DataFlow_DDS](ID, TableName, LSET, CET)
	VALUES (4, 'DIRECTORY', null, NULL)
INSERT INTO [dbo].[DataFlow_DDS](ID, TableName, LSET, CET)
	VALUES (5, 'STORE', null, NULL)
INSERT INTO [dbo].[DataFlow_DDS](ID, TableName, LSET, CET)
	VALUES (6, 'TONGDOANHTHU', null, NULL)
GO
truncate table DataFlow_DDS
GO
select * from DataFlow_DDS
GO