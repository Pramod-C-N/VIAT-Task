USE [brady]
GO
/****** Object:  Table [dbo].[WHTRules]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WHTRules](
	[RuleID] [int] NULL,
	[RuleText] [nvarchar](max) NULL,
	[RuleCommand] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
