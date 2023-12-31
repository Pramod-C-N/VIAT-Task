USE [brady]
GO
/****** Object:  Table [dbo].[logs_xml]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[logs_xml](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[uuid] [uniqueidentifier] NULL,
	[createdOn] [datetime] NULL,
	[createdBy] [int] NULL,
	[tenantId] [int] NULL,
	[signature] [nvarchar](max) NULL,
	[certificate] [nvarchar](max) NULL,
	[xml64] [nvarchar](max) NULL,
	[invoiceHash64] [nvarchar](100) NULL,
	[csid] [nvarchar](max) NULL,
	[complianceInvoiceResponse] [nvarchar](max) NULL,
	[reportInvoiceResponse] [nvarchar](max) NULL,
	[clearanceResponse] [nvarchar](max) NULL,
	[qrBase64] [nvarchar](max) NULL,
	[irnno] [int] NULL,
	[errors] [nvarchar](max) NULL,
	[totalAmount] [decimal](18, 2) NULL,
	[vatAmount] [decimal](18, 2) NULL,
	[status] [nvarchar](20) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
