USE [brady]
GO
/****** Object:  Table [dbo].[FileUpload_TransactionSummary]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileUpload_TransactionSummary](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[BatchId] [int] NULL,
	[TenantId] [int] NULL,
	[UniqueIdentifier] [uniqueidentifier] NOT NULL,
	[IRNNo] [nvarchar](max) NOT NULL,
	[NetInvoiceAmount] [decimal](18, 2) NOT NULL,
	[NetInvoiceAmountCurrency] [nvarchar](max) NULL,
	[SumOfInvoiceLineNetAmount] [decimal](18, 2) NOT NULL,
	[SumOfInvoiceLineNetAmountCurrency] [nvarchar](max) NULL,
	[TotalAmountWithoutVAT] [decimal](18, 2) NOT NULL,
	[TotalAmountWithoutVATCurrency] [nvarchar](max) NULL,
	[TotalVATAmount] [decimal](18, 2) NOT NULL,
	[CurrencyCode] [nvarchar](max) NULL,
	[TotalAmountWithVAT] [decimal](18, 2) NOT NULL,
	[PaidAmount] [decimal](18, 2) NOT NULL,
	[PaidAmountCurrency] [nvarchar](max) NULL,
	[PayableAmount] [decimal](18, 2) NOT NULL,
	[PayableAmountCurrency] [nvarchar](max) NULL,
	[AdvanceAmountwithoutVat] [decimal](18, 2) NOT NULL,
	[AdvanceVat] [decimal](18, 2) NOT NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[AdditionalData1] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
