USE [brady]
GO
/****** Object:  Table [dbo].[RuleBatch]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RuleBatch](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RuleGroupId] [int] NOT NULL,
	[ExecutionTime] [datetime] NOT NULL,
	[Success] [int] NULL,
	[Failed] [int] NULL
) ON [PRIMARY]
GO
