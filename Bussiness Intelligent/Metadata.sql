CREATE DATABASE SEMINAR_METADATA
GO

USE SEMINAR_METADATA
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[Data_Flow]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
  DROP TABLE [dbo].[Data_Flow]
GO

CREATE TABLE [dbo].[Data_Flow] (
  [ID] int IDENTITY(1, 1) NOT NULL,
  [TableName] NVARCHAR(500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
  [LSET] DATETIME NULL,
  [CET] DATETIME NULL,
)
ON [PRIMARY]
GO

-- Definition for indices : 
ALTER TABLE [dbo].[Data_Flow]
ADD CONSTRAINT [PK_Data_Flow] 
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
CREATE TRIGGER INSERT_DATAFLOW ON [dbo].[Data_Flow] AFTER INSERT
AS 
BEGIN
	UPDATE [dbo].[Data_Flow]
	SET LSET = GETDATE(), CET = GETDATE()
	WHERE ID IN (SELECT DISTINCT ID FROM inserted)
END
GO
--Khi chế độ IDENTITY_INSERT được bật (ON) cho một bảng, bạn có thể chèn các giá trị vào cột có kiểu dữ liệu IDENTITY (cột tự tăng) trong bảng đó
SET IDENTITY_INSERT [dbo].[Data_Flow] ON
-- SET IDENTITY_INSERT [dbo].[Data_Flow] OFF
INSERT INTO [dbo].[Data_Flow](ID, TableName, LSET, CET)
	VALUES (1, 'BRAND', null, NULL)
INSERT INTO [dbo].[Data_Flow](ID, TableName, LSET, CET)
	VALUES (2, 'PRODUCT',null, NULL)
INSERT INTO [dbo].[Data_Flow](ID, TableName, LSET, CET)
	VALUES (3, 'CATEGORY', null, NULL)
INSERT INTO [dbo].[Data_Flow](ID, TableName, LSET, CET)
	VALUES (4, 'DIRECTORY', null, NULL)
INSERT INTO [dbo].[Data_Flow](ID, TableName, LSET, CET)
	VALUES (5, 'STORE', null, NULL)

Go
truncate table Data_Flow
GO
select * from Data_Flow

GO
