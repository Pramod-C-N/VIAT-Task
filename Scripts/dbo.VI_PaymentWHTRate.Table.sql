USE [brady]
GO
/****** Object:  Table [dbo].[VI_PaymentWHTRate]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VI_PaymentWHTRate](
	[UniqueIdentifier] [uniqueidentifier] NULL,
	[StandardRate] [decimal](5, 2) NULL,
	[Batchid] [int] NULL,
	[RateSlno] [int] NULL,
	[LawRate] [numeric](5, 2) NULL,
	[EffRate] [numeric](5, 2) NULL,
	[ServiceName] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
