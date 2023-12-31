USE [brady]
GO
/****** Object:  Table [dbo].[Mst_WHTRates]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mst_WHTRates](
	[id] [int] NOT NULL,
	[UUID] [uniqueidentifier] NULL,
	[ServiceName] [nvarchar](max) NOT NULL,
	[creationtime] [datetime2](7) NOT NULL,
	[FromDate] [datetime] NULL,
	[Todate] [datetime] NULL,
	[StandardRate] [decimal](5, 2) NULL,
	[AffiliationRate] [decimal](5, 2) NULL,
	[Status] [int] NULL,
	[Standardrate_OOK] [decimal](18, 0) NULL,
	[AffiliationRate_OOK] [decimal](18, 0) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
