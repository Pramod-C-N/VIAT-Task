USE [brady]
GO
/****** Object:  Table [dbo].[RuleGroup]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RuleGroup](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Description] [nvarchar](max) NULL,
	[TransactionType] [nvarchar](100) NULL,
	[Type] [varchar](100) NULL,
	[ParentTable] [varchar](100) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
