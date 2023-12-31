USE [brady]
GO
/****** Object:  Table [dbo].[invoiceindicators]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[invoiceindicators](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TenantId] [int] NULL,
	[UUID] [uniqueidentifier] NOT NULL,
	[Invoice_flags] [nvarchar](20) NULL,
	[Description] [nvarchar](250) NULL,
	[TransTypePositionFm] [int] NOT NULL,
	[TransTypePositionTo] [int] NOT NULL,
	[TranstypeValue] [nvarchar](2) NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[CreatorUserId] [bigint] NULL,
	[LastModificationTime] [datetime2](7) NULL,
	[LastModifierUserId] [bigint] NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeleterUserId] [bigint] NULL,
	[DeletionTime] [datetime2](7) NULL,
	[Salestype] [nvarchar](max) NULL,
	[Purchasetype] [nvarchar](max) NULL,
 CONSTRAINT [PK_mst_invoiceindicators] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
