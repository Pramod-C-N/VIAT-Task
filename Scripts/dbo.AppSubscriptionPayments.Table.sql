USE [brady]
GO
/****** Object:  Table [dbo].[AppSubscriptionPayments]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppSubscriptionPayments](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[CreatorUserId] [bigint] NULL,
	[DayCount] [int] NOT NULL,
	[DeleterUserId] [bigint] NULL,
	[DeletionTime] [datetime2](7) NULL,
	[EditionId] [int] NOT NULL,
	[Gateway] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[LastModificationTime] [datetime2](7) NULL,
	[LastModifierUserId] [bigint] NULL,
	[SuccessUrl] [nvarchar](max) NULL,
	[PaymentPeriodType] [int] NULL,
	[Status] [int] NOT NULL,
	[TenantId] [int] NOT NULL,
	[InvoiceNo] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[ErrorUrl] [nvarchar](max) NULL,
	[ExternalPaymentId] [nvarchar](450) NULL,
	[IsRecurring] [bit] NOT NULL,
	[EditionPaymentType] [int] NOT NULL,
 CONSTRAINT [PK_AppSubscriptionPayments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_AppSubscriptionPayments_EditionId]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_AppSubscriptionPayments_EditionId] ON [dbo].[AppSubscriptionPayments]
(
	[EditionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AppSubscriptionPayments_ExternalPaymentId_Gateway]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_AppSubscriptionPayments_ExternalPaymentId_Gateway] ON [dbo].[AppSubscriptionPayments]
(
	[ExternalPaymentId] ASC,
	[Gateway] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AppSubscriptionPayments_Status_CreationTime]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_AppSubscriptionPayments_Status_CreationTime] ON [dbo].[AppSubscriptionPayments]
(
	[Status] ASC,
	[CreationTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AppSubscriptionPayments] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsRecurring]
GO
ALTER TABLE [dbo].[AppSubscriptionPayments] ADD  DEFAULT ((0)) FOR [EditionPaymentType]
GO
ALTER TABLE [dbo].[AppSubscriptionPayments]  WITH CHECK ADD  CONSTRAINT [FK_AppSubscriptionPayments_AbpEditions_EditionId] FOREIGN KEY([EditionId])
REFERENCES [dbo].[AbpEditions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AppSubscriptionPayments] CHECK CONSTRAINT [FK_AppSubscriptionPayments_AbpEditions_EditionId]
GO
