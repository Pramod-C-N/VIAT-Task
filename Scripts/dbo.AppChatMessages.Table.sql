USE [brady]
GO
/****** Object:  Table [dbo].[AppChatMessages]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppChatMessages](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[ReadState] [int] NOT NULL,
	[Side] [int] NOT NULL,
	[TargetTenantId] [int] NULL,
	[TargetUserId] [bigint] NOT NULL,
	[TenantId] [int] NULL,
	[UserId] [bigint] NOT NULL,
	[SharedMessageId] [uniqueidentifier] NULL,
	[ReceiverReadState] [int] NOT NULL,
 CONSTRAINT [PK_AppChatMessages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_AppChatMessages_TargetTenantId_TargetUserId_ReadState]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_AppChatMessages_TargetTenantId_TargetUserId_ReadState] ON [dbo].[AppChatMessages]
(
	[TargetTenantId] ASC,
	[TargetUserId] ASC,
	[ReadState] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AppChatMessages_TargetTenantId_UserId_ReadState]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_AppChatMessages_TargetTenantId_UserId_ReadState] ON [dbo].[AppChatMessages]
(
	[TargetTenantId] ASC,
	[UserId] ASC,
	[ReadState] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AppChatMessages_TenantId_TargetUserId_ReadState]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_AppChatMessages_TenantId_TargetUserId_ReadState] ON [dbo].[AppChatMessages]
(
	[TenantId] ASC,
	[TargetUserId] ASC,
	[ReadState] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AppChatMessages_TenantId_UserId_ReadState]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_AppChatMessages_TenantId_UserId_ReadState] ON [dbo].[AppChatMessages]
(
	[TenantId] ASC,
	[UserId] ASC,
	[ReadState] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AppChatMessages] ADD  DEFAULT ((0)) FOR [ReceiverReadState]
GO
