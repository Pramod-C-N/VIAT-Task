USE [brady]
GO
/****** Object:  Table [dbo].[Transactionoverride]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactionoverride](
	[uniqueidentifier] [uniqueidentifier] NULL,
	[tenantid] [int] NULL,
	[batchid] [int] NULL,
	[errortype] [bigint] NULL,
	[creationtime] [datetime2](7) NULL,
	[errormsg] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
