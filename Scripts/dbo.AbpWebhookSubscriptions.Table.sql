USE [brady]
GO
/****** Object:  Table [dbo].[AbpWebhookSubscriptions]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AbpWebhookSubscriptions](
	[Id] [uniqueidentifier] NOT NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[CreatorUserId] [bigint] NULL,
	[TenantId] [int] NULL,
	[WebhookUri] [nvarchar](max) NOT NULL,
	[Secret] [nvarchar](max) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Webhooks] [nvarchar](max) NULL,
	[Headers] [nvarchar](max) NULL,
 CONSTRAINT [PK_AbpWebhookSubscriptions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
