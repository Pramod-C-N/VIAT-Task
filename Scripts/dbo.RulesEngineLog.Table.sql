USE [brady]
GO
/****** Object:  Table [dbo].[RulesEngineLog]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RulesEngineLog](
	[TableName] [varchar](100) NULL,
	[RecordId] [int] NULL,
	[isSuccess] [int] NULL,
	[batchId] [int] NULL,
	[errorCode] [varchar](100) NULL,
	[refBatchId] [int] NULL,
	[Field] [nvarchar](500) NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[RulesEngineLog] ADD  DEFAULT (NULL) FOR [Field]
GO
