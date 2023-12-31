USE [brady]
GO
/****** Object:  Table [dbo].[PurchaseCreditNoteSummary]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseCreditNoteSummary](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TenantId] [int] NULL,
	[UniqueIdentifier] [uniqueidentifier] NOT NULL,
	[IRNNo] [nvarchar](max) NULL,
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
	[CreatorUserId] [bigint] NULL,
	[LastModificationTime] [datetime2](7) NULL,
	[LastModifierUserId] [bigint] NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeleterUserId] [bigint] NULL,
	[DeletionTime] [datetime2](7) NULL,
 CONSTRAINT [PK_PurchaseCreditNoteSummary] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_PurchaseCreditNoteSummary_TenantId]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_PurchaseCreditNoteSummary_TenantId] ON [dbo].[PurchaseCreditNoteSummary]
(
	[TenantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
