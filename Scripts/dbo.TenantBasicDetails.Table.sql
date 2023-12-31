USE [brady]
GO
/****** Object:  Table [dbo].[TenantBasicDetails]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TenantBasicDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
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
	[ParentEntityName] [nvarchar](max) NULL,
	[LegalRepresentative] [nvarchar](max) NULL,
	[ParentEntityCountryCode] [nvarchar](max) NULL,
	[LastReturnFiled] [nvarchar](max) NULL,
	[VATReturnFillingFrequency] [nvarchar](max) NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[CreatorUserId] [bigint] NULL,
	[LastModificationTime] [datetime2](7) NULL,
	[LastModifierUserId] [bigint] NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeleterUserId] [bigint] NULL,
	[DeletionTime] [datetime2](7) NULL,
	[TimeZone] [nvarchar](max) NULL,
	[isPhase1] [bit] NOT NULL,
	[FaxNo] [nvarchar](max) NULL,
	[Website] [nvarchar](max) NULL,
 CONSTRAINT [PK_TenantBasicDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_TenantBasicDetails_TenantId]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_TenantBasicDetails_TenantId] ON [dbo].[TenantBasicDetails]
(
	[TenantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TenantBasicDetails] ADD  DEFAULT (CONVERT([bit],(0))) FOR [isPhase1]
GO
