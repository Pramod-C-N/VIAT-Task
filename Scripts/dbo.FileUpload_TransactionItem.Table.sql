USE [brady]
GO
/****** Object:  Table [dbo].[FileUpload_TransactionItem]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileUpload_TransactionItem](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[BatchId] [int] NULL,
	[TenantId] [int] NULL,
	[UniqueIdentifier] [uniqueidentifier] NOT NULL,
	[IRNNo] [nvarchar](max) NOT NULL,
	[Identifier] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[BuyerIdentifier] [nvarchar](max) NULL,
	[SellerIdentifier] [nvarchar](max) NULL,
	[StandardIdentifier] [nvarchar](max) NULL,
	[Quantity] [decimal](18, 2) NOT NULL,
	[UOM] [nvarchar](max) NULL,
	[UnitPrice] [decimal](18, 2) NOT NULL,
	[CostPrice] [decimal](18, 2) NOT NULL,
	[DiscountPercentage] [decimal](18, 2) NOT NULL,
	[DiscountAmount] [decimal](18, 2) NOT NULL,
	[GrossPrice] [decimal](18, 2) NOT NULL,
	[NetPrice] [decimal](18, 2) NOT NULL,
	[VATRate] [decimal](18, 2) NOT NULL,
	[VATCode] [nvarchar](max) NULL,
	[VATAmount] [decimal](18, 2) NOT NULL,
	[LineAmountInclusiveVAT] [decimal](18, 2) NOT NULL,
	[CurrencyCode] [nvarchar](max) NULL,
	[TaxSchemeId] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[ExcemptionReasonCode] [nvarchar](max) NULL,
	[ExcemptionReasonText] [nvarchar](max) NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[AdditionalData1] [nvarchar](max) NULL,
	[AdditionalData2] [nvarchar](max) NULL,
	[Language] [nvarchar](max) NULL,
	[isOtherCharges] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[FileUpload_TransactionItem] ADD  DEFAULT ((0)) FOR [isOtherCharges]
GO
