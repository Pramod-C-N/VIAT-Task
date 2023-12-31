USE [brady]
GO
/****** Object:  Table [dbo].[AbpNotificationSubscriptions]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AbpNotificationSubscriptions](
	[Id] [uniqueidentifier] NOT NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[CreatorUserId] [bigint] NULL,
	[EntityId] [nvarchar](96) NULL,
	[EntityTypeAssemblyQualifiedName] [nvarchar](512) NULL,
	[EntityTypeName] [nvarchar](250) NULL,
	[NotificationName] [nvarchar](96) NULL,
	[TenantId] [int] NULL,
	[UserId] [bigint] NOT NULL,
 CONSTRAINT [PK_AbpNotificationSubscriptions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AbpNotificationSubscriptions_NotificationName_EntityTypeName_EntityId_UserId]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_AbpNotificationSubscriptions_NotificationName_EntityTypeName_EntityId_UserId] ON [dbo].[AbpNotificationSubscriptions]
(
	[NotificationName] ASC,
	[EntityTypeName] ASC,
	[EntityId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AbpNotificationSubscriptions_TenantId_NotificationName_EntityTypeName_EntityId_UserId]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_AbpNotificationSubscriptions_TenantId_NotificationName_EntityTypeName_EntityId_UserId] ON [dbo].[AbpNotificationSubscriptions]
(
	[TenantId] ASC,
	[NotificationName] ASC,
	[EntityTypeName] ASC,
	[EntityId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
