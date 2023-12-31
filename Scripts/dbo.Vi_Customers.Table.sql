USE [brady]
GO
/****** Object:  Table [dbo].[Vi_Customers]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vi_Customers](
	[Id] [int] NOT NULL,
	[TenantId] [int] NULL,
	[UniqueIdentifier] [uniqueidentifier] NOT NULL,
	[TenantType] [nvarchar](max) NULL,
	[ConstitutionType] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[LegalName] [nvarchar](max) NULL,
	[ContactPerson] [nvarchar](max) NULL,
	[ContactNumber] [nvarchar](max) NULL,
	[EmailID] [nvarchar](max) NULL,
	[Nationality] [nvarchar](max) NULL,
	[Designation] [nvarchar](max) NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[CreatorUserId] [bigint] NULL,
	[LastModificationTime] [datetime2](7) NULL,
	[LastModifierUserId] [bigint] NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeleterUserId] [bigint] NULL,
	[DeletionTime] [datetime2](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
