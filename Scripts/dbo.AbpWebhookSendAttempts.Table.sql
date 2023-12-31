USE [brady]
GO
/****** Object:  Table [dbo].[AbpWebhookSendAttempts]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AbpWebhookSendAttempts](
	[Id] [uniqueidentifier] NOT NULL,
	[WebhookEventId] [uniqueidentifier] NOT NULL,
	[WebhookSubscriptionId] [uniqueidentifier] NOT NULL,
	[Response] [nvarchar](max) NULL,
	[ResponseStatusCode] [int] NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[LastModificationTime] [datetime2](7) NULL,
	[TenantId] [int] NULL,
 CONSTRAINT [PK_AbpWebhookSendAttempts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_AbpWebhookSendAttempts_WebhookEventId]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_AbpWebhookSendAttempts_WebhookEventId] ON [dbo].[AbpWebhookSendAttempts]
(
	[WebhookEventId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AbpWebhookSendAttempts]  WITH CHECK ADD  CONSTRAINT [FK_AbpWebhookSendAttempts_AbpWebhookEvents_WebhookEventId] FOREIGN KEY([WebhookEventId])
REFERENCES [dbo].[AbpWebhookEvents] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AbpWebhookSendAttempts] CHECK CONSTRAINT [FK_AbpWebhookSendAttempts_AbpWebhookEvents_WebhookEventId]
GO
