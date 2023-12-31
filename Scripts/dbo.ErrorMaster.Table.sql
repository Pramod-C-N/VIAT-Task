USE [brady]
GO
/****** Object:  Table [dbo].[ErrorMaster]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ErrorMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[ErrorCode] [varchar](100) NOT NULL,
	[successMessage] [varchar](200) NULL,
	[failureMessage] [varchar](200) NULL
) ON [PRIMARY]
GO
