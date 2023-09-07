USE [brady]
GO
/****** Object:  Table [dbo].[RuleDetails]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RuleDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RuleId] [int] NULL,
	[RuleValue] [nvarchar](max) NULL,
	[RuleType] [varchar](100) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
