USE [brady]
GO
/****** Object:  UserDefinedTableType [dbo].[DataDictionaryType]    Script Date: 01-09-2023 17:11:23 ******/
CREATE TYPE [dbo].[DataDictionaryType] AS TABLE(
	[key] [nvarchar](max) NULL,
	[value] [nvarchar](max) NULL,
	[type] [int] NULL
)
GO
