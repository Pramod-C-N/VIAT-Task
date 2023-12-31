USE [brady]
GO
/****** Object:  Table [dbo].[AppInvoices]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppInvoices](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceDate] [datetime2](7) NOT NULL,
	[InvoiceNo] [nvarchar](max) NULL,
	[TenantAddress] [nvarchar](max) NULL,
	[TenantLegalName] [nvarchar](max) NULL,
	[TenantTaxNo] [nvarchar](max) NULL,
 CONSTRAINT [PK_AppInvoices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
