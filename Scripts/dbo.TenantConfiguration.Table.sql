USE [brady]
GO
/****** Object:  Table [dbo].[TenantConfiguration]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TenantConfiguration](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TenantId] [int] NULL,
	[isPhase1] [bit] NOT NULL,
	[TransactionType] [nvarchar](max) NULL,
	[ShipmentJson] [nvarchar](max) NULL,
	[AdditionalFieldsJson] [nvarchar](max) NULL,
	[EmailJson] [nvarchar](max) NULL,
	[AdditionalData1] [nvarchar](max) NULL,
	[AdditionalData2] [nvarchar](max) NULL,
	[AdditionalData3] [nvarchar](max) NULL,
	[isActive] [bit] NOT NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[CreatorUserId] [bigint] NULL,
	[LastModificationTime] [datetime2](7) NULL,
	[LastModifierUserId] [bigint] NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeleterUserId] [bigint] NULL,
	[DeletionTime] [datetime2](7) NULL,
	[Language] [nvarchar](max) NULL,
 CONSTRAINT [PK_TenantConfiguration] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_TenantConfiguration_TenantId]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_TenantConfiguration_TenantId] ON [dbo].[TenantConfiguration]
(
	[TenantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
