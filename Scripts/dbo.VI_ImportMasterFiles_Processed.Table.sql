USE [brady]
GO
/****** Object:  Table [dbo].[VI_ImportMasterFiles_Processed]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VI_ImportMasterFiles_Processed](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TenantId] [int] NULL,
	[UniqueIdentifier] [uniqueidentifier] NOT NULL,
	[TenantType] [nvarchar](max) NULL,
	[ConstitutionType] [nvarchar](max) NULL,
	[BusinessCategory] [nvarchar](max) NULL,
	[OperationalModel] [nvarchar](max) NULL,
	[TurnoverSlab] [nvarchar](max) NULL,
	[ContactPerson] [nvarchar](max) NULL,
	[ContactNumber] [nvarchar](max) NULL,
	[EmailID] [nvarchar](max) NULL,
	[Nationality] [nvarchar](max) NULL,
	[Designation] [nvarchar](max) NULL,
	[VATID] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[LegalName] [nvarchar](max) NULL,
	[ParentEntityName] [nvarchar](max) NULL,
	[LegalRepresentative] [nvarchar](max) NULL,
	[ParententityCountryCode] [nvarchar](max) NULL,
	[LastReturnFiled] [datetime2](7) NULL,
	[VATReturnFillingFrequency] [nvarchar](max) NULL,
	[DocumentLineIdentifier] [nvarchar](max) NULL,
	[DocumentType] [nvarchar](max) NULL,
	[DocumentNumber] [nvarchar](max) NULL,
	[RegistrationDate] [datetime2](7) NULL,
	[BusinessPurchase] [nvarchar](max) NULL,
	[BusinessSupplies] [nvarchar](max) NULL,
	[SalesVATCategory] [nvarchar](max) NULL,
	[PurchaseVATCategory] [nvarchar](max) NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[CreatorUserId] [bigint] NULL,
	[LastModificationTime] [datetime2](7) NULL,
	[InvoiceType] [nvarchar](max) NULL,
	[LastModifierUserId] [bigint] NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeleterUserId] [bigint] NULL,
	[DeletionTime] [datetime2](7) NULL,
	[batchid] [int] NULL,
	[MasterType] [nvarchar](max) NULL,
	[MasterId] [nvarchar](max) NULL,
	[OrgType] [nvarchar](max) NULL,
	[AffiliationStatus] [nvarchar](max) NULL,
 CONSTRAINT [PK_VI_ImportMasterFiles_Processed] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
