USE [brady]
GO
/****** Object:  Table [dbo].[taxmaster]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[taxmaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TenantId] [int] NULL,
	[TaxSchemeID] [nvarchar](3) NULL,
	[TaxID] [nvarchar](3) NULL,
	[TaxName] [nvarchar](10) NULL,
	[Rate] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[TaxCode] [nvarchar](1) NULL,
	[Eff_Fm_Dt] [datetime2](7) NOT NULL,
	[Eff_To_Dt] [datetime2](7) NOT NULL,
	[IsActive] [nvarchar](1) NULL,
	[UUID] [uniqueidentifier] NOT NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[CreatorUserId] [bigint] NULL,
	[LastModificationTime] [datetime2](7) NULL,
	[LastModifierUserId] [bigint] NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeleterUserId] [bigint] NULL,
	[DeletionTime] [datetime2](7) NULL,
 CONSTRAINT [PK_taxmaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
