USE [brady]
GO
/****** Object:  Table [dbo].[FileUpload_TransactionAddress]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileUpload_TransactionAddress](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[BatchId] [int] NULL,
	[TenantId] [int] NULL,
	[UniqueIdentifier] [uniqueidentifier] NOT NULL,
	[IRNNo] [nvarchar](max) NOT NULL,
	[Street] [nvarchar](max) NULL,
	[AdditionalStreet] [nvarchar](max) NULL,
	[BuildingNo] [nvarchar](max) NULL,
	[AdditionalNo] [nvarchar](max) NULL,
	[City] [nvarchar](max) NULL,
	[PostalCode] [nvarchar](max) NULL,
	[State] [nvarchar](max) NULL,
	[Neighbourhood] [nvarchar](max) NULL,
	[CountryCode] [nvarchar](max) NULL,
	[Type] [nvarchar](max) NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[AdditionalData1] [nvarchar](max) NULL,
	[Language] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
