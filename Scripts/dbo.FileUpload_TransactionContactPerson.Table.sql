USE [brady]
GO
/****** Object:  Table [dbo].[FileUpload_TransactionContactPerson]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileUpload_TransactionContactPerson](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[BatchId] [int] NULL,
	[TenantId] [int] NULL,
	[UniqueIdentifier] [uniqueidentifier] NOT NULL,
	[IRNNo] [nvarchar](max) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[EmployeeCode] [nvarchar](max) NULL,
	[ContactNumber] [nvarchar](max) NULL,
	[GovtId] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[Location] [nvarchar](max) NULL,
	[Type] [nvarchar](max) NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[AdditionalData1] [nvarchar](max) NULL,
	[Language] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
