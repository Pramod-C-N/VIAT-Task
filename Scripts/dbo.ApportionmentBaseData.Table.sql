USE [brady]
GO
/****** Object:  Table [dbo].[ApportionmentBaseData]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApportionmentBaseData](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TenantId] [int] NULL,
	[UniqueIdentifier] [uniqueidentifier] NOT NULL,
	[EffectiveFromDate] [datetime2](7) NULL,
	[EffectiveTillEndDate] [datetime2](7) NULL,
	[TaxableSupply] [decimal](18, 2) NULL,
	[TotalSupply] [decimal](18, 2) NULL,
	[TaxablePurchase] [decimal](18, 2) NULL,
	[TotalPurchase] [decimal](18, 2) NULL,
	[FinYear] [nvarchar](max) NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[CreatorUserId] [bigint] NULL,
	[LastModificationTime] [datetime2](7) NULL,
	[LastModifierUserId] [bigint] NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeleterUserId] [bigint] NULL,
	[DeletionTime] [datetime2](7) NULL,
	[TotalExemptPurchase] [decimal](18, 2) NULL,
	[TotalExemptSales] [decimal](18, 2) NULL,
	[ApportionmentPurchases] [decimal](18, 2) NULL,
	[ApportionmentSupplies] [decimal](18, 2) NULL,
	[MixedOverHeads] [decimal](18, 2) NULL,
	[ExemptSupply] [nvarchar](max) NULL,
	[ExemptPurchase] [nvarchar](max) NULL,
	[Type] [nvarchar](max) NULL,
	[Date] [nvarchar](50) NULL,
 CONSTRAINT [PK_ApportionmentBaseData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
