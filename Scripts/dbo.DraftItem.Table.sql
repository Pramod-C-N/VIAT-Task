USE [brady]
GO
/****** Object:  Table [dbo].[DraftItem]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DraftItem](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
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
	[AdditionalData1] [nvarchar](max) NULL,
	[AdditionalData2] [nvarchar](max) NULL,
	[Language] [nvarchar](max) NULL,
	[isOtherCharges] [bit] NOT NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[CreatorUserId] [bigint] NULL,
	[LastModificationTime] [datetime2](7) NULL,
	[LastModifierUserId] [bigint] NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeleterUserId] [bigint] NULL,
	[DeletionTime] [datetime2](7) NULL,
 CONSTRAINT [PK_DraftItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_DraftItem_TenantId]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_DraftItem_TenantId] ON [dbo].[DraftItem]
(
	[TenantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
