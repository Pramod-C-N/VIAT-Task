USE [brady]
GO
/****** Object:  Table [dbo].[SalesInvoice]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SalesInvoice](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TenantId] [int] NULL,
	[UniqueIdentifier] [uniqueidentifier] NOT NULL,
	[IRNNo] [nvarchar](max) NOT NULL,
	[InvoiceNumber] [nvarchar](max) NOT NULL,
	[IssueDate] [datetime2](7) NOT NULL,
	[DateOfSupply] [datetime2](7) NULL,
	[InvoiceCurrencyCode] [nvarchar](max) NOT NULL,
	[CurrencyCodeOriginatingCountry] [nvarchar](max) NULL,
	[PurchaseOrderId] [nvarchar](max) NULL,
	[BillingReferenceId] [nvarchar](max) NULL,
	[ContractId] [nvarchar](max) NULL,
	[LatestDeliveryDate] [datetime2](7) NULL,
	[Location] [nvarchar](max) NULL,
	[CustomerId] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NULL,
	[Additional_Info] [nvarchar](max) NULL,
	[PaymentType] [nvarchar](max) NULL,
	[PdfUrl] [nvarchar](max) NULL,
	[QrCodeUrl] [nvarchar](max) NULL,
	[XMLUrl] [nvarchar](max) NULL,
	[ArchivalUrl] [nvarchar](max) NULL,
	[PreviousInvoiceHash] [nvarchar](max) NULL,
	[PerviousXMLHash] [nvarchar](max) NULL,
	[XMLHash] [nvarchar](max) NULL,
	[PdfHash] [nvarchar](max) NULL,
	[XMLbase64] [nvarchar](max) NULL,
	[PdfBase64] [nvarchar](max) NULL,
	[IsArchived] [bit] NOT NULL,
	[TransTypeCode] [int] NOT NULL,
	[TransTypeDescription] [nvarchar](max) NULL,
	[AdvanceReferenceNumber] [nvarchar](max) NULL,
	[Invoicetransactioncode] [nvarchar](max) NULL,
	[BusinessProcessType] [nvarchar](max) NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[CreatorUserId] [bigint] NULL,
	[LastModificationTime] [datetime2](7) NULL,
	[LastModifierUserId] [bigint] NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeleterUserId] [bigint] NULL,
	[DeletionTime] [datetime2](7) NULL,
	[InvoiceNotes] [nvarchar](max) NULL,
	[XmlUuid] [nvarchar](max) NULL,
	[AdditionalData1] [nvarchar](max) NULL,
	[AdditionalData2] [nvarchar](max) NULL,
	[AdditionalData3] [nvarchar](max) NULL,
	[AdditionalData4] [nvarchar](max) NULL,
	[InvoiceTypeCode] [nvarchar](max) NULL,
	[Language] [nvarchar](max) NULL,
 CONSTRAINT [PK_SalesInvoice] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_SalesInvoice_TenantId]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_SalesInvoice_TenantId] ON [dbo].[SalesInvoice]
(
	[TenantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
