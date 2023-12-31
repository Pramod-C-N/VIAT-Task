USE [brady]
GO
/****** Object:  Table [dbo].[ImportBatchData]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImportBatchData](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TenantId] [int] NULL,
	[UniqueIdentifier] [uniqueidentifier] NOT NULL,
	[BatchId] [int] NOT NULL,
	[Filename] [nvarchar](max) NULL,
	[InvoiceType] [nvarchar](max) NULL,
	[IRNNo] [nvarchar](max) NULL,
	[InvoiceNumber] [nvarchar](max) NULL,
	[IssueDate] [datetime2](7) NULL,
	[IssueTime] [nvarchar](max) NULL,
	[InvoiceCurrencyCode] [nvarchar](max) NULL,
	[PurchaseOrderId] [nvarchar](max) NULL,
	[ContractId] [nvarchar](max) NULL,
	[SupplyDate] [datetime2](7) NULL,
	[SupplyEndDate] [datetime2](7) NULL,
	[BuyerMasterCode] [nvarchar](max) NULL,
	[BuyerName] [nvarchar](max) NULL,
	[BuyerVatCode] [nvarchar](max) NULL,
	[BuyerContact] [nvarchar](max) NULL,
	[BuyerCountryCode] [nvarchar](max) NULL,
	[InvoiceLineIdentifier] [nvarchar](max) NULL,
	[ItemMasterCode] [nvarchar](max) NULL,
	[ItemName] [nvarchar](max) NULL,
	[UOM] [nvarchar](max) NULL,
	[GrossPrice] [decimal](18, 2) NULL,
	[Discount] [decimal](18, 2) NULL,
	[NetPrice] [decimal](18, 2) NULL,
	[Quantity] [decimal](18, 2) NULL,
	[LineNetAmount] [decimal](18, 2) NULL,
	[VatCategoryCode] [nvarchar](max) NULL,
	[VatRate] [decimal](18, 2) NULL,
	[VatExemptionReasonCode] [nvarchar](max) NULL,
	[VatExemptionReason] [nvarchar](max) NULL,
	[VATLineAmount] [decimal](18, 2) NULL,
	[LineAmountInclusiveVAT] [decimal](18, 2) NULL,
	[Processed] [int] NOT NULL,
	[Error] [nvarchar](max) NULL,
	[BillingReferenceId] [nvarchar](max) NULL,
	[OrignalSupplyDate] [datetime2](7) NULL,
	[ReasonForCN] [nvarchar](max) NULL,
	[BillOfEntry] [nvarchar](max) NULL,
	[BillOfEntryDate] [datetime2](7) NULL,
	[CustomsPaid] [decimal](18, 2) NULL,
	[CustomTax] [decimal](18, 2) NULL,
	[WHTApplicable] [bit] NOT NULL,
	[PurchaseNumber] [nvarchar](max) NULL,
	[PurchaseCategory] [nvarchar](max) NULL,
	[LedgerHeader] [nvarchar](max) NULL,
	[TransType] [nvarchar](max) NULL,
	[AdvanceRcptAmtAdjusted] [decimal](18, 2) NULL,
	[VatOnAdvanceRcptAmtAdjusted] [decimal](18, 2) NULL,
	[AdvanceRcptRefNo] [nvarchar](max) NULL,
	[PaymentMeans] [nvarchar](max) NULL,
	[PaymentTerms] [nvarchar](max) NULL,
	[NatureofServices] [nvarchar](max) NULL,
	[Isapportionment] [bit] NOT NULL,
	[ExciseTaxPaid] [decimal](18, 2) NULL,
	[OtherChargesPaid] [decimal](18, 2) NULL,
	[TotalTaxableAmount] [decimal](18, 2) NULL,
	[VATDeffered] [bit] NOT NULL,
	[PlaceofSupply] [nvarchar](max) NULL,
	[RCMApplicable] [bit] NOT NULL,
	[OrgType] [nvarchar](max) NULL,
	[ExchangeRate] [decimal](18, 2) NULL,
	[AffiliationStatus] [nvarchar](max) NULL,
	[ReferenceInvoiceAmount] [decimal](18, 2) NULL,
	[PerCapitaHoldingForiegnCo] [nvarchar](max) NULL,
	[CapitalInvestedbyForeignCompany] [nvarchar](max) NULL,
	[CapitalInvestmentCurrency] [nvarchar](max) NULL,
	[CapitalInvestmentDate] [datetime2](7) NULL,
	[VendorConstitution] [nvarchar](max) NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[CreatorUserId] [bigint] NULL,
	[LastModificationTime] [datetime2](7) NULL,
	[LastModifierUserId] [bigint] NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeleterUserId] [bigint] NULL,
	[DeletionTime] [datetime2](7) NULL,
 CONSTRAINT [PK_ImportBatchData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_ImportBatchData_TenantId]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_ImportBatchData_TenantId] ON [dbo].[ImportBatchData]
(
	[TenantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
