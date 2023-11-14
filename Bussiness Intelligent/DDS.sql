CREATE DATABASE SEMINAR_DDS
GO
USE SEMINAR_DDS
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[BRAND_DDS]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
  DROP TABLE [dbo].[BRAND_DDS]
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[PRODUCT_DDS]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
  DROP TABLE [dbo].[PRODUCT_DDS]
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[TOTALREVENUE_DDS]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
  DROP TABLE [dbo].[TOTALREVENUE_DDS]
GO

-- CREATE TABLE
-- Create table
CREATE TABLE [dbo].[BRAND_DDS] (
	[BrandID] INT IDENTITY(1,1) NOT NULL,
	[BrandNK] INT NOT NULL,
	[SourceID] INT NOT NULL,
	[BrandName] NVARCHAR(500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
)
ON [PRIMARY]
GO

CREATE TABLE [dbo].[PRODUCT_DDS] (
	[ProductID] INT IDENTITY(1,1) NOT NULL,
	[ProductNK] INT NOT NULL,
	[SourceID] INT NOT NULL,
	[BrandID] INT NOT NULL,
	[CategoryID] INT NOT NULL,
	[StoreID] INT NOT NULL,
	[DanhmucID] INT NOT NULL,
	[ProductName] NVARCHAR(500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ShortDescription] NVARCHAR(4000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Price] INT NULL,
	[OriginalPrice] INT NULL,
	[Discount] INT NULL,
	[DiscountRate] INT NULL,
	[QuantitySold] INT NULL,
	[Rating] FLOAT NULL,
	[ReviewCount] INT NULL,
)
ON [PRIMARY]
GO

CREATE TABLE [dbo].[TOTALREVENUE_DDS]
(
  [ID] int IDENTITY(1, 1) NOT NULL,
  [IDBrand] int NOT NULL,
  [DoanhThu] BIGINT NULL,
  [BrandName] NVARCHAR(500) NULL,
  [TotalQuantitySold] int NOT NULL
)
ON [PRIMARY]
GO

---- Definition for indices :
ALTER TABLE [dbo].[BRAND_DDS]
ADD CONSTRAINT [PK_BRAND] 
PRIMARY KEY CLUSTERED ([BrandID])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[PRODUCT_DDS]
ADD CONSTRAINT [PK_PRODUCT] 
PRIMARY KEY CLUSTERED ([ProductID])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[TOTALREVENUE_DDS]
ADD CONSTRAINT [PK_TOTALREVENUE] 
PRIMARY KEY CLUSTERED ([ID])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

---select

select * from BRAND_DDS
select * from PRODUCT_DDS
select* from TOTALREVENUE_DDS

truncate table PRODUCT_DDS
truncate table BRAND_DDS
truncate table TOTALREVENUE_DDS