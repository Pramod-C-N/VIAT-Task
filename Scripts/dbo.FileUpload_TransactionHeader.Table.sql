USE [brady]
GO
/****** Object:  Table [dbo].[FileUpload_TransactionHeader]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileUpload_TransactionHeader](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[BatchId] [int] NULL,
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
	[InvoiceNotes] [nvarchar](max) NULL,
	[XmlUuid] [nvarchar](max) NULL,
	[AdditionalData1] [nvarchar](max) NULL,
	[AdditionalData2] [nvarchar](max) NULL,
	[AdditionalData3] [nvarchar](max) NULL,
	[AdditionalData4] [nvarchar](max) NULL,
	[InvoiceTypeCode] [nvarchar](max) NULL,
	[Language] [nvarchar](max) NULL,
	[Errors] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
