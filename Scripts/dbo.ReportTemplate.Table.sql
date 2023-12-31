USE [brady]
GO
/****** Object:  Table [dbo].[ReportTemplate]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReportTemplate](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TenantId] [int] NULL,
	[Html] [nvarchar](max) NULL,
	[Name] [nvarchar](100) NULL,
	[RowHtml] [nvarchar](max) NULL,
	[orientation] [varchar](10) NULL,
	[ChargesRow] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[ReportTemplate] ADD  DEFAULT ('P') FOR [orientation]
GO
