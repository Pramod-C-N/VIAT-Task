USE [brady]
GO
/****** Object:  Table [dbo].[WHTDTTRates]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WHTDTTRates](
	[id] [int] NOT NULL,
	[UUID] [uniqueidentifier] NULL,
	[ServiceName] [nvarchar](max) NOT NULL,
	[CountryCode] [numeric](3, 0) NOT NULL,
	[AlphaCode] [nvarchar](2) NOT NULL,
	[creationtime] [datetime2](7) NOT NULL,
	[FromDate] [datetime] NOT NULL,
	[Todate] [datetime] NOT NULL,
	[DTTRates] [decimal](5, 2) NULL,
	[SpecialRates] [decimal](5, 2) NULL,
	[Status] [int] NULL,
	[RuleID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[WHTDTTRates] ADD  DEFAULT (newid()) FOR [UUID]
GO
