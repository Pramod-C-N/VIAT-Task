USE [brady]
GO
/****** Object:  Table [dbo].[InvoiceStatus]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvoiceStatus](
	[invoiceType] [varchar](50) NULL,
	[status] [varchar](10) NULL,
	[irnno] [int] NULL,
	[batchId] [int] NULL,
	[invoiceNumber] [varchar](50) NULL,
	[isXmlSigned] [bit] NULL,
	[inputData] [nvarchar](max) NULL,
	[isPdfGenerated] [bit] NULL,
	[TenantId] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[InvoiceStatus] ADD  DEFAULT ((0)) FOR [isXmlSigned]
GO
