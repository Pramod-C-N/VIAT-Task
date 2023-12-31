USE [brady]
GO
/****** Object:  Table [dbo].[FileUpload_TransactionParty]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileUpload_TransactionParty](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[BatchId] [int] NULL,
	[TenantId] [int] NULL,
	[UniqueIdentifier] [uniqueidentifier] NOT NULL,
	[IRNNo] [nvarchar](max) NULL,
	[RegistrationName] [nvarchar](max) NULL,
	[VATID] [nvarchar](max) NULL,
	[GroupVATID] [nvarchar](max) NULL,
	[CRNumber] [nvarchar](max) NULL,
	[OtherID] [nvarchar](max) NULL,
	[CustomerId] [nvarchar](max) NULL,
	[Type] [nvarchar](max) NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[AdditionalData1] [nvarchar](max) NULL,
	[Language] [nvarchar](max) NULL,
	[OtherDocumentTypeId] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
