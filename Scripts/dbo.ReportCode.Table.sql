USE [brady]
GO
/****** Object:  Table [dbo].[ReportCode]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReportCode](
	[SlNo] [int] IDENTITY(1,1) NOT NULL,
	[ReportName] [nvarchar](max) NULL,
	[Module] [nvarchar](max) NULL,
	[ReportNumber] [nvarchar](max) NULL,
	[Code] [nvarchar](max) NULL,
	[SPname] [nvarchar](max) NULL,
	[QueryString] [nvarchar](max) NULL,
	[Active] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
