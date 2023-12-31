USE [brady]
GO
/****** Object:  Table [dbo].[tmp_items]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tmp_items](
	[Identifier] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[BuyerIdentifier] [nvarchar](max) NULL,
	[SellerIdentifier] [nvarchar](max) NULL,
	[StandardIdentifier] [nvarchar](max) NULL,
	[Quantity] [decimal](18, 2) NULL,
	[UOM] [nvarchar](max) NULL,
	[UnitPrice] [decimal](18, 2) NULL,
	[CostPrice] [decimal](18, 2) NULL,
	[DiscountPercentage] [decimal](18, 2) NULL,
	[DiscountAmount] [decimal](18, 2) NULL,
	[GrossPrice] [decimal](18, 2) NULL,
	[NetPrice] [decimal](18, 2) NULL,
	[VATRate] [decimal](18, 2) NULL,
	[VATCode] [nvarchar](max) NULL,
	[VATAmount] [decimal](18, 2) NULL,
	[LineAmountInclusiveVAT] [decimal](18, 2) NULL,
	[CurrencyCode] [nvarchar](max) NULL,
	[TaxSchemeId] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[ExcemptionReasonCode] [nvarchar](max) NULL,
	[ExcemptionReasonText] [nvarchar](max) NULL,
	[AdditionalData1] [nvarchar](max) NULL,
	[AdditionalData2] [nvarchar](max) NULL,
	[Language] [nvarchar](max) NULL,
	[uuid] [nvarchar](max) NULL,
	[isOtherCharges] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[tmp_items] ADD  DEFAULT ((0)) FOR [isOtherCharges]
GO
