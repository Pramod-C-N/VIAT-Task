USE [brady]
GO
/****** Object:  Table [dbo].[CompanyProfiles]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompanyProfiles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TenantId] [int] NULL,
	[UniqueIdentifier] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NULL,
	[LegalName] [nvarchar](100) NULL,
	[DateOfIncorporation] [datetime2](7) NOT NULL,
	[ConstitutionTypeUuid] [uniqueidentifier] NOT NULL,
	[ConstitutionType] [nvarchar](100) NULL,
	[ParentId] [int] NULL,
	[ParentUuid] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar](max) NULL,
	[TelephoneNo] [nvarchar](max) NULL,
	[Website] [nvarchar](max) NULL,
	[VatId] [nvarchar](max) NULL,
	[GroupVatId] [nvarchar](max) NULL,
	[CrNumber] [nvarchar](max) NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[CreatorUserId] [bigint] NULL,
	[LastModificationTime] [datetime2](7) NULL,
	[LastModifierUserId] [bigint] NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeleterUserId] [bigint] NULL,
	[DeletionTime] [datetime2](7) NULL,
	[TenantType] [varchar](200) NULL,
	[ContactPerson] [varchar](200) NULL,
	[ContactNO] [varchar](200) NULL,
	[EmailID] [varchar](200) NULL,
	[Nationality] [varchar](200) NULL,
	[Designation] [varchar](200) NULL,
	[ParentEntityName] [varchar](200) NULL,
	[LegalRep] [varchar](200) NULL,
	[countryofParententity] [varchar](200) NULL,
	[UUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_CompanyProfiles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
