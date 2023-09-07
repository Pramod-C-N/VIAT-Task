USE [brady]
GO
/****** Object:  Table [dbo].[TransformationMaster]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransformationMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Value] [varchar](200) NOT NULL,
	[Args] [int] NOT NULL
) ON [PRIMARY]
GO
