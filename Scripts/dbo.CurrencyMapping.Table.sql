USE [brady]
GO
/****** Object:  Table [dbo].[CurrencyMapping]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CurrencyMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TenantId] [int] NULL,
	[UUID] [uniqueidentifier] NOT NULL,
	[LocalCountryCode] [int] NOT NULL,
	[Alphacode2] [nvarchar](2) NULL,
	[Alphacode3] [nvarchar](3) NULL,
	[InvoiceCurrency] [nvarchar](3) NULL,
	[InvoiceCurrencyCountryCode] [int] NOT NULL,
	[AccountingCurrency] [nvarchar](3) NULL,
	[NationalCurrency] [nvarchar](3) NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[CreatorUserId] [bigint] NULL,
	[LastModificationTime] [datetime2](7) NULL,
	[LastModifierUserId] [bigint] NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeleterUserId] [bigint] NULL,
	[DeletionTime] [datetime2](7) NULL,
 CONSTRAINT [PK_CurrencyMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
