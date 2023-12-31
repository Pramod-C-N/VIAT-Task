USE [brady]
GO
/****** Object:  Table [dbo].[Rule]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rule](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RuleGroupId] [int] NULL,
	[SqlStatement] [nvarchar](max) NULL,
	[OnSuccessNext] [int] NULL,
	[OnFailureNext] [int] NULL,
	[StopCondition] [int] NULL,
	[Order] [int] NULL,
	[errorCode] [varchar](50) NULL,
	[key] [varchar](50) NULL,
	[isActive] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Rule] ADD  DEFAULT (NULL) FOR [key]
GO
ALTER TABLE [dbo].[Rule] ADD  DEFAULT ((1)) FOR [isActive]
GO
