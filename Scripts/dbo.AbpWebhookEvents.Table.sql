USE [brady]
GO
/****** Object:  Table [dbo].[AbpWebhookEvents]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AbpWebhookEvents](
	[Id] [uniqueidentifier] NOT NULL,
	[WebhookName] [nvarchar](max) NOT NULL,
	[Data] [nvarchar](max) NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[TenantId] [int] NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeletionTime] [datetime2](7) NULL,
 CONSTRAINT [PK_AbpWebhookEvents] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
