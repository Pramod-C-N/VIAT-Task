USE [brady]
GO
/****** Object:  Table [dbo].[AppFriendships]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppFriendships](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[FriendProfilePictureId] [uniqueidentifier] NULL,
	[FriendTenancyName] [nvarchar](max) NULL,
	[FriendTenantId] [int] NULL,
	[FriendUserId] [bigint] NOT NULL,
	[FriendUserName] [nvarchar](256) NOT NULL,
	[State] [int] NOT NULL,
	[TenantId] [int] NULL,
	[UserId] [bigint] NOT NULL,
 CONSTRAINT [PK_AppFriendships] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_AppFriendships_FriendTenantId_FriendUserId]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_AppFriendships_FriendTenantId_FriendUserId] ON [dbo].[AppFriendships]
(
	[FriendTenantId] ASC,
	[FriendUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AppFriendships_FriendTenantId_UserId]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_AppFriendships_FriendTenantId_UserId] ON [dbo].[AppFriendships]
(
	[FriendTenantId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AppFriendships_TenantId_FriendUserId]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_AppFriendships_TenantId_FriendUserId] ON [dbo].[AppFriendships]
(
	[TenantId] ASC,
	[FriendUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AppFriendships_TenantId_UserId]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_AppFriendships_TenantId_UserId] ON [dbo].[AppFriendships]
(
	[TenantId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
