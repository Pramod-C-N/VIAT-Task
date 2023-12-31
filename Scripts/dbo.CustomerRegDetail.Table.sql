USE [brady]
GO
/****** Object:  Table [dbo].[CustomerRegDetail]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerRegDetail](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UUID] [nvarchar](40) NULL,
	[TenantID] [int] NULL,
	[REFUUID] [nvarchar](40) NULL,
	[DocumentType] [nvarchar](140) NULL,
	[DocumentId] [nvarchar](140) NULL,
	[RegNo] [nvarchar](140) NULL,
	[RegDate] [datetime] NULL,
	[DocumentStatus] [bit] NULL
) ON [PRIMARY]
GO
