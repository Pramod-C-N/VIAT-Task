USE [brady]
GO
/****** Object:  Table [dbo].[VendorForeignEntity]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VendorForeignEntity](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TenantId] [int] NULL,
	[UniqueIdentifier] [uniqueidentifier] NOT NULL,
	[VendorID] [nvarchar](max) NULL,
	[VendorUniqueIdentifier] [uniqueidentifier] NOT NULL,
	[ForeignEntityName] [nvarchar](max) NULL,
	[ForeignEntityAddress] [nvarchar](max) NULL,
	[LegalRepresentative] [nvarchar](max) NULL,
	[Country] [nvarchar](max) NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[CreatorUserId] [bigint] NULL,
	[LastModificationTime] [datetime2](7) NULL,
	[LastModifierUserId] [bigint] NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeleterUserId] [bigint] NULL,
	[DeletionTime] [datetime2](7) NULL,
 CONSTRAINT [PK_VendorForeignEntity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_VendorForeignEntity_TenantId]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_VendorForeignEntity_TenantId] ON [dbo].[VendorForeignEntity]
(
	[TenantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
